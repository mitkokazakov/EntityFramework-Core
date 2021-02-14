using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace _03.MinionNames
{
    class StartUp
    {
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security=true";
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            sqlConnection.Open();

            int villianId = int.Parse(Console.ReadLine());

            string villianStringQuery = @"SELECT [Name] FROM Villains	
                                            WHERE Id = @villianId";

            using SqlCommand sqlSelectVillianCommand = new SqlCommand(villianStringQuery, sqlConnection);
            sqlSelectVillianCommand.Parameters.AddWithValue("@villianId", villianId);

            string villianName = sqlSelectVillianCommand.ExecuteScalar()?.ToString();

            if (villianName == null)
            {
                sb.AppendLine($"No villain with ID {villianId} exists in the database.");
            }
            else
            {
                sb.AppendLine($"Villain: {villianName}");

                string minionsQuery = @"SELECT M.[Name],M.Age FROM Minions M
                                        JOIN MinionsVillains MV ON M.Id = MV.MinionId
                                        JOIN Villains V ON V.Id = MV.VillainId
                                        WHERE V.[Name] = @villainName
                                        ORDER BY M.[Name] ASC";

                using SqlCommand selectMinionsCommand = new SqlCommand(minionsQuery,sqlConnection);

                selectMinionsCommand.Parameters.AddWithValue("@villainName",villianName);

                using SqlDataReader reader = selectMinionsCommand.ExecuteReader();

                if (!reader.HasRows)
                {
                    sb.AppendLine("(no minions)");
                    return;
                }
                int count = 0;

                while (reader.Read())
                {
                    count++;
                    sb.AppendLine($"{count}.{reader["Name"]} {reader["Age"]}");
                }
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
