using ProjectOnlineMobile2.ViewModels;
using AssignmentsModel = ProjectOnlineMobile2.Models2.Assignments.AssignmentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TasksPage : ContentPage
	{
        TasksPageViewModel viewModel;
        const string EDIT_TASK = "Edit Task";
        const string DELETE_TASK = "Delete Task";
        const string CANCEL_BUTTON = "Cancel";

        public TasksPage ()
		{
			InitializeComponent ();

            MessagingCenter.Instance.Subscribe<AssignmentsModel>(this,"DisplayActionSheet",(assignment) => {
                DisplayTaskOptions(assignment);
            });

            viewModel = BindingContext as TasksPageViewModel;
		}

        public async void DisplayTaskOptions(AssignmentsModel assignment)
        {
            string action = await this.DisplayActionSheet(assignment.TaskName, CANCEL_BUTTON, null, new string[] { EDIT_TASK,DELETE_TASK});

            if(action.Equals(EDIT_TASK))
            {
                MessagingCenter.Instance.Send<string>(string.Empty, "ShowEditTaskPage");
                MessagingCenter.Instance.Send<AssignmentsModel>(assignment, "ShowAssignmentDetails");
            }
            else if(action.Equals(DELETE_TASK))
            {
                bool confirm = await this.DisplayAlert(null, "Do you really want to delete this task?" +
                    " The timesheet associated with this task will also be deleted.", "Delete", "Cancel");

                if (confirm)
                {
                    viewModel.DeleteTask(assignment);
                }
            }
        }

    }
}