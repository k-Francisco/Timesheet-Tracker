using ProjectOnlineMobile2.Models2.Projects;
using ProjectOnlineMobile2.ViewModels;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectsModel = ProjectOnlineMobile2.Models2.Projects.ProjectModel;

namespace ProjectOnlineMobile2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectPage : ContentPage
    {
        private bool didAppear;
        private ProjectPageViewModel viewModel;

        public ProjectPage()
        {
            InitializeComponent();
            MessagingCenter.Instance.Subscribe<ProjectsModel>(this, "ProjectOptions", (project) => {
                ShowProjectOptions(project);
            });
            viewModel = this.BindingContext as ProjectPageViewModel;
        }

        private async void ShowProjectOptions(ProjectModel project)
        {
            var choice = await this.DisplayActionSheet(project.ProjectName, "Cancel", null, new string[] { "Edit Project", "Delete Project" });

            if(choice.Equals("Edit Project"))
            {
                //show edit project controller
            }
            else if(choice.Equals("Delete Project"))
            {
                //delete the project
                viewModel.DeleteProject(project);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!didAppear)
            {
                //1. load the projects that are stored in the database
                //2. get the projects from the server and sync with the local database
                viewModel.LoadProjectsFromDatabase();
                viewModel.SyncProjects();

                //to stop loading and syncing on every appearance of the page
                didAppear = true;
            }
        }
        
    }
}