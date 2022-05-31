using System;
using System.Threading;
using Xunit.Abstractions;

namespace CaseTester;

public class ThreadInfo {
  public int ThreadId { get; set; }
  public bool IsBackground { get; set; }
  public bool IsThreadPool { get; set; }

  public override string ToString() {
    return $"Thread id: {ThreadId}, Background: {IsBackground}, Thread pool: {IsThreadPool}. {DateTime.Now.ToString("mm:ss:fffff")}";
  }
}

public static class Shared {
  public static ThreadInfo GetThreadInfo() {
    var thread = Thread.CurrentThread;
    return new ThreadInfo {
      ThreadId = thread.ManagedThreadId,
      IsBackground = thread.IsBackground,
      IsThreadPool = thread.IsThreadPoolThread
    };
  }

  public static void WriteThreadInfo(ITestOutputHelper outputHelper, string prefix = "", string suffix = "") {
    outputHelper.WriteLine($"{prefix}{GetThreadInfo()}{suffix}");
  }
}