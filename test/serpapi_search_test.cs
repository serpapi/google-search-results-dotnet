using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerpApi;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerpApi.Test
{
  [TestClass]
  public class SerpApiSearchTest
  {
    private SerpApiSearch search;
    private String apiKey;

    private Hashtable ht;

    public SerpApiSearchTest()
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
      search = new SerpApiSearch(ht, apiKey);
      JObject data = search.GetJson();
      JArray coffeeShops = (JArray)data["organic_results"];
      Assert.IsNotNull(coffeeShops);

      // Release socket connection
      search.Close();
    }

  }

}