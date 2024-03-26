using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Web.Helper
{
    public class MustafeedFunctions
    {
        private const string key = "b14ca5693a4e4133btce2eb2317a1946";
        private const string _apikey = "4b82f0c4-e925-46aa-9227-66edb547d90f";
        private static string EncryptString(string plainInput)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainInput);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        public static string LoadData(string UserName, string language)
        {
            HttpClient h = new HttpClient();

            string path = $"https://outres.iau.edu.sa/commondata/api/v2/userinfo?userName={Uri.EscapeDataString(EncryptString(UserName))}&lang={language}";
            h.DefaultRequestHeaders.Add("apiKey", _apikey);

            var res = h.GetAsync(path);
            var resJson = res.Result.Content.ReadAsStringAsync();

            return resJson.Result;
        }
    }
}