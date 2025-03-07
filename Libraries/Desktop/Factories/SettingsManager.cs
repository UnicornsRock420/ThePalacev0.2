using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Xml;
using ThePalace.Common.Desktop.Entities.Ribbon;
using ThePalace.Core;
using ThePalace.Core.Client.Core;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Desktop.Factories;

public partial class SettingsManager : Disposable
{
    public static class SystemSettings
    {
        internal const string Filename = "SystemSettings.xml";
        internal const string RootNode = "System";
        internal const string RibbonNode = "Ribbon";
        internal const string UserFlagsNode = "UserFlags";
        internal const string RoomFlagsNode = "RoomFlags";

        public static ConcurrentDictionary<string, ItemBase> Ribbon { get; internal set; } = new();
    }
    public static class Localization
    {
        internal static string Filename
        {
            get
            {
                var filePath = $@"Localization\Localization.{CultureInfo.CurrentCulture.Name}.xml";
                if (File.Exists(filePath)) return filePath;
                else return @"Localization\Localization.en-US.xml";
            }
        }
        internal const string RootNode = "Localization";
        internal const string RibbonNode = "Ribbon";
        internal const string UserFlagsNode = "UserFlags";
        internal const string RoomFlagsNode = "RoomFlags";
    }

    private static readonly Keys[] _keyModifiers = new Keys[]
    {
        Keys.Control,
        Keys.Shift,
        Keys.Alt,
        Keys.LWin,
        Keys.RWin,
    };

    //private static readonly Keys[] _keyCharacters = new Keys[]
    //{
    //    Keys.D0,
    //    Keys.D1,
    //    Keys.D2,
    //    Keys.D3,
    //    Keys.D4,
    //    Keys.D5,
    //    Keys.D6,
    //    Keys.D7,
    //    Keys.D8,
    //    Keys.D9,
    //    Keys.NumPad0,
    //    Keys.NumPad1,
    //    Keys.NumPad2,
    //    Keys.NumPad3,
    //    Keys.NumPad4,
    //    Keys.NumPad5,
    //    Keys.NumPad6,
    //    Keys.NumPad7,
    //    Keys.NumPad8,
    //    Keys.NumPad9,
    //    Keys.A,
    //    Keys.B,
    //    Keys.C,
    //    Keys.D,
    //    Keys.E,
    //    Keys.F,
    //    Keys.G,
    //    Keys.H,
    //    Keys.I,
    //    Keys.J,
    //    Keys.K,
    //    Keys.L,
    //    Keys.M,
    //    Keys.N,
    //    Keys.O,
    //    Keys.P,
    //    Keys.Q,
    //    Keys.R,
    //    Keys.S,
    //    Keys.T,
    //    Keys.U,
    //    Keys.V,
    //    Keys.W,
    //    Keys.X,
    //    Keys.Y,
    //    Keys.Z,
    //};

    private static readonly Lazy<SettingsManager> _current = new();
    public static SettingsManager Current => _current.Value;

    public ConcurrentDictionary<string, ISettingBase> Settings { get; private set; } = new();

    public SettingsManager() { }
    ~SettingsManager() => Dispose(false);

    public new void Dispose()
    {
        if (IsDisposed) return;

        base.Dispose();

        Settings?.Clear();
        Settings = null;

        foreach (var ribbonNode in SystemSettings.Ribbon.Values)
            try { ribbonNode?.Unload(); } catch { }

        SystemSettings.Ribbon?.Clear();
        SystemSettings.Ribbon = null;
    }

    public SettingsManager Initialize()
    {
        try
        {
            var options = PluginManager.Current.GetTypes().ToList();
            foreach (var option in options)
            {
                try
                {
                    var instance = option.StaticProperty<ISettingBase>("Current");
                    if (instance == null) continue;

                    var path = Path.Combine(instance.Category, instance.Name);
                    Settings.TryAdd(path, instance);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif
                }
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            Debug.WriteLine(ex.Message);
#endif
        }

        return this;
    }

    internal string Read(string path) =>
        File.ReadAllLines(path).Join('\n');

    public void Load(Assembly assembly)
    {
        var load = (Func<string, XmlDocument>)(f =>
        {
            try
            {
                var settingsPath = Path.Combine(Environment.CurrentDirectory, f);
                if (File.Exists(settingsPath))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(Read(settingsPath));

                    return xmlDocument;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }

            return null;
        });

        var systemSettings = load(SystemSettings.Filename);
        var localizations = load(Localization.Filename);

        if (systemSettings != null &&
            localizations != null)
        {
            var item = null as ItemBase;

            var xNode = systemSettings[SystemSettings.RootNode][SystemSettings.RibbonNode] as XmlNode;
            foreach (var ribbonNode in xNode.ChildNodes.Cast<XmlNode>())
            {
                try
                {
                    var nodeName = ribbonNode.Name?.Trim();
                    if (string.IsNullOrWhiteSpace(nodeName) ||
                        SystemSettings.Ribbon.ContainsKey(nodeName)) continue;

                    item = ItemBase.Instance(nodeName);
                    if (item == null) continue;

                    item.HoverIcon = ribbonNode.ChildNodes
                        .Cast<XmlNode>()
                        .Where(n => n.Name == "Hover")
                        .SelectMany(n => n.Attributes
                            .Cast<XmlNode>()
                            .Where(a => a.Name == "path")
                            .Select(a => a.Value?.Trim()))
                        .FirstOrDefault();

                    if (item is StandardItem _standardItem)
                    {
                        _standardItem.Icon = ribbonNode.ChildNodes
                            .Cast<XmlNode>()
                            .Where(n => n.Name == "Icon")
                            .SelectMany(n => n.Attributes
                                .Cast<XmlNode>()
                                .Where(a => a.Name == "path")
                                .Select(a => a.Value?.Trim()))
                            .FirstOrDefault();
                    }
                    else if (item is BooleanItem _booleanItem)
                    {
                        _booleanItem.OnIcon = ribbonNode.ChildNodes
                            .Cast<XmlNode>()
                            .Where(n => n.Name == "IconOn")
                            .SelectMany(n => n.Attributes
                                .Cast<XmlNode>()
                                .Where(a => a.Name == "path")
                                .Select(a => a.Value?.Trim()))
                            .FirstOrDefault();
                        _booleanItem.OffIcon = ribbonNode.ChildNodes
                            .Cast<XmlNode>()
                            .Where(n => n.Name == "IconOff")
                            .SelectMany(n => n.Attributes
                                .Cast<XmlNode>()
                                .Where(a => a.Name == "path")
                                .Select(a => a.Value?.Trim()))
                            .FirstOrDefault();
                    }

                    SystemSettings.Ribbon.TryAdd(nodeName, item);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif
                }
            }

            xNode = localizations[Localization.RootNode][Localization.RibbonNode];
            foreach (var ribbonNode in xNode.ChildNodes.Cast<XmlNode>())
            {
                try
                {
                    var nodeName = ribbonNode.Name?.Trim();
                    if (string.IsNullOrWhiteSpace(nodeName) ||
                        !SystemSettings.Ribbon.ContainsKey(nodeName)) continue;

                    var buttonTitle = ribbonNode.Attributes
                        .Cast<XmlNode>()
                        .Where(a => a.Name == "title")
                        .Select(a => a.Value?.Trim())
                        .FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(buttonTitle)) continue;

                    SystemSettings.Ribbon[nodeName].Title = buttonTitle;
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif
                }
            }
        }

        foreach (var ribbonItem in SystemSettings.Ribbon.Values)
            ribbonItem.Load(assembly, "ThePalace.Core.Desktop.Core.Resources.icons");
    }

    public void Save()
    {
        // TODO: Save Settings


    }

    public T GetOption<T>(string settingName)
    {
        if (Settings.GetValue(settingName) is IOption<T> option)
            return option.Value;
        else return default;
    }
}