using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHealthy.DataModels
{
    class Historydb
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "currentWeight")]
        public float CurrentWeight { get; set; }

        [JsonProperty(PropertyName = "currentDate")]
        public DateTime CurrentDate { get; set; }
    }
}
