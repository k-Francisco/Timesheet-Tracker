using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2.LineWorkModel
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<LineWorkModel> Results { get; set; }
    }
    public class LineWorkModel : RealmObject
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("ActualWork")]
        public int ActualWork { get; set; }
        [JsonProperty("PlannedWork")]
        public int PlannedWork { get; set; }
        [JsonProperty("TimesheetLineId")]
        public int TimesheetLineId { get; set; }
        [JsonProperty("TimesheetPeriodId")]
        public int TimesheetPeriodId { get; set; }
        [JsonProperty("WorkDate")]
        public DateTimeOffset WorkDate { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }

        public string EntryTextActualHours { get; set; }
        public bool isNotSaved { get; set; }
    }


    //public class RootObject
    //{
    //    [JsonProperty("d")]
    //    public D D { get; set; }
    //}
    //public class D
    //{
    //    [JsonProperty("results")]
    //    public List<LineWorkModel> Results { get; set; }
    //}
    //public class LineWorkModel : RealmObject
    //{
    //    [JsonProperty("actualWork")]
    //    public int ActualWork { get; set; }
    //    [JsonProperty("plannedWork")]
    //    public int PlannedWork { get; set; }
    //    [JsonProperty("workDate")]
    //    public DateTimeOffset WorkDate { get; set; }
    //    [JsonProperty("lineIdId")]
    //    public int LineIdId { get; set; }
    //    [JsonProperty("periodIdId")]
    //    public int PeriodIdId { get; set; }
    //    [JsonProperty("ID")]
    //    public int ID { get; set; }

    //    public string EntryTextActualHours { get; set; }
    //    public string EntryTextPlannedHours { get; set; }
    //    public bool isNotSaved { get; set; }
    //}

}
