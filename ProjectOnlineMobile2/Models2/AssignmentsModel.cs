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
        [JsonProperty("ProjectName")]
        public ProjectName ProjectDetails { get; set; }
        [JsonProperty("ResourceName")]
        public ResourceName ResourceName { get; set; }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }
        [JsonProperty("TaskStartDate")]
        public DateTimeOffset TaskStartDate { get; set; }
        [JsonProperty("TaskWork")]
        public double TaskWork { get; set; }
        [JsonProperty("TaskFinishDate")]
        public DateTimeOffset TaskFinishDate { get; set; }
        [JsonProperty("TaskActualWork")]
        public double TaskActualWork { get; set; }
        [JsonProperty("TaskRemainingWork")]
        public string TaskRemainingWork { get; set; }
        [JsonProperty("TaskPercentComplete")]
        public string TaskPercentComplete { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }

        public string Project
        {
            get { return ProjectDetails.Name; }
        }
    }

    public class ProjectName : RealmObject
    {
        [JsonProperty("ProjectName")]
        public string Name { get; set; }
    }
    
    public class ResourceName : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
    }

}
