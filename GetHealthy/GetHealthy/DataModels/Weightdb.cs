using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHealthy.DataModels
{
    class Weightdb
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "currentWeight")]
        public float CurrentWeight { get; set; }

        [JsonProperty(PropertyName = "targetWeight")]
        public float TargetWeight { get; set; }

        [JsonProperty(PropertyName = "targetDate")]
        public DateTime TargetDate { get; set; }
    }
}
