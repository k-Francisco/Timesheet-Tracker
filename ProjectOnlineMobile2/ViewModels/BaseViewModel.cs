using Newtonsoft.Json;
using Plugin.Connectivity;
using ProjectOnlineMobile2.Services;
using Realms;
using SpevoCore.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TaskUpdatesModel = ProjectOnlineMobile2.Models2.TaskUpdatesModel.TaskUpdateRequestsModel;
using TaskUpdatesRoot = ProjectOnlineMobile2.Models2.TaskUpdatesModel.RootObject;

namespace ProjectOnlineMobile2.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        protected const string TASK_UPDATE_LIST_GUID = "65a1ae0c-d12b-470b-a194-89a58cfb6b17";
        protected SharepointApiWrapper SPapi { get; private set; }
        protected Realm realm { get; set; }
        protected SyncDataService syncDataService { get; set; }

        public BaseViewModel() {
            if (SPapi == null)
                SPapi = new SharepointApiWrapper();

            if (realm == null)
            {
                RealmConfiguration.DefaultConfiguration.SchemaVersion = 3;
                realm = Realm.GetInstance();
            }

            if (syncDataService == null)
                syncDataService = new SyncDataService();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs((propertyName)));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        public bool IsConnectedToInternet()
        {
            if (!CrossConnectivity.IsSupported)
                return true;

            return CrossConnectivity.Current.IsConnected;
        }

        public async void GetTaskUpdates()
        {
            try
            {
                if (IsConnectedToInternet())
                {
                    var apiResponse = await SPapi.GetListItemsByListGuid(TASK_UPDATE_LIST_GUID);

                    var ensure = apiResponse.EnsureSuccessStatusCode();

                    if (ensure.IsSuccessStatusCode)
                    {
                        var taskUpdatesList = JsonConvert.DeserializeObject<TaskUpdatesRoot>(await apiResponse.Content.ReadAsStringAsync());

                        var localUpdates = realm.All<TaskUpdatesModel>().ToList();

                        syncDataService.SyncTaskUpdates(taskUpdatesList, localUpdates);
                    }
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message, "GetTaskUpdates");
            }
        }
    }
}
