using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2.Assignments
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<AssignmentModel> Results { get; set; }
    }
    public class AssignmentModel : RealmObject
    {
        [JsonProperty("projectId")]
        public ProjectId ProjectId { get; set; }
        [JsonProperty("resourceName")]
        public ResourceName ResourceName { get; set; }
        [JsonProperty("taskName")]
        public string TaskName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("work")]
        public int Work { get; set; }
        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTimeOffset EndDate { get; set; }
        [JsonProperty("actualWork")]
        public int ActualWork { get; set; }
        [JsonProperty("remainingWork")]
        public string RemainingWork { get; set; }
        [JsonProperty("percentCompleted")]
        public string PercentCompleted { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }

        public string ProjectName
        {
            get { return ProjectId.ProjectName; }
        }
    }

    public class ProjectId : RealmObject
    {
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }
    }
   
    public class ResourceName : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
    }

}
