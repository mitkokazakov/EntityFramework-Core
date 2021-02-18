using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _07.PrintNames
{
    class StartUp
    {
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=MinionsDB;Integrated Security=true";
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            List<string> allMinionNames = new List<string>();

            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            sqlConnection.Open();

            string allNamesQuery = @"SELECT [Name] FROM Minions";

            using SqlCommand allNamesCommand = new SqlCommand(allNamesQuery,sqlConnection);
            using SqlDataReader reader = allNamesCommand.ExecuteReader();

            while (reader.Read())
            {
                allMinionNames.Add(reader["Name"].ToString());
            }

            foreach (var minion in allMinionNames)
            {
                Console.WriteLine(minion);
                
            }

            Console.WriteLine();
            Console.WriteLine();

            int count = allMinionNames.Count;
            int border = 0;
            int j = count - 1;
            string oddEven = "";

            if (count % 2 == 0)
            {
                border = (count / 2) - 1;
                oddEven = "even";
            }
            else
            {
                border = count / 2;
                oddEven = "odd";
            }

            for (int i = 0; i <= border; i++)
            {
                string currentName = allMinionNames[i];
                Console.WriteLine(currentName);

                if (i == border && oddEven == "odd")
                {
                    return;
                }
                currentName = allMinionNames[j];
                Console.WriteLine(currentName);
                j--;

            }

        }
    }
}
