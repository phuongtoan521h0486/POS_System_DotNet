using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace POS_System_DotNet.Models.Account
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActivate { get; set; }
        public bool Status { get; set; }

        [EnumDataType(typeof(AccountRole))]
        public string Role { get; set; }

        public string Phone { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Picture { get; set; }
    }
}
