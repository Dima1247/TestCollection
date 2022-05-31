using System;
using System.Threading;

namespace TestWebApp {
  public static class ThreadHelper {
    public static void CheckThread(string location) {
      Console.WriteLine($"{location}. Thread id: {Thread.CurrentThread.ManagedThreadId}. Is background: {Thread.CurrentThread.IsBackground}. Is thread pool: {Thread.CurrentThread.IsThreadPoolThread}");
    }
  }
}
