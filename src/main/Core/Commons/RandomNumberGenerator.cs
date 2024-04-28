using System;
using System.Security.Cryptography;

namespace Core.Commons;

public static class RandomNumberGeneratorInPercentageRange
{
    public static int Generate(int min, int max)
    {
        using RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();

        byte[] randomBytes = new byte[4];
        randomGenerator.GetBytes(randomBytes);

        int randomNumber = BitConverter.ToInt32(randomBytes, 0) % 100 + 1;

        double scaleFactor = (double) (max - min + 1) / int.MaxValue;
        int scaledNumber = (int) Math.Floor(scaleFactor * randomNumber) + min;

        return Math.Min(max, Math.Max(min, scaledNumber));
    }
}