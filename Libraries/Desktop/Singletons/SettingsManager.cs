using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lib.Logging.Entities;
using Microsoft.Extensions.Configuration;

namespace Lib.Common.Desktop.Singletons;

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

    private static readonly Keys[] _keyModifiers =
    [
        Keys.Control,
        Keys.Shift,
        Keys.Alt,
        Keys.LWin,
        Keys.RWin
    ];

    private static readonly Keys[] _keyMappableCharacters =
    [
        Keys.D0,
        Keys.D1,
        Keys.D2,
        Keys.D3,
        Keys.D4,
        Keys.D5,
        Keys.D6,
        Keys.D7,
        Keys.D8,
        Keys.D9,
        Keys.NumPad0,
        Keys.NumPad1,
        Keys.NumPad2,
        Keys.NumPad3,
        Keys.NumPad4,
        Keys.NumPad5,
        Keys.NumPad6,
        Keys.NumPad7,
        Keys.NumPad8,
        Keys.NumPad9,
        Keys.A,
        Keys.B,
        Keys.C,
        Keys.D,
        Keys.E,
        Keys.F,
        Keys.G,
        Keys.H,
        Keys.I,
        Keys.J,
        Keys.K,
        Keys.L,
        Keys.M,
        Keys.N,
        Keys.O,
        Keys.P,
        Keys.Q,
        Keys.R,
        Keys.S,
        Keys.T,
        Keys.U,
        Keys.V,
        Keys.W,
        Keys.X,
        Keys.Y,
        Keys.Z,
    ];

    private bool IsDisposed { get; set; }
    private IConfigurationBuilder _configurationBuilder;
    private IConfiguration _configuration;

    public bool IsServer { get; set; }

    public void Build()
    {
        if (IsDisposed) return;

        try
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(@"Config\AppSettings.json")
                .AddJsonFile(@"Config\UserSettings.json")
                .Build();
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }
    }

    public void Save()
    {
        if (IsDisposed) return;

        // TODO: Save Settings
    }

    public T Get<T>(string xPath)
    {
        if (IsDisposed) return default(T);

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
        if (IsDisposed) return false;

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