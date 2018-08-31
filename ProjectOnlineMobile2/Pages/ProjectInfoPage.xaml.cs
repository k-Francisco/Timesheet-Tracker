using ProjectsModel = ProjectOnlineMobile2.Models2.Projects.ProjectModel;
using TaskUpdatesModel = ProjectOnlineMobile2.Models2.TaskUpdatesModel.TaskUpdateRequestsModel;
using AssignmentsModel = ProjectOnlineMobile2.Models2.Assignments.AssignmentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectOnlineMobile2.ViewModels;
using Realms;
using System.Linq;
using System.Collections.Generic;
using ProjectOnlineMobile2.Models2;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjectInfoPage : ContentPage
	{
        private ProjectsModel project;
        private ProjectPageViewModel viewModel;
        private Realm _realm;
        private List<AssignmentsModel> Tasks = new List<AssignmentsModel>();

        public ProjectInfoPage ()
		{
			InitializeComponent ();

            _realm = Realm.GetInstance();

            viewModel = BindingContext as ProjectPageViewModel;

            MessagingCenter.Instance.Subscribe<ProjectsModel>(this, "ShowProjectDetails",
                (project) => {
                    this.project = project;
                });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (project != null)
            {
                ProjectWork.Text = string.Format("{0}h", project.ProjectWork.ToString());
                ProjectPercentComplete.Text = project.ProjectPercentComplete;
                ProjectDuration.Text = project.ProjectDuration;
                ProjectStartDate.Text = string.Format("{0: MMM dd, yyyy}", project.ProjectStartDate.Date);
                ProjectFinishDate.Text = string.Format("{0: MMM dd, yyyy}", project.ProjectFinishDate.Date);

                var updates = _realm.All<TaskUpdatesModel>()
                              .Where(p => p.TaskUpdateProjectNameId == project.ID)
                              .ToList();

                var user = _realm.All<UserModel>()
                            .FirstOrDefault()
                            .UserName;

                TaskUpdatesList.ItemsSource = updates;

                var localTasks = _realm.All<AssignmentsModel>()
                            .ToList();

                foreach (var item in localTasks)
                {
                    if (item.Project.Equals(project.ProjectName))
                        Tasks.Add(item);
                }

                ProjectTasksList.ItemsSource = Tasks;
                ProjectTasksList.HeightRequest = Tasks.Count * 85;

                if (!updates.Any() && project.OwnerName.Equals(user))
                    TaskUpdatesList.IsVisible = false;

                if (!Tasks.Any())
                    ProjectTasksList.IsVisible = false;
                
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Tasks.Clear();
            TaskUpdatesList.IsVisible = true;
            ProjectTasksList.IsVisible = true;
        }

    }
}