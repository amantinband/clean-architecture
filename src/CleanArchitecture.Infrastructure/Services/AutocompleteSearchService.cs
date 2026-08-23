using CleanArchitecture.Domain.Reminders;

namespace CleanArchitecture.Infrastructure.Services;

public class AutocompleteSearchService
{
    private readonly TrieNode _root = new();
    public void AddKeyword(string keyword)
    {
        var node = _root;
        foreach (var ch in keyword.ToLowerInvariant())
        {
            if (!node.Children.ContainsKey(ch))
            {
                node.Children[ch] = new TrieNode();
            }

            node = node.Children[ch];
        }

        node.IsKeyword = true;
        node.Frequency++;
    }

    public List<string> GetSuggestions(string prefix, int limit)
    {
        var node = _root;
        foreach (var ch in prefix.ToLowerInvariant())
        {
            if (!node.Children.TryGetValue(ch, out var nextNode))
            {
                return [];
            }

            node = nextNode;
        }

        var scoringResults = CollectKeywords(node, prefix.ToLowerInvariant());
        return scoringResults.OrderByDescending(x => x.Freq)
            .Take(limit)
            .Select(x => x.Keyword)
            .ToList();
    }

    private List<(string Keyword, int Freq)> CollectKeywords(TrieNode node, string currentPrefix)
    {
        if (node.IsKeyword)
        {
            return [(currentPrefix, node.Frequency)];
        }

        List<(string Keyword, int Freq)> result = [];
        foreach (var child in node.Children)
        {
            var childResult = CollectKeywords(child.Value, currentPrefix + child.Key);
            result.AddRange(childResult);
        }

        return result;
    }
}

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; } = [];
    public int Frequency { get; set; } = 0;
    public bool IsKeyword { get; set; }
    public List<string> TopSuggestions { get; set; } = [];
}
