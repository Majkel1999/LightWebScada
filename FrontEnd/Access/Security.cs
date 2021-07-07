using System.Security.Cryptography;
using System.Text;

namespace FrontEnd.Access
{
    public class Security
    {
        public static string HashPassword(string password)
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                var shaData = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Encoding.UTF8.GetString(shaData);
            }
        }
    }
}