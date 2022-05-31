using System;
using System.Threading.Tasks;

namespace TestApp.Tests {

  public class UriTest : ITest {

    ///<inheritdoc/>
    public async Task StartAsync() {
      Case1();
      //Case2();
      Case3();
      Case4();
    }

    private void Case1() {
      var uriBuilder = new UriBuilder("https://google.com") {
        Path = "index.html",
        Fragment = string.Format("?search={0}", "Dima")
      };

      Console.WriteLine(uriBuilder.Uri.ToString());
    }

    private void Case2() {
      var uriBuilder = new UriBuilder("https://google..com") {
        Path = "index.html",
        Fragment = string.Format("?search={0}", "Dima")
      };

      Console.WriteLine(uriBuilder.Uri.ToString());
    }

    private void Case3() {
      Uri.TryCreate("https://google.com", UriKind.Absolute, out Uri baseUri);
      var relativeUri = "index.html" + string.Format("#?search={0}", "Dima");

      Uri.TryCreate(baseUri, relativeUri, out Uri resultUri);

      Console.WriteLine(resultUri?.ToString() ?? "Error");
    }

    private void Case4() {
      Uri.TryCreate("https://google..com", UriKind.Absolute, out Uri baseUri);
      var relativeUri = "index.html" + string.Format("#?search={0}", "Dima");

      Uri.TryCreate(baseUri, relativeUri, out Uri resultUri);

      Console.WriteLine(resultUri?.ToString() ?? "Error");
    }
  }
}
