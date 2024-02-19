
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

namespace _1.DataAccessInCSharp10.Service.Command
{
    public interface Command
    {
        void GetData();
    }

    public class LocalDbCommand : Command
    {
        public void GetData()
        {
            using SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-J76PCRA\SQLEXPRESS;Initial Catalog=WAREHOUSEMANAGEMENT;Integrated Security=True");

            using (SqlCommand command = new SqlCommand("SELECT * FROM [Orders]", connection))
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(reader["Id"]);
                }
            }

        }
    }

    public class SqlitesCommand : Command
    {
        public void GetData()
        {
            using SqliteConnection connection = new SqliteConnection(@"Data Source=E:\LEARNING\SELF_TRAINING_FOLDER\AllCSharpKnowledge\AllCSharpKnowleadge\AllCSharpKnowleadge\1.DataAccessInCSharp10\Database\warehouse.db");

            using SqliteCommand command = new SqliteCommand("SELECT * FROM [Orders]", connection);
            connection.Open();

            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader["Id"]);
            }

        }
    }
}
