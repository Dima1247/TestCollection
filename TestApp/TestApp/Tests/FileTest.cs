using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TestApp.Tests {

  public class FileTest : ITest {

    private const string LARGE_FILE_PATH = "./Files/file_test_large_1.txt";

    ///<inheritdoc/>
    public async Task StartAsync() {
      //await Case1();
      //await Case2();
      //await Case3();
      //await Case4();
      //await Case5();
      await Case6();
    }

    private async Task Case1() {
      Console.WriteLine("Case 1. ReadToEndAsync file:");
      await using var fs = new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate);
      using var sr = new StreamReader(fs);

      var sw = new Stopwatch();
      sw.Start();
      var content = await sr.ReadToEndAsync();
      sw.Stop();

      Console.WriteLine("Speed: " + sw.Elapsed.TotalSeconds);
    }

    private async Task Case2() {
      Console.WriteLine("Case 2. ReadLineAsync file:");
      await using var fs = new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate);
      using var sr = new StreamReader(fs);

      var sw = new Stopwatch();
      sw.Start();
      while (!sr.EndOfStream) {
        var lineContent = await sr.ReadLineAsync();
      }
      sw.Stop();

      Console.WriteLine("Speed: " + sw.Elapsed.TotalSeconds);
    }

    private async Task Case3() {
      Console.WriteLine("Case 3. ReadToEndAsync file with array:");
      await using var fs = new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate);
      using var sr = new StreamReader(fs);

      var sw = new Stopwatch();
      sw.Start();

      string line;
      var arr = new List<string>();
      var content = await sr.ReadToEndAsync();
      using var reader = new StringReader(content);
      while ((line = await reader.ReadLineAsync()) != null) {
        arr.Add(line);
      }

      sw.Stop();

      Console.WriteLine("Arr length: " + arr.Count);
      Console.WriteLine("Speed: " + sw.Elapsed.TotalSeconds);
    }

    private async Task Case4() {
      Console.WriteLine("Case 4. ReadLineAsync file with array:");
      await using var fs = new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate);
      using var sr = new StreamReader(fs);

      var sw = new Stopwatch();
      sw.Start();

      var arr = new List<string>();
      while (!sr.EndOfStream) {
        var lineContent = await sr.ReadLineAsync();
        arr.Add(lineContent);
      }

      sw.Stop();

      Console.WriteLine("Arr length: " + arr.Count);
      Console.WriteLine("Speed: " + sw.Elapsed.TotalSeconds);
    }

    // The fastest
    private async Task Case5() {
      Console.WriteLine("Case 5. ReadLineAsync file with buffer:");
      await using var fs = new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate);
      await using var bs = new BufferedStream(fs);
      using var sr = new StreamReader(bs);

      var sw = new Stopwatch();
      sw.Start();

      var arr = new List<string>();
      while (!sr.EndOfStream) {
        var lineContent = await sr.ReadLineAsync();
        //arr.Add(lineContent);
      }

      sw.Stop();

      Console.WriteLine("Arr length: " + arr.Count);
      Console.WriteLine("Speed: " + sw.Elapsed.TotalSeconds);
    }

    // The best performance
    private async Task Case6() {
      Console.WriteLine("Case 6. ReadLineAsync file with arr and clearing:");
      await using var fs = new FileStream(LARGE_FILE_PATH, FileMode.OpenOrCreate);
      await using var bs = new BufferedStream(fs);
      using var sr = new StreamReader(bs);

      var sw = new Stopwatch();
      sw.Start();

      var arr = new List<string>();
      while (!sr.EndOfStream) {
        var lineContent = await sr.ReadLineAsync();
        arr.Add(lineContent);
      }

      sw.Stop();

      var count = arr.Count;

      var gen = GC.GetGeneration(arr);
      arr.Clear();
      GC.Collect(gen, GCCollectionMode.Forced);

      Console.WriteLine("Arr length: " + count);
      Console.WriteLine("Arr gen: " + gen);
      Console.WriteLine("Speed: " + sw.Elapsed.TotalSeconds);
    }
  }
}
