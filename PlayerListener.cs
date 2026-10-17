using CustomServerCore.Isolation;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace CustomServerCore
{
    public static class PlayerListener
    {
        public static void Initialize()
        {
            Provider.onServerConnected += OnPlayerConnected;
            Debug.Log("[CustomServerCore] Player tracking system successfully activated!");
        }

        private static void OnPlayerConnected(CSteamID steamID)
        {
            SafeInvoker.InvokeSafe("MedusaCore", "PlayerConnectedEvent", () =>
            {
                SteamPlayer client = PlayerTool.getSteamPlayer(steamID);
                if (client != null)
                {
                    string playerName = client.playerID.playerName;
                    string characterName = client.playerID.characterName;

                    Debug.Log($"[CustomServerCore] Player {playerName} (Character: {characterName}, SteamID: {steamID}) successfully connected to the server!");
                }
            });
        }
    }
}