using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static CaseTester.Shared;

namespace CaseTester.ThreadTests;

public class ThreadLockTest {
  private readonly ITestOutputHelper _outputHelper;

  public ThreadLockTest(ITestOutputHelper outputHelper) {
    _outputHelper = outputHelper;
  }

  /// <summary>
  /// Should run new short tasks with lock through the task factory
  /// New tasks should start and finish execution on the same threads
  /// </summary>
  [Fact]
  public void Case1() {
    var syncLock = new object();

    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    WriteTaskThreadInfo(0);

    Thread.Sleep(10000);

    async Task TestMethod() {
      var id = GetRandomId();
      await Task.Delay(1000);

      var threadInfoStart = GetThreadInfo();
      WriteTaskThreadInfo(id);

      lock (syncLock) {
        Task.Delay(1000).GetAwaiter().GetResult();
      }

      var threadInfoEnd = GetThreadInfo();
      WriteTaskThreadInfo(id);

      Assert.Equal(threadInfoStart.ThreadId, threadInfoEnd.ThreadId);
    }
  }

  /// <summary>
  /// Should run new long tasks with lock through the task factory
  /// New tasks should start and finish execution on the same threads
  /// </summary>
  [Fact]
  public void Case2() {
    var syncLock = new object();

    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    WriteTaskThreadInfo(0);

    Thread.Sleep(20000);

    async Task TestMethod() {
      var id = GetRandomId();
      await Task.Delay(3000);

      var threadInfoStart = GetThreadInfo();
      WriteTaskThreadInfo(id);

      lock (syncLock) {
        Task.Delay(3000).GetAwaiter().GetResult();
      }

      var threadInfoEnd = GetThreadInfo();
      WriteTaskThreadInfo(id);

      Assert.Equal(threadInfoStart.ThreadId, threadInfoEnd.ThreadId);
    }
  }

  /// <summary>
  /// Should run new short tasks with semaphore through the task factory
  /// New tasks should start and finish execution on the same threads
  /// </summary>
  [Fact]
  public void Case3() {
    var semaphore = new SemaphoreSlim(1);

    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    WriteTaskThreadInfo(0);

    Thread.Sleep(10000);

    async Task TestMethod() {
      var id = GetRandomId();
      await Task.Delay(1000);

      var threadInfoStart = GetThreadInfo();
      WriteTaskThreadInfo(id);

      try {
        await semaphore.WaitAsync();
        await Task.Delay(1000);
      }
      finally {
        semaphore.Release();
      }

      var threadInfoEnd = GetThreadInfo();
      WriteTaskThreadInfo(id);

      Assert.Equal(threadInfoStart.ThreadId, threadInfoEnd.ThreadId);
    }
  }

  /// <summary>
  /// Should run new long tasks with semaphore through the task factory
  /// New tasks should finish execution on the same thread
  /// </summary>
  [Fact]
  public void Case4() {
    var semaphore = new SemaphoreSlim(1);
    var endThreadId = -1;

    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    WriteTaskThreadInfo(0);

    Thread.Sleep(20000);

    async Task TestMethod() {
      var id = GetRandomId();
      await Task.Delay(3000);

      WriteTaskThreadInfo(id);

      try {
        await semaphore.WaitAsync();
        await Task.Delay(3000);

        if (endThreadId == -1) {
          endThreadId = GetThreadInfo().ThreadId;
        }
      }
      finally {
        semaphore.Release();
      }

      WriteTaskThreadInfo(id);
      Assert.Equal(endThreadId, GetThreadInfo().ThreadId);
    }
  }

  private int GetRandomId() {
    return new Random().Next(1, 999);
  }

  private void WriteTaskThreadInfo(int taskId) {
    WriteThreadInfo(_outputHelper, $"Task id: {taskId}, ");
  }
}