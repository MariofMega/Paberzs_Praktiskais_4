using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paberzs_Praktiskais_3.Models
{
    [Table("Developers")]
    public class Developer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("DeveloperName")]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        public List<Game> Games { get; set; } = new List<Game>();
    }
}