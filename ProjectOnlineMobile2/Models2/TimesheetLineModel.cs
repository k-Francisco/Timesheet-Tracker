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
        [JsonProperty("TaskName")]
        public LineTask TaskDetails { get; set; }
        [JsonProperty("ProjectName")]
        public LineProjectName ProjectDetails { get; set; }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Comment")]
        public string Comment { get; set; }
        [JsonProperty("Status")]
        public string Status { get; set; }
        [JsonProperty("TotalWork")]
        public int TotalWork { get; set; }
        [JsonProperty("LinePeriodId")]
        public int LinePeriodId { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }

        public string Project
        {
            get { return ProjectDetails.Name; }
        }

        public string Task
        {
            get { return TaskDetails.TaskName; }
        }

        public string StatusTranslation
        {
            get
            {
                if (Status.Equals("Approved"))
                    return "";

                return Status;
            }
        }
    }

    public class LineProjectName : RealmObject
    {
        [JsonProperty("ProjectName")]
        public string Name { get; set; }
    }

    public class LineTask : RealmObject
    {
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }
    }


    //public class RootObject
    //{
    //    [JsonProperty("d")]
    //    public D D { get; set; }
    //}
    //public class D
    //{
    //    [JsonProperty("results")]
    //    public List<LineModel> Results { get; set; }
    //}
    //public class LineModel : RealmObject
    //{
    //    [JsonProperty("taskName")]
    //    public string TaskName { get; set; }
    //    [JsonProperty("comment")]
    //    public string Comment { get; set; }
    //    [JsonProperty("status")]
    //    public string Status { get; set; }
    //    [JsonProperty("totalWork")]
    //    public int TotalWork { get; set; }
    //    [JsonProperty("projectId")]
    //    public int ProjectId { get; set; }
    //    [JsonProperty("ID")]
    //    public int ID { get; set; }
    //    [JsonProperty("periodId")]
    //    public int PeriodId { get; set; }

    //    public string ProjectName { get { return "Project Name"; } }
    //}

}
