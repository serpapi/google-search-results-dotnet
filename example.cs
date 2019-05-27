using System;
using GoogleSearchResults;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

class Program
{

  static void Main(string[] args)
  {
    // secret api key from https://serpapi.com/dashboard
    String apiKey = Environment.GetEnvironmentVariable("API_KEY");

    // Localized search for Coffee shop in Austin Texas
    Hashtable ht = new Hashtable();
    ht.Add("location", "Austin, Texas, United States");
    ht.Add("q", "Coffee");
    ht.Add("hl", "en");
    ht.Add("google_domain", "google.com");

    try
    {
      GoogleSearchResultsClient client = new GoogleSearchResultsClient(ht, apiKey);
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
    }
    catch (GoogleSearchResultsException ex)
    {
      Console.WriteLine("Exception:");
      Console.WriteLine(ex.ToString());
    }
  }
}