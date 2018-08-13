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

        public AddTaskPage()
        {
            InitializeComponent();

            _realm = Realm.GetInstance();
            

            MessagingCenter.Instance.Subscribe<string>(this, "SaveTask", (s) =>
            {
                var parameters = new string[] { "Create",
                    taskNameEntry.Text,
                    String.Format("{0:MM/dd/yyyy}",taskStartDate.Date),
                    _taskProjectSource[taskProject.SelectedIndex].ID.ToString(),
                    _resourceSource[resource.SelectedIndex].Resource.Id.ToString()
                };

                MessagingCenter.Instance.Send<string[]>(parameters, "TaskOptions");
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