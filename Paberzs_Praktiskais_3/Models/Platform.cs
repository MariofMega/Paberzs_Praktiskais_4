using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paberzs_Praktiskais_3.Models
{
    [Table("Platforms")]
    public class Platform
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Game> Games { get; set; } = new List<Game>();
    }
}