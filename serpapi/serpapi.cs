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
    public HttpClient client;

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
      this.parameterContext = parameter;

      // store ApiKey
      this.apiKeyContext = apiKey;

      // set search engine
      this.engineContext = engine;

      // initialize clean
      this.client = new HttpClient();

      // set default timeout to 60s
      this.setTimeoutSeconds(60);
    }

    /***
     * Set HTTP timeout in seconds
     */
    public void setTimeoutSeconds(int seconds)
    {
      this.client.Timeout = TimeSpan.FromSeconds(seconds);
    }

    /***
     * Get Json result
     */
    public JObject GetJson()
    {
      return getJsonResult("/search.json", GetParameter(true));
    }

    /***
     * Get search archive for JSON results
     */
    public JObject GetSearchArchiveJson(string searchId)
    {
      return getJsonResult("/searches/" + searchId + ".json", GetParameter(true));
    }

    /***
     * Get search HTML results
     */
    public string GetHtml()
    {
      return getRawResult("/search", GetParameter(false), false);
    }

    /***
   * Get search archive for JSON results
   */
    public JObject GetAccount()
    {
      return getJsonResult("/account", GetParameter(true));
    }

    public string getRawResult(string uri, string parameter, bool jsonEnabled)
    {
      // run asynchonous http query (.net framework implementation)
      Task<string> queryTask = createQuery(uri, parameter, jsonEnabled);
      // block until http query is completed
      queryTask.ConfigureAwait(true);
      // parse result into json
      return queryTask.Result;
    }

    public JObject getJsonResult(string uri, string parameter)
    {
      // get json result
      string buffer = getRawResult(uri, parameter, true);
      // parse json response (ignore http response status)
      JObject data = JObject.Parse(buffer);
      // report error if something went wrong
      if (data.ContainsKey("error"))
      {
        throw new SerpApiSearchException(data.GetValue("error").ToString());
      }
      return data;
    }

    // Convert parmaterContext into URL request.
    // 
    // note:
    //  - C# URL encoding is pretty buggy and the API provides method which are not functional.
    //  - System.Web.HttpUtility.UrlEncode breaks if apply the full URL
    ///
    public string GetParameter(bool jsonEnabled)
    {
      string s = "";
      foreach (DictionaryEntry entry in this.parameterContext)
      {
        if (s != "")
        {
          s += "&";
        }
        // encode each value in case of special character
        s += entry.Key + "=" + System.Web.HttpUtility.UrlEncode((string)entry.Value, System.Text.Encoding.UTF8);
      }
      if (apiKeyContext != null)
      {
        s += "&api_key=" + apiKeyContext;
      }

      // add 
      s += "&output=" + (jsonEnabled ? JSON_FORMAT : HTML_FORMAT);
      return s;
    }

    /***
     * Close socket connection associated to HTTP search
     */
    public void Close()
    {
      this.client.Dispose();
    }

    private async Task<string> createQuery(string uri, string parameter, bool jsonEnabled)
    {
      // build url
      String url = HOST + uri + "?" + parameter;
      // display url for debug: 
      //Console.WriteLine("url: " + url);
      try
      {
        HttpResponseMessage response = await this.client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        // return raw JSON
        if (jsonEnabled)
        {
          response.Dispose();
          return content;
        }
        // HTML response or other
        if (response.IsSuccessStatusCode)
        {
          response.Dispose();
          return content;
        }
        else
        {
          response.Dispose();
          throw new SerpApiSearchException("Http request fail: " + content);
        }
      }
      catch (Exception ex)
      {
        // handle HTTP issues
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
