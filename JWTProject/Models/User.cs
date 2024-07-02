using System.ComponentModel.DataAnnotations;

namespace JWTProject.Models
{
    public class User
    {
        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public string? PersonImage { get; set; }

        public string? Mail { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}
