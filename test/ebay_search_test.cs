using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerpApi;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerpApi.Test
{
  [TestClass]
  public class EbaySearchTest
  {
    private EbaySearch search;
    private String apiKey;

    private Hashtable ht;

    public EbaySearchTest()
    {
      apiKey = Environment.GetEnvironmentVariable("API_KEY");

      // search about coffee on ebay
      ht = new Hashtable();
      ht.Add("q", "Coffee");
    }

    [TestMethod]
    public void TestGetJson()
    {
      search = new EbaySearch(ht, apiKey);
      JObject data = search.GetJson();

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