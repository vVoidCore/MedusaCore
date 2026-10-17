using System;
using UnityEngine;

namespace CustomServerCore.Isolation
{
    public static class SafeInvoker
    {
        public static void InvokeSafe(string pluginName, string actionName, Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Medusa.Isolation] КРИТИЧЕСКАЯ ОШИБКА в плагине '{pluginName}' во время выполнения '{actionName}': {ex.Message}");
                Debug.LogException(ex);
            }
        }

        public static T InvokeSafe<T>(string pluginName, string actionName, Func<T> func, T defaultValue = default)
        {
            try
            {
                if (func != null)
                {
                    return func.Invoke();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Medusa.Isolation] КРИТИЧЕСКАЯ ОШИБКА в плагине '{pluginName}' во время выполнения '{actionName}': {ex.Message}");
                Debug.LogException(ex);
            }
            return defaultValue;
        }
    }
}