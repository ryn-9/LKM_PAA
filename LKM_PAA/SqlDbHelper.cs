using Npgsql;
using System.Data;

namespace LKM_PAA.Helpers
{
    public class SqlDbHelper
    {
        private readonly string _connString;

        public SqlDbHelper(string connString)
        {
            _connString = connString;
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            var result = new List<Dictionary<string, object>>();

            using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            using var cmd = new NpgsqlCommand(query, conn);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }

                result.Add(row);
            }

            return result;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            using var conn = new NpgsqlConnection(_connString);
            conn.Open();

            using var cmd = new NpgsqlCommand(query, conn);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            return cmd.ExecuteNonQuery();
        }
    }
}