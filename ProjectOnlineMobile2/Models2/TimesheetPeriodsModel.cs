using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2.TimesheetPeriodsModel
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<PeriodsModel> Results { get; set; }
    }
    public class PeriodsModel : RealmObject
    {
        [JsonProperty("Start")]
        public DateTimeOffset Start { get; set; }
        [JsonProperty("End")]
        public DateTimeOffset End { get; set; }
        [JsonProperty("PeriodName")]
        public string PeriodName { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }

        public override string ToString()
        {
            return PeriodName;
        }
    }

}
