using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2.LineModel
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<LineModel> Results { get; set; }
    }
    public class LineModel : RealmObject
    {
        [JsonProperty("taskName")]
        public string TaskName { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("totalWork")]
        public int TotalWork { get; set; }
        [JsonProperty("projectId")]
        public int ProjectId { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("periodId")]
        public int PeriodId { get; set; }

        public string ProjectName { get { return "Project Name"; } }
    }

}
