using Microsoft.VisualStudio.TestTools.UnitTesting;
using SerpApi;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SerpApi.Test
{
  [TestClass]
  public class GoogleSearchResultTest
  {
    private GoogleSearchResultsClient client;
    private String apiKey;

    private Hashtable ht;

    public GoogleSearchResultTest()
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
    public void TestGetLocation()
    {
      client = new GoogleSearchResultsClient(apiKey);
      JArray locations = client.GetLocation("Austin,TX", 3);
      int counter = 0;
      foreach (JObject location in locations)
      {
        counter++;
        Assert.IsNotNull(location);
        Assert.IsNotNull(location.GetValue("id"));
        Assert.IsNotNull(location.GetValue("name"));
        Assert.IsNotNull(location.GetValue("google_id"));
        Assert.IsNotNull(location.GetValue("gps"));
        // Console.WriteLine(location);
      }

      Assert.AreEqual(1, counter);
    }

    [TestMethod]
    public void TestGetJson()
    {
      client = new GoogleSearchResultsClient(ht, apiKey);
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

      // Release socket connection
      client.Close();
    }

    [TestMethod]
    public void TestGetArchive()
    {
      // Skip test on travis ci
      if (apiKey == null || apiKey == "demo")
      {
        return;
      }

      client = new GoogleSearchResultsClient(ht, apiKey);
      JObject data = client.GetJson();
      string id = (string)((JObject)data["search_metadata"])["id"];
      JObject archivedSearch = client.GetSearchArchiveJson(id);
      int expected = GetSize((JArray)data["organic_results"]);
      int actual = GetSize((JArray)archivedSearch["organic_results"]);
      Assert.IsTrue(expected == actual);
    }

    public void TestGetAccount()
    {
      // Skip test on travis ci
      if (apiKey == null || apiKey == "demo")
      {
        return;
      }
      JObject account = client.GetAccount();
      Dictionary<string, string> dict = account.ToObject<Dictionary<string, string>>();
      Assert.IsNotNull(dict["account_id"]);
      Assert.IsNotNull(dict["plan_id"]);
      Assert.AreEqual(dict["apiKey"], apiKey);
    }

    [TestMethod]
    public void TestGetHtml()
    {
      client = new GoogleSearchResultsClient(ht, apiKey);
      string htmlContent = client.GetHtml();
      Assert.IsNotNull(htmlContent);
      //Console.WriteLine(htmlContent);
      Assert.IsTrue(htmlContent.Contains("</body>"));

      // Release socket connection
      client.Close();
    }

    private int GetSize(JArray array)
    {
      int size = 0;
      foreach (JObject e in array)
      {
        size++;
      }
      return size;
    }
  }

}