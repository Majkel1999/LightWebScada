using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FrontEnd.DataModels;

namespace FrontEnd.Access
{
    public class UserCredentialHelper
    {
        public static bool IsUsernameTaken(string username)
        {
            var param = new DynamicParameters();
            param.Add("@Username", username);
            using (var db = new Npgsql.NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                if (db.Query("Select * From private.users where username=@username", param).Any())
                    return true;
                return false;
            }
        }

        public static void RegisterUser(UserRegistration user)
        {
            string sql = "INSERT INTO private.users(username,hashedpassword,email) Values (@username,@password,@email)";
            var parameter = new DynamicParameters();
            parameter.Add("@username", user.Username);
            parameter.Add("@password", HashPassword(user.Password));
            parameter.Add("@email", user.Email);
            using (var db = new Npgsql.NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                db.ExecuteAsync(sql, parameter).Wait();
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                var shaData = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                Console.WriteLine("Hashed pw:" + Encoding.UTF8.GetString(shaData));
                return Encoding.UTF8.GetString(shaData);
            }
        }
    }
}