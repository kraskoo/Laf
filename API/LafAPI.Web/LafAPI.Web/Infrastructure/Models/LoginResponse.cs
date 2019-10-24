namespace LafAPI.Web.Infrastructure.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class LoginResponse
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("avatarPath")]
        public string AvatarPath { get; set; }

        [JsonProperty("roles")]
        public IEnumerable<string> Roles { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonIgnore]
        public string Result { get; set; }
    }
}