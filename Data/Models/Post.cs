using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Repositories;

namespace Data.Models
{
    [Table("posts")]
    public class Post
    {
        [Key]
        public int post_id { get; set; }
        public string post_content { get; set; }
        public DateTime post_creationdate { get; set; }
        public int? post_replyuser { get; set; }
        [ForeignKey("post_replyuser")]
        public Post? post { get; set; }
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public User? user { get; set; }
        public int thread_id { get; set; }
        [ForeignKey("thread_id")]
        public Thread? thread { get; set; }

    }
}
