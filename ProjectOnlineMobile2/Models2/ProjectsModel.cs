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
        [JsonProperty("projectOwnerName")]
        public ProjectOwnerName ProjectOwnerName { get; set; }
        [JsonProperty("projectActualWork")]
        public int ProjectActualWork { get; set; }
        [JsonProperty("projectDescription")]
        public string ProjectDescription { get; set; }
        [JsonProperty("projectWork")]
        public int ProjectWork { get; set; }
        [JsonProperty("projectDuration")]
        public string ProjectDuration { get; set; }
        [JsonProperty("ProjectStartDate")]
        public DateTimeOffset ProjectStartDate { get; set; }
        [JsonProperty("projectRemainingWork")]
        public string ProjectRemainingWork { get; set; }
        [JsonProperty("projectFinishDate")]
        public DateTimeOffset ProjectFinishDate { get; set; }
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }
        [JsonProperty("projectPercentComplete")]
        public string ProjectPercentComplete { get; set; }
        [JsonProperty("projectStatus")]
        public string ProjectStatus { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }


        public double PercentCompletedInDecimal
        {
            get { return Convert.ToDouble((Convert.ToDouble(ProjectPercentComplete.Replace("%","")) / 100)); }
        }

        public string OwnerName
        {
            get { return ProjectOwnerName.Title; }
        }
    }
    public class ProjectOwnerName : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}
