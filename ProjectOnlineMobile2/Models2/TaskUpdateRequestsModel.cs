using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2.TaskUpdatesModel
{
    public class RootObject
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<TaskUpdateRequestsModel> Results { get; set; }
    }
    public class TaskUpdateRequestsModel : RealmObject
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("TaskUpdateTaskName")]
        public string TaskUpdateTaskName { get; set; }
        [JsonProperty("TaskUpdateStartDate")]
        public DateTimeOffset TaskUpdateStartDate { get; set; }
        [JsonProperty("TaskUpdateActualWork")]
        public int TaskUpdateActualWork { get; set; }
        [JsonProperty("TaskUpdateWork")]
        public int TaskUpdateWork { get; set; }
        [JsonProperty("TaskUpdateProjectNameId")]
        public int TaskUpdateProjectNameId { get; set; }
        [JsonProperty("TaskUpdateResourceNameId")]
        public int TaskUpdateResourceNameId { get; set; }
        [JsonProperty("TaskUpdateRemainingWork")]
        public string TaskUpdateRemainingWork { get; set; }
        [JsonProperty("TaskUpdatePercentComplete")]
        public string TaskUpdatePercentComplete { get; set; }
        [JsonProperty("TaskUpdateComment")]
        public string TaskUpdateComment { get; set; }
        [JsonProperty("TaskUpdateStatus")]
        public string TaskUpdateStatus { get; set; }
        [JsonProperty("TaskUpdateDateSent")]
        public DateTimeOffset TaskUpdateDateSent { get; set; }
        [JsonProperty("RequestType")]
        public string RequestType { get; set; }
        [JsonProperty("TaskUpdateFinishDate")]
        public DateTimeOffset TaskUpdateFinishDate { get; set; }
        [JsonProperty("TaskUpdateTaskId")]
        public int TaskUpdateTaskId { get; set; }
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("Modified")]
        public DateTimeOffset Modified { get; set; }
        [JsonProperty("Created")]
        public DateTimeOffset Created { get; set; }
    }
   
   
    

}
