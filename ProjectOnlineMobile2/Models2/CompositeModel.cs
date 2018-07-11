using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2.CompositeModel
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<CompositeModel> Results { get; set; }
    }
    public class CompositeModel
    {
        [JsonProperty("periodIdId")]
        public int PeriodIdId { get; set; }
        [JsonProperty("assignmentIdId")]
        public int AssignmentIdId { get; set; }
        [JsonProperty("projectIdId")]
        public int ProjectIdId { get; set; }
        [JsonProperty("timesheetLindIdId")]
        public int TimesheetLindIdId { get; set; }
    }

}
