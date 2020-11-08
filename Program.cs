using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Ignition.HackerRank
{
  class Program
  {
    static void Main(string[] args)
    {
      var author = "epaga";
      var response = GetResponse(author);
      var titles = new List<string>();

      titles.AddRange(GetNames(response.data));
      if(response.total_pages> 1)
      {
        for(int i = 2; i<= response.total_pages; i++)
        {
          response = GetResponse(author, i);
          titles.AddRange(GetNames(response.data));
        }
      }

      titles.ForEach(x => Console.WriteLine(x));
    }

    private static List<string> GetNames(List<Article> data)
    {
      var names = new List<string>();
      foreach (var article in data)
      {
        if (!string.IsNullOrEmpty(article.title))
        {
          names.Add(article.title);
        }
        else if (!string.IsNullOrEmpty(article.story_title))
        {
          names.Add(article.story_title);
        }
      }
      return names;
    }

    private static ResponseObject GetResponse(string author, int? page = null)
    {
      var uri = page == null 
        ? $"https://jsonmock.hackerrank.com/api/articles?author={author}"
        : $"https://jsonmock.hackerrank.com/api/articles?author={author}&page={page}";

      var httpClient = new HttpClient();

      var response = httpClient.GetAsync(uri).Result;
      var content = response.Content.ReadAsStringAsync().Result;
      return JsonConvert.DeserializeObject<ResponseObject>(content);
    }
  }
}
