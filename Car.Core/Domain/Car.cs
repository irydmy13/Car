using System;
using System.ComponentModel.DataAnnotations;

namespace Car.Core.Domain
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; }

        public string? Make { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public bool IsUsed { get; set; }
        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}