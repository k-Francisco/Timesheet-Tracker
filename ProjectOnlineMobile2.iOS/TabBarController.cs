﻿using Foundation;
using LineResult = ProjectOnlineMobile2.Models.TLL.TimesheetLineResult;
using ProjectOnlineMobile2.Pages;
using System;
using UIKit;
using Xamarin.Forms;
using ProjectOnlineMobile2.Models.TLL;
using ProjectOnlineMobile2.ViewModels;

namespace ProjectOnlineMobile2.iOS
{
    public partial class TabBarController : UITabBarController
    {
        private UIViewController _projectPageController, _tasksPageController, _timesheetPageController, _timesheetWorkPageController;
        private UINavigationController _projectNavController, _tasksNavController, _timesheetNavController;

        public TabBarController (IntPtr handle) : base (handle)
        {
            MessagingCenter.Instance.Subscribe<LineResult>(this, "PushTimesheetWorkPage", (line)=> {
                ExecutePushTimesheetWorkPage(line);
            });

            MessagingCenter.Instance.Subscribe<String[]>(this, "DisplayAlert", (s) => {

                //s[0] = message
                //s[1] = affirm button message
                //s[2] = identifier
                //further strings are for necessary parameters like periodId, lineId, etc
                var alertController = UIAlertController.Create("",
                s[0],
                UIAlertControllerStyle.Alert);

                alertController.AddAction(UIAlertAction.Create(s[1], UIAlertActionStyle.Default, alert => {
                    
                }));

                alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => {

                }));
                SelectedViewController.PresentViewController(alertController, true, null);
            });
        }

        private void ExecutePushTimesheetWorkPage(LineResult line)
        {
            var controller = new TimesheetWorkPage().CreateViewController();
            controller.Title = line.TaskName;
            controller.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Save", UIBarButtonItemStyle.Plain, (sender, e) => {
                MessagingCenter.Instance.Send<String>("", "SaveTimesheetWorkChanges");
            }), false);
            _timesheetNavController.PushViewController(controller, true);
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

            var tabs = new UIViewController[] { _projectNavController, _tasksNavController, _timesheetNavController };
            ViewControllers = tabs;
            SelectedViewController = _projectNavController;

        }

        private void DisplayTimesheetOptions(UIBarButtonItem buttonItem)
        {
            var alertController = UIAlertController.Create("Timesheet Period",
                AppDelegate.appDelegate.TimesheetPeriod,
                UIAlertControllerStyle.ActionSheet);

            alertController.AddAction(UIAlertAction.Create("Submit Timesheet", UIAlertActionStyle.Default, alert => {

                var submitAlertController = new UIAlertView() {
                    Title = "Comment",
                };
                submitAlertController.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                submitAlertController.AddButton("Submit");
                submitAlertController.AddButton("Cancel");
                submitAlertController.DismissWithClickedButtonIndex(1, true);
                submitAlertController.Clicked += (sender, args) => {
                    if(args.ButtonIndex == 0)
                    {
                        MessagingCenter.Instance.Send<String>(submitAlertController.GetTextField(0).Text, "SubmitTimesheet");
                    }
                };
                submitAlertController.Show();
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

            alertController.AddAction(UIAlertAction.Create("Complete", UIAlertActionStyle.Default, alert => {
                
            }));

            alertController.AddAction(UIAlertAction.Create("Pending", UIAlertActionStyle.Default, alert => {

            }));

            alertController.AddAction(UIAlertAction.Create("In Progress", UIAlertActionStyle.Default, alert => {

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