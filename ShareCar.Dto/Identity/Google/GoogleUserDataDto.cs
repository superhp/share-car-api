using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareCar.Dto.Identity.Google
{
    public class GoogleUserDataDto 
    {
        public long Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("givenName")]
        public string GivenName { get; set; }
        [JsonProperty("familyName")]
        public string FamilyName { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
