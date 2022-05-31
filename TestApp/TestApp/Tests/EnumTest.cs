using System;
using System.Threading.Tasks;

namespace TestApp.Tests {

  public class EnumTest : ITest {
    public enum TestEnum {
      En1 = 0,
      En2
    }

    ///<inheritdoc/>
    public async Task StartAsync() {
      Case1();
    }

    private void Case1() {
      Console.WriteLine(TestEnum.En1.ToString());
      Console.WriteLine(nameof(TestEnum.En1));
    }
  }
}
