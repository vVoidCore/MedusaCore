using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace CustomServerCore
{
    public static class PluginManager
    {
        public static List<ICustomPlugin> LoadedPlugins { get; private set; } = new List<ICustomPlugin>();
        private static string _pluginsDirectory;

        public static void Initialize()
        {
            _pluginsDirectory = Path.Combine(Paths.PluginPath, "CustomCorePlugins");
            if (!Directory.Exists(_pluginsDirectory))
            {
                Directory.CreateDirectory(_pluginsDirectory);
            }

            LoadPlugins();
        }

        public static void LoadPlugins()
        {
            if (string.IsNullOrEmpty(_pluginsDirectory))
            {
                _pluginsDirectory = Path.Combine(Paths.PluginPath, "CustomCorePlugins");
            }

            if (!Directory.Exists(_pluginsDirectory)) return;

            string[] dllFiles = Directory.GetFiles(_pluginsDirectory, "*.dll");
            Debug.Log($"[PluginManager] Поиск плагинов в директории: {_pluginsDirectory}");

            foreach (string dllPath in dllFiles)
            {
                try
                {
                    byte[] assemblyData = File.ReadAllBytes(dllPath);
                    Assembly assembly = Assembly.Load(assemblyData);

                    foreach (Type type in assembly.GetTypes())
                    {
                        if (typeof(ICustomPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            ICustomPlugin plugin = (ICustomPlugin)Activator.CreateInstance(type);
                            plugin.Load();
                            LoadedPlugins.Add(plugin);
                            Debug.Log($"[PluginManager] Успешно загружен плагин: {plugin.Name} (v{plugin.Version})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[PluginManager] Ошибка при загрузке плагина из файла {Path.GetFileName(dllPath)}: {ex.Message}");
                }
            }

            Debug.Log($"[PluginManager] Всего загружено кастомных плагинов: {LoadedPlugins.Count}");
        }

        public static void UnloadPlugins()
        {
            foreach (var plugin in LoadedPlugins)
            {
                try
                {
                    plugin.Unload();
                    Debug.Log($"[PluginManager] Выгружен плагин: {plugin.Name}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[PluginManager] Ошибка при выгрузке плагина {plugin.Name}: {ex.Message}");
                }
            }
            LoadedPlugins.Clear();
        }

        public static void ReloadPlugins()
        {
            Debug.Log("[PluginManager] Найдена команда перезагрузки. Выполняется Hot-Reload плагинов...");
            UnloadPlugins();
            LoadPlugins();
            Debug.Log("[PluginManager] Горячая перезагрузка плагинов успешно завершена!");
        }
    }
}