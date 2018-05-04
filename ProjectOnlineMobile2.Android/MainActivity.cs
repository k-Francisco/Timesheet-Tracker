
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;

using Android.Support.V7.App;
using LineResult = ProjectOnlineMobile2.Models.TLL.Result;
using Fragment = Android.Support.V4.App.Fragment;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Support.Design.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ProjectOnlineMobile2.Android.Fragments;
using Android.App;
using Android.Support.V4.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ProjectOnlineMobile2.Pages;
using Android.Widget;
using ProjectOnlineMobile2.Models;
using System;
using ProjectOnlineMobile2.Models.TLL;
using Android.Content;
using ProjectOnlineMobile2.Android.Activities;
using System.IO;
using ProjectOnlineMobile2.Database;

namespace ProjectOnlineMobile2.Android
{
    [Activity(Label = "")]
    public class MainActivity : AppCompatActivity
    {

        DrawerLayout drawerLayout;
        NavigationView navigationView;
        TextView userEmail, userName;
        private Fragment _projectsPage, _tasksPage, _timesheetPage, _homePage;

        IMenuItem previousItem;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            //set the user's name and email
            userName = navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.tvUserName);
            userEmail = navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.tvUserEmail);


            Forms.Init(this, savedInstanceState);
            _homePage = new HomePage().CreateSupportFragment(this);
            _timesheetPage = new TimesheetPage().CreateSupportFragment(this);
            _projectsPage = new ProjectPage().CreateSupportFragment(this);
            _tasksPage = new TasksPage().CreateSupportFragment(this);

            MessagingCenter.Instance.Subscribe<LineResult>(this, "PushTimesheetWorkPage", (timesheetLine) => {
                PushTimesheetWorkPage(timesheetLine);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "DoCreateTimesheet", (periodId) => {
                DisplayAlertDialog(periodId);
            });

            MessagingCenter.Instance.Subscribe<UserModel>(this, "UserInfo", (user) => {
                GetUserInfo(user);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "Toast", (message) => {
                DisplayWorkChangesToast(message);
            });

            //initialize the db
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"savedChangesDB.db");

            //handle navigation
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                if (previousItem != null)
                    previousItem.SetChecked(false);

                navigationView.SetCheckedItem(e.MenuItem.ItemId);

                previousItem = e.MenuItem;

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_projects:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_tasks:
                        ListItemClicked(1);
                        break;
                    case Resource.Id.nav_timesheets:
                        ListItemClicked(2);
                        break;
                }


                drawerLayout.CloseDrawers();
            };


            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                navigationView.SetCheckedItem(Resource.Id.nav_projects);
                ListItemClicked(0);
            }

        }

        private void DisplayWorkChangesToast(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        private void DisplayAlertDialog(string periodId)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("");
            alert.SetMessage("The timesheet for this period has not been created. Do you want to create this timesheet?");
            alert.SetPositiveButton("Create", (senderAlert, args) => {
                MessagingCenter.Instance.Send<string>(periodId, "CreateTimesheet");
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void PushTimesheetWorkPage(LineResult timesheetLine)
        {
            Intent intent = new Intent(this, typeof(TimesheetWorkActivity));
            intent.PutExtra("TASK_NAME", timesheetLine.TaskName);
            intent.PutExtra("LINE_ID", timesheetLine.Id);
            StartActivity(intent);
        }

        private void GetUserInfo(UserModel user)
        {
            userName.Text = user.D.Title;
            userEmail.Text = user.D.Email;
        }

        int oldPosition = -1;
        private void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == oldPosition)
                return;

            oldPosition = position;

            Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = _projectsPage;
                    SupportActionBar.Title = "Projects";
                    break;
                case 1:
                    fragment = _tasksPage;
                    SupportActionBar.Title = "My Tasks";
                    break;
                case 2:
                    fragment = _timesheetPage;
                    SupportActionBar.Title = "Timesheet";
                    break;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

