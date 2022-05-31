using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TestWebApp.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class FromBodyTestController : ControllerBase {
    [HttpGet]
    public string Get() {
      return "Check /data_light and /data_hard";
    }

    [HttpPost]
    public ActionTestData Post1(KeyValuePair<int, ActionTestData> testData) {
      return testData.Value;
    }

    [HttpPost("{id}")]
    public ActionTestData Post2(int id, KeyValuePair<int, ActionTestData> testData) {
      return testData.Key == id ? testData.Value : null;
    }

    [HttpPost("Extended")]
    public ActionTestData Post3(KeyValuePair<int, ActionTestData> testData, [FromQuery] string summary) {
      return testData.Value.Summary == summary ? testData.Value : null;
    }

    [HttpPost("Array")]
    public int[] Post4(int[] ids) {
      return ids;
    }

    [HttpPost("String")]
    public string Post5([FromBody] string str) {
      return str;
    }
  }

  public class ActionTestData {
    public string Summary { get; set; }
  }
}
