using Newtonsoft.Json;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTO
{
    public class UserPersonalInfoDTO
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("soldProducts")]
        public ProductCommonInfoDTO SoldProducts { get; set; }
    }
}
