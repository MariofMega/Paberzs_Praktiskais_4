using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paberzs_Praktiskais_3.Models
{
    [Table("Games")]
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Genre { get; set; }

        public int ReleaseYear { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }

        public int PlatformId { get; set; }
        public Platform Platform { get; set; }
    }
}