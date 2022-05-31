using Xunit;
using Xunit.Abstractions;

namespace CaseTester.EnumTests;

public enum TestEnum {
  None,
  TestRecord1
}

public class ConvertTest {
  private readonly ITestOutputHelper _outputHelper;

  public ConvertTest(ITestOutputHelper outputHelper) {
    _outputHelper = outputHelper;
  }

  /// <summary>
  /// Should convert enum to int
  /// Int should be direct enum element position
  /// </summary>
  [Fact]
  public void Case1() {
    var testRecord = (int)TestEnum.TestRecord1;
    _outputHelper.WriteLine(testRecord.ToString());

    Assert.Equal(1, testRecord);
  }

  /// <summary>
  /// Should convert enum to string
  /// String should be direct enum element name
  /// </summary>
  [Fact]
  public void Case2() {
    var testRecord = TestEnum.TestRecord1.ToString();
    _outputHelper.WriteLine(testRecord);

    Assert.Equal("TestRecord1", testRecord);
  }

  /// <summary>
  /// Should convert enum to string
  /// String should be direct enum element name
  /// </summary>
  [Fact]
  public void Case3() {
    var testRecord = nameof(TestEnum.TestRecord1);
    _outputHelper.WriteLine(testRecord);

    Assert.Equal("TestRecord1", testRecord);
  }
}