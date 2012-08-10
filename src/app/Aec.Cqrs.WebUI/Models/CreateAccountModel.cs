using System;
using System.ComponentModel.DataAnnotations;

namespace Aec.Cqrs.WebUI.Models
{
    public class CreateAccountModel
    {
        [Required]
        public Guid AccountID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}