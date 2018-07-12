using Android.App;
using Android.OS;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Support.V7.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Xamarin.Forms;
using System;
using ProjectOnlineMobile2.Pages;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Content;
using ProjectOnlineMobile2.Models.TLL;
using Newtonsoft.Json;
using Android.Content.PM;
using Android.Views.InputMethods;
using System.Threading.Tasks;

namespace ProjectOnlineMobile2.Droid
{
    [Activity(Label = "@string/app_name",
              MainLauncher = false,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {

        BottomNavigationView bottomNavigation;
        IMenu menu;
        Toolbar toolbar;
        DialogHelper dialogHelper;

        Fragment _homepageFragment, _projectsFragment, _tasksFragment, _timesheetFragment, _timesheetWorkFragment;

        public string UserName, UserEmail, TimesheetPeriod, TimesheetLineComment, TimesheetStatus;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);

            Forms.Init(this, bundle);

            MessagingCenter.Instance.Subscribe<string[]>(this, "DisplayAlert", (s) =>
            {
                DisplayAlert(s);
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models2.UserModel>(this, "UserInfo", (userInfo)=> {
                UserName = userInfo.UserName;
                UserEmail = userInfo.UserEmail;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetPeriod", (tsp) => {
                TimesheetPeriod = tsp;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetStatus", (status) => {
                SetTimesheetStatus(status);
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models2.LineModel.LineModel>(this, "PushTimesheetWorkPage", (timesheetLine) => {
                PushTimesheetWorkPage(timesheetLine);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "AddTimesheetLineDialog", (s)=> {
                dialogHelper.DisplayAddTimesheetLineDialog();
            });

            MessagingCenter.Instance.Subscribe<String>(this,"ExitWorkPage",(s)=> {
                exitWorkPage();
            });

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);
                toolbar.NavigationClick += (sender,e) => { exitWorkPage(); };
            }

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            _homepageFragment = new HomePage().CreateSupportFragment(this);
            _timesheetWorkFragment = new TimesheetWorkPage().CreateSupportFragment(this);
            _projectsFragment = new ProjectPage().CreateSupportFragment(this);
            _tasksFragment = new TasksPage().CreateSupportFragment(this);
            _timesheetFragment = new TimesheetPage().CreateSupportFragment(this);

            dialogHelper = new DialogHelper(this);

            LoadFragment(Resource.Id.menu_projects);
        }

        private void SetTimesheetStatus(string status)
        {
            if (status.Equals("1"))
            {
                TimesheetStatus = "In Progress";
            }
            else if (status.Equals("2"))
            {
                TimesheetStatus = "Submitted";
            }
            else if (status.Equals("3"))
            {
                TimesheetStatus = "Not Yet Created";
            }
            else if (status.Equals("4"))
            {
                TimesheetStatus = "Approved";
            }
        }

        private void DisplayAlert(string[] parameters)
        {
            //s[0] = message
            //s[1] = affirm button message
            //s[2] = cancel button message
            //s[3] = identifier
            //s[4] = period id

            if (parameters.Length > 2)
            {
                if (!string.IsNullOrEmpty(parameters[3]))
                {
                    if (parameters[3].Equals("CreateTimesheet"))
                    {
                        dialogHelper.DisplayCreateTimesheetDialog(parameters[0], parameters[4], parameters[1], parameters[2]);
                    }
                }
            }
            else
            {
                Toast.MakeText(this, parameters[0], ToastLength.Short).Show();
            }
        }

        private void exitWorkPage()
        {
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            toolbar.Title = "Timesheet Tracker";
            bottomNavigation.Visibility = ViewStates.Visible;

            InputMethodManager imm = InputMethodManager.FromContext(this.ApplicationContext);
            imm.HideSoftInputFromInputMethod(this.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);

            LoadFragment(Resource.Id.menu_timesheets);
        }

        private void PushTimesheetWorkPage(ProjectOnlineMobile2.Models2.LineModel.LineModel timesheetLine)
        {
            TimesheetLineComment = timesheetLine.Comment;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            toolbar.Title = timesheetLine.TaskName;
            bottomNavigation.Visibility = ViewStates.Gone;

            if (menu != null)
            {
                menu.Clear();
                MenuInflater.Inflate(Resource.Menu.work_page_menu, menu);
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, _timesheetWorkFragment)
                .Commit();

        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        Fragment fragment = null;
        void LoadFragment(int id)
        {
            
            if(id == Resource.Id.menu_projects)
            {
                fragment = _projectsFragment;

                if(menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.projects_menu, menu);
                }
            }
            else if(id == Resource.Id.menu_tasks)
            {
                fragment = _tasksFragment;

                if (menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.tasks_menu, menu);
                }
            }
            else if(id == Resource.Id.menu_timesheets)
            {
                fragment = _timesheetFragment;

                if (menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.timesheet_menu, menu);
                }
            }
            
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.projects_menu, menu);
            this.menu = menu;
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            if(item.ItemId == Resource.Id.menu_userinfo)
            {
                dialogHelper.DisplayUserInfo(UserName, UserEmail);
            }
            else if(item.ItemId == Resource.Id.menu_period_details)
            {
                dialogHelper.DisplayPeriodDetails(TimesheetPeriod, TimesheetStatus);
            }
            else if(item.ItemId == Resource.Id.menu_submit_timesheet)
            {
                dialogHelper.DisplaySubmitTimesheetDialog();
            }
            else if(item.ItemId == Resource.Id.menu_recall_timesheet)
            {
                MessagingCenter.Instance.Send<String>("", "RecallTimesheet");
            }
            else if(item.ItemId == Resource.Id.menu_add_line)
            {
                MessagingCenter.Instance.Send<String>("", "OpenProjectPicker");
            }
            else if(item.ItemId == Resource.Id.menu_all_tasks)
            {
                MessagingCenter.Instance.Send<String>(GetString(Resource.String.menu_all_tasks), "SortTasks");
            }
            else if(item.ItemId == Resource.Id.menu_inprogress_tasks)
            {
                MessagingCenter.Instance.Send<String>(GetString(Resource.String.menu_inprogress), "SortTasks");
            }
            else if(item.ItemId == Resource.Id.menu_completed_tasks)
            {
                MessagingCenter.Instance.Send<String>(GetString(Resource.String.menu_completed), "SortTasks");
            }
            else if (item.ItemId == Resource.Id.menu_save)
            {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }
            else if (item.ItemId == Resource.Id.menu_edit)
            {
                dialogHelper.DisplayUpdateLineDialog(TimesheetLineComment);
            }
            else if (item.ItemId == Resource.Id.menu_delete)
            {
                dialogHelper.DisplayConfirmationDialog("Do you really want to delete this line?", "Delete", "Cancel");
            }
            else if (item.ItemId == Resource.Id.menu_send_progress)
            {
                dialogHelper.DisplaySendProgressDialog();
            }


            return true;
        }

        private async void doDummyFunc()
        {
            
            try
            {

                const string PROJECT_LIST_GUID = "c04edc6b-c06e-479c-a11c-41f5aef38d16";
                //const string TIMESHEET_PERIODS_ID = "d2c6bb8d-490e-4d27-a9cf-bed646b7d28a";

                var core = new SpevoCore.Services.SharepointApiWrapper();

                //var body = "{'__metadata':{'type':'SP.Data.TimesheetPeriodsListItem'},'Title':'" + "1" + "'," +
                //           "'Period_x0020_Id':'" + Guid.NewGuid().ToString() + "'," +
                //           "'Start':'" + DateTime.Now.Date + "'," +
                //           "'End':'" + DateTime.Now.AddDays(1).Date + "'}";

                //var contents = new System.Net.Http.StringContent(body);
                //contents.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                //var formDigest = await core.GetFormDigest();

                //var getList = await core.GetListByTitle("Projects");
                //string query = "projectName,projectDescription,ProjectStartDate,projectFinishDate,projectDuration,projectPercentComplete,projectWork,projectActualWork,projectRemainingWork,projectStatus,projectOwnerName/Name&$expand=projectOwnerName/Name";
                string query = "items?$select=projectName,projectDescription,ProjectStartDate,projectFinishDate,projectDuration,projectPercentComplete,projectWork,projectActualWork,projectRemainingWork,projectStatus,projectOwnerName/Name&$expand=projectOwnerName/Name";
                var getListItems = await core.GetListItemsByListGuid(PROJECT_LIST_GUID,query);
                System.Diagnostics.Debug.WriteLine("",getListItems.RequestMessage.RequestUri.ToString());
                //var add = await core.AddListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue, TIMESHEET_PERIODS_ID, contents);
                ////var updated = await core.UpdateListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue, PROJECT_LIST_GUID, contents, "4");
                ////var deleted = await core.DeleteListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue, PROJECT_LIST_GUID, "4");

                if (getListItems.IsSuccessStatusCode)
                    System.Diagnostics.Debug.WriteLine("doDummyFunc", await getListItems.Content.ReadAsStringAsync());
                else
                    System.Diagnostics.Debug.WriteLine("doDummyFunc", getListItems.StatusCode + " ang status code");

                //DateTime date = new DateTime(2018, 12, 23, 0, 0, 0);
                //DateTime end = new DateTime(2018, 12, 31, 23, 59, 59);
                //int weekNumber = 52;
                //var formDigest = await core.GetFormDigest();
                //while (DateTime.Compare(date, end) <= 0)
                //{
                //    if (weekNumber < 52)
                //    {
                //        //System.Diagnostics.Debug.WriteLine("start of week " + weekNumber.ToString(), date.ToLongDateString() + " " + date.ToLongTimeString());
                //        var startDate = date;

                //        if (date.DayOfWeek.ToString().Equals("Sunday"))
                //            date = date.AddDays(6);
                //        else if (date.DayOfWeek.ToString().Equals("Monday"))
                //            date = date.AddDays(5);
                //        else if (date.DayOfWeek.ToString().Equals("Tuesday"))
                //            date = date.AddDays(4);
                //        else if (date.DayOfWeek.ToString().Equals("Wednesday"))
                //            date = date.AddDays(3);
                //        else if (date.DayOfWeek.ToString().Equals("Thursday"))
                //            date = date.AddDays(2);
                //        else if (date.DayOfWeek.ToString().Equals("Friday"))
                //            date = date.AddDays(1);

                //        date = date.AddHours(23);
                //        date = date.AddMinutes(59);
                //        date = date.AddSeconds(59);
                //        //System.Diagnostics.Debug.WriteLine("end of week " + weekNumber.ToString(), date.ToLongDateString() + " " + date.ToLongTimeString());
                //        var endDate = date;

                //        var body = "{'__metadata':{'type':'SP.Data.TimesheetPeriodsListItem'},'Title':'" + weekNumber.ToString() + "'," +
                //                   "'Period_x0020_Id':'" + Guid.NewGuid().ToString() + "'," +
                //                   "'Start':'" + startDate + "'," +
                //                   "'End':'" + endDate + "'}";

                //        var contents = new System.Net.Http.StringContent(body);
                //        contents.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                //        var add = await core.AddListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue, TIMESHEET_PERIODS_ID, contents);

                //        if (add.IsSuccessStatusCode)
                //            System.Diagnostics.Debug.WriteLine("doDummyFunc", "success ang week " + weekNumber.ToString());
                //        else
                //            System.Diagnostics.Debug.WriteLine("doDummyFunc", "failed ang week " + weekNumber.ToString() + " :" + add.StatusCode);

                //        date = date.AddSeconds(1);
                //        weekNumber += 1;
                //    }
                //    else
                //    {
                //        var difference = end.Date - date.Date;
                //        int days = (int)difference.TotalDays;
                //        //System.Diagnostics.Debug.WriteLine("start of week " + weekNumber.ToString(), date.ToLongDateString() + " " + date.ToLongTimeString());
                //        var startDate = date;
                //        date = date.AddDays(days);
                //        date = date.AddHours(23);
                //        date = date.AddMinutes(59);
                //        date = date.AddSeconds(59);
                //        //System.Diagnostics.Debug.WriteLine("end of week " + weekNumber.ToString(), date.ToLongDateString() + " " + date.ToLongTimeString());
                //        var endDate = date;

                //        var body = "{'__metadata':{'type':'SP.Data.TimesheetPeriodsListItem'},'Title':'" + weekNumber.ToString() + "'," +
                //                   "'Period_x0020_Id':'" + Guid.NewGuid().ToString() + "'," +
                //                   "'Start':'" + startDate + "'," +
                //                   "'End':'" + endDate + "'}";

                //        var contents = new System.Net.Http.StringContent(body);
                //        contents.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                //        var add = await core.AddListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue, TIMESHEET_PERIODS_ID, contents);

                //        if (add.IsSuccessStatusCode)
                //            System.Diagnostics.Debug.WriteLine("doDummyFunc", "success ang week " + weekNumber.ToString());
                //        else
                //            System.Diagnostics.Debug.WriteLine("doDummyFunc", "failed ang week " + weekNumber.ToString() + " :" + add.StatusCode);

                //        break;
                //    }
                //}
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("doDummyFunc", e.Message);
            }
            
        }
    }
}

