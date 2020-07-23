using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerpApi;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerpApi.Test
{
  [TestClass]
  public class YahooSearchTest
  {
    private YahooSearch search;
    private String apiKey;

    private Hashtable ht;

    public YahooSearchTest()
    {
      apiKey = Environment.GetEnvironmentVariable("API_KEY");
      parameter = new Hashtable();
      parameter.Add("q", "Coffee");
    }

    [TestMethod]
    public void TestGetJson()
    {
      search = new YahooSearch(parameter, apiKey);
      JObject data = search.GetJson();
      Assert.AreEqual(data["search_metadata"]["status"], "Success");

      // access organic results
      JArray coffeeShops = (JArray)data["organic_results"];
      Assert.IsNotNull(coffeeShops);
      // print the name of all coffee shop
      foreach (JObject coffeeShop in coffeeShops)
      {
        Console.WriteLine("Found: " + coffeeShop["title"]);
        Assert.IsNotNull(coffeeShop["title"]);
      }
    }
  }
}