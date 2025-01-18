using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostBody { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }
    }
}
