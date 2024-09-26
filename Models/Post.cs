using System.ComponentModel.DataAnnotations;

namespace GorBilet_example.Models
{
    public class Post
    {
        public string? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

    }
}
