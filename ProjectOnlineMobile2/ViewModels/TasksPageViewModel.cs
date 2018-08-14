using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;
using AssignmentsModel = ProjectOnlineMobile2.Models2.Assignments.AssignmentModel;
using AssignmentsRoot = ProjectOnlineMobile2.Models2.Assignments.RootObject;
using UserModel = ProjectOnlineMobile2.Models2.UserModel;

namespace ProjectOnlineMobile2.ViewModels
{
    public class TasksPageViewModel : BaseViewModel
    {
        private const string ASSIGNMENTS_LIST_GUID = "83b05574-7af6-4bf8-bfd2-0810c5967010";

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
        public ICommand ExecuteTaskLongPressCommand { get { return new Command<AssignmentsModel>(TaskLongPressCommand); } }

        public TasksPageViewModel()
        {
            MessagingCenter.Instance.Subscribe<String>(this, "SortTasks", (sortReference) =>
            {
                ExecuteSortTasks(sortReference);
            });

            MessagingCenter.Instance.Subscribe<string[]>(this, "CreateTask", (parameters) =>
            {
                CreateTask(parameters);
            });

            MessagingCenter.Instance.Subscribe<string[]>(this, "UploadEditedTask", (parameters) =>
            {
                EditTask(parameters);
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
            catch (Exception e)
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

        private void TaskLongPressCommand(AssignmentsModel assignment)
        {
            MessagingCenter.Instance.Send<AssignmentsModel>(assignment, "DisplayActionSheet");
        }

        private async void CreateTask(string[] parameters)
        {
            /**
             * parameters[0] = task name
             * parameters[1] = task start date
             * parameters[2] = task project
             * parameters[3] = resource id
            **/
            if (IsConnectedToInternet())
            {
                try
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "Saving", null, null }, "DisplayAlert");

                    var body = "{'__metadata':{'type':'SP.Data.TasksListItem'}," +
                    "'TaskName':'" + parameters[0] + "'," +
                    "'TaskStartDate':'" + parameters[1] + "'," +
                    "'ProjectNameId':'" + parameters[2] + "'," +
                    "'ResourceNameId':'" + parameters[3] + "'}";

                    var item = new StringContent(body);
                    item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                    var formDigest = await SPapi.GetFormDigest();

                    var add = await SPapi.AddListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue,
                        ASSIGNMENTS_LIST_GUID,
                        item);
                    var ensure = add.EnsureSuccessStatusCode();

                    if (ensure.IsSuccessStatusCode)
                    {
                        //display prompt that creation of project is successful
                        MessagingCenter.Instance.Send<string[]>(new string[] { "Successfully created the task", "OK", null }, "DisplayAlert");
                        MessagingCenter.Instance.Send<string>(string.Empty, "DismissModalViewController");
                        Debug.WriteLine(ensure.StatusCode.ToString(), "ADD TASK");
                    }
                    else
                    {
                        //display prompt that creation of project has failed
                        MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error creating the task", "OK", null }, "DisplayAlert");
                        Debug.WriteLine(ensure.StatusCode.ToString(), "ADD TASK");
                    }
                }
                catch(Exception e)
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error creating the task", "OK", null }, "DisplayAlert");
                    Debug.WriteLine(e.Message,"CreateTask");
                }
            }
            else
                MessagingCenter.Instance.Send<string[]>(new string[] { "Your device is not connected to the internet", "OK", null }, "DisplayAlert");

        }

        public async void EditTask(string[] parameters)
        {
            /**
             * parameters[0] = item ID
             * parameters[1] = task name
             * parameters[2] = task start date
             * parameters[3] = task work
             * parameters[4] = task actual work
             **/

            if (IsConnectedToInternet())
            {
                try
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "Saving", null, null }, "DisplayAlert");

                    var body = "{'__metadata':{'type':'SP.Data.TasksListItem'}," +
                    "'TaskName':'" + parameters[1] + "'," +
                    "'TaskStartDate':'" + parameters[2] + "'," +
                    "'TaskWork':'" + parameters[3] + "'," +
                    "'TaskActualWork':'" + parameters[4] + "'}";

                    var item = new StringContent(body);
                    item.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                    var formDigest = await SPapi.GetFormDigest();

                    var edit = await SPapi.UpdateListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue,
                        ASSIGNMENTS_LIST_GUID,
                        item,
                        parameters[0]);
                    var ensure = edit.EnsureSuccessStatusCode();

                    if (ensure.IsSuccessStatusCode)
                    {
                        //display prompt that creation of project is successful
                        MessagingCenter.Instance.Send<string[]>(new string[] { "Successfully edited the task", "OK", null }, "DisplayAlert");
                        MessagingCenter.Instance.Send<string>(string.Empty, "DismissModalViewController");

                        Debug.WriteLine("SUCCESS", "EDIT TASK");
                    }
                    else
                    {
                        //display prompt that creation of project has failed
                        MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error sending the request", "OK", null }, "DisplayAlert");

                        Debug.WriteLine("FAILED", "EDIT TASK");
                    }
                }
                catch(Exception e)
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error sending the request", "OK", null }, "DisplayAlert");

                    Debug.WriteLine(e.Message, "EditTask");
                }

            }
        }

        public async void DeleteTask(AssignmentsModel assignment)
        {
            if (IsConnectedToInternet())
            {
                try
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "Deleting", null, null }, "DisplayAlert");

                    var formDigest = await SPapi.GetFormDigest();

                    var delete = await SPapi.DeleteListItemByListGuid(formDigest.D.GetContextWebInformation.FormDigestValue,
                    ASSIGNMENTS_LIST_GUID,
                    assignment.ID.ToString());

                    var ensure = delete.EnsureSuccessStatusCode();

                    if (ensure.IsSuccessStatusCode)
                    {
                        //prompt user of successful deletion
                        MessagingCenter.Instance.Send<string[]>(new string[] { "Successfully edited the task", "OK", null }, "DisplayAlert");

                        Debug.WriteLine("Delete success!");

                        //refresh the items/tasks
                        ExecuteRefreshTasksCommand();
                    }
                    else
                    {
                        //prompt user of failed deletion
                        MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error sending the request", "OK", null }, "DisplayAlert");

                        Debug.WriteLine("Delete failed!");
                    }
                }
                catch(Exception e)
                {
                    MessagingCenter.Instance.Send<string[]>(new string[] { "There was an error sending the request", "OK", null }, "DisplayAlert");

                    Debug.WriteLine(e.Message, "DeleteTask");
                }
            }
        }
    }
}