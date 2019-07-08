using System;
using SerpApi;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

/***
 *
 */
namespace SerpApi
{
  public class GoogleSearchResultsClient
  {
    const string GOOGLE_ENGINE = "google";
    const string JSON_FORMAT = "json";

    const string HTML_FORMAT = "html";

    const string HOST = "https://serpapi.com";

    // contextual parameter provided to SerpApi
    public Hashtable parameterContext;
    // secret api key
    private string apiKeyContext;

    // search engine: google (default) or bing
    private string engineContext;

    public GoogleSearchResultsClient(string apiKey, string engine = GOOGLE_ENGINE)
    {
      initialize(new Hashtable(), apiKey, engine);
    }

    public GoogleSearchResultsClient(Hashtable parameter, string apiKey, string engine = GOOGLE_ENGINE)
    {
      initialize(parameter, apiKey, engine);
    }

    private void initialize(Hashtable parameter, string apiKey, string engine)
    {
      // assign query parameter
      parameterContext = parameter;

      // store ApiKey
      apiKeyContext = apiKey;

      // set search engine
      Regex checkEngine = new Regex(@"(google|bing|baidu)", RegexOptions.Compiled);
      if (!checkEngine.IsMatch(engine))
      {
        throw new SerpApiSearchResultsException("only google or bing or baidu are supported engine");
      }
      engineContext = engine;
    }

    /***
     * Get Json result
     */
    public JObject GetJson()
    {
      return getJsonResult("/search.json", GetParameter(parameterContext));
    }

    /*
     * Get list of location using Location API
     */
    public JArray GetLocation(string location, int limit)
    {
      if (engineContext != GOOGLE_ENGINE)
      {
        throw new SerpApiSearchResultsException("only " + GOOGLE_ENGINE + " is supported at this time by location API");
      }
      string buffer = getRawResult("/locations.json", "location=" + location + "&limit=" + limit, false);
      return JArray.Parse(buffer);
    }

    /***
     * Get search archive for JSON results
     */
    public JObject GetSearchArchiveJson(string searchId)
    {
      return getJsonResult("/searches/" + searchId + ".json", GetParameter(parameterContext));
    }

    /***
     * Get search HTML results
     */
    public string GetHtml()
    {
      return getRawResult("/search", GetParameter(parameterContext), false);
    }

    /***
   * Get search archive for JSON results
   */
    public JObject GetAccount()
    {
      return getJsonResult("/account", GetParameter(parameterContext));
    }

    public string getRawResult(string uri, string parameter, bool jsonEnabled)
    {
      // run asynchonous http query (.net framework implementation)
      Task<string> queryTask = Query(uri, parameter, jsonEnabled);
      // block until http query is completed
      queryTask.ConfigureAwait(true);
      // parse result into json
      return queryTask.Result;
    }

    public JObject getJsonResult(string uri, string parameter)
    {
      // parse json response (ignore http response status)
      JObject data = JObject.Parse(getRawResult(uri, parameter, true));

      // report error if something went wrong
      if (data.ContainsKey("error"))
      {
        throw new SerpApiSearchResultsException(data.GetValue("error").ToString());
      }
      return data;
    }


    public string GetParameter(Hashtable ht)
    {
      string s = "";
      int i = 0;
      foreach (string key in ht.Keys)
      {
        if (i > 0)
        {
          s += "&";
        }
        s += key + "=" + ht[key];
        i += 1;
      }
      if (apiKeyContext != null)
      {
        s += "&api_key=" + apiKeyContext;
      }
      return System.Web.HttpUtility.UrlPathEncode(s);
    }

    private async Task<string> Query(string uri, string parameter, bool jsonEnabled)
    {
      // build url
      String url = HOST + uri;
      if (parameter != null)
      {
        url += "?" + parameter ;
      }

      url += "&output=" + (jsonEnabled ? JSON_FORMAT : HTML_FORMAT);

      try
      {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        var content = await response.Content.ReadAsStringAsync();
        if (jsonEnabled)
        {
          return content;
        }

        // html response or other
        if (response.IsSuccessStatusCode)
        {
          return content;
        }
        else
        {
          throw new SerpApiSearchResultsException("Http request fail: " + content);
        }
      }
      catch (Exception ex)
      {
        throw new SerpApiSearchResultsException(ex.ToString());
      }
      throw new SerpApiSearchResultsException("Oops something went very wrong");
    }
  }

  class SerpApiSearchResultsException : Exception
  {
    public SerpApiSearchResultsException(string message) : base(message) { }
  }
}