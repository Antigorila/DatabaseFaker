using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Faker
{
    class DummyType
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DummyType() { }
        public DummyType(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void RemoveData(DummyData dummyData)
        {
            string filePath = $"dummies/{this.Name}.json";
            List<DummyData> datas = new List<DummyData>();
            List<DummyData> newDatas = new List<DummyData>();

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    datas = JsonSerializer.Deserialize<List<DummyData>>(jsonString);
                }
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].Data != dummyData.Data)
                {
                    newDatas.Add(datas[i]);
                }
            }

            string updatedJsonString = JsonSerializer.Serialize(newDatas, options);
            File.WriteAllText(filePath, updatedJsonString);
        }
        public DummyData GetOneRandomData()
        {
            Random rng = new Random();
            return GetAllData()[rng.Next(0, GetAllData().Count)];
        }
        public DummyData GetOneRandomData(int index)
        {
            return GetAllData()[index];
        }
        public List<DummyData> GetAllData()
        {
            List<DummyData> data = new List<DummyData>();
            string filePath = $"dummies/{this.Name}.json";
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    data = JsonSerializer.Deserialize<List<DummyData>>(jsonString);
                }
            }

            return data;
        }
        public void Create()
        {
            string filePath = "dummies/types.json";
            List<DummyType> types = new List<DummyType>();

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
                File.WriteAllText(filePath, string.Empty);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            types.Add(this);

            string updatedJsonString = JsonSerializer.Serialize(types, options);
            File.WriteAllText(filePath, updatedJsonString);

            string newTypeFilePath = $"dummies/{this.Name}.json";
            if (!File.Exists(newTypeFilePath))
            {
                File.WriteAllText(newTypeFilePath, "[]");
            }
        }
    }
}
