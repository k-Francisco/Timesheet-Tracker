using Foundation;
using LineModel = ProjectOnlineMobile2.Models2.LineModel.LineModel;
using ProjectOnlineMobile2.Pages;
using System;
using UIKit;
using Xamarin.Forms;


namespace ProjectOnlineMobile2.iOS
{
    public partial class TabBarController : UITabBarController
    {
        private UIViewController _projectPageController, _tasksPageController, _timesheetPageController, _timesheetWorkPageController;
        private UINavigationController _projectNavController, _tasksNavController, _timesheetNavController;
        private UIAlertView currentAlertView;

        private string TimesheetStatus;

        public TabBarController (IntPtr handle) : base (handle)
        {
            MessagingCenter.Instance.Subscribe<LineModel>(this, "PushTimesheetWorkPage", (line)=> {
                ExecutePushTimesheetWorkPage(line);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "AddTimesheetLineDialog", (s)=> {
                ExecuteAddTimesheetDialog();
            });

            MessagingCenter.Instance.Subscribe<String[]>(this, "DisplayAlert", (s) => {
                DisplayAlert(s);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "ExitWorkPage", (s)=> {
                _timesheetNavController.PopViewController(true);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetStatus", (status) => {
                SetTimesheetStatus(status);
            });
        }

        public override void ViewDidLoad()
        {
            this.TabBar.Translucent = false;
            this.TabBar.TintColor = UIColor.FromRGBA(49,117,47,255);

            UIBarButtonItem userButtonItem = new UIBarButtonItem(UIImage.FromFile("ic_user.png"), UIBarButtonItemStyle.Plain,
                (sender, args) =>
                {
                    DisplayLogoutAlert(sender as UIBarButtonItem);
                });

            UIBarButtonItem taskOptionsButtonItem = new UIBarButtonItem(UIImage.FromFile("ic_gear.png"), UIBarButtonItemStyle.Plain,
                (sender, args) => {
                    DisplayTaskOptions(sender as UIBarButtonItem);
                });

            UIBarButtonItem timesheetOptionsButtonItem = new UIBarButtonItem(UIImage.FromFile("ic_gear.png"), UIBarButtonItemStyle.Plain,
                (sender, args) => {
                    DisplayTimesheetOptions(sender as UIBarButtonItem);
                });

            var homePageController = new HomePage().CreateViewController();

            _timesheetWorkPageController = new TimesheetWorkPage().CreateViewController();
            _timesheetWorkPageController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIImage.FromFile("ic_gear.png"), UIBarButtonItemStyle.Plain, (sender, e) => {
                DisplayWorkPageOptions(sender as UIBarButtonItem);
            }), false);

            _projectPageController = new ProjectPage().CreateViewController();
            _projectPageController.Title = "Projects";
            _projectPageController.NavigationItem.SetLeftBarButtonItem(userButtonItem, true);

            _projectNavController = new UINavigationController();
            _projectNavController.TabBarItem = new UITabBarItem();
            _projectNavController.TabBarItem.Image = UIImage.FromFile("ic_projects.png");
            _projectNavController.PushViewController(_projectPageController, false);

            _tasksPageController = new TasksPage().CreateViewController();
            _tasksPageController.Title = "Tasks";
            _tasksPageController.NavigationItem.SetLeftBarButtonItem(userButtonItem, true);
            _tasksPageController.NavigationItem.SetRightBarButtonItem(taskOptionsButtonItem, true);

            _tasksNavController = new UINavigationController();
            _tasksNavController.TabBarItem = new UITabBarItem();
            _tasksNavController.TabBarItem.Image = UIImage.FromFile("ic_tasks.png");
            _tasksNavController.PushViewController(_tasksPageController, false);

            _timesheetPageController = new TimesheetPage().CreateViewController();
            _timesheetPageController.Title = "Timesheets";
            _timesheetPageController.NavigationItem.SetLeftBarButtonItem(userButtonItem, true);
            _timesheetPageController.NavigationItem.SetRightBarButtonItem(timesheetOptionsButtonItem, true);

            _timesheetNavController = new UINavigationController();
            _timesheetNavController.TabBarItem = new UITabBarItem();
            _timesheetNavController.TabBarItem.Image = UIImage.FromFile("ic_timesheet.png");
            _timesheetNavController.PushViewController(_timesheetPageController, false);

            MessagingCenter.Instance.Send<String>("", "SaveOfflineWorkChanges");

            var tabs = new UIViewController[] { _projectNavController, _tasksNavController, _timesheetNavController };
            ViewControllers = tabs;
            SelectedViewController = _projectNavController;

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
            if (currentAlertView != null)
                currentAlertView.DismissWithClickedButtonIndex(-1, true);    

            //s[0] = message
            //s[1] = affirm button message
            //s[2] = cancel button message
            //s[3] = identifier
            //s[4] = period id
            var alertController2 = new UIAlertView()
            {
                Title = parameters[0],
            };
            alertController2.AlertViewStyle = UIAlertViewStyle.Default;
            alertController2.AddButton(parameters[1]);

            if (parameters.Length > 3)
                alertController2.AddButton(parameters[2]);

            alertController2.DismissWithClickedButtonIndex(1, true);
            alertController2.Clicked += (sender, args) => {
                if (args.ButtonIndex == 0)
                {
                    if (parameters.Length > 2)
                    {
                        if (!string.IsNullOrEmpty(parameters[3]))
                        {
                            if (parameters[3].Equals("CreateTimesheet"))
                            {
                                MessagingCenter.Instance.Send<String>(parameters[4], "CreateTimesheet");
                            }
                        }
                    }
                }
            };

            alertController2.Show();
            currentAlertView = alertController2;
        }

        private void ExecutePushTimesheetWorkPage(LineModel line)
        {
            _timesheetWorkPageController.Title = line.TaskName;
            _timesheetNavController.PushViewController(_timesheetWorkPageController, true);
        }

        private void DisplayWorkPageOptions(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create("Timesheet Line",
                "",
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Save", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }));

            alertController.AddAction(UIAlertAction.Create("Send Progress", UIAlertActionStyle.Default, alert => {
                if (currentAlertView != null)
                    currentAlertView.DismissWithClickedButtonIndex(-1, true);

                var sendProgressAlertView = new UIAlertView()
                {
                    Title = "Comment",
                };
                sendProgressAlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

                sendProgressAlertView.AddButton("Send");
                sendProgressAlertView.AddButton("Cancel");
                sendProgressAlertView.DismissWithClickedButtonIndex(1, true);
                sendProgressAlertView.Clicked += (sender, args) => {
                    if (args.ButtonIndex == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(sendProgressAlertView.GetTextField(0).Text))
                        {
                            MessagingCenter.Instance.Send<String>(sendProgressAlertView.GetTextField(0).Text, "SendProgress");
                        }
                    }
                };
                sendProgressAlertView.Show();
                currentAlertView = sendProgressAlertView;
            }));

            alertController.AddAction(UIAlertAction.Create("Edit Line", UIAlertActionStyle.Default, alert => {

                if (currentAlertView != null)
                    currentAlertView.DismissWithClickedButtonIndex(-1, true);

                var updateLineAlertView = new UIAlertView()
                {
                    Title = "Update Line",
                };
                updateLineAlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                updateLineAlertView.GetTextField(0).Placeholder = "Comment";

                updateLineAlertView.AddButton("Update");
                updateLineAlertView.AddButton("Cancel");
                updateLineAlertView.DismissWithClickedButtonIndex(1, true);
                updateLineAlertView.Clicked += (sender, args) => {
                    if (args.ButtonIndex == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(updateLineAlertView.GetTextField(0).Text))
                        {
                            MessagingCenter.Instance.Send<String>(updateLineAlertView.GetTextField(0).Text, "UpdateTimesheetLine");
                        }
                    }
                };
                updateLineAlertView.Show();
                currentAlertView = updateLineAlertView;
            }));

            alertController.AddAction(UIAlertAction.Create("Delete Line", UIAlertActionStyle.Default, alert => {

                if (currentAlertView != null)
                    currentAlertView.DismissWithClickedButtonIndex(-1, true);

                var deleteLineAlertView = new UIAlertView()
                {
                    Title = "Do you really want to delete this line?",
                };
                deleteLineAlertView.AlertViewStyle = UIAlertViewStyle.Default;
                deleteLineAlertView.AddButton("Delete");
                deleteLineAlertView.AddButton("Cancel");
                deleteLineAlertView.DismissWithClickedButtonIndex(1, true);
                deleteLineAlertView.Clicked += (sender, args) => {
                    if (args.ButtonIndex == 0)
                    {
                        MessagingCenter.Instance.Send<String>("", "DeleteTimesheetLine");
                    }
                };
                deleteLineAlertView.Show();
                currentAlertView = deleteLineAlertView;
            }));

            alertController.AddAction(UIAlertAction.Create("Close", UIAlertActionStyle.Cancel, alert => {

            }));

            UIPopoverPresentationController presentationController = alertController.PopoverPresentationController;
            if (presentationController != null)
            {
                presentationController.SourceView = this.View;
                presentationController.BarButtonItem = buttonItem;
                presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            SelectedViewController.PresentModalViewController(alertController, true);
        }

        private void ExecuteAddTimesheetDialog()
        {
            if (currentAlertView != null)
                currentAlertView.DismissWithClickedButtonIndex(-1, true);

            MessagingCenter.Instance.Send<String>("", "CloseProjectPicker");

            var addLineAlertView = new UIAlertView()
            {
                Title = "Add Line",
            };
            addLineAlertView.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
            addLineAlertView.GetTextField(0).Placeholder = "Task Name";
            addLineAlertView.GetTextField(1).Placeholder = "Comment";
            addLineAlertView.GetTextField(1).SecureTextEntry = false;

            addLineAlertView.AddButton("Add");
            addLineAlertView.AddButton("Cancel");
            addLineAlertView.DismissWithClickedButtonIndex(1, true);
            addLineAlertView.Clicked += (sender, args) => {
                if (args.ButtonIndex == 0)
                {
                    MessagingCenter.Instance.Send<String[]>(new string[] { addLineAlertView.GetTextField(0).Text, addLineAlertView.GetTextField(1).Text }, "AddTimesheetLine");
                }
            };
            addLineAlertView.Show();
            currentAlertView = addLineAlertView;
        }

        private void DisplayTimesheetOptions(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create(TimesheetStatus,
                AppDelegate.appDelegate.TimesheetPeriod,
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Change Period", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<String>("", "OpenPeriodPicker");
            }));

            alertController.AddAction(UIAlertAction.Create("Add Line", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<String>("", "OpenProjectPicker");
            }));

            alertController.AddAction(UIAlertAction.Create("Submit Timesheet", UIAlertActionStyle.Default, alert => {

                if (currentAlertView != null)
                    currentAlertView.DismissWithClickedButtonIndex(-1, true);

                var submitAlertView = new UIAlertView() {
                    Title = "Comment",
                };
                submitAlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                submitAlertView.AddButton("Submit");
                submitAlertView.AddButton("Cancel");
                submitAlertView.DismissWithClickedButtonIndex(1, true);
                submitAlertView.Clicked += (sender, args) => {
                    if(args.ButtonIndex == 0)
                    {
                        MessagingCenter.Instance.Send<String>(submitAlertView.GetTextField(0).Text, "SubmitTimesheet");
                    }
                };
                submitAlertView.Show();
                currentAlertView = submitAlertView;
            }));

            alertController.AddAction(UIAlertAction.Create("Recall Timesheet", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<String>("", "RecallTimesheet");
            }));

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => {

            }));

            UIPopoverPresentationController presentationController = alertController.PopoverPresentationController;
            if(presentationController != null)
            {
                presentationController.SourceView = this.View;
                presentationController.BarButtonItem = buttonItem;
                presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            SelectedViewController.PresentModalViewController(alertController, true);
        }

        private void DisplayTaskOptions(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create("Filter tasks",
                "",
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("All", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<String>("All", "SortTasks");
            }));

            alertController.AddAction(UIAlertAction.Create("Completed", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<String>("Completed", "SortTasks");
            }));

            alertController.AddAction(UIAlertAction.Create("In Progress", UIAlertActionStyle.Default, alert => {
                MessagingCenter.Instance.Send<String>("In Progress", "SortTasks");
            }));

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => {

            }));

            UIPopoverPresentationController presentationController = alertController.PopoverPresentationController;
            if(presentationController != null)
            {
                presentationController.SourceView = this.View;
                presentationController.BarButtonItem = buttonItem;
                presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            SelectedViewController.PresentModalViewController(alertController, true);
        }

        private void DisplayLogoutAlert(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create(AppDelegate.appDelegate.UserName,
                AppDelegate.appDelegate.UserEmail,
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Log out", UIAlertActionStyle.Destructive, alert => {
                MessagingCenter.Instance.Send<String>("", "ClearAll");

                var controller = Storyboard.InstantiateViewController("LoginController") as LoginController;
                AppDelegate.appDelegate.Window.RootViewController = controller;
            }));

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => {

            }));

            UIPopoverPresentationController presentationController = alertController.PopoverPresentationController;
            if(presentationController != null)
            {
                presentationController.SourceView = this.View;
                presentationController.BarButtonItem = buttonItem;
                presentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            SelectedViewController.PresentModalViewController(alertController, true);
        }
    }
}