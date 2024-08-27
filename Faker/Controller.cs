using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Faker
{
    class Controller
    {
        public static bool IsDatabaseTxtContainsAllData()
        {
            if (DatabaseTxt()[0] != string.Empty)
            {
                if (! MatchingMask("127.0.0.1", DatabaseTxt()[0]))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (DatabaseTxt()[1] != string.Empty)
            {
                if (! MatchingMask("3306", DatabaseTxt()[1]))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (DatabaseTxt()[2] == string.Empty)
            {
                return false;
            }


            if (DatabaseTxt()[4] == string.Empty)
            {
                return false;
            }

            return true;
        }
        public static Exception error;

        public static bool CanConnectToDatabase()
        {
            try
            {
                SQL.Query("SELECT VERSION()");
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }

            return true;
        }

        public static bool MatchingMask(string mask, string input)
        {
            if (mask.Length != input.Length)
            {
                return false;
            }

            for (int i = 0; i < mask.Length; i++)
            {
                char maskChar = mask[i];
                char inputChar = input[i];

                if (char.IsDigit(maskChar))
                {
                    if (!char.IsDigit(inputChar))
                    {
                        return false;
                    }
                }
                else if (maskChar != inputChar)
                {
                    return false;
                }
            }

            return true;
        }

        public static void WriteDatabaseTxt(string datasource, string port, string username, string password, string database)
        {
            StreamWriter writeDB = new StreamWriter("database.txt");
            writeDB.WriteLine(datasource);
            writeDB.WriteLine(port);
            writeDB.WriteLine(username);
            writeDB.WriteLine(password);
            writeDB.WriteLine(database);
            writeDB.Close();

            RefreshDatabaseConnection();
        }

        public static string[] DatabaseTxt()
        {
            return File.ReadAllLines("database.txt");
        }

        public static void RefreshDatabaseConnection()
        {
            SQL.datasource = DatabaseTxt()[0];
            SQL.port = int.Parse(DatabaseTxt()[1]);
            SQL.username = DatabaseTxt()[2];
            SQL.password = DatabaseTxt()[3];
            SQL.database = DatabaseTxt()[4];

            SQL.connectionstring = $"datasource={SQL.datasource};port={SQL.port};username={SQL.username};password={SQL.password};database={SQL.database};";
            SQL.con = new MySqlConnection(SQL.connectionstring);
        }

        public static List<DummyType> DummyTypes()
        {
            List<DummyType> types = new List<DummyType>();
            string filePath = "dummies/types.json";
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);

                if (!string.IsNullOrWhiteSpace(jsonString))
                { 
                    types = JsonSerializer.Deserialize<List<DummyType>>(jsonString);
                }
            }
            else
            {
                File.Create("dummies/types.json");
            }

            DummyType Number = new DummyType("Number", "With this DummyType you can generate numbers between numbers or just random nubers.");
            DummyType Word = new DummyType("Word", "With this DummyType you can generate a word or words with by givin a word or a char count to it.");
            DummyType ForeginID = new DummyType("ForeginID", "With this DummyType you can generate a ForeginID to a different table.");

            types.Add(Number);
            types.Add(Word);
            types.Add(ForeginID);

            return types;
        }

        public static DummyType GetTypeByName(string name)
        {
            for (int i = 0; i < DummyTypes().Count; i++)
            {
                if (DummyTypes()[i].Name == name)
                {
                    return DummyTypes()[i];
                }
            }

            return null;
        }
    }
}
