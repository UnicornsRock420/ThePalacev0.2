﻿using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ThePalace.Common.Desktop.Entities.Ribbon;
using ThePalace.Common.Factories.System;
using ThePalace.Logging.Entities;

namespace ThePalace.Common.Desktop.Factories;

public class SettingsManager : SingletonDisposable<SettingsManager>
{
    public SettingsManager()
    {
        try
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddJsonFile("UserSettings.json")
                .Build();
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }
    }

    ~SettingsManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        foreach (var ribbonNode in Ribbon.Values)
            try
            {
                ribbonNode?.Unload();
            }
            catch
            {
            }

        Ribbon?.Clear();
        Ribbon = null;

        base.Dispose();

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

    private readonly IConfiguration _configuration;
    public static ConcurrentDictionary<string, ItemBase> Ribbon { get; internal set; } = new();

    public void Load(Assembly assembly)
    {
    }

    public void Save()
    {
        // TODO: Save Settings
    }

    public T Get<T>(string xPath)
    {
        try
        {
            return (T)Convert.ChangeType(_configuration[xPath], typeof(T));
        }
        catch
        {
        }
        
        return default;
    }

    public static class Localization
    {
    }
}