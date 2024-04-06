using Core.Index;


namespace Tests.Core;

public class IndexTest
{
    public IndexTest(ITestOutputHelper testOutputHelper)
    {
        Console.SetOut(new TestOutputHelperWriter(testOutputHelper));
    }

    [Fact]
    public async void GetGlobalIndexAreaNameCode_ShouldNotEmpty()
    {
        var result = await Investing.GetGlobalIndexAreaNameCode();
        Assert.NotEmpty(result);
    }

    [Fact]
    public async void GetGlobalIndexAreaNameUrl_ShouldNotEmpty()
    {
        var result = await Investing.GetGlobalIndexAreaNameUrl();
        Assert.NotEmpty(result);
    }

    [Fact]
    public async void GetIndexInvestingGlobalAreaIndexNameCode_ShouldNotEmpty()
    {
        var result = await Investing.GetIndexInvestingGlobalAreaIndexNameCode();
        Assert.NotEmpty(result);
    }

    [Fact]
    public async void GetIndexInvestingGlobalAreaIndexNameUrl_ShouldNotEmpty()
    {
        var result = await Investing.GetIndexInvestingGlobalAreaIndexNameUrl();
        Assert.NotEmpty(result);
    }
}