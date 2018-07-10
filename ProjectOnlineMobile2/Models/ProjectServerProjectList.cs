using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectOnlineMobile2.Models.PSPL
{
    public class ProjectServerProjectList
    {
        [JsonProperty("d")]
        public D D { get; set; }
    }
    public class D
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        //[JsonProperty("__metadata")]
        //public Metadata Metadata { get; set; }
        //[JsonProperty("AssignmentBaselines")]
        //public AssignmentBaselines AssignmentBaselines { get; set; }
        //[JsonProperty("Assignments")]
        //public Assignments Assignments { get; set; }
        //[JsonProperty("Deliverables")]
        //public Deliverables Deliverables { get; set; }
        //[JsonProperty("Dependencies")]
        //public Dependencies Dependencies { get; set; }
        //[JsonProperty("Issues")]
        //public Issues Issues { get; set; }
        //[JsonProperty("Risks")]
        //public Risks Risks { get; set; }
        //[JsonProperty("StagesInfo")]
        //public StagesInfo StagesInfo { get; set; }
        //[JsonProperty("Tasks")]
        //public Tasks Tasks { get; set; }
        [JsonProperty("ProjectId")]
        public string ProjectId { get; set; }
        //[JsonProperty("EnterpriseProjectTypeDescription")]
        //public string EnterpriseProjectTypeDescription { get; set; }
        //[JsonProperty("EnterpriseProjectTypeId")]
        //public string EnterpriseProjectTypeId { get; set; }
        //[JsonProperty("EnterpriseProjectTypeIsDefault")]
        //public bool EnterpriseProjectTypeIsDefault { get; set; }
        //[JsonProperty("EnterpriseProjectTypeName")]
        //public string EnterpriseProjectTypeName { get; set; }
        //[JsonProperty("OptimizerCommitDate")]
        //public object OptimizerCommitDate { get; set; }
        //[JsonProperty("OptimizerDecisionAliasLookupTableId")]
        //public object OptimizerDecisionAliasLookupTableId { get; set; }
        //[JsonProperty("OptimizerDecisionAliasLookupTableValueId")]
        //public object OptimizerDecisionAliasLookupTableValueId { get; set; }
        //[JsonProperty("OptimizerDecisionID")]
        //public object OptimizerDecisionID { get; set; }
        //[JsonProperty("OptimizerDecisionName")]
        //public object OptimizerDecisionName { get; set; }
        //[JsonProperty("OptimizerSolutionName")]
        //public object OptimizerSolutionName { get; set; }
        //[JsonProperty("ParentProjectId")]
        //public object ParentProjectId { get; set; }
        //[JsonProperty("PlannerCommitDate")]
        //public object PlannerCommitDate { get; set; }
        //[JsonProperty("PlannerDecisionAliasLookupTableId")]
        //public object PlannerDecisionAliasLookupTableId { get; set; }
        //[JsonProperty("PlannerDecisionAliasLookupTableValueId")]
        //public object PlannerDecisionAliasLookupTableValueId { get; set; }
        //[JsonProperty("PlannerDecisionID")]
        //public object PlannerDecisionID { get; set; }
        //[JsonProperty("PlannerDecisionName")]
        //public object PlannerDecisionName { get; set; }
        //[JsonProperty("PlannerEndDate")]
        //public object PlannerEndDate { get; set; }
        //[JsonProperty("PlannerSolutionName")]
        //public object PlannerSolutionName { get; set; }
        //[JsonProperty("PlannerStartDate")]
        //public object PlannerStartDate { get; set; }
        //[JsonProperty("ProjectActualCost")]
        //public string ProjectActualCost { get; set; }
        //[JsonProperty("ProjectActualDuration")]
        //public string ProjectActualDuration { get; set; }
        //[JsonProperty("ProjectActualFinishDate")]
        //public object ProjectActualFinishDate { get; set; }
        //[JsonProperty("ProjectActualOvertimeCost")]
        //public string ProjectActualOvertimeCost { get; set; }
        //[JsonProperty("ProjectActualOvertimeWork")]
        //public string ProjectActualOvertimeWork { get; set; }
        //[JsonProperty("ProjectActualRegularCost")]
        //public string ProjectActualRegularCost { get; set; }
        //[JsonProperty("ProjectActualRegularWork")]
        //public string ProjectActualRegularWork { get; set; }
        //[JsonProperty("ProjectActualStartDate")]
        //public DateTime ProjectActualStartDate { get; set; }
        [JsonProperty("ProjectActualWork")]
        public string ProjectActualWork { get; set; }
        //[JsonProperty("ProjectACWP")]
        //public string ProjectACWP { get; set; }
        //[JsonProperty("ProjectAuthorName")]
        //public string ProjectAuthorName { get; set; }
        //[JsonProperty("ProjectBCWP")]
        //public string ProjectBCWP { get; set; }
        //[JsonProperty("ProjectBCWS")]
        //public string ProjectBCWS { get; set; }
        //[JsonProperty("ProjectBudgetCost")]
        //public string ProjectBudgetCost { get; set; }
        //[JsonProperty("ProjectBudgetWork")]
        //public string ProjectBudgetWork { get; set; }
        //[JsonProperty("ProjectCalculationsAreStale")]
        //public bool ProjectCalculationsAreStale { get; set; }
        //[JsonProperty("ProjectCalendarDuration")]
        //public int ProjectCalendarDuration { get; set; }
        //[JsonProperty("ProjectCategoryName")]
        //public object ProjectCategoryName { get; set; }
        //[JsonProperty("ProjectCompanyName")]
        //public object ProjectCompanyName { get; set; }
        //[JsonProperty("ProjectCost")]
        //public string ProjectCost { get; set; }
        //[JsonProperty("ProjectCostVariance")]
        //public string ProjectCostVariance { get; set; }
        //[JsonProperty("ProjectCPI")]
        //public string ProjectCPI { get; set; }
        [JsonProperty("ProjectCreatedDate")]
        public DateTimeOffset ProjectCreatedDate { get; set; }
        //[JsonProperty("ProjectCurrency")]
        //public string ProjectCurrency { get; set; }
        //[JsonProperty("ProjectCV")]
        //public string ProjectCV { get; set; }
        //[JsonProperty("ProjectCVP")]
        //public string ProjectCVP { get; set; }
        [JsonProperty("ProjectDescription")]
        public string ProjectDescription { get; set; }
        [JsonProperty("ProjectDuration")]
        public string ProjectDuration { get; set; }
        //[JsonProperty("ProjectDurationVariance")]
        //public string ProjectDurationVariance { get; set; }
        //[JsonProperty("ProjectEAC")]
        //public string ProjectEAC { get; set; }
        //[JsonProperty("ProjectEarlyFinish")]
        //public DateTime ProjectEarlyFinish { get; set; }
        //[JsonProperty("ProjectEarlyStart")]
        //public DateTime ProjectEarlyStart { get; set; }
        //[JsonProperty("ProjectEarnedValueIsStale")]
        //public bool ProjectEarnedValueIsStale { get; set; }
        //[JsonProperty("ProjectEnterpriseFeatures")]
        //public bool ProjectEnterpriseFeatures { get; set; }
        [JsonProperty("ProjectFinishDate")]
        public DateTimeOffset ProjectFinishDate { get; set; }
        //[JsonProperty("ProjectFinishVariance")]
        //public string ProjectFinishVariance { get; set; }
        //[JsonProperty("ProjectFixedCost")]
        //public string ProjectFixedCost { get; set; }
        //[JsonProperty("ProjectIdentifier")]
        //public string ProjectIdentifier { get; set; }
        //[JsonProperty("ProjectKeywords")]
        //public object ProjectKeywords { get; set; }
        //[JsonProperty("ProjectLateFinish")]
        //public DateTime ProjectLateFinish { get; set; }
        //[JsonProperty("ProjectLateStart")]
        //public DateTime ProjectLateStart { get; set; }
        [JsonProperty("ProjectLastPublishedDate")]
        public DateTimeOffset ProjectLastPublishedDate { get; set; }
        //[JsonProperty("ProjectManagerName")]
        //public object ProjectManagerName { get; set; }
        //[JsonProperty("ProjectModifiedDate")]
        //public DateTime ProjectModifiedDate { get; set; }
        [JsonProperty("ProjectName")]
        public string ProjectName { get; set; }
        //[JsonProperty("ProjectOvertimeCost")]
        //public string ProjectOvertimeCost { get; set; }
        //[JsonProperty("ProjectOvertimeWork")]
        //public string ProjectOvertimeWork { get; set; }
        //[JsonProperty("ProjectOwnerId")]
        //public string ProjectOwnerId { get; set; }
        [JsonProperty("ProjectOwnerName")]
        public string ProjectOwnerName { get; set; }
        [JsonProperty("ProjectPercentCompleted")]
        public int ProjectPercentCompleted { get; set; }
        [JsonProperty("ProjectPercentWorkCompleted")]
        public int ProjectPercentWorkCompleted { get; set; }
        //[JsonProperty("ProjectRegularCost")]
        //public string ProjectRegularCost { get; set; }
        //[JsonProperty("ProjectRegularWork")]
        //public string ProjectRegularWork { get; set; }
        //[JsonProperty("ProjectRemainingCost")]
        //public string ProjectRemainingCost { get; set; }
        //[JsonProperty("ProjectRemainingDuration")]
        //public string ProjectRemainingDuration { get; set; }
        //[JsonProperty("ProjectRemainingOvertimeCost")]
        //public string ProjectRemainingOvertimeCost { get; set; }
        //[JsonProperty("ProjectRemainingOvertimeWork")]
        //public string ProjectRemainingOvertimeWork { get; set; }
        //[JsonProperty("ProjectRemainingRegularCost")]
        //public string ProjectRemainingRegularCost { get; set; }
        //[JsonProperty("ProjectRemainingRegularWork")]
        //public string ProjectRemainingRegularWork { get; set; }
        //[JsonProperty("ProjectRemainingWork")]
        //public string ProjectRemainingWork { get; set; }
        //[JsonProperty("ProjectResourcePlanWork")]
        //public string ProjectResourcePlanWork { get; set; }
        //[JsonProperty("ProjectSPI")]
        //public string ProjectSPI { get; set; }
        //[JsonProperty("ProjectStartDate")]
        //public DateTime ProjectStartDate { get; set; }
        //[JsonProperty("ProjectStartVariance")]
        //public string ProjectStartVariance { get; set; }
        //[JsonProperty("ProjectStatusDate")]
        //public object ProjectStatusDate { get; set; }
        //[JsonProperty("ProjectSubject")]
        //public object ProjectSubject { get; set; }
        //[JsonProperty("ProjectSV")]
        //public string ProjectSV { get; set; }
        //[JsonProperty("ProjectSVP")]
        //public string ProjectSVP { get; set; }
        //[JsonProperty("ProjectTCPI")]
        //public string ProjectTCPI { get; set; }
        //[JsonProperty("ProjectTimephased")]
        //public object ProjectTimephased { get; set; }
        [JsonProperty("ProjectTitle")]
        public string ProjectTitle { get; set; }
        [JsonProperty("ProjectType")]
        public int ProjectType { get; set; }
        //[JsonProperty("ProjectVAC")]
        //public string ProjectVAC { get; set; }
        //[JsonProperty("ProjectWork")]
        //public string ProjectWork { get; set; }
        //[JsonProperty("ProjectWorkspaceInternalUrl")]
        //public object ProjectWorkspaceInternalUrl { get; set; }
        //[JsonProperty("ProjectWorkVariance")]
        //public string ProjectWorkVariance { get; set; }
        ////[JsonProperty("ResourcePlanUtilizationDate")]
        ////public object ResourcePlanUtilizationDate { get; set; }
        //[JsonProperty("ResourcePlanUtilizationType")]
        //public int ResourcePlanUtilizationType { get; set; }
        //[JsonProperty("WorkflowCreatedDate")]
        //public object WorkflowCreatedDate { get; set; }
        //[JsonProperty("WorkflowError")]
        //public object WorkflowError { get; set; }
        //[JsonProperty("WorkflowErrorResponseCode")]
        //public object WorkflowErrorResponseCode { get; set; }
        //[JsonProperty("WorkflowInstanceId")]
        //public object WorkflowInstanceId { get; set; }
        //[JsonProperty("WorkflowOwnerId")]
        //public object WorkflowOwnerId { get; set; }
        //[JsonProperty("WorkflowOwnerName")]
        //public object WorkflowOwnerName { get; set; }
        //[JsonProperty("ProjectDepartments")]
        //public object ProjectDepartments { get; set; }
        //[JsonProperty("dsdsdsd")]
        //public object Dsdsdsd { get; set; }

        public double PercentCompletedInDecimal
        {
            get { return Convert.ToDouble((Convert.ToDouble(ProjectPercentCompleted) / 100)); }
        }

        public bool IsUserAssignedToThisProject { get; set; }

    }
    //public class Metadata
    //{
    //    [Key]
    //    [JsonProperty("id")]
    //    public string Id { get; set; }
    //    [JsonProperty("uri")]
    //    public string Uri { get; set; }
    //    [JsonProperty("type")]
    //    public string Type { get; set; }
    //}
    //public class AssignmentBaselines
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
    //public class Assignments
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Deliverables
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Dependencies
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Issues
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Risks
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class StagesInfo
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}
    //public class Tasks
    //{
    //    [Key]
    //    [JsonProperty("__deferred")]
    //    public Deferred Deferred { get; set; }
    //}

}
