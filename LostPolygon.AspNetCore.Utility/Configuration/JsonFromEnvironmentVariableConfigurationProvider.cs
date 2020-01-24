using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LostPolygon.AspNetCore.Utility {
    public class JsonFromEnvironmentVariableConfigurationProvider : ConfigurationProvider {
        private readonly JsonFromEnvironmentVariableConfigurationSource _configuration;

        public JsonFromEnvironmentVariableConfigurationProvider(JsonFromEnvironmentVariableConfigurationSource configuration) {
            _configuration = configuration;
        }

        public override void Load() {
            string? value = Environment.GetEnvironmentVariable(_configuration.EnvVariable ?? "");
            if (String.IsNullOrWhiteSpace(value))
                return;

            if (_configuration.ClearEnvVariable && _configuration.EnvVariable != null) {
                Environment.SetEnvironmentVariable(_configuration.EnvVariable, "");
            }
            try {
                LoadFromJsonString(value);
            } catch (FormatException) {
                value = Regex.Unescape(value);
                LoadFromJsonString(value);
            }
        }

        private void LoadFromJsonString(string value) {
            // Abuse JsonConfigurationProvider to delegate loading the JSON data
            ExposedJsonConfigurationProvider jsonConfigurationProvider =
                new ExposedJsonConfigurationProvider(new JsonConfigurationSource());

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            MemoryStream valueStream = new MemoryStream(valueBytes);
            jsonConfigurationProvider.Load(valueStream);
            Data = jsonConfigurationProvider.Data;
        }

        private class ExposedJsonConfigurationProvider : JsonConfigurationProvider {
            public new IDictionary<string, string> Data => base.Data;

            public ExposedJsonConfigurationProvider(JsonConfigurationSource source) : base(source) {
            }
        }
    }
}
