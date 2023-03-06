using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HRMS_V2.Application.Models.Authentication
{
    public class TokenRequestModel
    {
        /// <summary>
        /// The username of the user logging in.
        /// </summary>
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The password for the user logging in.
        /// </summary>
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
