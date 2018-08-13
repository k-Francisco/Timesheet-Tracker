using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectsModel = ProjectOnlineMobile2.Models2.Projects.ProjectModel;
using ProjectsRoot = ProjectOnlineMobile2.Models2.Projects.RootObject;
using UserModel = ProjectOnlineMobile2.Models2.UserModel;

namespace ProjectOnlineMobile2.ViewModels
{
    public class ProjectPageViewModel : BaseViewModel
    {
        private const string PROJECTS_LIST_GUID = "248a4da4-f54a-4ebf-9193-2fe4bd13a517";

        private ObservableCollection<ProjectsModel> _projectList = new ObservableCollection<ProjectsModel>();

        public ObservableCollection<ProjectsModel> ProjectList
        {
            get { return _projectList; }
            set { SetProperty(ref _projectList, value); }
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ProjectPageViewModel()
        {
            MessagingCenter.Instance.Subscribe<string[]>(this, "AddProject", (parameters) =>
            {
                AddProject(parameters);
            });
        }

        private async void AddProject(string[] parameters)
        {
            /**
             * parameters[0] = project name
             * parameters[1] = project description
             * parameters[2] = project start date
             * parameters[3] = project type
              **/

            try
            {
                if (IsConnectedToInternet())
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "Saving", null, null }, "DisplayAlert");

                    var user = realm.All<UserModel>().FirstOrDefault();

                    var body = "{'__metadata':{'type':'SP.Data.ProjectsListItem'}," +
                    "'ProjectName':'" + parameters[0] + "'," +
                    "'ProjectDescription':'" + parameters[1] + "'," +
                    "'ProjectStartDate':'" + parameters[2] + "'," +
                    "'ProjectType':'" + parameters[3] + "'," +
                    "'ProjectOwnerId':'" + user.UserId + "'}";

                    var item = new StringContent(body);
                    item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                    var formDigest = await SPapi.GetFormDigest();

                    var add = await SPapi.AddListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue,
                        PROJECTS_LIST_GUID,
                        item);
                    var ensure = add.EnsureSuccessStatusCode();

                    if (ensure.IsSuccessStatusCode)
                    {
                        //display prompt that creation of project is successful
                        MessagingCenter.Instance.Send<string[]>(new string[] { "Successfully created the project", "OK", null }, "DisplayAlert");
                        MessagingCenter.Instance.Send<string>(string.Empty, "DismissModalViewController");
                        Debug.WriteLine(ensure.StatusCode.ToString(), "ADD PROJECT");
                    }
                    else
                    {
                        //display prompt that creation of project has failed
                        MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error creating the project", "OK", null }, "DisplayAlert");
                        Debug.WriteLine(ensure.StatusCode.ToString(), "ADD PROJECT");
                    }
                }
                else
                    MessagingCenter.Instance.Send<string[]>(new string[] { "Your device is not connected to the internet", "OK", null }, "DisplayAlert");
            }
            catch (Exception e)
            {
                MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error creating the project", "OK", null }, "DisplayAlert");
                Debug.WriteLine(e.Message, "AddProjectError");
            }
        }

        public void LoadProjectsFromDatabase()
        {
            var localProjects = realm.All<ProjectsModel>().ToList();

            foreach (var item in localProjects)
            {
                ProjectList.Add(item);
            }
        }

        public ICommand RefreshProjects { get { return new Command(ExecuteRefreshProjects); } }
        private void ExecuteRefreshProjects()
        {
            try
            {
                IsRefreshing = true;
                if (IsConnectedToInternet())
                {
                    realm.Write(() =>
                    {
                        realm.RemoveAll<ProjectsModel>();
                    });

                    ProjectList.Clear();

                    SyncProjects();
                }
                else
                    IsRefreshing = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteRefreshProjects", e.Message);
                IsRefreshing = false;
            }
        }

        public async void SyncProjects()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    IsRefreshing = true;

                    string query = "$select=ID," +
                        "ProjectName," +
                        "ProjectDescription," +
                        "ProjectStartDate," +
                        "ProjectFinishDate," +
                        "ProjectDuration," +
                        "ProjectPercentComplete," +
                        "ProjectWork," +
                        "ProjectActualWork," +
                        "ProjectRemainingWork," +
                        "ProjectStatus," +
                        "ProjectOwner/Title&$expand=ProjectOwner/Title";

                    var api = await SPapi.GetListItemsByListGuid(PROJECTS_LIST_GUID, query);

                    if (api.IsSuccessStatusCode)
                    {
                        //projects from the local database
                        var localProjects = realm.All<ProjectsModel>().ToList();

                        //projects from the server
                        var projectsList = JsonConvert.DeserializeObject<ProjectsRoot>(await api.Content.ReadAsStringAsync());

                        //sync the two lists
                        syncDataService.SyncProjects(projectsList, localProjects, ProjectList);
                    }

                    IsRefreshing = false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("SyncProjects", e.Message);
                IsRefreshing = false;

                MessagingCenter.Instance.Send<string[]>(new string[] { "There was a problem syncing the projects. Please try again", "Close" }, "DisplayAlert");
            }
        }
    }
}