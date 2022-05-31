using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static CaseTester.Shared;

namespace CaseTester.ThreadTests;

public class TaskFactoryTest {
  /// <summary>
  /// Should run new task with await through the task factory
  /// New task should be on the different thread id
  /// New task should be on thread pool
  /// </summary>
  [Fact]
  public void Case1() {
    var threadInfoBase = GetThreadInfo();
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    async Task TestMethod() {
      await Task.Delay(2000);

      var threadInfoNew = GetThreadInfo();
      Assert.NotEqual(threadInfoBase.ThreadId, threadInfoNew.ThreadId);
      Assert.True(threadInfoNew.IsThreadPool);
    }
  }

  /// <summary>
  /// Should run new task with await through the task factory
  /// New task should be on the different thread id
  /// New task should be on thread pool
  /// </summary>
  [Fact]
  public void Case2() {
    var threadInfoBase = GetThreadInfo();
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    async void TestMethod() {
      await Task.Delay(2000);

      var threadInfoNew = GetThreadInfo();
      Assert.NotEqual(threadInfoBase.ThreadId, threadInfoNew.ThreadId);
      Assert.True(threadInfoNew.IsThreadPool);
    }
  }

  /// <summary>
  /// Should run new task without await through the task factory
  /// New task should be on the different thread id
  /// New task shouldn't be on thread pool
  /// </summary>
  [Fact]
  public void Case3() {
    var threadInfoBase = GetThreadInfo();
    Task.Factory.StartNew(TestMethod, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    async Task TestMethod() {
      Thread.Sleep(2000);

      var threadInfoNew = GetThreadInfo();
      Assert.NotEqual(threadInfoBase.ThreadId, threadInfoNew.ThreadId);
      Assert.False(threadInfoNew.IsThreadPool);
    }
  }
}