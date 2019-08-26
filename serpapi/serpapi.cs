using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

/***
 * Client for SerpApi.com
 */
namespace SerpApi
{
  public class SerpApiClient
  {
    public const string GOOGLE_ENGINE = "google";
    public const string BAIDU_ENGINE = "baidu";
    public const string BING_ENGINE = "bing";

    const string JSON_FORMAT = "json";

    const string HTML_FORMAT = "html";

    const string HOST = "https://serpapi.com";

    // contextual parameter provided to SerpApi
    public Hashtable parameterContext;
    // secret api key
    private string apiKeyContext;

    // search engine: google (default) or bing
    private string engineContext;

    public SerpApiClient(string apiKey, string engine = GOOGLE_ENGINE)
    {
      initialize(new Hashtable(), apiKey, engine);
    }

    public SerpApiClient(Hashtable parameter, string apiKey, string engine = GOOGLE_ENGINE)
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
        throw new SerpApiClientException("only google or bing or baidu are supported engine");
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
        throw new SerpApiClientException(data.GetValue("error").ToString());
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
        url += "?" + parameter;
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
          throw new SerpApiClientException("Http request fail: " + content);
        }
      }
      catch (Exception ex)
      {
        throw new SerpApiClientException(ex.ToString());
      }
      throw new SerpApiClientException("Oops something went very wrong");
    }
  }

  public class SerpApiClientException : Exception
  {
    public SerpApiClientException(string message) : base(message) { }
  }

  public class GoogleSearchResultsClient : SerpApiClient
  {

    public GoogleSearchResultsClient(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiClient.GOOGLE_ENGINE) { }

    public GoogleSearchResultsClient(String apiKey) : base(new Hashtable(), apiKey, SerpApiClient.GOOGLE_ENGINE) { }

    /*
     * Get list of location using Location API
     */
    public JArray GetLocation(string location, int limit)
    {
      string buffer = getRawResult("/locations.json", "location=" + location + "&limit=" + limit, false);
      return JArray.Parse(buffer);
    }
  }

  public class BingSearchResultsClient : SerpApiClient
  {
    public BingSearchResultsClient(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiClient.BING_ENGINE) { }
  }

  public class BaiduSearchResultsClient : SerpApiClient
  {
    public BaiduSearchResultsClient(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiClient.BAIDU_ENGINE) { }
  }
}