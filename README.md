# Google/Bing/Baidu Search Result in Dotnet / CSharp / .Net

[![Build Status](https://travis-ci.org/serpapi/google-search-results-dotnet.svg?branch=master)](https://travis-ci.org/serpapi/google-search-results-dotnet)
[![NuGet version](https://badge.fury.io/nu/google-search-results-dotnet.svg)](https://badge.fury.io/nu/google-search-results-dotnet)

This Dotnet package is meant to scrape and parse results from Google, Bing, Baidu, Yandex, Yahoo, Ebay and more using [SerpApi](https://serpapi.com).

This extension is in development. But the code can be re-use for production because the API is already stable.

The following services are provided:
 * [Search API](https://serpapi.com/search-api) 
 * [Location API](https://serpapi.com/locations-api) - not implemented
 * [Search Archive API](https://serpapi.com/search-archive-api)  - not implemented
 * [Account API](https://serpapi.com/account-api) - not implemented

SerpApi provides a [script builder](https://serpapi.com/demo) to get you started quickly.


## Installation

To install the package.
```bash
dotnet add package google-search-results-dotnet --version 1.4.0
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
      String apiKey = "";

      // Localized search for Coffee shop in Austin Texas
      Hashtable ht = new Hashtable();
      ht.Add("location", "Austin, Texas, United States");
      ht.Add("q", "Coffee");
      ht.Add("hl", "en");
      ht.Add("google_domain", "google.com");

      try
      {
        BaiduSearchResultsClient client = new BaiduSearchResultsClient(ht, apiKey);

        Console.WriteLine("Search coffee in Austin, Texas on Google [1 credit]");
        JObject data = client.GetJson();
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

        // close socket
        client.Close()
      }
      catch (SerpApiClientException ex)
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

## Bing search engine
```csharp
client =  new BingSearchResultsClient(parameter, apiKey);
```

A full example is available here.
https://github.com/serpapi/google-search-results-dotnet/blob/master/example/bing/

## Baidu search engine
```csharp
client =  new BaiduSearchResultsClient(parameter, apiKey);
```

A full example is available here.
https://github.com/serpapi/google-search-results-dotnet/blob/master/example/baidu/

## Yahoo search engine
```csharp
client =  new YahooSearchResultsClient(parameter, apiKey);
```
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/yahoo_search_results_client_test.cs

## Yandex search engine
```csharp
client =  new YandexSearchResultsClient(parameter, apiKey);
```
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/yahoo_search_results_client_test.cs

## Ebay search engine
```csharp
client =  new EbaySearchResultsClient(parameter, apiKey);
```
test: https://github.com/serpapi/google-search-results-dotnet/blob/master/test/ebay_search_results_client_test.cs

## Test

This API is fully unit tested. The tests can be used as implementation examples.
https://github.com/serpapi/google-search-results-dotnet/tree/master/test

## Changes log
### 1.5
 * Add support for Yandex, Ebay, Yahoo
  
#### 1.4
 * Bug fix: Release Socket connection when requests finish. 
   Because Dotnet does not release the ressource when the HTTP Client is closed.
 * Add Yahoo support: YahooSearchResultsClient
 * Create only one client for all the connection
  
#### 1.3 
 * Add bing and baidu support
 * Allow custom HTTP timeout using: setTimeoutSeconds
 * Fix exception class visibility and renamed to SerpApiClientException

#### 1.2
 * Initial release matching SerpApi 1.2 internal API

TODO
---
 * [ ] Add advanced examples like: https://github.com/serpapi/google-search-results-ruby (wait for user feedback)
