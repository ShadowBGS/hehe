﻿using asp2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace asp2.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }
        public int CityId { get; set; }
        

        public PointOfInterest(string name)
        {
            Name = name;
        }
    }
}
