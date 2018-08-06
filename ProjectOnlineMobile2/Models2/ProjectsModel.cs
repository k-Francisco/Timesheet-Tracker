using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;

namespace ProjectOnlineMobile2.Models2.Projects
{

    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<ProjectModel> Results { get; set; }
    }
    public class ProjectModel : RealmObject
    {
        [JsonProperty("ProjectOwner")]
        public ProjectOwner ProjectOwner { get; set; }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }
        [JsonProperty("ProjectDescription")]
        public string ProjectDescription { get; set; }
        [JsonProperty("ProjectStartDate")]
        public DateTimeOffset ProjectStartDate { get; set; }
        [JsonProperty("ProjectWork")]
        public int ProjectWork { get; set; }
        [JsonProperty("ProjectDuration")]
        public string ProjectDuration { get; set; }
        [JsonProperty("ProjectActualWork")]
        public int ProjectActualWork { get; set; }
        [JsonProperty("ProjectPercentComplete")]
        public string ProjectPercentComplete { get; set; }
        [JsonProperty("ProjectRemainingWork")]
        public string ProjectRemainingWork { get; set; }
        [JsonProperty("ProjectStatus")]
        public string ProjectStatus { get; set; }
        [JsonProperty("ProjectFinishDate")]
        public DateTimeOffset ProjectFinishDate { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }

        public double PercentCompletedInDecimal
        {
            get { return Convert.ToDouble((Convert.ToDouble(ProjectPercentComplete.Replace("%", "")) / 100)); }
        }

        public string OwnerName
        {
            get { return ProjectOwner.Title; }
        }
    }

    public class ProjectOwner : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
    }

}
