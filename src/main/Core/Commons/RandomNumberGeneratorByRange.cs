using System;
using System.Security.Cryptography;

namespace Core.Commons;

public static class RandomNumberGeneratorByRange
{
    public static int Generate(int min, int max)
    {
        using RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();

        byte[] randomBytes = new byte[4];
        randomGenerator.GetBytes(randomBytes);

        int randomNumber = BitConverter.ToInt32(randomBytes, 0);
        
        return Math.Abs(randomNumber % (max - min + 1)) + min;
    }
}