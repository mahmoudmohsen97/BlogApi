using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
    public class Blog
    {
        public int BlogId { get; set; }

        public string BlogName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Post>? Posts {get;set;}

        public string AppUseId { get; set; }
        [ForeignKey("AppUseId")]
        public AppUser AppUser { get; set; }
    }
}
