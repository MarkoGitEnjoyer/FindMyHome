
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }
    }
}
