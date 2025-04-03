﻿using System.Security.Cryptography;
using System.Text;

namespace BingoBackend.TestUtils;

public static class RandomUtil
{
    public static string GetRandomHexString(int length)
    {
        var bytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        var sb = new StringBuilder();
        foreach (var b in bytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    public static string GetPrefixedRandomHexString(string prefix, int hexLength)
    {
        return prefix + GetRandomHexString(hexLength);
    }
}