# Search Results in Dotnet / CSharp / .Net powered by SerpApi.com

![dotnet 2.2 - build / test](https://github.com/serpapi/google-search-results-dotnet/workflows/dotnet%202.2%20-%20build%20/%20test/badge.svg)
[![NuGet version](https://badge.fury.io/nu/google-search-results-dotnet.svg)](https://badge.fury.io/nu/google-search-results-dotnet)

This Dotnet package is meant to scrape and parse results from Google, Bing, Baidu, Yandex, Yahoo, Ebay and more using [SerpApi](https://serpapi.com).

Master is currently tracking the version 2.0 of this API which has been release yet. See other branch for released version.

The following services are provided:
 * [Search API](https://serpapi.com/search-api) 
 * [Location API](https://serpapi.com/locations-api) - not implemented
 * [Search Archive API](https://serpapi.com/search-archive-api)  - not implemented
 * [Account API](https://serpapi.com/account-api) - not implemented

SerpApi provides a [script builder](https://serpapi.com/demo) to get you started quickly.

## Installation

To install the package.
```bash
dotnet add package google-search-results-dotnet --version 2.0.0
```

More commands available at https://www.nuget.org/packages/google-search-results-dotnet

## Quick start 

Let's run a search on Google.

```csharp
using System;
using SerpApi;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Baidu
{
  class Program
  {

    static void Main(string[] args)
    {
      // secret api key from https://serpapi.com/dashboard
      String apiKey = "<secret_api_key>";

      // Localized search for Coffee shop in Austin Texas
      Hashtable ht = new Hashtable();
      ht.Add("location", "Austin, Texas, United States");
      ht.Add("q", "Coffee");
      ht.Add("hl", "en");
      ht.Add("google_domain", "google.com");

      try
      {
         GoogleSearch search = new GoogleSearch(ht, apiKey);

        Console.WriteLine("Search coffee in Austin, Texas on Google [1 credit]");
        JObject data = search.GetJson();
        Console.WriteLine("local coffee shop");
        JArray coffeeShops = (JArray)data["local_results"]["places"];
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
        JObject archivedSearch = search.GetSearchArchiveJson(id);
        foreach (JObject coffeeShop in (JArray)archivedSearch["organic_results"])
        {
          Console.WriteLine("Found: " + coffeeShop["title"]);
        }

        //  Get account information
        Console.WriteLine("Account information: [0 credit]");
        JObject account = search.GetAccount();
        Dictionary<string, string> dictObj = account.ToObject<Dictionary<string, string>>();
        foreach (string key in dictObj.Keys)
        {
          Console.WriteLine(key + " = " + dictObj[key]);
        }

        // close socket
        search.Close()
      }
      catch (SerpApiSearchException ex)
      {
        Console.WriteLine("Exception:");
        Console.WriteLine(ex.ToString());
      }
    }
  }
}
```

This example displays the top 3 coffee shop in Austin Texas found in the local_results.
Then it displays all 10 coffee shop found in the regular google search named: organic_results.

## Google search engine
```csharp
GoogleSearch search = GoogleSearch(parameter, apiKey);
JObject data = search.GetJson();
```

test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/google_search_test.cs  
doc: https://serpapi.com/search-api

## Bing search engine
```csharp
BingSearch search = BingSearch(parameter, apiKey);
JObject data = search.GetJson();
```

A full example is available here.
https://github.com/serpapi/google-search-results-dotnet/blob/master/example/bing/  
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/bing_search_test.cs  
doc: https://serpapi.com/bing-search-api


## Baidu search engine
```csharp
BaiduSearch search = BaiduSearch(parameter, apiKey);
JObject data = search.GetJson();
```

A full example is available here.
https://github.com/serpapi/google-search-results-dotnet/blob/master/example/baidu/  
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/baidu_search_test.cs  
doc: https://serpapi.com/baidy-search-api

## Yahoo search engine
```csharp
YahooSearch search = YahooSearch(parameter, apiKey);
JObject data = search.GetJson();
```
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/yahoo_search_test.cs  
doc: https://serpapi.com/yahoo-search-api  

## Yandex search engine
```csharp
YandexSearch search = YandexSearch(parameter, apiKey);
JObject data = search.GetJson();
```
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/yandex_search_test.cs  
doc: https://serpapi.com/yandex-search-api 

## Ebay search engine
```csharp
EbaySearch search = EbaySearch(parameter, apiKey);
JObject data = search.GetJson();
```
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/ebay_search_test.cs  
doc: https://serpapi.com/ebay-search-api 

## Generic search for other search engine
Here an example using walmart as search engine.
```csharp
SerpApiSearch search = SerpApiSearch(parameter, apiKey, "walmart")
```
see the list of engine supported from the [documentation](https://serpapi.com/search-api).

## Test
This library is fully unit tested.  
The tests can be used as implementation examples.  
https://github.com/serpapi/google-search-results-dotnet/tree/master/test

## Changes log
### 2.0
 * Reduce class name to <engine>Search 

### 1.5
 * Add support for Yandex, Ebay, Yahoo
  
#### 1.4
 * Bug fix: Release Socket connection when requests finish. 
   Because Dotnet does not release the ressource when the HTTP Client is closed.
 * Add Yahoo support: YahooSearch
 * Create only one search for all the connection
  
#### 1.3 
 * Add bing and baidu support
 * Allow custom HTTP timeout using: setTimeoutSeconds
 * Fix exception class visibility and renamed to SerpApiSearchException

#### 1.2
 * Initial release matching SerpApi 1.2 internal API

TODO
---
 * [ ] Add advanced examples like: https://github.com/serpapi/google-search-results-ruby (wait for user feedback)
