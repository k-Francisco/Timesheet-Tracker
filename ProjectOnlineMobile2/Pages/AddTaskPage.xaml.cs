using Realms;
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
        private Realm realm;

        public AddTaskPage()
        {
            InitializeComponent();

            realm = Realm.GetInstance();

            var localProjects = realm.All<ProjectsModel>().ToList();
            taskProject.ItemsSource = localProjects;
            taskProject.SelectedIndex = 0;

            var localResources = realm.All<ResourceModel>().ToList();
            resource.ItemsSource = localResources;
            resource.SelectedIndex = 0;

            MessagingCenter.Instance.Subscribe<string>(this, "SaveTask", (s) =>
            {
                var parameters = new string[] { "Create",
                    taskNameEntry.Text,
                    taskStartDate.Date.ToString(),
                    localProjects[taskProject.SelectedIndex].ID.ToString(),
                    localResources[resource.SelectedIndex].Resource.Id.ToString()
                };

                MessagingCenter.Instance.Send<string[]>(parameters, "TaskOptions");
            });
        }
    }
}