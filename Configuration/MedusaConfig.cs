using BepInEx;
using System;
using System.IO;
using UnityEngine;

namespace CustomServerCore.Configuration
{
    public class PluginConfigBase
    {
        public bool EnableDebug { get; set; } = false;
    }

    public static class ConfigManager
    {
        private static string _baseConfigPath;

        public static void Initialize()
        {
            _baseConfigPath = Path.Combine(Paths.ConfigPath, "MedusaCore", "PluginsConfigs");
            if (!Directory.Exists(_baseConfigPath))
            {
                Directory.CreateDirectory(_baseConfigPath);
            }
        }

        public static T LoadConfig<T>(string pluginName) where T : PluginConfigBase, new()
        {
            if (string.IsNullOrEmpty(_baseConfigPath))
            {
                Initialize();
            }

            string pluginDir = Path.Combine(_baseConfigPath, pluginName);
            if (!Directory.Exists(pluginDir))
            {
                Directory.CreateDirectory(pluginDir);
            }

            string configFilePath = Path.Combine(pluginDir, "config.json");

            try
            {
                T config;
                if (File.Exists(configFilePath))
                {
                    string json = File.ReadAllText(configFilePath);
                    config = UnityEngine.JsonUtility.FromJson<T>(json);
                    if (config == null)
                    {
                        config = new T();
                    }
                }
                else
                {
                    config = new T();
                    SaveConfig(pluginName, config);
                }

                Debug.Log($"[Medusa.Config] Конфигурация для плагина '{pluginName}' успешно загружена.");
                return config;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Medusa.Config] Ошибка загрузки конфига для '{pluginName}': {ex.Message}");
                return new T();
            }
        }

        public static void SaveConfig<T>(string pluginName, T config) where T : PluginConfigBase
        {
            try
            {
                string pluginDir = Path.Combine(_baseConfigPath, pluginName);
                if (!Directory.Exists(pluginDir))
                {
                    Directory.CreateDirectory(pluginDir);
                }

                string configFilePath = Path.Combine(pluginDir, "config.json");
                string json = UnityEngine.JsonUtility.ToJson(config, true);
                File.WriteAllText(configFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Medusa.Config] Ошибка сохранения конфига для '{pluginName}': {ex.Message}");
            }
        }
    }
}