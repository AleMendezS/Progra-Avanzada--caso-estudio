using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PAW.ProjectArchitecture
{
    public class JsonProvider
    {
        public static async Task<T> DeserializeAsync<T>(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            T deserialized = await JsonSerializer.DeserializeAsync<T>(stream);
            return deserialized!;
        }

        public static T DeserializeSimple<T>(string content)
        {
            return JsonSerializer.Deserialize<T>(content, GetJsonSerializerOptions())!;
        }

        public static async Task<T> DeserializeAsync<T>(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            return await DeserializeAsync<T>(bytes);
        }

        public static string Serialize(object content)
        {
            var serialized = JsonSerializer.Serialize(content);
            return serialized;
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
            };
        }

    }
}
