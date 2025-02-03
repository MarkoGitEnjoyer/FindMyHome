using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Models
{
    [Table("animals")]
    public class Animal
    {
        [Key]
        public int animal_id { get; set; }
        public string animal_status { get; set; }
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public User? user { get; set; }

        public byte[] animal_picture { get; set; }
        public string animal_breed { get; set; }
        public string animal_description { get; set; }
        public double animal_longtitude { get; set; }
        public double animal_latitude { get; set; }
        public DateTime animal_date { get; set; }
        public string animal_type { get; set; }


    }
}
