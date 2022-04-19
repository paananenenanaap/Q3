using System.Globalization;
using System.Text.RegularExpressions;

var caseItems = new List<CaseItem>
{
    new()
    {
        Id          = 1,
        DateTime    = new DateTime(2021, 04, 10),
        Title       = "case item number one: the first item",
        Description = "this is the first item in a list of case items with very important information in them. there will be more items below."
    },
    new()
    {
        Id          = 2,
        DateTime    = new DateTime(),
        Title       = "case item TWO: not the first item",
        Description = "tHiS Is nOt tHe fIrSt iTeM In a lIsT Of iTeMs."
    },
    new()
    {
        Id          = 4,
        DateTime    = new DateTime(2025, 04, 19),
        Title       = string.Empty,
        Description = "THIS IS ALL UPPERCASE! why is this all lowercase?"
    },
    new()
    {
        Id          = 5,
        Title       = "CASE 5: the last one",
        Description = string.Empty
    }
};

var parsedItems = CaseItem.ParseCaseItem(caseItems);

foreach (var item in parsedItems)
{
    Console.WriteLine($"Id: {item.Id}");
    if (item.DateTime != null) Console.WriteLine($"DateTime: {item.DateTime.Value:yyyy-MM-dd}");
    Console.WriteLine($"Title: {item.Title}");
    Console.WriteLine($"Description: {item.Description}");
    Console.WriteLine(Environment.NewLine);
}

public class CaseItem
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? DateTime { get; init; }

    public static List<CaseItem> ParseCaseItem(IEnumerable<CaseItem> caseItems)
    {
        var textInfo = new CultureInfo("en-AU",false).TextInfo;
        var parsedCaseItems = new List<CaseItem>();
        var previousId = 0;
        foreach (var parsedItem in caseItems.Select(item => new CaseItem
        {
            Id = item.Id == previousId + 1
                         ? item.Id
                         : previousId + 1,
            DateTime = item.DateTime != null && item.DateTime.Value > System.DateTime.Now
                         ? System.DateTime.Now.AddDays(-1)
                         : item.DateTime,
            Title = string.IsNullOrEmpty(item.Title) ? string.Empty : textInfo.ToTitleCase(item.Title.ToLower()),
            Description = ChangeToSentenceCase(item.Description?.ToLower())
        }))
        {
            parsedCaseItems.Add(parsedItem);
            previousId = parsedItem.Id;
        }
        return parsedCaseItems;
    }

    private static string ChangeToSentenceCase(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var sentences = Regex.Split(text, @"(?<=[.!?])");
        var parsedText = string.Empty;
        foreach (var sentence in sentences)
        {
            var s = sentence.Trim(' ');
            if (s.Length == 1)
            {
                parsedText += sentence;
            }
            else
            {
                parsedText += $"{UppercaseFirst(s)} ";
            }
        }
        return parsedText;
    }

    private static string UppercaseFirst(string sentence)
    {
        if (string.IsNullOrEmpty(sentence) || sentence.Length == 1) return sentence;
        var a = sentence.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
}
