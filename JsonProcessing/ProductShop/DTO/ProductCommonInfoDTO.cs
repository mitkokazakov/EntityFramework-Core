using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTO
{
    public class ProductCommonInfoDTO
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("products")]
        public SingleProductInfoDTO[] Products { get; set; }
    }
}
