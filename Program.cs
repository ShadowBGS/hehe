using System.Data.Common;
using System.Reflection;
using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using asp2;
using asp2.DbContexts;
using asp2.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddProblemDetails();
//builder.Services.AddProblemDetails(options =>
//{
  //  options.CustomizeProblemDetails = ctx =>
  //  {
   //     ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional info example");
 //   };
//  });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
var app = builder.Build();
#endif
builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(dbContextOptions=> dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var SecretForKey = builder.Configuration["Authentication:SecretForKey"];
var issuer = builder.Configuration["Authentication:Issuer"];
var audience = builder.Configuration["Authentication:Audience"];

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = false, 
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretForKey))
    };
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnAuthenticationFailed = context => { Log.Error(" while token failed " + context.Exception.Message + " " + context.Exception.StackTrace + " " + context.Exception.Source + " " + context.Exception.Data); return Task.CompletedTask; },
        OnMessageReceived = context => { Log.Error(" Incoming request made with token " + JsonConvert.SerializeObject(context.Request.Headers.ToList())); return Task.CompletedTask; },
        OnForbidden = context => { Log.Error(" Incoming request made with token " + JsonConvert.SerializeObject(context.Request.Headers.ToList())); return Task.CompletedTask; },

    };

});

//To make it so one people from a particular city can use the point of interest controller

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("MustBeFromSurulere", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.RequireClaim("city", "Surulere");

//    });
//});

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
}).AddMvc().AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;
});

var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction => {

    foreach(var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc($"{description.GroupName}", new()
        {
            Title ="City Info API",
            Version =description.ApiVersion.ToString(),
            Description ="Through this API you can access cities and their points of interest"
        });
    }


    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    setupAction.IncludeXmlComments(xmlCommentsFullPath);
    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });
    setupAction.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new OpenApiReference{
                Type =ReferenceType.SecurityScheme,
                Id = "CityInfoApiBearerAuth"}
            },
            new List<string>()
        }
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =ForwardedHeaders.XForwardedFor| ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{  
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            setupAction.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant()); 
        }
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
   

app.Run();
