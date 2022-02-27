using System.Security.Cryptography;
using System.Text;

namespace ProgTracker.Infrastructure.AspNetCore.Security;

public class Membership
{
    private readonly string _key;

    public Membership(string key)
    {
        _key = key;
    }

    public bool Compare(string rawData, string encodedData)
    {
        return Encode(rawData) == encodedData;
    }

    public string Encode(string rawData)
    {
        var hash = new HMACSHA1
        {
            Key = HexToByte(_key)
        };

        return Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(rawData)));
    }

    private static byte[] HexToByte(string hexString)
    {
        var returnBytes = new byte[hexString.Length / 2];

        for (var i = 0; i < returnBytes.Length; i++)
        {
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }

        return returnBytes;
    }
}