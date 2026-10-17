using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomServerCore.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            Type type = typeof(T);
            if (Services.ContainsKey(type))
            {
                Debug.LogWarning($"[Medusa.Services] Сервис {type.Name} уже зарегистрирован. Перезапись...");
                Services[type] = service;
            }
            else
            {
                Services.Add(type, service);
                Debug.Log($"[Medusa.Services] Сервис {type.Name} успешно зарегистрирован.");
            }
        }

        public static void Unregister<T>() where T : class
        {
            Type type = typeof(T);
            if (Services.ContainsKey(type))
            {
                Services.Remove(type);
                Debug.Log($"[Medusa.Services] Сервис {type.Name} удален из реестра.");
            }
        }

        public static T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            Debug.LogWarning($"[Medusa.Services] Запрашиваемый сервис {type.Name} не найден в реестре!");
            return null;
        }
    }
}