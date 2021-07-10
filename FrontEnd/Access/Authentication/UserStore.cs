using System.Linq;
using System.Threading.Tasks;
using FrontEnd.DataModels.DatabaseModels;
using Dapper;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using FrontEnd.DataModels;

namespace FrontEnd.Access.Authentication
{
    public class UserStore : IUserStore<User>
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

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.UserName.ToString());
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@username", userName);
            parameters.Add("@id", user.Id);
            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                return db.ExecuteAsync("UPDATE private.users SET username=@username WHERE id=@id", parameters);
            }
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.UserName.ToString().ToLowerInvariant());

        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return SetUserNameAsync(user, normalizedName, cancellationToken);
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            int rows = 0;
            string sql = "INSERT INTO private.users(username,hashedpassword,email) Values (@username,@password,@email)";

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@username", user.UserName);
            parameter.Add("@password", user.HashedPassword);
            parameter.Add("@email", user.Email);

            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                rows = db.ExecuteAsync(sql, parameter).Result;
            }
            if (rows > 0)
                return Task.Run(() => new IdentityResult());
            return Task.Run(() => IdentityResult.Failed());
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            int rows = 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@username", user.UserName);
            parameters.Add("@password", user.HashedPassword);
            parameters.Add("@email", user.Email);
            parameters.Add("@id", user.Id);

            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                rows = db.ExecuteAsync("UPDATE private.users SET username=@username,hashedpassword=@password,email=@email WHERE id=@id", parameters).Result;
            }
            if (rows > 0)
                return Task.Run(() => new IdentityResult());
            return Task.Run(() => IdentityResult.Failed());
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            int rows = 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", user.Id);

            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                rows = db.ExecuteAsync("DELETE FROM private.users WHERE id=@id", parameters).Result;
            }
            if (rows > 0)
                return Task.Run(() => new IdentityResult());
            return Task.Run(() => IdentityResult.Failed());
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", userId);

            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                return Task.Run(() => db.QueryAsync<User>("SELECT * FROM private.users WHERE id=@id").Result.FirstOrDefault());
            }
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@username", normalizedUserName);

            using (NpgsqlConnection db = new NpgsqlConnection("Server=serwer.lan;Port=45432;Database=ScadaData;User Id=Frontend;Password=front;"))
            {
                return Task.Run(() => db.QueryAsync<User>("SELECT * FROM private.users WHERE username=@username").Result.FirstOrDefault());
            }
        }

        public void Dispose()
        {
            
        }
    }
}