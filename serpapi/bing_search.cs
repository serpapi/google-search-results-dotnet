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
  public class BingSearch : SerpApiSearch
  {
    public BingSearch(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiSearch.BING_ENGINE) { }
  }
}