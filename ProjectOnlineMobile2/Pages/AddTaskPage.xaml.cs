using ProjectOnlineMobile2.ViewModels;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectsModel = ProjectOnlineMobile2.Models2.Projects.ProjectModel;
using ResourceModel = ProjectOnlineMobile2.Models2.ResourceModel.ResourceModel;

namespace ProjectOnlineMobile2.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTaskPage : ContentPage
    {
        private Realm _realm;
        private List<ProjectsModel> _taskProjectSource = new List<ProjectsModel>();
        private List<ResourceModel> _resourceSource = new List<ResourceModel>();
        private TasksPageViewModel viewModel;

        public AddTaskPage()
        {
            InitializeComponent();

            _realm = Realm.GetInstance();
            viewModel = BindingContext as TasksPageViewModel;

            MessagingCenter.Instance.Subscribe<string>(this, "SaveTask", (s) =>
            {
                if (string.IsNullOrWhiteSpace(taskNameEntry.Text))
                {
                    this.DisplayAlert(null, "Please fill in the name of the task", "OK");
                }
                else
                {
                    var project = _realm.All<ProjectsModel>()
                                    .Where(p=>p.ID == _taskProjectSource[taskProject.SelectedIndex].ID)
                                    .FirstOrDefault();

                    if(project != null)
                    {
                        if (DateTime.Compare(project.ProjectStartDate.Date,taskStartDate.Date) > 0)
                        {
                            this.DisplayAlert(null, 
                                string.Format("The task cannot be earlier than the start date of the project ({0: MMM dd, yyyy})", project.ProjectStartDate.Date),
                                "OK");
                        }
                        else
                        {
                            var parameters = new string[] {
                                            project.ProjectType,
                                            taskNameEntry.Text,
                                            String.Format("{0:MM/dd/yyyy}",taskStartDate.Date),
                                            _taskProjectSource[taskProject.SelectedIndex].ID.ToString(),
                                            _resourceSource[resource.SelectedIndex].Resource.Id.ToString()
                                         };

                            viewModel.CreateTask(parameters);
                        }
                    }
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var localProjects = _realm.All<ProjectsModel>().ToList();
            foreach (var item in localProjects)
            {
                _taskProjectSource.Add(item);
            }
            taskProject.ItemsSource = _taskProjectSource;
            taskProject.SelectedIndex = 0;

            var localResources = _realm.All<ResourceModel>().ToList();
            foreach (var item in localResources)
            {
                _resourceSource.Add(item);
            }
            resource.ItemsSource = _resourceSource;
            resource.SelectedIndex = 0;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            taskNameEntry.Text = "";
            taskStartDate.Date = DateTime.Now;

            _taskProjectSource.Clear();
            _resourceSource.Clear();
            
        }
    }
}