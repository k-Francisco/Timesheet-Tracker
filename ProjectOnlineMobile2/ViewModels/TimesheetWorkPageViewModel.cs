using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using ProjectOnlineMobile2.Models;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using WorkRoot = ProjectOnlineMobile2.Models2.LineWorkModel.RootObject;
using LineWorkModel = ProjectOnlineMobile2.Models2.LineWorkModel.LineWorkModel;
using System.Net.Http;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TimesheetWorkPageViewModel : BaseViewModel
    {
        private ObservableCollection<LineWorkModel> _lineWorkList = new ObservableCollection<LineWorkModel>(); 
        public ObservableCollection<LineWorkModel> LineWorkList
        {
            get { return _lineWorkList; }
            set { SetProperty(ref _lineWorkList, value); }
        }

        private bool _headerVisibility = false;
        public bool HeaderVisibility
        {
            get { return _headerVisibility; }
            set { SetProperty(ref _headerVisibility, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        const string TIMESHEETWORK_LIST_GUID = "5aeee3e3-758e-4aab-a5d6-3f2b877e394c";
        int _periodId, _lineId;
        private List<int> _completeLineIds { get; set; }

        public ICommand RefreshLineWork { get { return new Command(SyncTimesheetLineWork); } }

        public TimesheetWorkPageViewModel()
        {
            MessagingCenter.Instance.Subscribe<string>(this, "SendPeriodIdToWorkPage", (periodId)=> {
                _periodId = Convert.ToInt32(periodId);
            });

            MessagingCenter.Instance.Subscribe<string>(this, "SendCurrentLineIdToWorkPage", (lineid) => {
                _lineId = Convert.ToInt32(lineid);
            });

            MessagingCenter.Instance.Subscribe<List<int>>(this, "SendLineIdsToWorkPage", (lineIds) => {
                _completeLineIds = lineIds;
            });

            //MessagingCenter.Instance.Subscribe<String>(this, "SaveOfflineWorkChanges", (s)=> {
            //    SaveOfflineWorkChanges();
            //});

            MessagingCenter.Instance.Subscribe<String>(this, "SaveTimesheetWorkChanges", (s) =>
            {
                ExecuteSaveTimesheetWorkChanges();
            });

            //MessagingCenter.Instance.Subscribe<String>(this, "DeleteTimesheetLine", (s)=> {
            //    ExecuteDeleteTimesheetLine();
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "UpdateTimesheetLine", (comment) => {
            //    ExecuteUpdateTimesheetLine(comment);
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "SendProgress", (comment) => {
            //    ExecuteSendProgress(comment);
            //});

        }

        public void LoadWorkFromDatabase()
        {
            HeaderVisibility = false;
            var localWorkModel = realm.All<LineWorkModel>()
                .Where(p=> p.TimesheetPeriodId == _periodId && p.TimesheetLineId == _lineId)
                .ToList();

            foreach (var item in localWorkModel)
            {
                LineWorkList.Add(item);
            }

            if(localWorkModel.Any())
                HeaderVisibility = true;
        }

        public async void SyncTimesheetLineWork()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    IsRefreshing = true;

                    var query = "$select=WorkDate," +
                        "ActualWork," +
                        "PlannedWork," +
                        "TimesheetLineId," +
                        "TimesheetPeriodId," +
                        "ID" +
                        "&$filter=";

                    StringBuilder sb = new StringBuilder(query);

                    foreach (var item in _completeLineIds)
                    {
                        sb.Append("(TimesheetLineId eq " + item.ToString() +") or ");
                    }
                    //remove the last or in the query
                    sb.Remove((sb.Length - 4), 4);

                    var apiResponse = await SPapi.GetListItemsByListGuid(TIMESHEETWORK_LIST_GUID, sb.ToString());
                    
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var localLineWorkModels = realm.All<LineWorkModel>().ToList();
                        var workList = JsonConvert.DeserializeObject<WorkRoot>(await apiResponse.Content.ReadAsStringAsync());

                        syncDataService.SyncTimesheetLineWork(localLineWorkModels, workList.D.Results, LineWorkList, _periodId, _lineId);
                    }
                    else
                    {
                        Debug.WriteLine("SyncTimesheetLineWork", apiResponse.StatusCode.ToString());
                    }

                    IsRefreshing = false;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncTimesheetLineWork", e.Message);
            }
        }

        public void OnExitPage()
        {
            var localWorkModel = realm.All<LineWorkModel>()
                .Where(p => p.TimesheetPeriodId == _periodId && p.TimesheetLineId == _lineId)
                .ToList();

            foreach (var item in localWorkModel)
            {
                if (!item.isNotSaved)
                {
                    realm.Write(() =>
                    {
                        item.EntryTextActualHours = string.Empty;
                    });
                }
            }

            LineWorkList.Clear();

            HeaderVisibility = false;
        }

        private async void ExecuteSaveTimesheetWorkChanges()
        {
            var localWorkModel = realm.All<LineWorkModel>()
                    .Where(p => p.TimesheetPeriodId == _periodId && p.TimesheetLineId == _lineId)
                    .ToList();

            if (IsConnectedToInternet())
                MessagingCenter.Instance.Send<String[]>(new string[] { "Saving progress...", "Close" }, "DisplayAlert");
            else
                MessagingCenter.Instance.Send<String[]>(new string[] { "The changes that were made will be uploaded when the device is connected to the internet", "Close" }, "DisplayAlert");


            foreach (var item in localWorkModel)
            {
                try
                {
                    if (IsConnectedToInternet())
                    {
                        var formDigest = await SPapi.GetFormDigest();

                        if (!string.IsNullOrWhiteSpace(item.EntryTextActualHours))
                        {
                            string actualHours;

                            if (string.IsNullOrWhiteSpace(item.EntryTextActualHours))
                                actualHours = item.ActualWork.ToString();
                            else
                                actualHours = item.EntryTextActualHours;

                            var body = ConstructBody(actualHours);

                            var apiResponse = await SPapi.UpdateListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue,
                                TIMESHEETWORK_LIST_GUID, body, item.ID.ToString());

                            if (apiResponse.IsSuccessStatusCode)
                            {
                                realm.Write(() => {
                                    item.ActualWork = Convert.ToInt32(actualHours);
                                    item.EntryTextActualHours = "";
                                    item.isNotSaved = false;
                                });
                            }
                            else
                            {
                                realm.Write(() => {
                                    item.isNotSaved = true;
                                });
                            }


                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(item.EntryTextActualHours))
                        {
                            realm.Write(() => {
                                item.isNotSaved = true;
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error saving the timesheet. Please try again later", "Close" }, "DisplayAlert");
                    Debug.WriteLine("ExecuteSaveTimesheetWorkChanges", e.Message);

                    realm.Write(() => {
                        item.isNotSaved = true;
                    });
                }
            }
        }

        private StringContent ConstructBody(string actualHours)
        {
            var body = "{'__metadata':{'type':'SP.Data.TimesheetLineWorkListListItem'}," +
                "'ActualWork':'" + actualHours +"'}";

            var contents = new StringContent(body);
            contents.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

            return contents;
        }

        //private async void ExecuteSendProgress(string comment)
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Sending progress...", "Close" }, "DisplayAlert");
        //            var formDigest = await SPapi.GetFormDigest();

        //            var response = await PSapi.SubmitTimesheetLineProgress(comment, _periodId, _lineId, formDigest.D.GetContextWebInformation.FormDigestValue);

        //            if (response)
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "Successfully sent the progress","Close" }, "DisplayAlert");
        //            }
        //            else
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "There was a problem sending the progress. Please try again", "Close" }, "DisplayAlert");
        //            }
        //        }
        //        else
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Your device is not connected to the internet", "Close" }, "DisplayAlert");
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("ExecuteSendProgress", e.Message);
        //        MessagingCenter.Instance.Send<String[]>(new string[] { "There was a problem sending the progress. Please try again", "Close" }, "DisplayAlert");
        //    }
        //}

        //private void ExecuteWorkPagePushed()
        //{
        //    try
        //    {
        //        savedLineWork = realm.All<SavedTimesheetLineWork>()
        //       .Where(p => p.PeriodId == _periodId && p.LineId == _lineId)
        //       .ToList()
        //       .OrderBy(p => p.WorkModel.Start.DateTime);

        //        foreach (var item in savedLineWork)
        //        {
        //            LineWork.Add(item.WorkModel);
        //        }

        //        if (savedLineWork.Any())
        //            HeaderVisibility = true;

        //        SyncTimesheetLineWork();
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("WorkPagePushed", e.Message);
        //    }
        //}

        //private async void ExecuteUpdateTimesheetLine(string comment)
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Updating timesheet line...", "Close" }, "DisplayAlert");

        //            string body = "{ \"__metadata\":{ \"type\":\"PS.TimeSheetLine\"}, " +
        //                "'Comment':'" + comment + "'}";

        //            var formDigest = await SPapi.GetFormDigest();

        //            var updateLine = await PSapi.UpdateTimesheetLine(body, _lineId, _periodId, formDigest.D.GetContextWebInformation.FormDigestValue);
        //            if (updateLine)
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "Successfully updated line", "Close" }, "DisplayAlert");
        //                MessagingCenter.Instance.Send<String>("", "RefreshTimesheetLines");
        //                MessagingCenter.Instance.Send<String>("", "ExitWorkPage");
        //            }
        //            else
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error adding the line. Please try again", "Close" }, "DisplayAlert");
        //            }
        //        }
        //        else
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Your device is not connected to the internet", "Close" }, "DisplayAlert");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("ExecuteUpdateTimesheetLine", e.Message);
        //        MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error adding the line. Please try again", "Close" }, "DisplayAlert");
        //    }
        //}

        //private async void ExecuteDeleteTimesheetLine()
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Deleting timesheet line...", "Close" }, "DisplayAlert");

        //            var formDigest = await SPapi.GetFormDigest();

        //            var delete = await PSapi.DeleteTimesheetLine(_lineId, _periodId, formDigest.D.GetContextWebInformation.FormDigestValue);
        //            if (delete)
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "Successfully deleted the line", "Close" }, "DisplayAlert");
        //                MessagingCenter.Instance.Send<String>("", "RefreshTimesheetLines");
        //                MessagingCenter.Instance.Send<String>("", "ExitWorkPage");
        //            }
        //            else
        //            {
        //                MessagingCenter.Instance.Send<String[]>(new string[] { "There was an error deleting the line", "Close" }, "DisplayAlert");
        //            }
        //        }
        //        else
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Your device is not connected to the internet", "Close" }, "DisplayAlert");
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("ExecuteDeleteTimesheetLine", e.Message);
        //    }
        //}

        //private async void SaveOfflineWorkChanges()
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            var formDigest = await SPapi.GetFormDigest();

        //            var allSavedLineWork = realm.All<SavedTimesheetLineWork>()
        //                .ToList();

        //            foreach (var item in allSavedLineWork)
        //            {
        //                if(item.WorkModel.isNotSaved == true)
        //                {
        //                    string actualHours, plannedHours;

        //                    if (string.IsNullOrWhiteSpace(item.WorkModel.EntryTextActualHours))
        //                        actualHours = item.WorkModel.ActualWork;
        //                    else
        //                        actualHours = item.WorkModel.EntryTextActualHours + "h";

        //                    if (string.IsNullOrWhiteSpace(item.WorkModel.EntryTextPlannedHours))
        //                        plannedHours = item.WorkModel.PlannedWork;
        //                    else
        //                        plannedHours = item.WorkModel.EntryTextPlannedHours + "h";

        //                    var body = "{'parameters':{'ActualWork':'" + actualHours + "', " +
        //                    "'PlannedWork':'" + plannedHours + "', " +
        //                    "'Start':'" + string.Format("{0:MM/dd/yyyy hh:mm:ss tt}", item.WorkModel.Start.DateTime) + "', " +
        //                    "'NonBillableOvertimeWork':'0h', " +
        //                    "'NonBillableWork':'0h', " +
        //                    "'OvertimeWork':'0h'}}";

        //                    var response = await PSapi.AddTimesheetLineWork(item.PeriodId, item.LineId, body, formDigest.D.GetContextWebInformation.FormDigestValue);

        //                    if (response)
        //                    {
        //                        realm.Write(() => {
        //                            item.WorkModel.ActualWork = actualHours;
        //                            item.WorkModel.PlannedWork = plannedHours;
        //                            item.WorkModel.EntryTextActualHours = "";
        //                            item.WorkModel.EntryTextPlannedHours = "";
        //                            item.WorkModel.isNotSaved = false;
        //                        });
        //                    }
        //                }
        //            }

        //            realm.Refresh();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("SaveOfflineWorkChanges", e.Message);
        //    }
        //}
    }
}
