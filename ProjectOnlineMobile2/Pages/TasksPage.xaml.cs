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
                DisplayActionSheet(assignment);
            });

            viewModel = BindingContext as TasksPageViewModel;
		}

        public async void DisplayActionSheet(AssignmentsModel assignment)
        {
            string action = await this.DisplayActionSheet(assignment.TaskName, CANCEL_BUTTON, "", new string[] { EDIT_TASK,DELETE_TASK});

            if(action.Equals(EDIT_TASK))
            {
                //edit task here
            }
            else if(action.Equals(DELETE_TASK))
            {
                //delete task here
            }

        }
    }
}