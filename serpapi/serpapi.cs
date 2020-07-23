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
  public class SerpApiSearch
  {
    public const string GOOGLE_ENGINE = "google";
    public const string BAIDU_ENGINE = "baidu";
    public const string BING_ENGINE = "bing";
    public const string YAHOO_ENGINE = "yahoo";
    public const string YANDEX_ENGINE = "yandex";
    public const string EBAY_ENGINE = "ebay";

    const string JSON_FORMAT = "json";

    const string HTML_FORMAT = "html";

    const string HOST = "https://serpapi.com";

    // contextual parameter provided to SerpApi
    public Hashtable parameterContext;
    // secret api key
    private string apiKeyContext;

    // search engine: google (default) or bing
    private string engineContext;

    // Core HTTP search
    public HttpClient search;

    public SerpApiSearch(string apiKey, string engine = GOOGLE_ENGINE)
    {
      initialize(new Hashtable(), apiKey, engine);
    }

    public SerpApiSearch(Hashtable parameter, string apiKey, string engine = GOOGLE_ENGINE)
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
      engineContext = engine;

      // initialize clean
      this.search = new HttpClient();

      // set default timeout to 60s
      this.setTimeoutSeconds(60);
    }

    /***
     * Set HTTP timeout in seconds
     */
    public void setTimeoutSeconds(int seconds)
    {
      this.search.Timeout = TimeSpan.FromSeconds(seconds);
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
        throw new SerpApiSearchException(data.GetValue("error").ToString());
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

    /***
     * Close socket connection associated to HTTP search
     */
    public void Close()
    {
      this.search.Dispose();
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
        HttpResponseMessage response = await this.search.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        if (jsonEnabled)
        {
          response.Dispose();
          return content;
        }

        // html response or other
        if (response.IsSuccessStatusCode)
        {
          response.Dispose();
          return content;
        }
        else
        {
          throw new SerpApiSearchException("Http request fail: " + content);
        }
      }
      catch (Exception ex)
      {
        throw new SerpApiSearchException(ex.ToString());
      }
      throw new SerpApiSearchException("Oops something went very wrong");
    }
  }

  public class SerpApiSearchException : Exception
  {
    public SerpApiSearchException(string message) : base(message) { }
  }

}
