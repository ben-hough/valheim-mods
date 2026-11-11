using System;
using System.Reflection;
using BepInEx.Bootstrap;
using Jotunn.Managers;
using UnityEngine;

namespace GlymorsFeasts
{
    internal static class FeastHelpers
    {
        internal static bool IsPluginLoaded(string guid)
        {
            if (string.IsNullOrEmpty(guid) || Chainloader.PluginInfos == null)
            {
                return false;
            }
            return Chainloader.PluginInfos.ContainsKey(guid);
        }

        internal static bool IsAssemblyLoaded(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                return false;
            }
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (string.Equals(asm.GetName().Name, assemblyName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                catch
                {
                    // Ignore reflection-only or dynamic assemblies that cannot report a name.
                }
            }
            return false;
        }

        internal static GameObject FindPrefab(string name)
        {
            GameObject prefab = PrefabManager.Instance.GetPrefab(name);
            if (prefab != null)
            {
                return prefab;
            }
            if (ZNetScene.instance != null)
            {
                return ZNetScene.instance.GetPrefab(name);
            }
            return null;
        }
    }
}