using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerpApi;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerpApi.Test
{
  [TestClass]
  public class EbaySearchResultsClientTest
  {
    private EbaySearchResultsClient client;
    private String apiKey;

    private Hashtable ht;

    public EbaySearchResultsClientTest()
    {
      apiKey = Environment.GetEnvironmentVariable("API_KEY");

      // Localized search for Coffee shop in Austin Texas
      ht = new Hashtable();
      ht.Add("q", "Coffee");
    }

    [TestMethod]
    public void TestGetJson()
    {
      client = new EbaySearchResultsClient(ht, apiKey);
      JObject data = client.GetJson();

      Assert.AreEqual(data["search_metadata"]["status"], "Success");

      JArray coffeeShops = (JArray)data["organic_results"];
      Assert.IsNotNull(coffeeShops);
      foreach (JObject coffeeShop in coffeeShops)
      {
        Console.WriteLine("Found: " + coffeeShop["title"]);
        Assert.IsNotNull(coffeeShop["title"]);
      }
    }
  }
}