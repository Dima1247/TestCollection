using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TestApp.Tests {

  public class ExtraTest : ITest {
    ///<inheritdoc/>
    public async Task StartAsync() {
      //Case1();
      //Case2();
      //Case3();
      Case4();
    }

    private void Case1() {
      Console.WriteLine("Case 1:");

      string val = null;

      Console.WriteLine("some null string: " + val);

      Console.WriteLine("null check: " + !new ExtraTest());
    }

    public static bool operator !(ExtraTest obj)
        => false;


    public class Message {
      public string Title { get; set; }
    }

    private static string Case2(Message message) {
      Console.WriteLine("Case 2:");
      var serializer = new XmlSerializer(typeof(Message));

      using var ms = new MemoryStream();
      using var sw = XmlWriter.Create(ms);
      serializer.Serialize(sw, message);

      return Encoding.UTF8.GetString(ms.ToArray());
    }

    private static void Case3() {
      Console.WriteLine("Case 3:");
      //var list = new List<int> { 1, 2, 3 };
      List<int> list = null;
      var inList = list?.Select(v => v > 1).ToList();

      foreach (var v in inList?.ToList()) {
        Console.WriteLine(v);
      }
    }
    private static void Case4() {
      Console.WriteLine("Case 4:");
      var list = new List<int> { 1, 2, 3, 3 };
      var inList = list.Distinct().ToList();

      Console.WriteLine(string.Join(',', list));
      Console.WriteLine(string.Join(',', inList));
    }
  }
}
