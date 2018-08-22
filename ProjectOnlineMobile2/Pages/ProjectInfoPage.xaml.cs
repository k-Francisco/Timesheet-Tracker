using ProjectsModel = ProjectOnlineMobile2.Models2.Projects.ProjectModel;
using TaskUpdatesModel = ProjectOnlineMobile2.Models2.TaskUpdatesModel.TaskUpdateRequestsModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectOnlineMobile2.ViewModels;
using Realms;
using System.Linq;

namespace ProjectOnlineMobile2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjectInfoPage : ContentPage
	{
        private ProjectsModel project;
        private ProjectPageViewModel viewModel;
        private Realm _realm;

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

                TaskUpdatesList.ItemsSource = updates;
            }
        }
    }
}