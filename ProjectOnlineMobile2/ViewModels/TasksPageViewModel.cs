﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AssignmentsRoot = ProjectOnlineMobile2.Models2.Assignments.RootObject;
using AssignmentsModel = ProjectOnlineMobile2.Models2.Assignments.AssignmentModel;
using UserModel = ProjectOnlineMobile2.Models2.UserModel;
using Newtonsoft.Json;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TasksPageViewModel : BaseViewModel
    {
        const string ASSIGNMENTS_LIST_GUID = "83b05574-7af6-4bf8-bfd2-0810c5967010";

        private ObservableCollection<AssignmentsModel> _tasks = new ObservableCollection<AssignmentsModel>();
        public ObservableCollection<AssignmentsModel> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ICommand RefreshTasksCommand { get { return new Command(ExecuteRefreshTasksCommand); } }
        public ICommand ExecuteTaskClickedCommand { get { return new Command<AssignmentsModel>(TaskClicked); } }

        public TasksPageViewModel()
        {
            MessagingCenter.Instance.Subscribe<String>(this, "SortTasks", (sortReference) =>
            {
                ExecuteSortTasks(sortReference);
            });

            LoadAssignmentsFromDatabase();
            SyncUserTasks();
        }

        private void LoadAssignmentsFromDatabase()
        {
            var localAssignments = realm.All<AssignmentsModel>().ToList();

            foreach (var item in localAssignments)
            {
                Tasks.Add(item);
            }
        }

        private async void SyncUserTasks()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    IsRefreshing = true;

                    var user = realm.All<UserModel>().FirstOrDefault();

                    string query = "$select=TaskName," +
                        "TaskStartDate," +
                        "TaskFinishDate," +
                        "TaskPercentComplete," +
                        "TaskWork," +
                        "TaskActualWork," +
                        "TaskRemainingWork," +
                        "ID," +
                        "ProjectName/ProjectName," +
                        "ResourceName/Title" +
                        "&$expand=ProjectName,ResourceName" +
                        "&$filter=ResourceNameStringId eq " + user.UserId.ToString();

                    var api = await SPapi.GetListItemsByListGuid(ASSIGNMENTS_LIST_GUID, query);

                    if (api.IsSuccessStatusCode)
                    {
                        //tasks from the local database
                        var localAssignments = realm.All<AssignmentsModel>().ToList();

                        //tasks from the server
                        var assignmentsList = JsonConvert.DeserializeObject<AssignmentsRoot>(await api.Content.ReadAsStringAsync());

                        //sync the two lists
                        syncDataService.SyncUserTasks(localAssignments, assignmentsList.D.Results, Tasks);
                    }

                    IsRefreshing = false;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("SyncUserTasks", e.Message);
                IsRefreshing = false;
            }
            
        }

        private void ExecuteRefreshTasksCommand()
        {
            try
            {
                IsRefreshing = true;
                if (IsConnectedToInternet())
                {
                    realm.Write(() =>
                    {
                        realm.RemoveAll<AssignmentsModel>();
                    });

                    Tasks.Clear();

                    SyncUserTasks();
                }
                else
                    IsRefreshing = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExecuteRefreshTasksCommand", e.Message);
                IsRefreshing = false;
            }

        }

        private void ExecuteSortTasks(string sort)
        {
            Tasks.Clear();

            var localTasks = realm.All<AssignmentsModel>().ToList();

            foreach (var item in localTasks)
            {
                if (sort.Equals("All"))
                {
                    Tasks.Add(item);
                }
                else if (sort.Equals("In Progress"))
                {
                    if (!item.TaskPercentComplete.Equals("100%"))
                    {
                        Tasks.Add(item);
                    }
                }
                else if (sort.Equals("Completed"))
                {
                    if (!item.TaskPercentComplete.Equals("100%"))
                    {
                        Tasks.Add(item);
                    }
                }
            }
        }

        private void TaskClicked(AssignmentsModel assignment)
        {
            MessagingCenter.Instance.Send<AssignmentsModel>(assignment, "DisplayActionSheet");
        }

        public void EditTask(AssignmentsModel assignment)
        {

        }

        public void DeleteTask(AssignmentsModel assignment)
        {

        }
    }
}
