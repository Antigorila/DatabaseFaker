using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Faker
{
    class DummyData
    {
        public DummyType Type { get; set; }
        public string Data { get; set; }
        public DummyData() { }
        public DummyData(string type, string data)
        {
            Type = Controller.GetTypeByName(type);
            Data = data;
        }

        public DummyData(DummyType type, string data)
        {
            Type = type;
            Data = data;
        }

        public void Create()
        {
            string filePath = $"dummies/{this.Type.Name}.json";
            List<DummyData> datas = new List<DummyData>();

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

            datas.Add(this);

            string updatedJsonString = JsonSerializer.Serialize(datas, options);
            File.WriteAllText(filePath, updatedJsonString);
        }
    }
}
