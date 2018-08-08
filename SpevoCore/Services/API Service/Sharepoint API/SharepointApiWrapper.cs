using Refit;
using SpevoCore.Models.FormDigest;
using SpevoCore.Models.User;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpevoCore.Services
{
    public class SharepointApiWrapper : BaseWrapper, ISharepointAPI
    {
        private static string _sharepointUrl = "https://sharepointevo.sharepoint.com/timesheettracker";
        private HttpClient _client;
        private ISharepointAPI api;

        public SharepointApiWrapper()
        {
            if (_client == null)
            {
                _client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_sharepointUrl)
                };
                _client.DefaultRequestHeaders.Accept.Add(mediaType);
            }

            if (api == null)
                api = RestService.For<ISharepointAPI>(_client);
        }

        public async Task<UserModel> GetCurrentUser()
        {
            try
            {
                return await api.GetCurrentUser();
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetCurrentUser", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetSiteUsers(string query)
        {
            try
            {
                return await _client.GetAsync(_sharepointUrl + "/_api/web/siteusers?" + query);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetListItemsByListGuid", e.Message);
                return null;
            }
        }

        public async Task<FormDigestModel> GetFormDigest()
        {
            try
            {
                return await api.GetFormDigest();
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetFormDigest", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetListByTitle(string listTitle)
        {
            try
            {
                return await api.GetListByTitle(listTitle);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetListByTitle", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetListByGuid(string listGuid)
        {
            try
            {
                return await api.GetListByGuid(listGuid);
            }
            catch (Exception e)
            {
                Debug.WriteLine("", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle)
        {
            try
            {
                return await api.GetListItemsByListTitle(listTitle);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetAllItemsInList", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle, string query)
        {
            try
            {
                return await _client.GetAsync(_sharepointUrl + "/api/web/lists/getbytitle('" + listTitle + "')/items?" + query);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetAllItemsInList", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid)
        {
            try
            {
                return await api.GetListItemsByListGuid(listGuid);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetAllListItemsByListGuid", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid, string query)
        {
            try
            {
                return await _client.GetAsync(_sharepointUrl + "/_api/web/lists(guid'" + listGuid + "')/items?" + query);
            }
            catch (Exception e)
            {
                Debug.WriteLine("GetListItemsByListGuid", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> AddListItemByListTitle(string formDigest, string listTitle, StringContent item)
        {
            try
            {
                return await api.AddListItemByListTitle(formDigest, listTitle, item);
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddListItem", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> AddListItemByListGuid(string formDigest, string listGuid, StringContent item)
        {
            try
            {
                return await api.AddListItemByListGuid(formDigest, listGuid, item);
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddListItemByListGuid", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> UpdateListItemByListTitle(string formDigest, string listTitle, StringContent item, string itemToBeReplacedId)
        {
            try
            {
                return await api.UpdateListItemByListTitle(formDigest, listTitle, item, itemToBeReplacedId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("UpdateListItem", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> UpdateListItemByListGuid(string formDigest, string listGuid, StringContent item, string itemToBeReplacedId)
        {
            try
            {
                return await api.UpdateListItemByListGuid(formDigest, listGuid, item, itemToBeReplacedId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("UpdateListItemByListGuid", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> DeleteListItemByListTitle(string formDigest, string listTitle, string itemToBeDeletedId)
        {
            try
            {
                return await api.DeleteListItemByListTitle(formDigest, listTitle, itemToBeDeletedId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("DeleteListItem", e.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage> DeleteListItemByListGuid(string formDigest, string listGuid, string itemToBeDeletedId)
        {
            try
            {
                return await api.DeleteListItemByListGuid(formDigest, listGuid, itemToBeDeletedId);
            }
            catch (Exception e)
            {
                Debug.WriteLine("DeleteListItemByListGuid", e.Message);
                return null;
            }
        }
    }
}