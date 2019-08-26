using System;
using SerpApi;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Google
{
  class Program
  {

    static void Main(string[] args)
    {
      // secret api key from https://serpapi.com/dashboard
      String apiKey = Environment.GetEnvironmentVariable("API_KEY");
      if (apiKey == null)
      {
        Console.WriteLine("API_KEY environment variable be set your secret api key visit: https://serpapi.com/dashboard");
        Environment.Exit(1);
      }

      // Localized search for Coffee shop in Austin Texas
      Hashtable ht = new Hashtable();
      ht.Add("location", "Austin, Texas, United States");
      ht.Add("q", "Coffee");
      ht.Add("hl", "en");
      ht.Add("google_domain", "google.com");

      try
      {
        GoogleSearchResultsClient client = new GoogleSearchResultsClient(ht, apiKey);

        Console.WriteLine("Get location matching: Austin");
        JArray locations = client.GetLocation("Austin,TX", 3);
        foreach (JObject location in locations)
        {
          Console.WriteLine(location);
        }

        Console.WriteLine("Search coffee in Austin, Texas on Google [1 credit]");
        JObject data = client.GetJson();
        Console.WriteLine("local coffee shop");
        JArray coffeeShops = (JArray)data["local_results"];
        foreach (JObject coffeeShop in coffeeShops)
        {
          Console.WriteLine("Found: " + coffeeShop["title"]);
        }
        Console.WriteLine("organic result coffee shop");
        coffeeShops = (JArray)data["organic_results"];
        foreach (JObject coffeeShop in coffeeShops)
        {
          Console.WriteLine("Found: " + coffeeShop["title"]);
        }

        string id = (string)((JObject)data["search_metadata"])["id"];
        Console.WriteLine("Search from the archive: " + id + ". [0 credit]");
        JObject archivedSearch = client.GetSearchArchiveJson(id);
        foreach (JObject coffeeShop in (JArray)archivedSearch["organic_results"])
        {
          Console.WriteLine("Found: " + coffeeShop["title"]);
        }

        //  Get account information
        Console.WriteLine("Account information: [0 credit]");
        JObject account = client.GetAccount();
        Dictionary<string, string> dictObj = account.ToObject<Dictionary<string, string>>();
        foreach (string key in dictObj.Keys)
        {
          Console.WriteLine(key + " = " + dictObj[key]);
        }
        // write:
        // account_id = xx
        // api_key = xx
        // account_email = victor.benarbia@gmail.com
        // plan_id =
        // plan_name = No Plan
        // searches_per_month = 0
        // this_month_usage = 0
        // this_hour_searches = 1
        // last_hour_searches = 0

      }
      catch (SerpApiClientException ex)
      {
        Console.WriteLine("Exception:");
        Console.WriteLine(ex.ToString());
      }
    }
  }
}