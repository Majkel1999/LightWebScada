using System.Linq;
using Dapper;

namespace FrontEnd.Access
{
    public class UserCredentialHelper
    {
        public static bool CheckUserPassword(string username, string password)
        {
            var param = new DynamicParameters();
            param.Add("@username", username);
            using (var db = new Npgsql.NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                string pwhash = db.Query<string>("Select hashedpassword from private.users where username=@username", param).FirstOrDefault();
                if (pwhash == Security.HashPassword(password))
                    return true;
            }
            return false;
        }
    }
}