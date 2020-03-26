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
  public class BaiduSearchResultsClient : SerpApiClient
  {
    public BaiduSearchResultsClient(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiClient.BAIDU_ENGINE) { }
  }
}