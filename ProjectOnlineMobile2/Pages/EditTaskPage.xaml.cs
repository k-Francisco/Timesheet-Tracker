using AssignmentsModel = ProjectOnlineMobile2.Models2.Assignments.AssignmentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using ProjectOnlineMobile2.ViewModels;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTaskPage : ContentPage
	{
        private AssignmentsModel assignment;
        private TasksPageViewModel viewModel;

		public EditTaskPage ()
		{
			InitializeComponent ();

            MessagingCenter.Instance.Subscribe<string>(this, "SaveEditedTask", (s) => {
                ValidateEntries();
            });

            MessagingCenter.Instance.Subscribe<AssignmentsModel>(this, "ShowAssignmentDetails",
                (assignment)=> {
                    this.assignment = assignment;
                });

            taskName.TextChanged += (s,e) => { DetailsChanged(); };

            taskActualWork.TextChanged += (s,e) => { DetailsChanged(); };

            taskWork.TextChanged += (s,e) => { DetailsChanged(); };

            taskStartDate.DateSelected += (s,e) => { DetailsChanged(); };
		}

        private void DetailsChanged()
        {
            previewTaskName.Text = string.IsNullOrWhiteSpace(taskName.Text) ? taskName.Placeholder : taskName.Text;

            previewActualHours.Text = string.IsNullOrWhiteSpace(taskActualWork.Text) ?
                    string.Format("{0}h", assignment.TaskActualWork) : string.Format("{0}h", taskActualWork.Text);

            previewWork.Text = string.IsNullOrWhiteSpace(taskWork.Text) ?
                    string.Format("{0}h", assignment.TaskWork) : string.Format("{0}h", taskWork.Text);

            previewTaskStartDate.Text = assignment.TaskStartDate.Date.Equals(taskStartDate.Date) ?
                    string.Format("{0: MM/dd/yyyy}", assignment.TaskStartDate.Date) : string.Format("{0: MM/dd/yyyy}", taskStartDate.Date);

            if (string.IsNullOrWhiteSpace(taskWork.Text) && string.IsNullOrWhiteSpace(taskActualWork.Text))
            {
                previewPercentComplete.Text = assignment.TaskPercentComplete;
                previewRemainingWork.Text = assignment.TaskRemainingWork;
            }
            else if (string.IsNullOrWhiteSpace(taskWork.Text) && !string.IsNullOrWhiteSpace(taskActualWork.Text))
            {
                previewPercentComplete.Text = string.Format("{0:0.}%", ((Double.Parse(taskActualWork.Text) / assignment.TaskWork) * 100));
                previewRemainingWork.Text = string.Format("{0}h",(assignment.TaskWork - Double.Parse(taskActualWork.Text)));
            }
            else if (!string.IsNullOrWhiteSpace(taskWork.Text) && string.IsNullOrWhiteSpace(taskActualWork.Text))
            {
                previewPercentComplete.Text = string.Format("{0:0.}%", ((assignment.TaskActualWork / Double.Parse(taskWork.Text)) * 100));
                previewRemainingWork.Text = string.Format("{0}h", (Double.Parse(taskWork.Text) - assignment.TaskActualWork));
            }
            else
            {
                previewPercentComplete.Text = string.Format("{0:0.}%", ((Double.Parse(taskActualWork.Text) / Double.Parse(taskWork.Text)) * 100));
                previewRemainingWork.Text = string.Format("{0}h", (Double.Parse(taskWork.Text) - Double.Parse(taskActualWork.Text)));
            }
        }

        private void ValidateEntries()
        {
            try
            {
                bool validate = true;

                if (string.IsNullOrWhiteSpace(previewTaskName.Text) && string.IsNullOrWhiteSpace(previewWork.Text) &&
                    string.IsNullOrWhiteSpace(previewActualHours.Text) && taskStartDate.Date.Equals(assignment.TaskStartDate.Date))
                    validate = false;

                if (string.IsNullOrWhiteSpace(taskWork.Text) && !string.IsNullOrWhiteSpace(taskActualWork.Text))
                {
                    if (Double.Parse(taskActualWork.Text) > assignment.TaskWork)
                    {
                        this.DisplayAlert(null, "The task's actual work cannot be greater than the task's work", "OK");
                        validate = false;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(taskWork.Text) && string.IsNullOrWhiteSpace(taskActualWork.Text))
                {
                    if (assignment.TaskActualWork > Double.Parse(taskWork.Text))
                    {
                        this.DisplayAlert(null, "The task's actual work cannot be greater than the task's work", "OK");
                        validate = false;
                    }
                }
                else
                {
                    if (Double.Parse(taskActualWork.Text) > Double.Parse(taskWork.Text))
                    {
                        this.DisplayAlert(null, "The task's actual work cannot be greater than the task's work", "OK");
                        validate = false;
                    }
                }

                if (validate)
                {
                    var parameters = new string[] { assignment.ID.ToString(),
                                                    previewTaskName.Text,
                                                    previewTaskStartDate.Text,
                                                    previewWork.Text,
                                                    previewActualHours.Text
                                                  };
                    viewModel.EditTask(parameters);
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            taskName.Placeholder = assignment.TaskName;
            taskStartDate.Date = assignment.TaskStartDate.Date;
            taskWork.Placeholder = string.Format("{0}h", assignment.TaskWork);
            taskActualWork.Placeholder = string.Format("{0}h",assignment.TaskActualWork);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            taskName.Text = "";
            taskWork.Text = "";
            taskActualWork.Text = "";
        }
    }
}