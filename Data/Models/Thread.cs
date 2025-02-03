using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
        [Table("threads")]
        public class Thread
        {
            [Key]
            public int thread_id { get; set; }
            public string thread_title { get; set; }
            public DateTime thread_creationdate { get; set; }
            public int user_id { get; set; }
            [ForeignKey("user_id")]
            public User? user { get; set; }
            public int category_id { get; set; }
            [ForeignKey("user_id")]
            public Category? category { get; set; }
        }
}