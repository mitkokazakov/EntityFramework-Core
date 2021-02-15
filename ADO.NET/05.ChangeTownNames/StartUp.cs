using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _05.ChangeTownNames
{
    class StartUp
    {
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security=true";
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            sqlConnection.Open();

            string countryName = Console.ReadLine();

            string getContryCodeQuery = @"SELECT Id FROM Countries WHERE [Name] = @name";

            using SqlCommand getCountryIdCommand = new SqlCommand(getContryCodeQuery,sqlConnection);
            getCountryIdCommand.Parameters.AddWithValue("@name",countryName);

            var countryCode = getCountryIdCommand.ExecuteScalar();

            string getTownsQuery = @"SELECT [Name] FROM Towns WHERE CountryCode = @countryCode";

            using SqlCommand getAllTowns = new SqlCommand(getTownsQuery,sqlConnection);
            getAllTowns.Parameters.AddWithValue("@countryCode",countryCode);

            using SqlDataReader reader = getAllTowns.ExecuteReader();

            int count = 0;
            List<string> affectedTowns = new List<string>();
            while (reader.Read())
            {
                count++;
                affectedTowns.Add(reader["Name"].ToString());
            }

            reader.Close();

            string updateTowns = @"UPDATE Towns
                                      SET [Name] = UPPER([Name])
                                      WHERE CountryCode = @countryCode";

            using SqlCommand updateTownCommand = new SqlCommand(updateTowns,sqlConnection);
            updateTownCommand.Parameters.AddWithValue("@countryCode", countryCode);
            updateTownCommand.ExecuteNonQuery();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{count} town names were affected.");
            sb.Append("[");
            sb.Append(String.Join(",", affectedTowns));
            sb.Append("]");

            Console.WriteLine(sb.ToString());
            
        }
    }
}
