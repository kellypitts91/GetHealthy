using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHealthy.DataModels
{
    class FoodDiarydb
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "foodItem")]
        public string FoodItem { get; set; }

        [JsonProperty(PropertyName = "dateOfEntry")]
        public DateTime DateOfEntry { get; set; }
    }
}
