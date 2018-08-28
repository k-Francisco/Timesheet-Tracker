using ProjectOnlineMobile2.Pages;
using System;
using UIKit;
using Xamarin.Forms;
using LineModel = ProjectOnlineMobile2.Models2.LineModel.LineModel;

namespace ProjectOnlineMobile2.iOS
{
    public partial class TabBarController : UITabBarController
    {
        private UIViewController _projectPageController, _tasksPageController, _timesheetPageController, _timesheetWorkPageController;
        private UINavigationController _projectNavController, _tasksNavController, _timesheetNavController;

        private UINavigationController _addProjectNavController, _addTaskNavController, _editTaskNavController;
        private UIViewController _addProjectController, _addTaskController, _editTaskController, _projectInfoController;

        private UIAlertView currentAlertView;

        private string TimesheetStatus;

        public TabBarController(IntPtr handle) : base(handle)
        {
            MessagingCenter.Instance.Subscribe<LineModel>(this, "PushTimesheetWorkPage", (line) =>
            {
                ExecutePushTimesheetWorkPage(line);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "AddTimesheetLineDialog", (s) =>
            {
                ExecuteAddTimesheetDialog();
            });

            MessagingCenter.Instance.Subscribe<String[]>(this, "DisplayAlert", (s) =>
            {
                DisplayAlert(s);
            });

            MessagingCenter.Instance.Subscribe<string>(this, "ShowEditTaskPage", (s) =>
            {
                SelectedViewController.PresentModalViewController(_editTaskNavController, true);
            });

            MessagingCenter.Instance.Subscribe<string>(this, "DismissModalViewController", (s) =>
            {
                SelectedViewController.DismissModalViewController(true);
            });

            MessagingCenter.Instance.Subscribe<string>(this, "DismissCurrentAlertView", (s) =>
            {
                currentAlertView.DismissWithClickedButtonIndex(-1, true);
                currentAlertView = null;
            });

            MessagingCenter.Instance.Subscribe<string>(this, "ShowProjectDetails", (projectName)=> {
                _projectInfoController.Title = projectName;
                _projectNavController.PushViewController(_projectInfoController, true);
            });

            MessagingCenter.Instance.Subscribe<String>(this, "TimesheetStatus", (status) =>
            {
                SetTimesheetStatus(status);
            });

            MessagingCenter.Instance.Subscribe<LineModel>(this, "DisplayEditLineCommentAlert", (line) =>
            {
                DisplayEditLineCommentAlert(line);
            });
        }

        public override void ViewDidLoad()
        {
            this.TabBar.Translucent = false;
            this.TabBar.TintColor = UIColor.FromRGBA(49, 117, 47, 255);

            #region UIBarButtonItems region

            UIBarButtonItem userButtonItem = new UIBarButtonItem(UIImage.FromFile("ic_user.png"), UIBarButtonItemStyle.Plain,
                (sender, args) =>
                {
                    DisplayLogoutAlert(sender as UIBarButtonItem);
                });

            UIBarButtonItem projectOptionsButtonItem = new UIBarButtonItem(UIImage.FromFile("ic_gear.png"), UIBarButtonItemStyle.Plain,
                (sender, args) =>
                {
                    ShowProjectOptionsDialog(sender as UIBarButtonItem);
                });

            UIBarButtonItem taskOptionsButtonItem = new UIBarButtonItem(UIImage.FromFile("ic_gear.png"), UIBarButtonItemStyle.Plain,
                (sender, args) =>
                {
                    DisplayTaskOptions(sender as UIBarButtonItem);
                });

            UIBarButtonItem timesheetOptionsButtonItem = new UIBarButtonItem(UIImage.FromFile("ic_gear.png"), UIBarButtonItemStyle.Plain,
                (sender, args) =>
                {
                    DisplayTimesheetOptions(sender as UIBarButtonItem);
                });

            UIBarButtonItem CancelButtonItem = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain,
                (sender, args) =>
                {
                    SelectedViewController.DismissModalViewController(true);
                });

            #endregion UIBarButtonItems region

            #region main controllers region

            var homePageController = new HomePage().CreateViewController();

            _timesheetWorkPageController = new TimesheetWorkPage().CreateViewController();
            _timesheetWorkPageController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIImage.FromFile("ic_gear.png"), UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                DisplayWorkPageOptions(sender as UIBarButtonItem);
            }), false);

            _projectPageController = new ProjectPage().CreateViewController();
            _projectPageController.Title = "Projects";
            _projectPageController.NavigationItem.SetLeftBarButtonItem(userButtonItem, true);
            _projectPageController.NavigationItem.SetRightBarButtonItem(projectOptionsButtonItem, true);

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

            #endregion main controllers region

            #region details controller region

            _projectInfoController = new ProjectInfoPage().CreateViewController();

            _addProjectController = new AddProjectPage().CreateViewController();
            _addProjectController.Title = "Create Project";
            _addProjectController.NavigationItem.SetLeftBarButtonItem(CancelButtonItem, true);
            _addProjectController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain,
                    (sender, args) =>
                    {
                        MessagingCenter.Instance.Send<string>(string.Empty, "SaveProject");
                    }), true);

            _addTaskController = new AddTaskPage().CreateViewController();
            _addTaskController.Title = "Create Task";
            _addTaskController.NavigationItem.SetLeftBarButtonItem(CancelButtonItem, true);
            _addTaskController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain,
                    (sender, args) =>
                    {
                        MessagingCenter.Instance.Send<string>(string.Empty, "SaveTask");
                    }), true);

            _editTaskController = new EditTaskPage().CreateViewController();
            _editTaskController.Title = "Edit Task";
            _editTaskController.NavigationItem.SetLeftBarButtonItem(CancelButtonItem, true);
            _editTaskController.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain,
                    (sender, args) =>
                    {
                        MessagingCenter.Instance.Send<string>(string.Empty, "SaveEditedTask");
                    }), true);

            _addProjectNavController = new UINavigationController(_addProjectController);
            _addTaskNavController = new UINavigationController(_addTaskController);
            _editTaskNavController = new UINavigationController(_editTaskController);

            #endregion details controller region

            MessagingCenter.Instance.Send<String>("", "SaveOfflineWorkChanges");

            var tabs = new UIViewController[] { _projectNavController, _tasksNavController, _timesheetNavController };
            ViewControllers = tabs;
            SelectedViewController = _projectNavController;
        }

        private void DisplayEditLineCommentAlert(LineModel line)
        {
            if (currentAlertView != null)
                currentAlertView.DismissWithClickedButtonIndex(-1, true);

            var updateLineAlertView = new UIAlertView()
            {
                Title = line.Task,
            };
            updateLineAlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            updateLineAlertView.GetTextField(0).Placeholder = "Comment";

            updateLineAlertView.AddButton("Update");
            updateLineAlertView.AddButton("Cancel");
            updateLineAlertView.DismissWithClickedButtonIndex(1, true);
            updateLineAlertView.Clicked += (sender, args) =>
            {
                if (args.ButtonIndex == 0)
                {
                    if (!string.IsNullOrWhiteSpace(updateLineAlertView.GetTextField(0).Text))
                    {
                        MessagingCenter.Instance.Send<string[]>(new string[] { line.ID.ToString(), updateLineAlertView.GetTextField(0).Text }, "SaveEditedComment");
                    }
                }
            };
            updateLineAlertView.Show();
            currentAlertView = updateLineAlertView;
        }

        private void ShowProjectOptionsDialog(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Create Project", UIAlertActionStyle.Default,
                alert =>
                {
                    SelectedViewController.PresentModalViewController(_addProjectNavController, true);
                }));

            alertController.AddAction(UIAlertAction.Create("Close", UIAlertActionStyle.Cancel, alert =>
            {
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

            //parameters[0] = message
            //parameters[1] = affirm button message
            //parameters[2] = cancel button message
            var alertController2 = new UIAlertView()
            {
                Title = parameters[0],
            };
            alertController2.AlertViewStyle = UIAlertViewStyle.Default;
            if (!string.IsNullOrEmpty(parameters[1]))
                alertController2.AddButton(parameters[1]);

            if (!string.IsNullOrEmpty(parameters[2]))
                alertController2.AddButton(parameters[2]);

            alertController2.DismissWithClickedButtonIndex(1, true);
            alertController2.Clicked += (sender, args) =>
            {
                if (args.ButtonIndex == 0)
                {
                    //TODO HERE
                }
            };

            alertController2.Show();
            currentAlertView = alertController2;
        }

        private void ExecutePushTimesheetWorkPage(LineModel line)
        {
            _timesheetWorkPageController.Title = line.Task;
            _timesheetNavController.PushViewController(_timesheetWorkPageController, true);
        }

        private void DisplayWorkPageOptions(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create(null,
                null,
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Save", UIAlertActionStyle.Default, alert =>
            {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }));

            //alertController.AddAction(UIAlertAction.Create("Send Progress", UIAlertActionStyle.Default, alert =>
            //{
            //    if (currentAlertView != null)
            //        currentAlertView.DismissWithClickedButtonIndex(-1, true);

            //    var sendProgressAlertView = new UIAlertView()
            //    {
            //        Title = "Comment",
            //    };
            //    sendProgressAlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

            //    sendProgressAlertView.AddButton("Send");
            //    sendProgressAlertView.AddButton("Cancel");
            //    sendProgressAlertView.DismissWithClickedButtonIndex(1, true);
            //    sendProgressAlertView.Clicked += (sender, args) =>
            //    {
            //        if (args.ButtonIndex == 0)
            //        {
            //            if (!string.IsNullOrWhiteSpace(sendProgressAlertView.GetTextField(0).Text))
            //            {
            //                MessagingCenter.Instance.Send<String>(sendProgressAlertView.GetTextField(0).Text, "SendProgress");
            //            }
            //        }
            //    };
            //    sendProgressAlertView.Show();
            //    currentAlertView = sendProgressAlertView;
            //}));

            //alertController.AddAction(UIAlertAction.Create("Edit Line", UIAlertActionStyle.Default, alert =>
            //{
            //    if (currentAlertView != null)
            //        currentAlertView.DismissWithClickedButtonIndex(-1, true);

            //    var updateLineAlertView = new UIAlertView()
            //    {
            //        Title = "Update Line",
            //    };
            //    updateLineAlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            //    updateLineAlertView.GetTextField(0).Placeholder = "Comment";

            //    updateLineAlertView.AddButton("Update");
            //    updateLineAlertView.AddButton("Cancel");
            //    updateLineAlertView.DismissWithClickedButtonIndex(1, true);
            //    updateLineAlertView.Clicked += (sender, args) =>
            //    {
            //        if (args.ButtonIndex == 0)
            //        {
            //            if (!string.IsNullOrWhiteSpace(updateLineAlertView.GetTextField(0).Text))
            //            {
            //                MessagingCenter.Instance.Send<String>(updateLineAlertView.GetTextField(0).Text, "UpdateTimesheetLine");
            //            }
            //        }
            //    };
            //    updateLineAlertView.Show();
            //    currentAlertView = updateLineAlertView;
            //}));

            //alertController.AddAction(UIAlertAction.Create("Delete Line", UIAlertActionStyle.Default, alert =>
            //{
            //    if (currentAlertView != null)
            //        currentAlertView.DismissWithClickedButtonIndex(-1, true);

            //    var deleteLineAlertView = new UIAlertView()
            //    {
            //        Title = "Do you really want to delete this line?",
            //    };
            //    deleteLineAlertView.AlertViewStyle = UIAlertViewStyle.Default;
            //    deleteLineAlertView.AddButton("Delete");
            //    deleteLineAlertView.AddButton("Cancel");
            //    deleteLineAlertView.DismissWithClickedButtonIndex(1, true);
            //    deleteLineAlertView.Clicked += (sender, args) =>
            //    {
            //        if (args.ButtonIndex == 0)
            //        {
            //            MessagingCenter.Instance.Send<String>("", "DeleteTimesheetLine");
            //        }
            //    };
            //    deleteLineAlertView.Show();
            //    currentAlertView = deleteLineAlertView;
            //}));

            alertController.AddAction(UIAlertAction.Create("Close", UIAlertActionStyle.Cancel, alert =>
            {
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
            addLineAlertView.Clicked += (sender, args) =>
            {
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

            alertController.AddAction(UIAlertAction.Create("Change Period", UIAlertActionStyle.Default, alert =>
            {
                MessagingCenter.Instance.Send<String>("", "OpenPeriodPicker");
            }));

            //alertController.AddAction(UIAlertAction.Create("Add Line", UIAlertActionStyle.Default, alert =>
            //{
            //    MessagingCenter.Instance.Send<String>("", "OpenProjectPicker");
            //}));

            alertController.AddAction(UIAlertAction.Create("Submit Timesheet", UIAlertActionStyle.Default, alert =>
            {
                if (currentAlertView != null)
                    currentAlertView.DismissWithClickedButtonIndex(-1, true);

                var submitAlertView = new UIAlertView()
                {
                    Title = "Comment",
                };
                submitAlertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                submitAlertView.AddButton("Submit");
                submitAlertView.AddButton("Cancel");
                submitAlertView.DismissWithClickedButtonIndex(1, true);
                submitAlertView.Clicked += (sender, args) =>
                {
                    if (args.ButtonIndex == 0)
                    {
                        MessagingCenter.Instance.Send<String>(submitAlertView.GetTextField(0).Text, "SubmitTimesheet");
                    }
                };
                submitAlertView.Show();
                currentAlertView = submitAlertView;
            }));

            alertController.AddAction(UIAlertAction.Create("Recall Timesheet", UIAlertActionStyle.Default, alert =>
            {
                MessagingCenter.Instance.Send<String>("", "RecallTimesheet");
            }));

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert =>
            {
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

        private void DisplayTaskOptions(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create(null,
                null,
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Create Task", UIAlertActionStyle.Default, alert =>
            {
                SelectedViewController.PresentModalViewController(_addTaskNavController, true);
            }));

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert =>
            {
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

        private void DisplayLogoutAlert(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create(AppDelegate.appDelegate.UserName,
                AppDelegate.appDelegate.UserEmail,
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Log out", UIAlertActionStyle.Destructive, alert =>
            {
                MessagingCenter.Instance.Send<String>("", "ClearAll");

                var controller = Storyboard.InstantiateViewController("LoginController") as LoginController;
                AppDelegate.appDelegate.Window.RootViewController = controller;
            }));

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert =>
            {
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
    }
}