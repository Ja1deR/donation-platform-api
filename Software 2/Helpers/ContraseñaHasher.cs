using System.Security.Cryptography;
using System.Text;

public class ContraseñaHasher
{
    // Clave AES-256 en hexadecimal (32 bytes = 64 caracteres hex)
    private static readonly byte[] key = HexToBytes("e10adc3949ba59abbe56e057f20f883ee10adc3949ba59abbe56e057f20f883e");

    // IV desde string (16 bytes)
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("ABCDEFGH12345678").Take(16).ToArray();

    public static string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        var cipherBytes = Convert.FromBase64String(cipherText);
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var msDecrypt = new MemoryStream(cipherBytes))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }

    // Convierte un string hexadecimal a byte[]
    private static byte[] HexToBytes(string hex)
    {
        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }
}