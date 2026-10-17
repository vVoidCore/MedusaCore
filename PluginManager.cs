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
            Debug.Log($"[PluginManager] Searching for plugins in directory: {_pluginsDirectory}");

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
                            Debug.Log($"[PluginManager] Successfully loaded plugin: {plugin.Name} (v{plugin.Version})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[PluginManager] Failed to load plugin from file {Path.GetFileName(dllPath)}: {ex.Message}");
                }
            }

            Debug.Log($"[PluginManager] Total custom plugins loaded: {LoadedPlugins.Count}");
        }

        public static void UnloadPlugins()
        {
            foreach (var plugin in LoadedPlugins)
            {
                try
                {
                    plugin.Unload();
                    Debug.Log($"[PluginManager] Unloaded plugin: {plugin.Name}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[PluginManager] Error while unloading plugin {plugin.Name}: {ex.Message}");
                }
            }
            LoadedPlugins.Clear();
        }

        public static void ReloadPlugins()
        {
            Debug.Log("[PluginManager] Reload command received. Performing plugins Hot-Reload...");
            UnloadPlugins();
            LoadPlugins();
            Debug.Log("[PluginManager] Plugins Hot-Reload successfully completed!");
        }
    }
}