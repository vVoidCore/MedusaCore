using System.Collections.Generic;

namespace CustomServerCore.Permissions
{
    public class PlayerPermissionData
    {
        public ulong SteamID { get; set; }
        public string PlayerName { get; set; }
        public string Group { get; set; } = "default";
        public string Prefix { get; set; } = "";
        public List<string> CustomPermissions { get; set; } = new List<string>();
    }

    public class GroupData
    {
        public string Name { get; set; }
        public string DefaultPrefix { get; set; } = "";
        public List<string> Permissions { get; set; } = new List<string>();
    }

    public class PermissionsDatabase
    {
        public Dictionary<string, GroupData> Groups { get; set; } = new Dictionary<string, GroupData>();
        public List<PlayerPermissionData> Players { get; set; } = new List<PlayerPermissionData>();
    }
}