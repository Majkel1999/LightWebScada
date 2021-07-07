using System.Linq;
using System.Threading.Tasks;
using FrontEnd.DataModels;
using Dapper;
using Npgsql;

namespace FrontEnd.Access.Authentication
{
    public class UserStore
    {
        public async Task<ActionResult> RegisterUserAsync(UserRegistration user)
        {
            int rows = 0;
            string sql = "INSERT INTO private.users(username,hashedpassword,email) Values (@username,@password,@email)";

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@username", user.Username);
            parameter.Add("@password", Security.HashPassword(user.Password));
            parameter.Add("@email", user.Email);

            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                rows = await db.ExecuteAsync(sql, parameter);
            }
            if (rows > 0)
                return ActionResult.Success;
            return ActionResult.Failure;
        }

        public async Task<bool> CheckUserExistsAsync(string username)
        {
            var param = new DynamicParameters();
            param.Add("@username", username);
            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                if ((await db.QueryAsync("Select * From private.users where username=@username", param)).Any())
                    return true;
                return false;
            }
        }
    }
}