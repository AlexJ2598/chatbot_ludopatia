namespace chatbot_ludo.Common.Models
{
    using Newtonsoft.Json;
    using System;

    public class User
    {
        [JsonProperty("id")]
        public Guid UserId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailConfirmed")]
        public bool EmailConfirmed { get; set; }
    }
}

