using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ExternalLoginAnd2FA.Web.Models
{
    public class AddPhoneNumberModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
    }
}
