using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Text.Json.Serialization;

namespace MiniAmazon.Models
{
    public class Users
    {
        //UserID, Name, Email, Password, Role
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string  Role { get; set; }

        [JsonIgnore]
        // Navigation property
        public ICollection<Orders> Orders { get; set; }
    }

}
