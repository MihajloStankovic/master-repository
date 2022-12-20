using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MovieStore.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Director { get; set; }

        [Required]
        [StringLength(30)]
        public string MovieName { get; set; }

        [Required]
        [StringLength(20)]
        public string Genre { get; set; }

        [Required]
        public DateTime releaseDate { get; set; }
    }
}
