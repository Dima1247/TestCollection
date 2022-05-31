using System;
using System.Threading.Tasks;

namespace TestApp.Tests {

  public class NewOverrideTest : ITest {
    ///<inheritdoc/>
    public async Task StartAsync() {
      Case1();
    }

    private void Case1() {
      var testClassB1 = new ClassB1();
      var testClassB2 = new ClassB2();

      Console.WriteLine($"Test {nameof(ClassB1)} with override keyword:");

      testClassB1.MethodA();
      (testClassB1 as ClassA)?.MethodA();

      Console.WriteLine($"\nTest {nameof(ClassB2)} with new keyword:");

      testClassB2.MethodA();
      (testClassB2 as ClassA)?.MethodA();
    }

    private void Case2() {
      var testClassB1 = new ClassB1();
      var testClassB2 = new ClassB2();

      Console.WriteLine($"Test {nameof(ClassB1)} with override keyword:");

      testClassB1.MethodA();
      (testClassB1 as ClassA)?.MethodA();

      Console.WriteLine($"\nTest {nameof(ClassB2)} with new keyword:");

      testClassB2.MethodA();
      (testClassB2 as ClassA)?.MethodA();
    }

    private abstract class ClassA {
      public virtual void MethodA() {
        Console.WriteLine($"{nameof(ClassA)} - {nameof(MethodA)}");
      }
    }

    private class ClassB1 : ClassA {
      public override void MethodA() {
        Console.WriteLine($"{nameof(ClassB1)} - {nameof(MethodA)}");
      }
    }

    private class ClassB2 : ClassA {
      public new void MethodA() {
        Console.WriteLine($"{nameof(ClassB2)} - {nameof(MethodA)}");
      }
    }
  }
}
