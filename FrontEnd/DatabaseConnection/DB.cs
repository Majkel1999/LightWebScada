using Dapper;
using FrontEnd.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FrontEnd.DatabaseConnection
{
    public class DB
    {
        public static List<UserLogin> GetUsersList()
        {
            using (var db = new Npgsql.NpgsqlConnection("Server = serwer.lan; Port = 45432; Database = ScadaData; User Id = Frontend; Password = front;"))
            {
                return db.Query<UserLogin>("Select * From private.Users").ToList();
            }
        }
    }
}
