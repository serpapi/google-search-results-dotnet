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
  public class YandexSearch : SerpApiSearch
  {
    public YandexSearch(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiSearch.YANDEX_ENGINE) { }
  }
}