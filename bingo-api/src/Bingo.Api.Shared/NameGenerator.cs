namespace Bingo.Api.Shared;

public class NameGenerator
{
    public enum NameTypes
    {
        Player,
        Team
    }

    private readonly Dictionary<NameTypes, int> _lastNamesRead = new();

    private readonly Dictionary<NameTypes, string[]> _namesDict = new();
    private readonly Random _random = new();

    public NameGenerator()
    {
        foreach (var type in Enum.GetValues<NameTypes>())
        {
            LoadNamesFile(type);
            _lastNamesRead[type] = -1;
        }
    }

    private void LoadNamesFile(NameTypes type)
    {
        var fileName = type + "_names.txt";
        var filePath = FileSystemHelper.FindFileOfDirectory("data", fileName);
        var names = File.ReadAllLines(filePath);
        _random.Shuffle(names);
        _namesDict[type] = names;
    }

    public string GetNew(NameTypes type)
    {
        var nameIndex = _lastNamesRead[type] + 1;
        if (nameIndex >= _namesDict[type].Length)
        {
            nameIndex = 0;
            LoadNamesFile(type);
        }

        var name = _namesDict[type][nameIndex];
        _lastNamesRead[type] = nameIndex;
        return name;
    }
}