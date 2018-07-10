    using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Models.ResourceAssignmentModel
{
    public class ResourceAssignmentModel
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<AssignmentResult> Results { get; set; }
    }
    public class AssignmentResult : RealmObject
    {
        //[JsonProperty("__metadata")]
        //public Metadata Metadata { get; set; }
        //[JsonProperty("Baseline")]
        //public Baseline Baseline { get; set; }
        //[JsonProperty("Project")]
        //public Project Project { get; set; }
        //[JsonProperty("Resource")]
        //public Resource Resource { get; set; }
        //[JsonProperty("Task")]
        //public Task Task { get; set; }
        //[JsonProperty("TimephasedData")]
        //public TimephasedData TimephasedData { get; set; }
        [JsonProperty("ProjectId")]
        public string ProjectId { get; set; }
        [JsonProperty("AssignmentId")]
        public string AssignmentId { get; set; }
        [JsonProperty("AssignmentActualCost")]
        public string AssignmentActualCost { get; set; }
        //[JsonProperty("AssignmentActualFinishDate")]
        //public object AssignmentActualFinishDate { get; set; }
        [JsonProperty("AssignmentActualOvertimeCost")]
        public string AssignmentActualOvertimeCost { get; set; }
        [JsonProperty("AssignmentActualOvertimeWork")]
        public string AssignmentActualOvertimeWork { get; set; }
        [JsonProperty("AssignmentActualRegularCost")]
        public string AssignmentActualRegularCost { get; set; }
        [JsonProperty("AssignmentActualRegularWork")]
        public string AssignmentActualRegularWork { get; set; }
        //[JsonProperty("AssignmentActualStartDate")]
        //public object AssignmentActualStartDate { get; set; }
        [JsonProperty("AssignmentActualWork")]
        public string AssignmentActualWork { get; set; }
        [JsonProperty("AssignmentACWP")]
        public string AssignmentACWP { get; set; }
        //[JsonProperty("AssignmentAllUpdatesApplied")]
        //public object AssignmentAllUpdatesApplied { get; set; }
        [JsonProperty("AssignmentBCWP")]
        public string AssignmentBCWP { get; set; }
        [JsonProperty("AssignmentBCWS")]
        public string AssignmentBCWS { get; set; }
        [JsonProperty("AssignmentBookingDescription")]
        public string AssignmentBookingDescription { get; set; }
        [JsonProperty("AssignmentBookingId")]
        public int AssignmentBookingId { get; set; }
        [JsonProperty("AssignmentBookingName")]
        public string AssignmentBookingName { get; set; }
        [JsonProperty("AssignmentBudgetCost")]
        public string AssignmentBudgetCost { get; set; }
        [JsonProperty("AssignmentBudgetMaterialWork")]
        public string AssignmentBudgetMaterialWork { get; set; }
        [JsonProperty("AssignmentBudgetWork")]
        public string AssignmentBudgetWork { get; set; }
        [JsonProperty("AssignmentCost")]
        public string AssignmentCost { get; set; }
        [JsonProperty("AssignmentCostVariance")]
        public string AssignmentCostVariance { get; set; }
        [JsonProperty("AssignmentCreatedDate")]
        public DateTimeOffset AssignmentCreatedDate { get; set; }
        [JsonProperty("AssignmentCreatedRevisionCounter")]
        public int AssignmentCreatedRevisionCounter { get; set; }
        [JsonProperty("AssignmentCV")]
        public string AssignmentCV { get; set; }
        [JsonProperty("AssignmentDelay")]
        public string AssignmentDelay { get; set; }
        [JsonProperty("AssignmentFinishDate")]
        public DateTimeOffset AssignmentFinishDate { get; set; }
        [JsonProperty("AssignmentFinishVariance")]
        public string AssignmentFinishVariance { get; set; }
        [JsonProperty("AssignmentIsOverallocated")]
        public bool AssignmentIsOverallocated { get; set; }
        [JsonProperty("AssignmentIsPublished")]
        public bool AssignmentIsPublished { get; set; }
        [JsonProperty("AssignmentMaterialActualWork")]
        public string AssignmentMaterialActualWork { get; set; }
        [JsonProperty("AssignmentMaterialWork")]
        public string AssignmentMaterialWork { get; set; }
        [JsonProperty("AssignmentModifiedDate")]
        public DateTimeOffset AssignmentModifiedDate { get; set; }
        [JsonProperty("AssignmentModifiedRevisionCounter")]
        public int AssignmentModifiedRevisionCounter { get; set; }
        [JsonProperty("AssignmentOvertimeCost")]
        public string AssignmentOvertimeCost { get; set; }
        [JsonProperty("AssignmentOvertimeWork")]
        public string AssignmentOvertimeWork { get; set; }
        [JsonProperty("AssignmentPeakUnits")]
        public string AssignmentPeakUnits { get; set; }
        [JsonProperty("AssignmentPercentWorkCompleted")]
        public int AssignmentPercentWorkCompleted { get; set; }
        [JsonProperty("AssignmentRegularCost")]
        public string AssignmentRegularCost { get; set; }
        [JsonProperty("AssignmentRegularWork")]
        public string AssignmentRegularWork { get; set; }
        [JsonProperty("AssignmentRemainingCost")]
        public string AssignmentRemainingCost { get; set; }
        [JsonProperty("AssignmentRemainingOvertimeCost")]
        public string AssignmentRemainingOvertimeCost { get; set; }
        [JsonProperty("AssignmentRemainingOvertimeWork")]
        public string AssignmentRemainingOvertimeWork { get; set; }
        [JsonProperty("AssignmentRemainingRegularCost")]
        public string AssignmentRemainingRegularCost { get; set; }
        [JsonProperty("AssignmentRemainingRegularWork")]
        public string AssignmentRemainingRegularWork { get; set; }
        [JsonProperty("AssignmentRemainingWork")]
        public string AssignmentRemainingWork { get; set; }
        [JsonProperty("AssignmentResourcePlanWork")]
        public string AssignmentResourcePlanWork { get; set; }
        [JsonProperty("AssignmentResourceType")]
        public int AssignmentResourceType { get; set; }
        [JsonProperty("AssignmentStartDate")]
        public DateTimeOffset AssignmentStartDate { get; set; }
        [JsonProperty("AssignmentStartVariance")]
        public string AssignmentStartVariance { get; set; }
        [JsonProperty("AssignmentSV")]
        public string AssignmentSV { get; set; }
        [JsonProperty("AssignmentType")]
        public int AssignmentType { get; set; }
        //[JsonProperty("AssignmentUpdatesAppliedDate")]
        //public object AssignmentUpdatesAppliedDate { get; set; }
        [JsonProperty("AssignmentVAC")]
        public string AssignmentVAC { get; set; }
        [JsonProperty("AssignmentWork")]
        public string AssignmentWork { get; set; }
        [JsonProperty("AssignmentWorkVariance")]
        public string AssignmentWorkVariance { get; set; }
        [JsonProperty("IsPublic")]
        public bool IsPublic { get; set; }
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }
        [JsonProperty("ResourceId")]
        public string ResourceId { get; set; }
        [JsonProperty("ResourceName")]
        public string ResourceName { get; set; }
        [JsonProperty("TaskId")]
        public string TaskId { get; set; }
        [JsonProperty("TaskIsActive")]
        public bool TaskIsActive { get; set; }
        [JsonProperty("TaskName")]
        public string TaskName { get; set; }
        [JsonProperty("TimesheetClassId")]
        public string TimesheetClassId { get; set; }
        [JsonProperty("TypeDescription")]
        public string TypeDescription { get; set; }
        [JsonProperty("TypeName")]
        public string TypeName { get; set; }
        //[JsonProperty("RBS_R")]
        //public object RBS { get; set; }
        //[JsonProperty("CostType_R")]
        //public object CostType { get; set; }
        //[JsonProperty("Health_T")]
        //public object Health { get; set; }
        //[JsonProperty("ResourceDepartments_R")]
        //public object ResourceDepartments { get; set; }
        //[JsonProperty("FlagStatus_T")]
        //public object FlagStatus { get; set; }

        public Double PercentCompletedInDecimal
        {
            get { return Convert.ToDouble((Convert.ToDouble(AssignmentPercentWorkCompleted) / 100)); }
        }

        public string FormattedAssignmentWork
        {
            get { return String.Format("{0:0.0}h", Double.Parse(AssignmentWork)); }
        }
        public string FormattedAssignmentActualWork
        {
            get { return String.Format("{0:0.0}h", Double.Parse(AssignmentActualWork)); }
        }
        public string FormattedAssignmentRemainingWork
        {
            get { return String.Format("{0:0}h", Double.Parse(AssignmentRemainingWork)); }
        }

    }
    public class Metadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    //public class Baseline
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Deferred
    //{
    //    [Key]
    //    [JsonProperty("uri")]
    //    public string Uri { get; set; }
    //}
    //public class Project
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Resource
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Task
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class TimephasedData
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}

}
