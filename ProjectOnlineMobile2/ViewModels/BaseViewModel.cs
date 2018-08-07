using Plugin.Connectivity;
using ProjectOnlineMobile2.Services;
using Realms;
using SpevoCore.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectOnlineMobile2.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        protected SharepointApiWrapper SPapi { get; private set; }
        protected Realm realm { get; set; }

        protected SyncDataService syncDataService { get; set; }

        public BaseViewModel() {
            if (SPapi == null)
                SPapi = new SharepointApiWrapper();

            if (realm == null)
            {
                RealmConfiguration.DefaultConfiguration.SchemaVersion = 7;
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
    }
}
