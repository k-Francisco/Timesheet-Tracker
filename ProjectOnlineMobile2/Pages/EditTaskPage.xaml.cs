using AssignmentsModel = ProjectOnlineMobile2.Models2.Assignments.AssignmentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTaskPage : ContentPage
	{
        private AssignmentsModel assignment;
		public EditTaskPage ()
		{
			InitializeComponent ();

            MessagingCenter.Instance.Subscribe<string>(this, "SaveEditedTask", (s) => {
                var parameters = new string[] { assignment.ID.ToString(),
                    previewTaskName.Text,
                    previewTaskStartDate.Text,
                    previewWork.Text,
                    previewActualHours.Text
                };

                MessagingCenter.Instance.Send<string[]>(parameters, "UploadEditedTask");
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
            previewTaskStartDate.Text = assignment.TaskStartDate.Date.Equals(taskStartDate.Date) ?
                string.Format("{0: MM/dd/yyyy}", assignment.TaskStartDate.Date) : string.Format("{0: MM/dd/yyyy}", taskStartDate.Date);
            previewWork.Text = string.IsNullOrWhiteSpace(taskWork.Text) ? string.Format("{0}h", assignment.TaskWork) : taskWork.Text;
            previewActualHours.Text = string.IsNullOrWhiteSpace(taskActualWork.Text) ? string.Format("{0}h", assignment.TaskActualWork) : taskActualWork.Text;

            previewPercentComplete.Text = string.IsNullOrEmpty(taskWork.Text) || string.IsNullOrEmpty(taskActualWork.Text) ?
                assignment.TaskPercentComplete : string.Format("{0}%", ((Double.Parse(taskActualWork.Text) / Double.Parse(taskWork.Text)) * 100));
            previewRemainingWork.Text = string.IsNullOrEmpty(taskWork.Text) || string.IsNullOrEmpty(taskActualWork.Text) ?
                string.Format("{0}h", assignment.TaskWork) : string.Format("{0}h", (Double.Parse(taskWork.Text) - Double.Parse(taskActualWork.Text)));
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