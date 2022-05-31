using System;
using System.Text;
using System.Threading.Tasks;
using TestApp.Tests;

namespace TestApp {
  public static class Program {
    private async static Task Main() {
      try {
        var choiceText = new StringBuilder("Which test should we run?\n")
          .AppendLine("Json - 0")
          .AppendLine("Using - 1")
          .AppendLine("Enum - 2")
          .AppendLine("File - 3")
          .AppendLine("Async - 4")
          .AppendLine("Uri - 5")
          .AppendLine("Exception - 6")
          .AppendLine("Reflection - 7")
          .AppendLine("New-Override - 8")
          .AppendLine("Sql - 9")
          .AppendLine("Extra - e")
          .AppendLine("Enter your choice: ");

        Console.Write(choiceText);
        var choice = Console.ReadLine();
        Console.WriteLine();

        ITest test = choice switch {
          "0" => new JsonTest(),
          "1" => new UsingTest(),
          "2" => new EnumTest(),
          "3" => new FileTest(),
          "4" => new AsyncTest(),
          "5" => new UriTest(),
          "6" => new ExceptionTest(),
          "7" => new ReflectionTest(),
          "8" => new NewOverrideTest(),
          "9" => new SqlTest(),
          "e" => new ExtraTest(),
          _ => null
        };

        if (test == null) {
          return;
        }

        await test.StartAsync();
        Console.ReadLine();
      }
      catch (Exception ex) {
        Console.WriteLine($"Exception: {ex.Message}");
      }
    }
  }
}