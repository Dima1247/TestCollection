using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestWebApp.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class AsyncTestController : ControllerBase {
    private static readonly string[] Summaries = new[] {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    public string Get() {
      return "Check /data_light and /data_hard";
    }
    
    [HttpGet("data_light")]
    public Task<IEnumerable<AsyncTestData>> GetDataLight() {
      ThreadHelper.CheckThread(nameof(GetDataLight));
      return GetDataAsync();
    }

    [HttpGet("data_hard")]
    public async Task<IEnumerable<AsyncTestData>> GetDataHard() {
      ThreadHelper.CheckThread(nameof(GetDataHard));
      return await GetDataAsync();
    }

    private Task<IEnumerable<AsyncTestData>> GetDataAsync() {
      const int threadCount = 5;
      var currentTask = Task.Run(() => {
        ThreadHelper.CheckThread($"{nameof(GetDataAsync)}{0}_Inner");
        return GenerateAsyncTestData();
      });

      for (int i = 1; i <= threadCount; i++) {
        var local_i = i;
        currentTask = Task.Run(() => {
          ThreadHelper.CheckThread($"{nameof(GetDataAsync)}{local_i}_Inner");
          return currentTask;
        });
      }

      return currentTask;
    }

    private Task<IEnumerable<AsyncTestData>> GetDataAsync3() {
      ThreadHelper.CheckThread(nameof(GetDataAsync3));
      return Task.Run(() => {
        ThreadHelper.CheckThread(nameof(GetDataAsync3) + "_Inner");
        return GetDataAsync2();
      });
    }

    private Task<IEnumerable<AsyncTestData>> GetDataAsync2() {
      ThreadHelper.CheckThread(nameof(GetDataAsync2));
      return Task.Run((Func<Task<IEnumerable<AsyncTestData>>>)(() => {
        ThreadHelper.CheckThread(nameof(GetDataAsync2) + "_Inner");
        return this.GetDataAsync1();
      }));
    }

    private Task<IEnumerable<AsyncTestData>> GetDataAsync1() {
      ThreadHelper.CheckThread(nameof(GetDataAsync1));
      return Task.Run(() => {
        ThreadHelper.CheckThread(nameof(GetDataAsync1) + "_Inner");
        return GenerateAsyncTestData();
      });
    }

    private IEnumerable<AsyncTestData> GenerateAsyncTestData() {
      return Enumerable.Range(1, 5).Select(index => new AsyncTestData {
        Summary = Summaries[new Random().Next(Summaries.Length)]
      }).ToArray();
    }

    private static async Task<JObject> GetJsonAsync(Uri uri) {
      var httpClientHandler = new HttpClientHandler {
        ClientCertificateOptions = ClientCertificateOption.Manual,
        ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
      };
      using (var client = new HttpClient(httpClientHandler)) {
        var jsonString = await client.GetStringAsync(uri);
        return JObject.Parse(jsonString);
      }
    }
  }

  #region Test classes

  public class AsyncTestData {
    public string Summary { get; set; }
  }

  public class MyAwaitableMiddleware {
    private readonly RequestDelegate _next;

    public MyAwaitableMiddleware(RequestDelegate next) {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
      ThreadHelper.CheckThread(nameof(MyAwaitableMiddleware));
      //await Task.Delay(1000);
      await _next.Invoke(context);
    }

    public static void ApplyMiddleware(IApplicationBuilder app) {
      app.UseMiddleware<MyAwaitableMiddleware>();
    }
  }

  public class MyMiddleware {
    private readonly RequestDelegate _next;

    public MyMiddleware(RequestDelegate next) {
      _next = next;
    }

    public Task Invoke(HttpContext context) {
      ThreadHelper.CheckThread(nameof(MyMiddleware));
      return _next.Invoke(context);
    }

    public static void ApplyMiddleware(IApplicationBuilder app) {
      app.UseMiddleware<MyMiddleware>();
    }
  }

  #endregion

}