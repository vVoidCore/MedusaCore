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
            Debug.Log("[CustomServerCore] Система отслеживания игроков успешно активирована!");
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

                    Debug.Log($"[CustomServerCore] Игрок {playerName} (Персонаж: {characterName}, SteamID: {steamID}) успешно зашел на сервер!");
                }
            });
        }
    }
}