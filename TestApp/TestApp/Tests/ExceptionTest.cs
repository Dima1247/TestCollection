using System;
using System.Threading.Tasks;

namespace TestApp.Tests {

  public class ExceptionTest : ITest {

    ///<inheritdoc/>
    public async Task StartAsync() {
      Case1();
      Case2();
    }

    private void Case1() {
      Console.WriteLine("Case 1: Usual exception to string");
      var ex = new Exception("Exception text");
      ex.Data.Add("key1", "val1");

      Console.WriteLine(ex.Message);
      Console.WriteLine("------------");
      Console.WriteLine(ex.ToString());
    }
    private void Case2() {
      Console.WriteLine("Case 2: Argument exception to string");
      var ex = new ArgumentException("Exception text");
      ex.Data.Add("key1", "val1");

      Console.WriteLine(ex.Message);
      Console.WriteLine("------------");
      Console.WriteLine(ex.ToString());
    }
  }
}
