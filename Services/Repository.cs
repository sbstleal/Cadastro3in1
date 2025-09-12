using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WpfAppCadastro.Services
{
    public class Repository<T>
    {
        private readonly string _filePath;

        public Repository(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> Load()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public void Save(List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}