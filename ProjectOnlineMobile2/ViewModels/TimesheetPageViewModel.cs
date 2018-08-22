using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Newtonsoft.Json;
using PeriodsRoot = ProjectOnlineMobile2.Models2.TimesheetPeriodsModel.RootObject;
using PeriodsModel = ProjectOnlineMobile2.Models2.TimesheetPeriodsModel.PeriodsModel;
using CompositeRoot = ProjectOnlineMobile2.Models2.CompositeModel.RootObject;
using LineRoot = ProjectOnlineMobile2.Models2.LineModel.RootObject;
using LineModel = ProjectOnlineMobile2.Models2.LineModel.LineModel;
using ProjectOnlineMobile2.Models2;
using System.Threading.Tasks;
using System.Net.Http;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TimesheetPageViewModel : BaseViewModel
    {

        private ObservableCollection<PeriodsModel> _periodList = new ObservableCollection<PeriodsModel>();
        public ObservableCollection<PeriodsModel> PeriodList
        {
            get { return _periodList; }
            set { SetProperty(ref _periodList, value); }
        }


        private ObservableCollection<LineModel> _periodLines = new ObservableCollection<LineModel>();
        public ObservableCollection<LineModel> PeriodLines
        {
            get { return _periodLines; }
            set { SetProperty(ref _periodLines, value); }
        }

        private ObservableCollection<String> _projectsAssigned = new ObservableCollection<string>();
        public ObservableCollection<String> ProjectsAssigned
        {
            get { return _projectsAssigned; }
            set { SetProperty(ref _projectsAssigned, value); }
        }

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        private int _selectedProject;
        public int SelectedProject
        {
            get { return _selectedProject; }
            set { SetProperty(ref _selectedProject, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetProperty(ref _isEmpty, value); }
        }

        private bool _openPicker = false;
        public bool OpenPicker
        {
            get { return _openPicker; }
            set { SetProperty(ref _openPicker, value); }
        }

        public ICommand SelectedProjectChangedCommand { get; set; }

        CompositeRoot compositeList;
        const string TIMESHEET_PERIODS_LIST_GUID = "9b209820-64ae-4edd-ad61-1490d980b362";
        const string TIMESHEET_LINES_LIST_GUID = "ee561f4c-2604-46c3-8d36-63693cad1510";
        const string COMPOSITE_LIST_GUID = "7fe8ecb3-8a7f-4874-ac2f-dac80517f439";

        public TimesheetPageViewModel()
        {

            //MessagingCenter.Instance.Subscribe<String>(this, "CreateTimesheet", (periodId) =>
            //{
            //    CreateTimesheet(periodId);
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "SubmitTimesheet", (comment) => {
            //    ExecuteSubmitTimesheet(comment);
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "RecallTimesheet", (s) => {
            //    ExecuteRecallTimesheet();
            //});

            //MessagingCenter.Instance.Subscribe<String[]>(this, "AddTimesheetLine", (s) => {
            //    //first index in the string array is task name
            //    //second index in the string array is comment

            //    ExecuteAddTimesheetLine(s[0], s[1]);
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "RefreshTimesheetLines", (s)=> {
            //    ExecuteRefreshLinesCommand();
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "AddAssignedProjects", (s)=> {
            //    ExecuteAddAssignedProjects();
            //});

            MessagingCenter.Instance.Subscribe<string[]>(this, "SaveEditedComment", (parameters)=> {
                SaveEditedComment(parameters);
            });

            LoadPeriodsFromLocalDatabase();
            SyncPeriods();
            FindTodaysPeriod();
            GetCompositeListFromServer();

            //SelectedProjectChangedCommand = new Command(ExecuteSelectedProjectChangedCommand);
            //TimesheetLineClicked = new Command<LineResult>(ExecuteTimesheetLineClicked);
        }

        private void LoadPeriodsFromLocalDatabase()
        {
            var localPeriods = realm.All<PeriodsModel>().ToList();

            foreach (var item in localPeriods)
            {
                PeriodList.Add(item);
            }
        }

        private void FindTodaysPeriod()
        {
            if (PeriodList.Any())
            {
                for (int i = 0; i < PeriodList.Count; i++)
                {
                    if (DateTime.Compare(DateTime.Now, PeriodList[i].Start.DateTime) >= 0 &&
                            DateTime.Compare(DateTime.Now, PeriodList[i].End.DateTime) < 0)
                    {
                        SelectedIndex = i;
                        ExecuteSelectedItemChangedCommand();
                        break;
                    }
                }
                MessagingCenter.Instance.Send<String>(PeriodList[SelectedIndex].ToString(), "TimesheetPeriod");
            }
        }

        private async void SyncPeriods()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var localPeriods = realm.All<PeriodsModel>().ToList();

                    string query = "$select=ID," +
                    "PeriodName," +
                    "Start," +
                    "End";

                    var apiResponse = await SPapi.GetListItemsByListGuid(TIMESHEET_PERIODS_LIST_GUID, query);

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var periodsList = JsonConvert.DeserializeObject<PeriodsRoot>(await apiResponse.Content.ReadAsStringAsync());
                        syncDataService.SyncTimesheetPeriods(localPeriods, periodsList.D.Results, PeriodList);

                        if (SelectedIndex < 0)
                            FindTodaysPeriod();
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncPeriods", e.Message);
            }
        }

        public ICommand SelectedItemChangedCommand { get { return new Command(ExecuteSelectedItemChangedCommand); } }
        private void ExecuteSelectedItemChangedCommand()
        {
            PeriodLines.Clear();

            var periodId = PeriodList[SelectedIndex].ID;

            var localLines = realm.All<LineModel>()
                .Where(p => p.LinePeriodId == periodId)
                .ToList();

            foreach (var item in localLines)
            {
                PeriodLines.Add(item);
            }

            MessagingCenter.Instance.Send<String>(PeriodList[SelectedIndex].ToString(), "TimesheetPeriod");
        }

        public async void SyncTimesheetLines()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var localLines = realm.All<LineModel>().ToList();
                    var userId = realm.All<UserModel>().FirstOrDefault().UserId;
                    string query;

                    query = "$select=ID," +
                    "Comment," +
                    "Status," +
                    "TotalWork," +
                    "LinePeriodId," +
                    "ProjectName/ProjectName," +
                    "TaskName/TaskName" +
                    "&$expand=ProjectName,TaskName" +
                    "&$filter=ResourceStringId eq " + userId.ToString() +
                    "and ";

                    StringBuilder sb = new StringBuilder(query);

                    foreach (var item in compositeList.D.Results)
                    {
                        sb.Append("(ID eq " + item.TimesheetLineId + ") or ");
                    }
                    //remove the last 'or' in the query
                    sb.Remove((sb.Length - 4), 4);


                    var apiResponse = await SPapi.GetListItemsByListGuid(TIMESHEET_LINES_LIST_GUID, sb.ToString());
                    
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var lineList = JsonConvert.DeserializeObject<LineRoot>(await apiResponse.Content.ReadAsStringAsync());

                        syncDataService.SyncTimesheetLines(localLines, lineList.D.Results, PeriodLines, PeriodList[SelectedIndex].ID);
                    }

                    IsRefreshing = false;
                }
                else
                {
                    ExecuteSelectedItemChangedCommand();
                    IsRefreshing = false;
                }

                if (PeriodLines.Any())
                    IsEmpty = false;
                else
                    IsEmpty = true;
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetLines", e.Message);
                IsRefreshing = false;
            }
        }

        public ICommand RefreshLinesCommand { get { return new Command(ExecuteRefreshLinesCommand); } }
        private void ExecuteRefreshLinesCommand()
        {
            IsRefreshing = true;

            if (IsConnectedToInternet())
            {
                realm.Write(() =>
                {
                    realm.RemoveAll<LineModel>();
                });

                PeriodLines.Clear();

                GetCompositeListFromServer();
                Task.Delay(1500);
                SyncTimesheetLines();
            }
            else
                IsRefreshing = false;
        }

        public ICommand TimesheetLineClicked { get { return new Command<LineModel>(ExecuteTimesheetLineClicked); } }
        private void ExecuteTimesheetLineClicked(LineModel line)
        {
            MessagingCenter.Instance.Send<LineModel>(line, "PushTimesheetWorkPage");
            MessagingCenter.Instance.Send<string>(PeriodList[SelectedIndex].ID.ToString(), "SendPeriodIdToWorkPage");
            MessagingCenter.Instance.Send<string>(line.ID.ToString(), "SendCurrentLineIdToWorkPage");

            if (IsConnectedToInternet())
            {
                try
                {
                    var completeLineIds = new List<int>();
                    foreach (var item in compositeList.D.Results)
                    {
                        completeLineIds.Add(item.TimesheetLineId);
                    }
                    MessagingCenter.Instance.Send<List<int>>(completeLineIds, "SendLineIdsToWorkPage");
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message, "ExecuteTimesheetLineClicked");
                }
            }
        }

        private async void GetCompositeListFromServer()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var userId = realm.All<UserModel>().FirstOrDefault().UserId;

                    string query = "$select=ProjectId," +
                        "TaskId," +
                        "TimesheetPeriodId," +
                        "TimesheetLineId" +
                        "&$filter=ResourceStringId eq " + userId.ToString();

                    var apiResponse = await SPapi.GetListItemsByListGuid(COMPOSITE_LIST_GUID, query);

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        compositeList = JsonConvert.DeserializeObject<CompositeRoot>(await apiResponse.Content.ReadAsStringAsync());
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("GetCompositeListFromServer", e.Message);
            }
        }

        public ICommand ExecuteLongPress { get { return new Command<LineModel>(ItemLongPress); } }
        private void ItemLongPress(LineModel line)
        {
            MessagingCenter.Instance.Send<LineModel>(line, "EditComment");
        }

        private async void SaveEditedComment(string[] parameters)
        {
            /**
             * parameters[0] = timesheet line ID
             * parameters[1] = comment
             **/

            if (IsConnectedToInternet())
            {

                try
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "Saving", null, null }, "DisplayAlert");

                    var body = "{'__metadata':{'type':'SP.Data.TimesheetLinesListListItem'}," +
                    "'Comment':'" + parameters[1] + "'}";

                    var item = new StringContent(body);
                    item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                    var formDigest = await SPapi.GetFormDigest();

                    var edit = await SPapi.UpdateListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue,
                        TIMESHEET_LINES_LIST_GUID,
                        item,
                        parameters[0]);
                    var ensure = edit.EnsureSuccessStatusCode();

                    if (ensure.IsSuccessStatusCode)
                    {
                        //display prompt that creation of project is successful
                        MessagingCenter.Instance.Send<string[]>(new string[] { "Successfully edited the line", "OK", null }, "DisplayAlert");

                        Debug.WriteLine("SUCCESS", "EDIT TIMESHEET LINE COMMENT");
                    }
                    else
                    {
                        //display prompt that creation of project has failed
                        MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error sending the request", "OK", null }, "DisplayAlert");

                        Debug.WriteLine("FAILED", "EDIT TIMESHEET LINE COMMENT");
                    }
                }
                catch (Exception e)
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error sending the request", "OK", null }, "DisplayAlert");

                    Debug.WriteLine(e.Message, "SaveEditedComment");
                }

            }

        }

        //private void ExecuteTimesheetLineClicked(LineResult timesheetLine)
        //{
        //    lineId = timesheetLine.Id;
        //    string[] ids = { _periodList[SelectedIndex].Id, timesheetLine.Id };
        //    MessagingCenter.Send<LineResult>(timesheetLine, "PushTimesheetWorkPage");
        //    MessagingCenter.Send<String[]>(ids, "TimesheetWork");

        //}

        //private void ExecuteAddAssignedProjects()
        //{
        //    ProjectsAssigned.Add("Personal Task");

        //    var savedProjects = realm.All<ProjectResult>().ToList();
        //    foreach (var item in savedProjects)
        //    {
        //        if (item.IsUserAssignedToThisProject)
        //            ProjectsAssigned.Add(item.ProjectName);
        //    }
        //}

        //private async void ExecuteAddTimesheetLine(string taskName, string comment)
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Adding timesheet line...", "Close" }, "DisplayAlert");

        //            string body = "";

        //            var project = realm.All<ProjectResult>()
        //                           .Where(p => p.ProjectName.Equals(ProjectsAssigned[SelectedProject]))
        //                           .FirstOrDefault();

        //            var formDigest = await SPapi.GetFormDigest();

        //            if (project != null)
        //            {
        //                body = "{'parameters':" +
        //                    "{'TaskName':'" + taskName + "', " +
        //                    "'Comment':'" + comment + "', " +
        //                    "'ProjectId':'" + project.ProjectId + "'}}";
        //            }
        //            else
        //            {
        //                body = "{'parameters':{'TaskName':'" + taskName + "', " +
        //                    "'Comment':'" + comment + "'}}";
        //            }

        //            var addLine = await PSapi.AddTimesheetLine(PeriodList[SelectedIndex].Id, body, formDigest.D.GetContextWebInformation.FormDigestValue);
        //            if (addLine)
        //            {
        //                string[] alertStrings = { "Successfully added line", "Close" };
        //                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");

        //                ExecuteRefreshLinesCommand();
        //            }
        //            else
        //            {
        //                string[] alertStrings = { "There was an error adding the line. Please try again", "Close" };
        //                MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
        //            }
        //        }
        //        else
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Your device is not connected to the internet", "Close" }, "DisplayAlert");
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("ExecuteAddTimesheetLine", e.Message);
        //        string[] alertStrings = { "There was an error adding the line. Please try again", "Close" };
        //        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
        //    }
        //}

        //private void ExecuteSelectedProjectChangedCommand()
        //{

        //    MessagingCenter.Instance.Send<String>("", "CloseProjectPicker");
        //    MessagingCenter.Instance.Send<String>("", "AddTimesheetLineDialog");
        //}

        //private void ExecuteRefreshLinesCommand()
        //{
        //    try
        //    {
        //        IsRefreshing = true;

        //        if (IsConnectedToInternet())
        //        {
        //            realm.Write(()=> {
        //                realm.RemoveRange<SavedLinesModel>(_savedLines);
        //            });
        //            PeriodLines.Clear();

        //            realm.Refresh();

        //            SyncTimesheetLines(_savedLines.ToList());
        //        }
        //        else
        //            IsRefreshing = false;

        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("ExecuteRefreshLinesCommand", e.Message);
        //        IsRefreshing = false;
        //    }
        //}

        //private async void SyncTimesheetPeriods(List<TimesheetPeriodsResult> savedPeriods)
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            var periods = await PSapi.GetAllTimesheetPeriods();
        //            syncDataService.SyncTimesheetPeriods(savedPeriods, periods.D.Results, PeriodList);
        //            if(SelectedIndex <= 0)
        //            {
        //                FindTodaysPeriod();
        //            }
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("SyncTimesheetPeriods", e.Message);
        //    }
        //}

        //private void FindTodaysPeriod()
        //{
        //    for (int i = 0; i < PeriodList.Count; i++)
        //    {
        //        if (DateTime.Compare(DateTime.Now, PeriodList[i].Start.DateTime) >= 0 &&
        //                DateTime.Compare(DateTime.Now, PeriodList[i].End.DateTime) < 0)
        //        {
        //            SelectedIndex = i;
        //            ExecuteSelectedItemChangedCommand();
        //            break;
        //        }
        //    }
        //}

        //private async void ExecuteRecallTimesheet()
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Recalling timesheet...","Close"}, "DisplayAlert");
        //            var formDigest = await SPapi.GetFormDigest();

        //            var recall = await PSapi.RecallTimesheet(PeriodList[SelectedIndex].Id, formDigest.D.GetContextWebInformation.FormDigestValue);

        //            if (recall)
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "Successfully recalled timesheet", "Close" }, "DisplayAlert");
        //                ExecuteRefreshLinesCommand();
        //                GetTimesheetStatus();
        //            }
        //            else
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error recalling the timesheet. Please try again", "Close" }, "DisplayAlert");
        //            }
        //        }
        //        else
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "The device is not connected to the internet", "Close" }, "DisplayAlert");
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("ExecuteRecallTimesheet", e.Message);
        //        MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error recalling the timesheet. Please try again", "Close" }, "DisplayAlert");
        //    }
        //}

        //private async void ExecuteSubmitTimesheet(string comment)
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Submitting timesheet...","Close"}, "DisplayAlert");
        //            var formDigest = await SPapi.GetFormDigest();

        //            var submit = await PSapi.SubmitTimesheet(PeriodList[SelectedIndex].Id, comment, formDigest.D.GetContextWebInformation.FormDigestValue);

        //            if (submit)
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "Successfully submitted the timesheet", "Close" }, "DisplayAlert");
        //                ExecuteRefreshLinesCommand();
        //                GetTimesheetStatus();
        //            }
        //            else
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "There was a problem submitting the timesheet. Please try again", "Close" }, "DisplayAlert");
        //            }
        //        }
        //        else
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Your device is not connected to the internet", "Close" }, "DisplayAlert");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("ExecuteSubmitTimesheet", e.Message);
        //        MessagingCenter.Instance.Send<String[]>(new string[] { "There was a problem submitting the timesheet. Please try again", "Close" }, "DisplayAlert");
        //    }
        //}

        //private async void CreateTimesheet(string periodId)
        //{
        //    if (IsConnectedToInternet())
        //    {
        //        var formDigest = await SPapi.GetFormDigest();

        //        if (await PSapi.CreateTimesheet(periodId, formDigest.D.GetContextWebInformation.FormDigestValue))
        //        {
        //            ExecuteRefreshLinesCommand();
        //        }
        //        else
        //        {
        //            string[] alertStrings = { "There was an error creating the timesheet. Please try again", "Close"};
        //            MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
        //        }
        //    }
        //    else
        //    {
        //        string[] alertStrings = { "The device is not connected to the internet", "Close" };
        //        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
        //    }
        //}

        //private void ExecuteSelectedItemChangedCommand()
        //{
        //    var periodId = PeriodList[SelectedIndex].Id;
        //    _savedLines = realm.All<SavedLinesModel>().Where(p => p.PeriodId == periodId);

        //    GetTimesheetStatus();

        //    PeriodLines.Clear();
        //    if (IsConnectedToInternet())
        //    {
        //        ExecuteRefreshLinesCommand();
        //    }
        //    else
        //    {
        //        foreach (var item in _savedLines.ToList())
        //        {
        //            PeriodLines.Add(item.LineModel);
        //        }
        //    }

        //    MessagingCenter.Instance.Send<String>(PeriodList[SelectedIndex].ToString(), "TimesheetPeriod");
        //}

        //private async void GetTimesheetStatus()
        //{
        //    try
        //    {
        //        var timesheetModel = await PSapi.GetTimesheet(PeriodList[SelectedIndex].Id);
        //        MessagingCenter.Instance.Send<String>(timesheetModel.D.Status.ToString(), "TimesheetStatus");
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("GetTimesheetStatus", e.Message);
        //    }
        //}

        //private async void SyncTimesheetLines(List<SavedLinesModel> savedLines)
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            var periodLines = await PSapi.GetTimesheetLinesByPeriod(PeriodList[SelectedIndex].Id);
        //            syncDataService.SyncTimesheetLines(savedLines, periodLines.D.Results, PeriodLines, PeriodList[SelectedIndex].Id);
        //            IsRefreshing = false;
        //        }
        //        else
        //        {
        //            IsRefreshing = false;
        //            string[] alertStrings = { "Your device is not connected to the internet", "Close" };
        //            MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("SyncTimesheetLines", e.Message);
        //        IsRefreshing = false;
        //    }
        //}
    }
}
