# Medusa Core 🛡️

A modern, lightweight, modular, and secure server core for Unturned (powered by BepInEx and .NET Framework 4.8), built as a fast and clean alternative to legacy frameworks.

## ✨ Features

- **Modular Architecture** — Plugin isolation and seamless inter-plugin communication via `ServiceLocator`.
- **Safety (`SafeInvoker`)** — Protects the server from crashes and critical errors if a plugin fails.
- **Permissions & Groups** — Built-in hierarchical permission system (`permissions.json`) supporting prefixes and wildcards (`*`).
- **Configuration Management** — Automatic serialization and saving of plugin settings.
- **Soft Hot-Reload** — Ability to update plugins on the fly through logical unloading and reloading of assemblies.
- **Clean Codebase** — Zero bloat, no unnecessary dependencies, and fully structured source code.

## 🚀 Installation

1. Install BepInEx 5.x for your Unturned dedicated server.
2. Place `CustomServerCore.dll` into the `BepInEx/plugins/` directory.
3. Start the server to generate the default configuration structure.

## 📂 Directory Structure

- Core and plugin configurations: `BepInEx/config/MedusaCore/`
- Custom plugins directory: `BepInEx/plugins/CustomCorePlugins/`

## 🛠️ Creating a Plugin

A custom plugin must implement the `ICustomPlugin` interface:

```csharp
using CustomServerCore;
using UnityEngine;

public class MyAwesomePlugin : ICustomPlugin
{
    public string Name => "MyAwesomePlugin";
    public string Version => "1.0.0";

    public void Load()
    {
        Debug.Log("Plugin loaded!");
    }

    public void Unload()
    {
        Debug.Log("Plugin unloaded!");
    }
}
