using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DataVerseManagerV2.Services
{
    public class DataStore<T>
    {
        private List<T> items = new();

        public void AddItem(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return items;
        }

        public void RemoveItem(T item)
        {
            items.Remove(item);
        }

        public void SaveToJson(string filePath)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
        }
    }

}
