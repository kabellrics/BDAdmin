using Common;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BusinessComicVine : IBusinessComicVine
    {
        //public String apilink = @"search/?api_key=YOUR-KEY&format=json&field_list=image,issue_number,name,volume&limit=10&sort=name:asc&resources=issues&";
        private String Key;
        private RestClient restClient;
        public BusinessComicVine()
        {
            Key = "";
        }
        public async Task<List<ComicVineResult>> GetProposalForFichier(String Name)
        {
            try
            {
                restClient = new RestClient("https://comicvine.gamespot.com/api/search");
                var request = new RestRequest(Method.GET);
                request.AddParameter("api_key", Key);
                request.AddParameter("format", "json");
                request.AddParameter("field_list", "image,issue_number,name,volume");
                request.AddParameter("limit", "10");
                request.AddParameter("resources", "issue");
                request.AddParameter("query", Name);
                var response = await restClient.ExecuteAsync<RootObject>(request);
                RootObject resultObject = JsonConvert.DeserializeObject<RootObject>(response.Content);
                List<ComicVineResult> ProposalIssue = new List<ComicVineResult>();
                foreach(Result result in resultObject.results)
                {
                    ProposalIssue.Add(new ComicVineResult(result.image.original_url, result.issue_number, result.name, result.volume.name));
                }
                return ProposalIssue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
