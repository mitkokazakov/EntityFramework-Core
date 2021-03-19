using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    class SalesDiscountDTO
    {
        [JsonProperty("car")]
        public SingleCarDTO Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        public string Discount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("priceWithDiscount")]
        public string PriceWithDiscount => (decimal.Parse(this.Price) - ((decimal.Parse(this.Discount) / 100m)* decimal.Parse(this.Price))).ToString("f2");
    }
}
