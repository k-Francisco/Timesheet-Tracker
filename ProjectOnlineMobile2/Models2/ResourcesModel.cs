using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models2.ResourceModel
{
    public class ResourceRoot
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<ResourceModel> Results { get; set; }
    }
    public class ResourceModel : RealmObject
    {
        [JsonProperty("Resource")]
        public Resource Resource { get; set; }

        public override string ToString()
        {
            return Resource.Title;
        }
    }
    public class Resource : RealmObject
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("EMail")]
        public string EMail { get; set; }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Department")]
        public string Department { get; set; }
    }


}
