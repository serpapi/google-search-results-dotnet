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
  public class YahooSearch : SerpApiSearch
  {
    public YahooSearch(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiSearch.YAHOO_ENGINE) { }
  }
}