using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tj = System.Text.Json;

namespace TestApp.Tests {

  public class JsonTest : ITest {
    public class TestObject {
      private string _privateString = "123";
      private readonly string _privateReadonlyString = "234";
      private static string _privateStaticString = "345";
      private readonly static string _privateReadonlyStaticString = "456";
      private const string _privateConstString = "567";

      public string _publicString = "123";
      public readonly string _publicReadonlyString = "234";
      public static string _publicStaticString = "345";
      public readonly static string _publicReadonlyStaticString = "456";
      public const string _publicConstString = "567";

      private string PrivateString => "123";
      private static string PrivateStaticString => "345";

      public string PublicString => "123";
      public static string PublicStaticString => "345";
    }

    public class Test {
      public string Name { get; set; }
      public int? Age { get; set; }
      public DateTimeOffset? DoB { get; set; }
      public Test InnerTest { get; set; }
    }

    ///<inheritdoc/>
    public async Task StartAsync() {
      //await Case1();
      await Case2();
    }

    private async Task Case1() {
      Console.WriteLine("Case 1.");

      List<Test> list = new List<Test> {
        new Test {
          Name = "Dima",
          Age = 21
        }
      };

      IEnumerable<Test> enumer = new List<Test> {
        new Test {
          Name = "Dima",
          Age = 21
        }
      };

      var lJson = JsonConvert.SerializeObject(list);
      var eJson = JsonConvert.SerializeObject(enumer);

      Console.WriteLine(lJson);
      Console.WriteLine(eJson);
      Console.WriteLine();
    }

    private async Task Case2() {
      Console.WriteLine("Case 2.");

      var testObj = new TestObject();
      var lJson = JsonConvert.SerializeObject(testObj);

      Console.WriteLine(lJson);
      Console.WriteLine();
    }

    private async Task Trash() {
      var testD1 = new Test {
        Name = "testD1",
        InnerTest = new Test {
          Name = "testD1.innerTest",
          Age = 0
        }
      };

      var testD2 = new Test {
        InnerTest = null
      };

      Test testD3 = null;

      if (testD1?.InnerTest?.Age == 0) {
        Console.WriteLine("D1");
      }

      if (testD2?.InnerTest?.Age == 0) {
        Console.WriteLine("D2");
      }

      if (testD3?.InnerTest?.Age == 0) {
        Console.WriteLine("D3");
      }

      //Console.WriteLine("Hello World!");

      //var jsonString = "{\"name\": \"true\", \"name2\": null}";
      ////var jsonString = string.Empty;
      //var jsonObject = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(jsonString);
      //var val1 = jsonObject.TryGetProperty("name", out var name) ? name.ToString() : string.Empty;

      //var str = "{ \"1440167924916\":{ \"id\":1440167924916,\"type\":\"text\",\"content\":\"It\'s a test!\"} }";
      //var jsonString = "{\"name\": \"true\", \"name2\": null}";
      //var str2 = "{'name': 'true', 'name2': null}";
      //var strObj = JsonConvert.DeserializeObject(str2).ToString();

      //var str3 = 
      //var strObj2 = System.Text.Json.JsonSerializer.Deserialize<object>(str2).ToString();

      //var rand = new Random(1);
      //Console.WriteLine(rand.Next());
      //Console.WriteLine(rand.Next());
      //Console.WriteLine(rand.Next());
      //Console.WriteLine(rand.Next());





      var testUser = new Test { Name = "Dima", Age = 10 };

      //var testUser = new List<Test> { new Test { Name = "Dima", Age = 10 }, new Test { Name = "Den", Age = 12 } };

      var nJson = JsonConvert.SerializeObject(testUser, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
      var tJson = tj.JsonSerializer.Serialize(testUser, new tj.JsonSerializerOptions { IgnoreNullValues = true });

      Console.WriteLine("newtonsoft");
      JObject nobj = JsonConvert.DeserializeObject<JObject>(nJson);

      foreach (var pair in nobj) {
        Console.WriteLine("----- " + nobj.GetValue(pair.Key));
        Console.WriteLine(pair.Key + " - " + pair.Value);
      }

      //Console.WriteLine("newtonsoft");
      //JArray narr = JsonConvert.DeserializeObject<JArray>(nJson);

      //foreach (var pair in narr)
      //{
      //    if (pair.Type == JTokenType.Object)
      //    {
      //        var obj = (JObject)pair;
      //        var name = obj.GetValue("name").Value<int>();
      //        Console.WriteLine(name);
      //    }
      //}

      Console.WriteLine("textjson");
      tj.JsonElement tobj = tj.JsonSerializer.Deserialize<tj.JsonElement>(tJson);

      foreach (var pair in tobj.EnumerateObject()) {
        Console.WriteLine(pair.Name + " - " + pair.Value);
      }

      //Console.WriteLine("textjson");
      //tj.JsonElement tarr = tj.JsonSerializer.Deserialize<tj.JsonElement>(tJson);

      //foreach (var pair in tarr.EnumerateArray())
      //{
      //    if (pair.ValueKind == tj.JsonValueKind.Object)
      //    {
      //        pair.TryGetProperty("name", out var val);
      //        Console.WriteLine(val.GetInt32());
      //    }
      //}




      var testUser1 = new Test { Name = "Dima", Age = 10, DoB = DateTimeOffset.UtcNow, InnerTest = new Test { Name = "Dima", Age = 10 } };
      var testUser2 = new Test { Name = "Dima", Age = 10, DoB = DateTimeOffset.UtcNow, InnerTest = new Test { Name = "Dima", Age = 10 } };

      var nJson1 = JsonConvert.SerializeObject(testUser1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
      var nJson2 = JsonConvert.SerializeObject(testUser2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
      var tJson1 = tj.JsonSerializer.Serialize(testUser1, new tj.JsonSerializerOptions { IgnoreNullValues = true });
      var tJson2 = tj.JsonSerializer.Serialize(testUser2, new tj.JsonSerializerOptions { IgnoreNullValues = true });

      Console.WriteLine("\n\n\n");

      Console.WriteLine("newtonsoft");
      //var nobj1 = JsonConvert.DeserializeObject<JObject>(nJson1);
      //var nobj2 = JsonConvert.DeserializeObject<JObject>(nJson2);
      //Console.WriteLine(nobj1.ToString());
      //var res1 = JObject.DeepEquals(nobj1, nobj2);
      //Console.WriteLine(res1);

      Console.WriteLine("textjson");
      tj.JsonElement tobj1 = tj.JsonSerializer.Deserialize<tj.JsonElement>(tJson1);
      tj.JsonElement tobj2 = tj.JsonSerializer.Deserialize<tj.JsonElement>(tJson2);

      var nobj1 = JsonConvert.DeserializeObject<JToken>(tobj1.ToString());
      var nobj2 = JsonConvert.DeserializeObject<JToken>(tobj2.ToString());

      var prop = tobj1.GetProperty("DoB");
      Console.WriteLine(tj.JsonSerializer.Deserialize<DateTimeOffset>(prop.GetRawText()));
      //if (prop.ValueKind == tj.JsonValueKind.String)
      //    Console.WriteLine(tobj1.GetProperty("Age").GetString());

      var res1 = JToken.DeepEquals(nobj1, nobj2);
      Console.WriteLine(res1);

      var res2 = tobj1.ToString().Equals(tobj2.ToString());
      Console.WriteLine(res2);


      Console.WriteLine("\n\nemptycheck");

      var str = "";

      try {
        var newtJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
        Console.WriteLine("newt: " + newtJson);
      }
      catch (JsonException je) {
        Console.WriteLine(je.Message);
      }

      try {

        var textJson = tj.JsonSerializer.Deserialize<Dictionary<string, object>>(str);
        Console.WriteLine("text: " + textJson);
      }
      catch (tj.JsonException je) {
        Console.WriteLine(je.Message);
      }
    }

  }
}
