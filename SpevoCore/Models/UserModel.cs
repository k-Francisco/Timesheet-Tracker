using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpevoCore.Models.User
{
    public class UserModel
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("__metadata")]
        public Metadata_User Metadata { get; set; }
        [JsonProperty("Alerts")]
        public Alerts Alerts { get; set; }
        [JsonProperty("Groups")]
        public Groups Groups { get; set; }
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("IsHiddenInUI")]
        public bool IsHiddenInUI { get; set; }
        [JsonProperty("LoginName")]
        public string LoginName { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("PrincipalType")]
        public int PrincipalType { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("IsEmailAuthenticationGuestUser")]
        public bool IsEmailAuthenticationGuestUser { get; set; }
        [JsonProperty("IsShareByEmailGuestUser")]
        public bool IsShareByEmailGuestUser { get; set; }
        [JsonProperty("IsSiteAdmin")]
        public bool IsSiteAdmin { get; set; }
        [JsonProperty("UserId")]
        public UserId UserId { get; set; }
    }
    public class Metadata_User
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class Alerts
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class Deferred
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
    public class Groups
    {
        [JsonProperty("__deferred")]
        public Deferred Deferred { get; set; }
    }
    public class UserId
    {
        [JsonProperty("__metadata")]
        public Metadata1 Metadata { get; set; }
        [JsonProperty("NameId")]
        public string NameId { get; set; }
        [JsonProperty("NameIdIssuer")]
        public string NameIdIssuer { get; set; }
    }
    public class Metadata1
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
