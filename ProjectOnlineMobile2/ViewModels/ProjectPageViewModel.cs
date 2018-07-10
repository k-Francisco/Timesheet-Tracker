using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectsModel = ProjectOnlineMobile2.Models2.Projects.ProjectModel;
using ProjectsRoot = ProjectOnlineMobile2.Models2.Projects.RootObject;

namespace ProjectOnlineMobile2.ViewModels
{
    public class ProjectPageViewModel : BaseViewModel
    {

        private const string PROJECTS_LIST_GUID = "c04edc6b-c06e-479c-a11c-41f5aef38d16";

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

        public ICommand RefreshProjects { get { return new Command(ExecuteRefreshProjects); } }

        public ProjectPageViewModel()
        {
            //empty constructor
        }

        public void LoadProjectsFromDatabase()
        {
            var localProjects = realm.All<ProjectsModel>().ToList();

            foreach (var item in localProjects)
            {
                ProjectList.Add(item);
            }
        }

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
                        "projectName," +
                        "projectDescription," +
                        "ProjectStartDate," +
                        "projectFinishDate," +
                        "projectDuration," +
                        "projectPercentComplete," +
                        "projectWork," +
                        "projectActualWork," +
                        "projectRemainingWork," +
                        "projectStatus," +
                        "projectOwnerName/Title&$expand=projectOwnerName/Title";

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
