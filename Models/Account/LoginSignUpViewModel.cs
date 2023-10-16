using System.Data.Entity;

namespace techwales.Models.Account
{
    public class LoginSignUpViewModel : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}