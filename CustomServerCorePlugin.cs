using BepInEx;
using CustomServerCore;
using CustomServerCore.Configuration;
using CustomServerCore.Permissions;

[BepInPlugin("com.medusa.core", "Medusa Core", "1.0.0")]
public class CustomServerCorePlugin : BaseUnityPlugin
{
    public static CustomServerCorePlugin Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        Logger.LogInfo("========================================");
        Logger.LogInfo("   MEDUSA CORE - Unleash the power      ");
        Logger.LogInfo("========================================");

        PermissionManager.Initialize();
        ConfigManager.Initialize();
        PlayerListener.Initialize();
        CustomServerCore.PluginManager.Initialize();

        Logger.LogInfo("[Medusa] Ядро успешно инициализировано!");
    }

    private void OnDestroy()
    {
        CustomServerCore.PluginManager.UnloadPlugins();
        Logger.LogInfo("[Medusa] Ядро выгружено.");
    }
}