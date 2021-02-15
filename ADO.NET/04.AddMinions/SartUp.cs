using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Text;

namespace _04.AddMinions
{
    class SartUp
    {
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security=true";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            sqlConnection.Open();

            StringBuilder sb = new StringBuilder();

            //Proccess input

            string[] minionInput = Console.ReadLine().Split(": ").ToArray();
            string[] villainInput = Console.ReadLine().Split(": ").ToArray();

            string[] minionInfo = minionInput[1].Split(" ").ToArray();
            string[] villainInfo = villainInput[1].Split(" ").ToArray();

            //Queries
            var townId = ReturnTownId(sb, sqlConnection,minionInfo);
            var villainId = ReturnVillainId(sb, sqlConnection, villainInfo);
            AddMinion(sb,sqlConnection,townId,villainId,minionInfo,villainInfo);

            //Console.WriteLine(villainId);
            Console.WriteLine(sb.ToString());
        }

        private static void AddMinion(StringBuilder sb, SqlConnection sqlConnection, string townId, string villainId, string[] minions, string[] villains)
        {
            string minionName = minions[0];
            string minionAge = minions[1];
            string villainName = villains[0];

            string addMininQuery = @"  INSERT INTO Minions([Name],Age,TownId)
                                          VALUES
                                          (@minionName,@age,@id)";

            SqlCommand addMinionCommand = new SqlCommand(addMininQuery, sqlConnection);
            addMinionCommand.Parameters.AddWithValue("@minionName", minionName);
            addMinionCommand.Parameters.AddWithValue("@age", minionAge);
            addMinionCommand.Parameters.AddWithValue("@id", townId);

            addMinionCommand.ExecuteNonQuery();

            string getMinionIdQuery = @"SELECT Id FROM Minions
                                    WHERE [Name] = @minionName";

            SqlCommand getMinionIdCommand = new SqlCommand(getMinionIdQuery, sqlConnection);
            getMinionIdCommand.Parameters.AddWithValue("@minionName", minionName);
            var minionId = getMinionIdCommand.ExecuteScalar();

            string addMinionToVillain = @" INSERT INTO MinionsVillains(MinionId,VillainId)
                                              VALUES
                                              (@minionId,@villainId)";

            SqlCommand insertMinionVillain = new SqlCommand(addMinionToVillain,sqlConnection);
            insertMinionVillain.Parameters.AddWithValue("@minionId",minionId);
            insertMinionVillain.Parameters.AddWithValue("@villainId",villainId);
            insertMinionVillain.ExecuteNonQuery();

            sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

        }
        private static string ReturnVillainId(StringBuilder sb, SqlConnection sqlConnection, string[] villains)
        {
            string villainName = villains[0];

            string villainQuery = @"SELECT Id FROM Villains
                                    WHERE [Name] = @villainName";

            using SqlCommand villainCommand = new SqlCommand(villainQuery, sqlConnection);
            villainCommand.Parameters.AddWithValue("@villainName", villainName);
            var currentId = villainCommand.ExecuteScalar()?.ToString();

            if (currentId == null)
            {
                string addVillainQuery = @"INSERT INTO Villains([Name],EvilnessFactorId)
                                            VALUES
                                            (@villainName,4)";

                using SqlCommand addVillain = new SqlCommand(addVillainQuery, sqlConnection);
                addVillain.Parameters.AddWithValue("@villainName", villainName);
                addVillain.ExecuteNonQuery();

                sb.AppendLine($"Villain {villainName} was added to the database.");

                currentId = villainCommand.ExecuteScalar()?.ToString();
                return currentId;
            }

            return currentId;

        }
        private static string ReturnTownId(StringBuilder sb, SqlConnection sqlConnection, string[] minions)
        {
            string minionName = minions[0];
            string minionAge = minions[1];
            string minionTown = minions[2];


            string townQuery = @"SELECT Id FROM Towns
                                WHERE [Name] = @townName";

            using SqlCommand townIdCommand = new SqlCommand(townQuery, sqlConnection);
            townIdCommand.Parameters.AddWithValue("@townName", minionTown);
            var currentId = townIdCommand.ExecuteScalar()?.ToString();

            if (currentId != null)
            {
                //sb.AppendLine($"Town exist");
            }
            else
            {
                string addTownQuery = @"INSERT INTO Towns([Name],CountryCode)
                                        VALUES
                                        (@townName,1)";
                using SqlCommand addTown = new SqlCommand(addTownQuery, sqlConnection);
                addTown.Parameters.AddWithValue("@townName", minionTown);
                addTown.ExecuteNonQuery();

                sb.AppendLine($"Town {minionTown} was added to the database.");
                currentId = townIdCommand.ExecuteScalar()?.ToString();
            }

            return currentId;
        }
    }
}
