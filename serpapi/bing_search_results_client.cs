using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

/***
 * Client for SerpApi.com
 */
namespace SerpApi
{
  public class BingSearchResultsClient : SerpApiClient
  {
    public BingSearchResultsClient(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiClient.BING_ENGINE) { }
  }
}