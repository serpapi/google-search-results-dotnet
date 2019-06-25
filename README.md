# Google Search Result in Dotnet / CSharp / .Net

This Dotnet package is meant to scrape and parse Google results using [SerpApi](https://serpapi.com).

This is work in progress..

The following services are provided:
 * [Search API](https://serpapi.com/search-api) 
 * [Location API](https://serpapi.com/locations-api) - not implemented
 * [Search Archive API](https://serpapi.com/search-archive-api)  - not implemented
 * [Account API](https://serpapi.com/account-api) - not implemented

Serp API provides a [script builder](https://serpapi.com/demo) to get you started quickly.

Feel free to fork this repository to add more backends.

## Installation

Install dotnet 
I have only tested on OSX only.

## Quick start 

```csharp
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
    String apiKey = "";

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
```

This example displays the top 3 coffee shop in Austin Texas found in the local_results.
Then it displays all 10 coffee shop found in the regular google search named: organic_results.

TODO
 * [ ] Add test
 * [x] Implement all 3 API
 * [ ] Provide advanced examples like: https://github.com/serpapi/google-search-results-ruby
 * [ ] Enable CI integration
 * [ ] Publish package
