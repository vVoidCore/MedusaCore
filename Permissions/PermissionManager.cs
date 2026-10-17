using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CustomServerCore.Permissions
{
    public static class PermissionManager
    {
        private static PermissionsDatabase _db;
        private static string _configPath;

        public static void Initialize()
        {
            string configDir = Path.Combine(Paths.ConfigPath, "MedusaCore");
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            _configPath = Path.Combine(configDir, "permissions.json");
            LoadOrCreateDatabase();
        }

        private static void LoadOrCreateDatabase()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    string json = File.ReadAllText(_configPath);
                    _db = UnityEngine.JsonUtility.FromJson<PermissionsDatabase>(json);

                    if (_db == null || _db.Groups == null)
                    {
                        CreateDefaultDatabase();
                    }
                }
                else
                {
                    CreateDefaultDatabase();
                }
                Debug.Log("[Medusa.Permissions] Permission system successfully loaded.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Medusa.Permissions] Failed to load permissions database: {ex.Message}");
                CreateDefaultDatabase();
            }
        }

        private static void CreateDefaultDatabase()
        {
            _db = new PermissionsDatabase();

            _db.Groups.Add("default", new GroupData
            {
                Name = "default",
                DefaultPrefix = "",
                Permissions = new List<string> { "medusa.chat", "medusa.kits.use" }
            });

            _db.Groups.Add("vip", new GroupData
            {
                Name = "vip",
                DefaultPrefix = "[VIP]",
                Permissions = new List<string> { "medusa.chat", "medusa.kits.use", "medusa.vip.kit", "medusa.color.chat" }
            });

            _db.Groups.Add("admin", new GroupData
            {
                Name = "admin",
                DefaultPrefix = "[ADMIN]",
                Permissions = new List<string> { "*" }
            });

            SaveDatabase();
        }

        public static void SaveDatabase()
        {
            try
            {
                string json = UnityEngine.JsonUtility.ToJson(_db, true);
                File.WriteAllText(_configPath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Medusa.Permissions] Failed to save permissions database: {ex.Message}");
            }
        }

        public static PlayerPermissionData GetPlayer(ulong steamID, string playerName = "")
        {
            var player = _db.Players.FirstOrDefault(p => p.SteamID == steamID);
            if (player == null)
            {
                player = new PlayerPermissionData
                {
                    SteamID = steamID,
                    PlayerName = playerName,
                    Group = "default"
                };
                _db.Players.Add(player);
                SaveDatabase();
            }
            else if (!string.IsNullOrEmpty(playerName) && player.PlayerName != playerName)
            {
                player.PlayerName = playerName;
                SaveDatabase();
            }
            return player;
        }

        public static bool HasPermission(ulong steamID, string permission)
        {
            var player = GetPlayer(steamID);

            if (player.CustomPermissions.Contains(permission) || player.CustomPermissions.Contains("*"))
                return true;

            if (_db.Groups.TryGetValue(player.Group, out var group))
            {
                if (group.Permissions.Contains(permission) || group.Permissions.Contains("*"))
                    return true;
            }

            return false;
        }

        public static string GetPlayerPrefix(ulong steamID)
        {
            var player = GetPlayer(steamID);
            if (!string.IsNullOrEmpty(player.Prefix))
                return player.Prefix;

            if (_db.Groups.TryGetValue(player.Group, out var group))
            {
                return group.DefaultPrefix;
            }

            return "";
        }
    }
}