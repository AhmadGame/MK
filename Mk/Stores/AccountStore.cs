using Mk.Models;
using Npgsql;
using NpgsqlTypes;

namespace Mk.Stores
{
    public class AccountStore : Store
    {
        public Account GetByEmail(string email)
        {
            Account account = null;
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM account WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            account = new Account
                            {
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                IsAdmin = reader.GetBoolean(reader.GetOrdinal("isadmin"))
                            };
                        }
                    }
                }
            }

            return account;
        }

        public void Add(Account account)
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO account (email, password, isadmin) VALUES (@email, @password, @isadmin)";
                    cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, account.Email);
                    cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, account.Password);
                    cmd.Parameters.AddWithValue("@isadmin", NpgsqlDbType.Boolean, account.IsAdmin);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}