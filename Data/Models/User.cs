

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string user_phone { get; set; }
        public string user_fullname { get; set; }
        public byte[] user_picture { get; set; }
        public bool user_isadmin { get; set; }
        public User(string user_phone, string user_fullname, byte[] user_picture, bool user_isadmin)
        {
            this.user_phone = user_phone;
            this.user_fullname = user_fullname;
            this.user_picture = user_picture;
            this.user_isadmin = user_isadmin;
        }
    }
}
