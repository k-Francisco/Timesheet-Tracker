using System;
using LineWorkResult = ProjectOnlineMobile2.Models.TLWM.WorkResult;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using ProjectOnlineMobile2.Models;
using System.Linq;
using ProjectOnlineMobile2.Models.TSPL;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TimesheetWorkPageViewModel : BaseViewModel
    {
        private ObservableCollection<LineWorkResult> _lineWork; 
        public ObservableCollection<LineWorkResult> LineWork
        {
            get { return _lineWork; }
            set { SetProperty(ref _lineWork, value); }
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

        private string _periodId { get; set; }
        private string _lineId { get; set; }

        public IOrderedEnumerable<SavedTimesheetLineWork> savedLineWork { get; set; }

        public ICommand RefreshLineWork { get; set; }

        public TimesheetWorkPageViewModel()
        {
            //LineWork = new ObservableCollection<LineWorkResult>();

            //RefreshLineWork = new Command(ExecuteRefreshLineWork);

            //MessagingCenter.Instance.Subscribe<String>(this, "SaveOfflineWorkChanges", (s)=> {
            //    SaveOfflineWorkChanges();
            //});

            //MessagingCenter.Instance.Subscribe<String[]>(this, "TimesheetWork", (ids) =>
            //{
            //    _periodId = ids[0];
            //    _lineId = ids[1];
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "WorkPagePushed", (s)=> {
            //    ExecuteWorkPagePushed();
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "SaveTimesheetWorkChanges", (s) =>
            //{
            //    ExecuteSaveTimesheetWorkChanges();
            //});

            //MessagingCenter.Instance.Subscribe<String>(this, "ClearEntries", (s) =>
            //{
            //    ExecuteClearEntries();
            //});

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

        //private void ExecuteClearEntries()
        //{
        //    try
        //    {
        //        foreach (var item in savedLineWork)
        //        {
        //            if (item.WorkModel.isNotSaved != true)
        //            {
        //                realm.Write(() => {
        //                    item.WorkModel.EntryTextActualHours = string.Empty;
        //                    item.WorkModel.EntryTextPlannedHours = string.Empty;
        //                });
        //            }
        //        }

        //        LineWork.Clear();

        //        HeaderVisibility = false;
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine("ExecuteClearEntries", e.Message);
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

        //private void ExecuteRefreshLineWork()
        //{
        //    HeaderVisibility = false;
        //    LineWork.Clear();
        //    IsRefreshing = true;

        //    realm.Write(()=> {
        //        foreach (var item in savedLineWork)
        //        {
        //            realm.Remove(item);
        //        }
        //    });

        //    realm.Refresh();

        //    SyncTimesheetLineWork();
        //}

        //private async void ExecuteSaveTimesheetWorkChanges()
        //{
        //    try
        //    {
        //        if (IsConnectedToInternet())
        //        {
        //            MessagingCenter.Instance.Send<String[]>(new string[] { "Saving progress...", "Close" }, "DisplayAlert");
        //            var formDigest = await SPapi.GetFormDigest();

        //            foreach (var item in savedLineWork)
        //            {
        //                if (!string.IsNullOrWhiteSpace(item.WorkModel.EntryTextActualHours) || !string.IsNullOrWhiteSpace(item.WorkModel.EntryTextPlannedHours))
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

        //                    var response = await PSapi.AddTimesheetLineWork(_periodId, _lineId, body, formDigest.D.GetContextWebInformation.FormDigestValue);

        //                    if (response)
        //                    {
        //                        realm.Write(()=> {
        //                            item.WorkModel.ActualWork = actualHours;
        //                            item.WorkModel.PlannedWork = plannedHours;
        //                            item.WorkModel.EntryTextActualHours = "";
        //                            item.WorkModel.EntryTextPlannedHours = "";
        //                            item.WorkModel.isNotSaved = false;
        //                        });
        //                        MessagingCenter.Instance.Send<String>("", "RefreshTimesheetLines");
        //                    }
        //                    else
        //                    {
        //                        realm.Write(() => {
        //                            item.WorkModel.isNotSaved = true;
        //                        });
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            string[] alertStrings = { "The changes that were made will be saved when the device is connected to the internet", "Close" };
        //            MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");

        //            foreach (var item in savedLineWork)
        //            {
        //                realm.Write(() => {
        //                    if (!string.IsNullOrWhiteSpace(item.WorkModel.EntryTextActualHours) || !string.IsNullOrWhiteSpace(item.WorkModel.EntryTextPlannedHours))
        //                        item.WorkModel.isNotSaved = true;
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("ExecuteSaveTimesheetWorkChanges", e.Message);
        //        string[] alertStrings = { "There was an error saving the timesheet. Please try again later", "Close" };
        //        MessagingCenter.Instance.Send<String[]>(alertStrings, "DisplayAlert");
        //    }
        //}

        //private async void SyncTimesheetLineWork()
        //{
        //    try
        //    {
        //        var savedPeriod = realm.All<TimesheetPeriodResult>()
        //            .Where(p => p.Id.Equals(_periodId))
        //            .FirstOrDefault();

        //        List<LineWorkResult> tempDates = new List<LineWorkResult>();

        //        if (IsConnectedToInternet())
        //        {
        //            IsRefreshing = true;

        //            var workHours = await PSapi.GetTimesheetLineWork(_periodId, _lineId);

        //            if (!savedLineWork.ToList().Any())
        //            {

        //                for (int i = 1; i <= savedPeriod.End.Day - savedPeriod.Start.Day; i++)
        //                {
        //                    tempDates.Add(new LineWorkResult()
        //                    {
        //                        Start = savedPeriod.Start.AddDays(i).DateTime,
        //                        End = savedPeriod.Start.AddDays(i).DateTime,
        //                        ActualWork = "0h",
        //                        PlannedWork = "0h",
        //                    });
        //                }

        //                foreach (var item in workHours.D.Results)
        //                {
        //                    foreach (var item2 in tempDates)
        //                    {
        //                        if (item.Start.DateTime.ToShortDateString().Equals(item2.Start.DateTime.ToShortDateString()))
        //                        {
        //                            item2.ActualWork = item.ActualWork;
        //                            item2.ActualWorkMilliseconds = item.ActualWorkMilliseconds;
        //                            item2.ActualWorkTimeSpan = item.ActualWorkTimeSpan;
        //                            item2.End = item.End;
        //                            item2.ActualWorkMilliseconds = item.ActualWorkMilliseconds;
        //                            item2.ActualWorkTimeSpan = item.ActualWorkTimeSpan;
        //                            item2.Comment = item.Comment;
        //                            item2.Id = item.Id;
        //                            item2.NonBillableOvertimeWork = item.NonBillableOvertimeWork;
        //                            item2.NonBillableOvertimeWorkMilliseconds = item.NonBillableOvertimeWorkMilliseconds;
        //                            item2.NonBillableOvertimeWorkTimeSpan = item.NonBillableOvertimeWorkTimeSpan;
        //                            item2.NonBillableWork = item.NonBillableWork;
        //                            item2.NonBillableWorkMilliseconds = item.NonBillableWorkMilliseconds;
        //                            item2.NonBillableWorkTimeSpan = item.NonBillableWorkTimeSpan;
        //                            item2.OvertimeWork = item.OvertimeWork;
        //                            item2.OvertimeWorkMilliseconds = item.OvertimeWorkMilliseconds;
        //                            item2.OvertimeWorkTimeSpan = item.OvertimeWorkTimeSpan;
        //                            item2.PlannedWork = item.PlannedWork;
        //                            item2.PlannedWorkMilliseconds = item.PlannedWorkMilliseconds;
        //                            item2.PlannedWorkTimeSpan = item.PlannedWorkTimeSpan;
        //                        }
        //                    }
        //                }

        //                foreach (var item in tempDates)
        //                {
        //                    LineWork.Add(item);
        //                    realm.Write(() => {
        //                        realm.Add(new SavedTimesheetLineWork()
        //                        {
        //                            PeriodId = _periodId,
        //                            LineId = _lineId,
        //                            WorkModel = item
        //                        });
        //                    });
        //                    savedLineWork.ToList().Add(new SavedTimesheetLineWork() {
        //                        PeriodId = _periodId,
        //                            LineId = _lineId,
        //                            WorkModel = item
        //                    });
        //                }
        //                HeaderVisibility = true;

        //                savedLineWork = realm.All<SavedTimesheetLineWork>()
        //                               .Where(p => p.PeriodId == _periodId && p.LineId == _lineId)
        //                               .ToList()
        //                               .OrderBy(p => p.WorkModel.Start.DateTime);
        //            }
        //            else
        //            {
        //                syncDataService.SyncTimesheetLineWork(workHours, savedLineWork);
        //                HeaderVisibility = true;
        //            }

        //            IsRefreshing = false;

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("SyncTimesheetLineWorkViewModel", e.Message);
        //        IsRefreshing = false;

        //        MessagingCenter.Instance.Send<String[]>(new string[] { "An error has occured. Please try again", "Close" }, "DisplayAlert");
        //    }
        //}
    }
}
