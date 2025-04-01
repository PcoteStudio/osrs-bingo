namespace BingoBackend.TestUtils;

public static class RandomStringGenerator
{
    public enum CharacterType
    {
        Letters = 0,
        Numbers = 1,
        LettersAndNumbers = 2
    }

    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";

    private static readonly Random Random = new();

    public static string Generate(int length, CharacterType allowedCharacters = CharacterType.LettersAndNumbers)
    {
        var characters = "";
        if (allowedCharacters is CharacterType.Letters or CharacterType.LettersAndNumbers)
            characters += Letters;
        if (allowedCharacters is CharacterType.Numbers or CharacterType.LettersAndNumbers)
            characters += Numbers;

        return new string(Enumerable.Repeat(characters, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static string GenerateRandomLength(int minLength, int maxLength,
        CharacterType allowedCharacters = CharacterType.LettersAndNumbers)
    {
        var length = Random.Next(minLength, maxLength);
        return Generate(length, allowedCharacters);
    }
}