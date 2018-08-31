using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ProjectOnlineMobile2.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Fragment = Android.Support.V4.App.Fragment;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace ProjectOnlineMobile2.Droid
{
    [Activity(Label = "@string/app_name",
              MainLauncher = false,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {
        private BottomNavigationView bottomNavigation;
        private IMenu menu;
        private Toolbar toolbar;
        private DialogHelper dialogHelper;

        private Fragment _homepageFragment, _projectsFragment, _tasksFragment, _timesheetFragment, _timesheetWorkFragment;
        private Fragment _addProjectFragment, _addTaskFragment, _editTaskFragment, _projectInfoFragment;

        public string UserName, UserEmail, TimesheetPeriod, TimesheetLineComment, TimesheetStatus;
        private int _pastFragmentId = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);

            Forms.Init(this, bundle);

            MessagingCenter.Instance.Subscribe<string[]>(this, "DisplayAlert", (s) =>
            {
                DisplayAlert(s);
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models2.UserModel>(this, "UserInfo", (userInfo) =>
            {
                UserName = userInfo.UserName;
                UserEmail = userInfo.UserEmail;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetPeriod", (tsp) =>
            {
                TimesheetPeriod = tsp;
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetStatus", (status) =>
            {
                SetTimesheetStatus(status);
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models2.LineModel.LineModel>(this, "PushTimesheetWorkPage", (timesheetLine) =>
            {
                PushTimesheetWorkPage(timesheetLine);
            });

            MessagingCenter.Instance.Subscribe<string>(this, "ShowProjectDetails", (projectName) =>
            {
                ShowProjectDetails(projectName);
            });

            MessagingCenter.Instance.Subscribe<string>(this, "ShowEditTaskPage", (s) =>
            {
                PushOtherPages(Resource.Id.menu_tasks, "Edit task", Resource.Menu.edit_task_menu, _editTaskFragment);
            });

            MessagingCenter.Instance.Subscribe<ProjectOnlineMobile2.Models2.LineModel.LineModel>(this, "EditComment", (line) =>
            {
                dialogHelper.DisplayUpdateLineDialog(line.Comment, line.ID.ToString());
            });

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);
                toolbar.NavigationClick += (sender, e) => { GoBack(); };
            }

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            _homepageFragment = new HomePage().CreateSupportFragment(this);
            _timesheetWorkFragment = new TimesheetWorkPage().CreateSupportFragment(this);
            _projectsFragment = new ProjectPage().CreateSupportFragment(this);
            _tasksFragment = new TasksPage().CreateSupportFragment(this);
            _timesheetFragment = new TimesheetPage().CreateSupportFragment(this);

            _addProjectFragment = new AddProjectPage().CreateSupportFragment(this);
            _addTaskFragment = new AddTaskPage().CreateSupportFragment(this);
            _editTaskFragment = new EditTaskPage().CreateSupportFragment(this);
            _projectInfoFragment = new ProjectInfoPage().CreateSupportFragment(this);

            dialogHelper = new DialogHelper(this);

            LoadFragment(Resource.Id.menu_projects);
        }

        private void ShowProjectDetails(string projectName)
        {
            _pastFragmentId = Resource.Id.menu_projects;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            toolbar.Title = projectName;
            bottomNavigation.Visibility = ViewStates.Gone;

            menu.Clear();

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, _projectInfoFragment)
                .Commit();
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

            Toast.MakeText(this, parameters[0], ToastLength.Short).Show();
        }

        private void GoBack()
        {
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            toolbar.Title = "Timesheet Tracker";
            bottomNavigation.Visibility = ViewStates.Visible;

            InputMethodManager imm = InputMethodManager.FromContext(this.ApplicationContext);
            imm.HideSoftInputFromInputMethod(this.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);

            LoadFragment(_pastFragmentId);
        }

        private void PushTimesheetWorkPage(ProjectOnlineMobile2.Models2.LineModel.LineModel timesheetLine)
        {
            _pastFragmentId = Resource.Id.menu_timesheets;

            TimesheetLineComment = timesheetLine.Comment;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            toolbar.Title = timesheetLine.Task;
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

        private void PushOtherPages(int pastFragmentId, string toolbarTitle, int pageMenu, Fragment fragment)
        {
            _pastFragmentId = pastFragmentId;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            toolbar.Title = toolbarTitle;
            bottomNavigation.Visibility = ViewStates.Gone;

            if (menu != null)
            {
                menu.Clear();
                MenuInflater.Inflate(pageMenu, menu);
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        private Fragment fragment = null;

        private void LoadFragment(int id)
        {
            if (id == Resource.Id.menu_projects)
            {
                fragment = _projectsFragment;

                if (menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.projects_menu, menu);
                }
            }
            else if (id == Resource.Id.menu_tasks)
            {
                fragment = _tasksFragment;

                if (menu != null)
                {
                    menu.Clear();
                    MenuInflater.Inflate(Resource.Menu.tasks_menu, menu);
                }
            }
            else if (id == Resource.Id.menu_timesheets)
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
            if (item.ItemId == Resource.Id.menu_userinfo)
            {
                dialogHelper.DisplayUserInfo(UserName, UserEmail);
            }
            else if (item.ItemId == Resource.Id.menu_period_details)
            {
                dialogHelper.DisplayPeriodDetails(TimesheetPeriod, TimesheetStatus);
            }
            else if (item.ItemId == Resource.Id.menu_submit_timesheet)
            {
                dialogHelper.DisplaySubmitTimesheetDialog();
            }
            else if (item.ItemId == Resource.Id.menu_recall_timesheet)
            {
                MessagingCenter.Instance.Send<String>("", "RecallTimesheet");
            }
            else if (item.ItemId == Resource.Id.menu_createproject)
            {
                PushOtherPages(Resource.Id.menu_projects, "Create Project", Resource.Menu.add_project_menu, _addProjectFragment);
            }
            else if (item.ItemId == Resource.Id.menu_save_project)
            {
                MessagingCenter.Instance.Send<string>(string.Empty, "SaveProject");
            }
            else if (item.ItemId == Resource.Id.menu_createtask)
            {
                PushOtherPages(Resource.Id.menu_tasks, "Create Task", Resource.Menu.add_task_menu, _addTaskFragment);
            }
            else if (item.ItemId == Resource.Id.menu_save_task)
            {
                MessagingCenter.Instance.Send<string>(string.Empty, "SaveTask");
            }
            else if (item.ItemId == Resource.Id.menu_edit_task)
            {
                MessagingCenter.Instance.Send<string>(string.Empty, "SaveEditedTask");
            }
            else if (item.ItemId == Resource.Id.menu_save)
            {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }

            return true;
        }
    }
}