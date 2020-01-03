using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerpApi;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerpApi.Test
{
  [TestClass]
  public class BaiduSearchResultTest
  {
    private BaiduSearchResultsClient client;
    private String apiKey;

    private Hashtable ht;

    public BaiduSearchResultTest()
    {
      apiKey = Environment.GetEnvironmentVariable("API_KEY");

      // Localized search for Coffee shop in Austin Texas
      ht = new Hashtable();
      ht.Add("location", "Austin, Texas, United States");
      ht.Add("q", "Coffee");
      ht.Add("hl", "en");
      ht.Add("google_domain", "google.com");
    }

    [TestMethod]
    public void TestGetJson()
    {
      client = new BaiduSearchResultsClient(ht, apiKey);
      JObject data = client.GetJson();
      JArray coffeeShops = (JArray)data["local_results"]["places"];
      int counter = 0;
      foreach (JObject coffeeShop in coffeeShops)
      {
        Assert.IsNotNull(coffeeShop["title"]);
        counter++;
      }
      Assert.IsTrue(counter >= 1);

      coffeeShops = (JArray)data["organic_results"];
      Assert.IsNotNull(coffeeShops);
      foreach (JObject coffeeShop in coffeeShops)
      {
        Console.WriteLine("Found: " + coffeeShop["title"]);
        Assert.IsNotNull(coffeeShop["title"]);
      }
    }
  }
}