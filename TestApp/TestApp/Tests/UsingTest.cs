using System;
using System.IO;
using System.Threading.Tasks;

namespace TestApp.Tests {

  public class UsingTest : ITest {
    public class MyResource : IDisposable, IAsyncDisposable {

      private int _count;

      public MyResource() {
        Console.WriteLine($"Creating resource: {GetHashCode()}");
      }

      public void ShowCount() {
        Console.WriteLine("Count: " + _count);
      }

      public void SetCount(int count) {
        Console.WriteLine("Setting count...");
        _count = count;
      }

      public void DoSomeTask() {
        Console.WriteLine("Starting some task...");
        for (int i = 0; i < 3; i++) {
          Console.WriteLine("Doing some task...");
        }
      }

      public void Dispose() {
        Console.WriteLine($"Releasing all the resources (object: {GetHashCode()})...\n");
      }

      public ValueTask DisposeAsync() {
        Console.WriteLine($"Asynchronously releasing all the resources (object: {GetHashCode()})...\n");
        return new ValueTask();
      }
    }

    ///<inheritdoc/>
    public async Task StartAsync() {
      //Case1();
      //Case2();
      //Case3();
      //await Case4();
      //Case6();
      //Case5();
      //Case7();
    }

    private void Case1() {
      Console.WriteLine("Case 1:");

      var ms = new MyResource();
      try {
        ms.DoSomeTask();
      }
      finally {
        ms.Dispose();
      }
    }

    private void Case2() {
      Console.WriteLine("Case 2:");

      using var ms = new MyResource();
      ms.DoSomeTask();
    }

    private void Case3() {
      Console.WriteLine("Case 3:");

      MyResource GetMyResource() {
        using var ms = new MyResource();
        ms.SetCount(1);
        ms.ShowCount();
        return ms;
      }

      using var ms2 = GetMyResource();
      ms2.DoSomeTask();
      ms2.ShowCount();
    }

    private async Task Case4() {
      Console.WriteLine("Case 4:");

      MyResource GetMyResource() {
        var ms = new MyResource();
        ms.SetCount(1);
        ms.ShowCount();
        return ms;
      }

      await using var ms2 = GetMyResource();
      ms2.DoSomeTask();
      ms2.ShowCount();

      await using var ms3 = ms2;
      ms3.DoSomeTask();
      ms3.ShowCount();
    }

    private void Case5() {
      Console.WriteLine("Case 5:");

      FileStream GetFileStream() {
        using var fs = new FileStream("./Files/using_test.txt", FileMode.OpenOrCreate);
        fs.Write(new byte[] { 65 });
        return fs;
      }

      using var fs2 = GetFileStream();
      fs2.Write(new byte[] { 66 });
    }

    private void Case6() {
      Console.WriteLine("Case 6:");

      FileStream GetFileStream() {
        var fs = new FileStream("./Files/using_test.txt", FileMode.OpenOrCreate);
        fs.Write(new byte[] { 65 });
        return fs;
      }

      using var fs2 = GetFileStream();
      fs2.Write(new byte[] { 66 });

      using var fs3 = fs2;
      fs3.Write(new byte[] { 67 });
    }

    private void Case7() {
      Console.WriteLine("Case 7:");

      {
        using (var myRes = new MyResource()) {
          using var myRes2 = new MyResource();
        }
      }

      Console.WriteLine("Case end");
    }
  }
}
