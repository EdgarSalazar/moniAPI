using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Moni.Models
{
    public class User : ModelBase
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}
