using System.Security.Cryptography;

using System.Text;

public class ContraseñaHasher

{

    private static byte[] key = Encoding.UTF8.GetBytes("e10adc3949ba59abbe56e057f20f883e"); 

    private static byte[] iv = Encoding.UTF8.GetBytes("ABCDEFGH12345678").Take(16).ToArray();
    

    public static string GetMD5(string str)

    {

        MD5 md5 = MD5.Create();

        ASCIIEncoding encoding = new ASCIIEncoding();

        byte[]? stream = null;

        StringBuilder sb = new StringBuilder();

        stream = md5.ComputeHash(encoding.GetBytes(str));

        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);

        return sb.ToString();

    }

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

                var encryptedBytes = msEncrypt.ToArray();

                return Convert.ToBase64String(encryptedBytes);

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

            {

                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))

                using (var srDecrypt = new StreamReader(csDecrypt))

                {

                    return srDecrypt.ReadToEnd();

                }

            }

        }

    }

}