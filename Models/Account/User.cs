using System.ComponentModel.DataAnnotations;

namespace techwales.Models.Account
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long Mobile { get; set; }
        public string Password { get; set; }
        public bool Isactive { get; set; }
        public bool Isremembered { get; set; }
        public string Phone { get; set; }   

    }
}