using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHealthyApp.DataModels
{
    public class EnterWeight
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "currentWeight")]
        public float currentWeight { get; set; }

        [JsonProperty(PropertyName = "targetWeight")]
        public float targetWeight { get; set; }
    }
}
