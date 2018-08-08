using Refit;
using SpevoCore.Models.FormDigest;
using SpevoCore.Models.User;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpevoCore.Services.Sharepoint_API
{
    public interface ISharepointAPI
    {
        [Get("/_api/web/currentuser?")]
        Task<UserModel> GetCurrentUser();

        Task<HttpResponseMessage> GetSiteUsers(string query);

        [Post("/_api/contextinfo")]
        Task<FormDigestModel> GetFormDigest();

        [Get("/_api/web/lists/getbytitle('{listTitle}')")]
        Task<HttpResponseMessage> GetListByTitle(string listTitle);

        [Get("/_api/web/lists(guid'{listGuid}')")]
        Task<HttpResponseMessage> GetListByGuid(string listGuid);

        [Get("/_api/web/lists/getbytitle('{listTitle}')/items")]
        Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle);

        Task<HttpResponseMessage> GetListItemsByListTitle(string listTitle, string query);

        [Get("/_api/web/lists(guid'{listGuid}')/items")]
        Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid);

        Task<HttpResponseMessage> GetListItemsByListGuid(string listGuid, string query);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')/items")]
        Task<HttpResponseMessage> AddListItemByListTitle([Header("X-RequestDigest")] string formDigest,
                                  string listTitle,
                                  StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose")]
        [Post("/_api/web/lists(guid'{listGuid}')/items")]
        Task<HttpResponseMessage> AddListItemByListGuid([Header("X-RequestDigest")] string formDigest,
                                           string listGuid,
                                           StringContent item);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "X-HTTP-Method:MERGE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')/items({itemToBeReplacedId})")]
        Task<HttpResponseMessage> UpdateListItemByListTitle([Header("X-RequestDigest")] string formDigest,
                                     string listTitle,
                                     StringContent item,
                                     string itemToBeReplacedId);

        [Headers("Accept:application/json;odata=verbose",
                 "Content-Type:application/json;odata=verbose",
                 "X-HTTP-Method:MERGE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists(guid'{listGuid}')/items({itemToBeReplacedId})")]
        Task<HttpResponseMessage> UpdateListItemByListGuid([Header("X-RequestDigest")] string formDigest,
                                              string listGuid,
                                              StringContent item,
                                              string itemToBeReplacedId);

        [Headers("X-HTTP-Method:DELETE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists/getbytitle('{listTitle}')/items({itemToBeDeletedId})")]
        Task<HttpResponseMessage> DeleteListItemByListTitle([Header("X-RequestDigest")] string formDigest,
                                    string listTitle,
                                    string itemToBeDeletedId);

        [Headers("X-HTTP-Method:DELETE",
                 "IF-MATCH:*")]
        [Post("/_api/web/lists(guid'{listGuid}')/items({itemToBeDeletedId})")]
        Task<HttpResponseMessage> DeleteListItemByListGuid([Header("X-RequestDigest")] string formDigest,
                                              string listGuid,
                                              string itemToBeDeletedId);
    }
}