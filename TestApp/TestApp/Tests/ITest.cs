using System.Threading.Tasks;

namespace TestApp.Tests {

  /// <summary>
  /// Provides testing logic
  /// </summary>
  public interface ITest {

    /// <summary>
    /// Executes test flow
    /// </summary>
    public Task StartAsync();

  }
}
