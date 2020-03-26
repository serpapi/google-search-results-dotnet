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
  public class YandexSearchResultsClient : SerpApiClient
  {
    public YandexSearchResultsClient(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiClient.YANDEX_ENGINE) { }
  }
}