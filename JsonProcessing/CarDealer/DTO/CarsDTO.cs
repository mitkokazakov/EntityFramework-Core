using CarDealer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class CarsDTO
    {
        [JsonProperty("car")]
        public SingleCarDTO Cars { get; set; }

        [JsonProperty("parts")]
        public SinglePartDTO[] Parts { get; set; }
    }
}
