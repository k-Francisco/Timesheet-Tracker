using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Plugin.Connectivity;
using ProjectOnlineMobile2.Models2;
using SpevoCore.Services.Token_Service;
using ResourceRoot = ProjectOnlineMobile2.Models2.ResourceModel.ResourceRoot;
using ResourceModel = ProjectOnlineMobile2.Models2.ResourceModel.ResourceModel;
using Newtonsoft.Json;

namespace ProjectOnlineMobile2.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        const string RESOURCES_LIST_GUID = "7b6e6088-380c-4782-b176-8ce910771922";

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { SetProperty(ref _userEmail, value); }
        }

        public HomePageViewModel()
        {
            GetUserInfo();

            MessagingCenter.Instance.Subscribe<String>(this, "ClearAll", (s)=> {
                realm.Write(()=> {
                    realm.RemoveAll();
                });
                Settings.ClearAll();
            });

            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) => {
                if(IsConnectedToInternet())
                   MessagingCenter.Instance.Send<String>("", "SaveOfflineWorkChanges");
            };

        }

        private async void GetUserInfo()
        {
            try
            {
                var userInfo = realm.All<UserModel>();

                if (IsConnectedToInternet())
                {
                    if(!userInfo.Any())
                    {
                        var user = await SPapi.GetCurrentUser();
                        UserName = user.D.Title;
                        UserEmail = user.D.Email;

                        var userModel = new UserModel()
                        {
                            UserName = user.D.Title,
                            UserEmail = user.D.Email,
                            UserId = user.D.Id,
                        };

                        realm.Write(()=> {
                            realm.Add<UserModel>(userModel);
                        });
                        MessagingCenter.Instance.Send<UserModel>(userModel, "UserInfo");
                    }
                    else
                    {
                        var user = userInfo.First();
                        MessagingCenter.Instance.Send<UserModel>(user, "UserInfo");

                        UserName = userInfo.First().UserName;
                        UserEmail = userInfo.First().UserEmail;
                    }
                }
                else
                {
                    if(userInfo != null)
                    {
                        UserName = userInfo.First().UserName;
                        UserEmail = userInfo.First().UserEmail;
                        MessagingCenter.Instance.Send<UserModel>(userInfo.First(), "UserInfo");
                    }
                }

                realm.Refresh();

                SyncSiteUsers();

            }
            catch(Exception e)
            {
                Debug.WriteLine("GetUserInfo", e.Message);
            }
        }

        private async void SyncSiteUsers()
        {
            if (IsConnectedToInternet())
            {
                try
                {
                    var query = "$select=Resource/Title,Resource/EMail,Resource/Id,Resource/Department&$expand=Resource";
                    var response = await SPapi.GetListItemsByListGuid(RESOURCES_LIST_GUID,query);
                    
                    var usersFromServer = JsonConvert.DeserializeObject<ResourceRoot>(await response.Content.ReadAsStringAsync());
                    var localUsers = realm.All<ResourceModel>().ToList();

                    syncDataService.SyncSiteUsers(usersFromServer, localUsers);
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message,"SyncSiteUsers");
                }
            }
        }

    }
}
