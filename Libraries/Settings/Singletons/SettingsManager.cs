using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lib.Logging.Entities;
using Microsoft.Extensions.Configuration;

namespace Lib.Settings.Singletons
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        public SettingsManager()
        {
            try
            {
                _configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile(@"Config\AppSettings.json");

                if (IsServer)
                {
                    _configurationBuilder = _configurationBuilder
                        .AddJsonFile(@"Config\ServerSettings.json");
                }
                else
                {
                    _configurationBuilder = _configurationBuilder
                        .AddJsonFile(@"Config\UserSettings.json");
                }

                _configurationBuilder.Build();
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);
            }

            Build();
        }

        ~SettingsManager()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            IsDisposed = true;

            GC.SuppressFinalize(this);
        }

        private bool IsDisposed { get; set; }
        private IConfigurationBuilder _configurationBuilder;
        private IConfiguration _configuration;

        public bool IsServer { get; set; }

        public void Build()
        {
            try
            {
                _configurationBuilder.Build();
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);
            }
        }

        public void Save()
        {
            // TODO: Save Settings
        }

        public T Get<T>(string xPath)
        {
            try
            {
                return _configuration
                    .GetSection(xPath)
                    .Get<T>();
            }
            catch
            {
            }

            return default;
        }

        public bool Set<T>(string fPath, string xPath, object value)
        {
            try
            {
                var json = File.ReadAllText(fPath);
                var jsonObj = JsonSerializer.Deserialize<JsonObject>(json);
                var _xPath = xPath.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                var @ref = (JsonNode?)jsonObj;

                for (var j = 0; j < _xPath.Length; j++)
                {
                    if (@ref[_xPath[j]] == null) break;

                    @ref = @ref[_xPath[j]];
                }

                switch (@ref)
                {
                    case JsonArray jsonArray:
                        if (value is not object[] objArray) return false;

                        jsonArray.Clear();

                        foreach (var obj in objArray)
                            jsonArray.Add(obj.ToString());

                        break;
                }

                File.WriteAllText(fPath, jsonObj.ToJsonString(new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                    MaxDepth = 24,
                }));

                Build();

                return true;
            }
            catch
            {
            }

            return false;
        }

        public static class Localization
        {
        }
    }
}