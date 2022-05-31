using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace TestApp.Tests {
  public class ReflectionTest : ITest {
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

    ///<inheritdoc/>
    public async Task StartAsync() {
      Case1();
      Case2();
      Case3();
      Case4();
      Case5();
    }

    private void Case1() {
      var value = "1";

      //GenericTryParse2(value, out int val);
    }

    private void Case2() {
      Console.WriteLine("\nCase2. Reflection private fields check:");
      var testObj = new TestObject();
      var testObjType = typeof(TestObject);

      var privateString = testObjType.GetField("_privateString", BindingFlags.NonPublic | BindingFlags.Instance);
      var privateReadonlyString = testObjType.GetField("_privateReadonlyString", BindingFlags.NonPublic | BindingFlags.Instance);
      var privateStaticString = testObjType.GetField("_privateStaticString", BindingFlags.NonPublic | BindingFlags.Static);
      var privateReadonlyStaticString = testObjType.GetField("_privateReadonlyStaticString", BindingFlags.NonPublic | BindingFlags.Static);
      var privateConstString = testObjType.GetField("_privateConstString", BindingFlags.NonPublic | BindingFlags.Static);

      Console.WriteLine(privateString?.ToString() ?? $"{nameof(privateString)} not found");
      Console.WriteLine(privateReadonlyString?.ToString() ?? $"{nameof(privateReadonlyString)} not found");
      Console.WriteLine(privateStaticString?.ToString() ?? $"{nameof(privateStaticString)} not found");
      Console.WriteLine(privateReadonlyStaticString?.ToString() ?? $"{nameof(privateReadonlyStaticString)} not found");
      Console.WriteLine(privateConstString?.ToString() ?? $"{nameof(privateConstString)} not found");

      Console.WriteLine("---/---");

      Console.WriteLine($"{nameof(privateString)} - {privateString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(privateReadonlyString)} - {privateReadonlyString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(privateStaticString)} - {privateStaticString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(privateReadonlyStaticString)} - {privateReadonlyStaticString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(privateConstString)} - {privateConstString?.GetValue(testObj)}");
    }

    private void Case3() {
      Console.WriteLine("\nCase3. Reflection public fields check:");
      var testObj = new TestObject();
      var testObjType = typeof(TestObject);

      var publicString = testObjType.GetField("_publicString");
      var publicReadonlyString = testObjType.GetField("_publicReadonlyString");
      var publicStaticString = testObjType.GetField("_publicStaticString", BindingFlags.Public | BindingFlags.Static);
      var publicReadonlyStaticString = testObjType.GetField("_publicReadonlyStaticString", BindingFlags.Public | BindingFlags.Static);
      var publicConstString = testObjType.GetField("_publicConstString", BindingFlags.Public | BindingFlags.Static);

      Console.WriteLine(publicString?.ToString() ?? $"{nameof(publicString)} not found");
      Console.WriteLine(publicReadonlyString?.ToString() ?? $"{nameof(publicReadonlyString)} not found");
      Console.WriteLine(publicStaticString?.ToString() ?? $"{nameof(publicStaticString)} not found");
      Console.WriteLine(publicReadonlyStaticString?.ToString() ?? $"{nameof(publicReadonlyStaticString)} not found");
      Console.WriteLine(publicConstString?.ToString() ?? $"{nameof(publicConstString)} not found");

      Console.WriteLine("---/---");

      Console.WriteLine($"{nameof(publicString)} - {publicString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(publicReadonlyString)} - {publicReadonlyString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(publicStaticString)} - {publicStaticString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(publicReadonlyStaticString)} - {publicReadonlyStaticString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(publicConstString)} - {publicConstString?.GetValue(testObj)}");
    }

    private void Case4() {
      Console.WriteLine("\nCase4. Reflection private properties check:");
      var testObj = new TestObject();
      var testObjType = typeof(TestObject);

      var privateString = testObjType.GetProperty("PrivateString", BindingFlags.NonPublic | BindingFlags.Instance);
      var privateStaticString = testObjType.GetProperty("PrivateStaticString", BindingFlags.NonPublic | BindingFlags.Static);

      Console.WriteLine(privateString?.ToString() ?? $"{nameof(privateString)} not found");
      Console.WriteLine(privateStaticString?.ToString() ?? $"{nameof(privateStaticString)} not found");

      Console.WriteLine("---/---");

      Console.WriteLine($"{nameof(privateString)} - {privateString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(privateStaticString)} - {privateStaticString?.GetValue(testObj)}");
    }

    private void Case5() {
      Console.WriteLine("\nCase5. Reflection public properties check:");
      var testObj = new TestObject();
      var testObjType = typeof(TestObject);

      var publicString = testObjType.GetProperty("PublicString");
      var publicStaticString = testObjType.GetProperty("PublicStaticString", BindingFlags.Public | BindingFlags.Static);

      Console.WriteLine(publicString?.ToString() ?? $"{nameof(publicString)} not found");
      Console.WriteLine(publicStaticString?.ToString() ?? $"{nameof(publicStaticString)} not found");

      Console.WriteLine("---/---");

      Console.WriteLine($"{nameof(publicString)} - {publicString?.GetValue(testObj)}");
      Console.WriteLine($"{nameof(publicStaticString)} - {publicStaticString?.GetValue(testObj)}");
    }

    private static bool GenericTryParse2<T>(string input, out T value) {
      var type = typeof(T);

      T newValue = default;
      var method = type.GetMethod(name: "TryParse", bindingAttr: BindingFlags.Static | BindingFlags.Public, null, types: new Type[] { typeof(string), typeof(T) }, null);
      method.Invoke(null, new object[] { input, newValue });

      var converter = TypeDescriptor.GetConverter(typeof(T));

      if (converter != null && converter.IsValid(input)) {
        value = (T)converter.ConvertFromString(input);
        return true;
      }

      value = default;
      return false;
    }
  }
}

