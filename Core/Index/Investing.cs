using AngleSharp;

namespace Core.Index;

public static class Investing
{
    private static readonly HttpClient HttpClient = new();
    private static readonly IBrowsingContext Context = BrowsingContext.New(Configuration.Default);

    static Investing()
    {
        HttpClient.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
    }

    public static async Task<Dictionary<string, int>> GetGlobalIndexAreaNameCode()
    {
        var content = await HttpClient.GetStringAsync("https://cn.investing.com/indices/global-indices");

        var document = await Context.OpenAsync(req => req.Content(content));

        var optionElements = document
            .QuerySelectorAll("select#country > option")
            .Where(element => element.GetAttribute("value")!.Contains("c_id"))
            .ToArray();

        var dict = new Dictionary<string, int>(optionElements.Length);

        foreach (var optionElement in optionElements)
            dict.TryAdd(
                optionElement.TextContent,
                int.Parse(optionElement.GetAttribute("value")!.Split("?")[1].Split("=")[1])
            );

        return dict;
    }

    public static async Task<Dictionary<string, string>> GetGlobalIndexAreaNameUrl()
    {
        var content = await HttpClient.GetStringAsync("https://cn.investing.com/indices/global-indices");

        var document = await Context.OpenAsync(req => req.Content(content));

        var optionElements = document
            .QuerySelectorAll("select#country > option")
            .Where(element => element.GetAttribute("value")!.Contains("c_id"))
            .ToArray();

        var dict = new Dictionary<string, string>(optionElements.Length);

        foreach (var optionElement in optionElements)
            dict.TryAdd(optionElement.TextContent, optionElement.GetAttribute("value")!);

        return dict;
    }

    public static async Task<Dictionary<string, int>> GetIndexInvestingGlobalAreaIndexNameCode(string area = "中国")
    {
        var nameUrlDict = await GetGlobalIndexAreaNameUrl();
        var content = await HttpClient.GetStringAsync(
            $"https://cn.investing.com{nameUrlDict[area]}&majorIndices=on&primarySectors=on&additionalIndices=on&otherIndices=on");

        var document = await Context.OpenAsync(req => req.Content(content));

        var optionElements = document
            .QuerySelectorAll("span.alertBellGrayPlus");

        var dict = new Dictionary<string, int>(optionElements.Length);

        foreach (var optionElement in optionElements)
            dict.TryAdd(optionElement.GetAttribute("data-name")!, int.Parse(optionElement.GetAttribute("data-id")!));

        return dict;
    }

    public static async Task<Dictionary<string, string>> GetIndexInvestingGlobalAreaIndexNameUrl(string area = "中国")
    {
        var nameUrlDict = await GetGlobalIndexAreaNameUrl();
        var content = await HttpClient.GetStringAsync(
            $"https://cn.investing.com{nameUrlDict[area]}&majorIndices=on&primarySectors=on&additionalIndices=on&otherIndices=on");

        var document = await Context.OpenAsync(req => req.Content(content));

        var tableRowElements = document
            .QuerySelectorAll("div#cross_rates_container > table > tbody > tr");

        var dict = new Dictionary<string, string>(tableRowElements.Length);

        foreach (var tableRowElement in tableRowElements)
        {
            var aElement = tableRowElement.QuerySelector("a")!;
            dict.TryAdd(aElement.GetAttribute("title")!, aElement.GetAttribute("href")!);
        }

        return dict;
    }
}