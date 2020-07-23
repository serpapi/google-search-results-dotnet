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
  public class EbaySearch : SerpApiSearch
  {
    public EbaySearch(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiSearch.EBAY_ENGINE) { }
  }
}