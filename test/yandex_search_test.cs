using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerpApi;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerpApi.Test
{
  [TestClass]
  public class YandexSearchTest
  {
    private YandexSearch search;
    private String apiKey;

    private Hashtable ht;

    public YandexSearchTest()
    {
      apiKey = Environment.GetEnvironmentVariable("API_KEY"); 

      // search about coffee on Yandex
      parameter = new Hashtable();
      parameter.Add("q", "Coffee");
    }

    [TestMethod]
    public void TestGetJson()
    {
      search = new YandexSearch(parameter, apiKey);
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