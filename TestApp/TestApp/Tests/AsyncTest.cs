using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp.Tests {
  public class AsyncTest : ITest {
    ///<inheritdoc/>
    public async Task StartAsync() {
      // await ConfigureAwaitTest.Case1();
      // await ConfigureAwaitTest.Case2();
      // await ConfigureAwaitTest.Case3();
      // await ConfigureAwaitTest.Case4();
      // await ConfigureAwaitTest.Case5();

      // await ThreadPoolTest.Case1();
      // await ThreadPoolTest.Case2();
      // await ThreadPoolTest.Case3();

      TaskFactoryTest.Case1();
      TaskFactoryTest.Case2();
      TaskFactoryTest.Case3();
    }

    public static class ConfigureAwaitTest {
      public static async Task Case1() {
        Console.WriteLine($"{nameof(ConfigureAwaitTest)} {nameof(Case1)}.");
        Console.WriteLine("ConfigureAwait test. Start");

        Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId);

        await TestMethod();

        Console.WriteLine("Next thread: " + Thread.CurrentThread.ManagedThreadId);

        Console.WriteLine("ConfigureAwait test. After ConfigureAwait message");
        Console.WriteLine();
      }

      public static async Task Case2() {
        Console.WriteLine($"{nameof(ConfigureAwaitTest)} {nameof(Case2)}.");
        Console.WriteLine("ConfigureAwait test. Start");

        Console.WriteLine("Current thread: " + Thread.CurrentThread.ManagedThreadId);

        await TestMethod().ConfigureAwait(false);

        Console.WriteLine("Next thread: " + Thread.CurrentThread.ManagedThreadId);

        Console.WriteLine("ConfigureAwait test. After ConfigureAwait message");
        Console.WriteLine();
      }

      public static async Task Case3() {
        Console.WriteLine($"{nameof(ConfigureAwaitTest)} {nameof(Case3)}.");

        async Task InnerTestExceptionMethod() {
          try {
            await TestExceptionMethod();
          }
          catch (Exception ex) {
            throw new Exception($"{nameof(InnerTestExceptionMethod)}. Some exception");
          }
        }

        try {
          await InnerTestExceptionMethod();
        }
        catch (Exception ex) {
          Console.WriteLine(ex.Message);
          //Will write down InnerTestExceptionMethod exception
        }
      }

      public static async Task Case4() {
        Console.WriteLine($"{nameof(ConfigureAwaitTest)} {nameof(Case4)}.");

        Task InnerTestExceptionMethod() {
          try {
            return TestExceptionMethod();
          }
          catch (Exception ex) {
            throw new Exception($"{nameof(InnerTestExceptionMethod)}. Some exception");
          }
        }

        try {
          await InnerTestExceptionMethod();
        }
        catch (Exception ex) {
          Console.WriteLine(ex.Message);
          //Will write down TestExceptionMethod exception
        }
      }

      public static async Task Case5() {
        Console.WriteLine($"{nameof(ConfigureAwaitTest)} {nameof(Case5)}.");

        WriteTaskThreadInfo(0);

        await TestConfiguredMethod();

        var worker = Task.Factory.StartNew(TestConfiguredMethod, CancellationToken.None,
          TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning, TaskScheduler.Default);

        var worker2 = Task.Factory.StartNew(TestConfiguredMethod, CancellationToken.None,
          TaskCreationOptions.AttachedToParent, TaskScheduler.Default);
      }

      private static async Task TestMethod() {
        Console.WriteLine($"{nameof(TestMethod)}. Start");
        await Task.Delay(1000);
        Console.WriteLine($"{nameof(TestMethod)}. End");
      }

      private static async Task TestExceptionMethod() {
        Console.WriteLine($"{nameof(TestExceptionMethod)}. Start");
        await Task.Delay(1000);
        throw new Exception($"{nameof(TestExceptionMethod)}. Some exception");
        Console.WriteLine($"{nameof(TestExceptionMethod)}. End");
      }

      private static ConfiguredTaskAwaitable TestConfiguredMethod() {
        Console.WriteLine($"{nameof(TestMethod)}. Start");
        WriteTaskThreadInfo(1);
        return Task.Delay(1000).ConfigureAwait(false);
        Console.WriteLine($"{nameof(TestMethod)}. End");
      }
    }

    public static class ThreadPoolTest {
      public static async Task Case1() {
        Console.WriteLine($"{nameof(ThreadPoolTest)} {nameof(Case1)}.");
        WriteThreadPoolCount();

        for (int i = 0; i < 10; i++) {
          ThreadPool.QueueUserWorkItem((string data) => WriteTaskThreadInfo(int.Parse(data)), i.ToString(), false);
        }

        for (int i = 0; i < 100; i++) {
          Thread.Sleep(1000);
          WriteThreadPoolCount();
        }
      }

      public static async Task Case2() {
        Console.WriteLine($"{nameof(ThreadPoolTest)} {nameof(Case2)}.");
        WriteThreadPoolCount();

        for (int i = 0; i < 10; i++) {
          new Task(() => WriteTaskThreadInfo(i)).Start();
        }

        for (int i = 0; i < 100; i++) {
          Thread.Sleep(1000);
          WriteThreadPoolCount();
        }
      }

      public static async Task Case3() {
        Console.WriteLine($"{nameof(ThreadPoolTest)} {nameof(Case3)}.");
        WriteThreadPoolCount();

        for (int i = 0; i < 10; i++) {
          //Change to see difference
          TestMethod(i);
          //await TestMethodTP(i);
        }

        for (int i = 0; i < 100; i++) {
          Thread.Sleep(1000);
          WriteThreadPoolCount();
        }
      }

      private static Task TestMethod(int i) {
        WriteTaskThreadInfo(i);
        return TestMethod2(i);
      }

      private static async Task TestMethod2(int i) {
        //Uncomment to see magic :)
        //await Task.Delay(1000);
        WriteTaskThreadInfo(i);
      }
    }

    public static class TaskFactoryTest {
      /// <summary>
      /// Check if 
      /// </summary>
      public static void Case1() {
        Console.WriteLine($"{nameof(TaskFactoryTest)} {nameof(Case1)}.");

        Task.Factory.StartNew(TestMethod1, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        WriteTaskThreadInfo(1);

        async Task TestMethod1() {
          await Task.Delay(2000);
          WriteTaskThreadInfo(1);
        }
      }

      public static void Case2() {
        Console.WriteLine($"{nameof(TaskFactoryTest)} {nameof(Case2)}.");

        Task.Factory.StartNew(TestMethod2, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        WriteTaskThreadInfo(2);

        async Task TestMethod2() {
          Thread.Sleep(2000);
          WriteTaskThreadInfo(2);
        }
      }

      public static void Case3() {
        Console.WriteLine($"{nameof(TaskFactoryTest)} {nameof(Case3)}.");

        Task.Factory.StartNew(TestMethod3, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        WriteTaskThreadInfo(3);

        async void TestMethod3() {
          await Task.Delay(2000);
          WriteTaskThreadInfo(3);
        }
      }
    }

    public static class LockTest {
      private static object _syncLock = new object();

      /// <summary>
      /// Check if lock 
      /// </summary>
      public static async Task Case1() {
        Console.WriteLine($"{nameof(LockTest)} {nameof(Case1)}.");

        // lock (_syncLock) {
        //   await TestMethod(1);
        // }
      }

      private static Task TestMethod(int i) {
        WriteTaskThreadInfo(i);
        return TestMethod2(i);
      }

      private static async Task TestMethod2(int i) {
        //Uncomment to see magic :)
        //await Task.Delay(1000);
        WriteTaskThreadInfo(i);
      }
    }

    #region thread helpers

    private static void WriteTaskThreadInfo(int taskId) {
      var thread = Thread.CurrentThread;
      Console.WriteLine(
        $"Task id: {taskId}. Thread id: {thread.ManagedThreadId}. Background: {thread.IsBackground}. Thread pool: {thread.IsThreadPoolThread}");
    }

    private static void WriteThreadPoolCount() {
      Console.WriteLine("Thread count: " + ThreadPool.ThreadCount);
    }

    #endregion
  }
}