using System;
using GoogleSearchResults;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

/***
 *
 */
namespace GoogleSearchResults
{
    class GoogleSearchResultsClient
    {
      const string HOST = "https://serpapi.com";

      // contextual parameter provided to SerpApi
      public Hashtable parameterContext;
      // secret api key
      private string apiKeyContext;

      // search engine: google (default) or bing
      private string engineContext;

      public GoogleSearchResultsClient(Hashtable parameter, string apiKey, string engine = "google")
      {
        // assign query parameter
        parameterContext = parameter;

        // store ApiKey
        apiKeyContext = apiKey;

        // set search engine
        Regex checkEngine = new Regex(@"(google|bing)", RegexOptions.Compiled);
        if (!checkEngine.IsMatch(engine))
        {
          throw new GoogleSearchResultsException("only google or bing are supported engine");
        }
        engineContext = engine;
      }

      public JObject GetJson()
      {
        parameterContext.Add("output", "json");
        // run asynchonous http query (.net framework implementation)
        Task<string> queryTask = Query("/search.json");
        // block until http query is completed
        queryTask.ConfigureAwait(true);
        // parse result into json
        string s = queryTask.Result;

        // parse json response (ignore http response status)
        JObject data = JObject.Parse(s);

        // report error if something went wrong
        if (data.ContainsKey("error"))
        {
          throw new GoogleSearchResultsException(data.GetValue("error").ToString());
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
        return System.Web.HttpUtility.UrlPathEncode(s);
      }

      public async Task<string> Query(string uri)
      {
        // build url
        String url = HOST + uri + "?" + GetParameter(parameterContext);
        try
        {
          HttpClient client = new HttpClient();
          Console.WriteLine(url);
          HttpResponseMessage response = await client.GetAsync(url);

          var content = await response.Content.ReadAsStringAsync();
          if(parameterContext["output"].Equals("json")) {
            return content;
          }
          
          // html response or other
          if (response.IsSuccessStatusCode)
          {
            return content;
          } else {
            throw new GoogleSearchResultsException("Http request fail: " + content);
          }
        }
        catch (Exception ex)
        {
          throw new GoogleSearchResultsException(ex.ToString());
        }
        throw new GoogleSearchResultsException("Oops something went very wrong");
      }
    }
  }

  class GoogleSearchResultsException : Exception
  {
    public GoogleSearchResultsException(string message) : base(message){}
  }
}