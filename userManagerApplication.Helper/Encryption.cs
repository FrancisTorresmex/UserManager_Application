using System.Text;
using System.Security.Cryptography;

namespace userManagerApplication.Helper
{
    //Encryption MD5
    public class Encryption
    {
        private const string hash = "u53rm4n493rU53RM4N493R="; //encryption key

        public string Encrypt(string msg)
        {
            try
            {
                byte[] data = UTF8Encoding.UTF8.GetBytes(msg);

                MD5 md5 = MD5.Create();
                TripleDES tripleDes = TripleDES.Create();

                tripleDes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                tripleDes.Mode = CipherMode.ECB;

                ICryptoTransform transform = tripleDes.CreateEncryptor();
                byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

                return Convert.ToBase64String(result);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            
        }

        public string Decrypt(string msg)
        {
            try
            {
                byte[] data = Convert.FromBase64String(msg);

                MD5 md5 = MD5.Create();
                TripleDES tripleDes = TripleDES.Create();

                tripleDes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                tripleDes.Mode = CipherMode.ECB;

                ICryptoTransform transform = tripleDes.CreateDecryptor();
                byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

                return UTF8Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            
        }
    }
}