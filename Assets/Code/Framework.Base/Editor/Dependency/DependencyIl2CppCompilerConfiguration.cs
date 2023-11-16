using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace General.Editor
{
#if Il2CPP_DEBUG
#elif Il2CPP_RELEASE
#elif Il2CPP_MASTER
#endif

    internal static class DependencyIl2CppCompilerConfiguration
    {
        private const string Il2CppDebug = "Il2CPP_DEBUG";
        private const string Il2CppRelease = "Il2CPP_RELEASE";
        private const string Il2CppMaster = "Il2CPP_MASTER";
        private const string SectionName = "Il2CPP";

        private static Il2CppCompilerConfiguration configuration;
        private static int delay = 10;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (ScriptingImplementation.IL2CPP == GetScriptingBackend())
            {
                if (DependencyController.HasSection(SectionName))
                {
                    if (DependencyController.HasDefine(Il2CppDebug))
                        configuration = Il2CppCompilerConfiguration.Debug;
                    else if (DependencyController.HasDefine(Il2CppRelease))
                        configuration = Il2CppCompilerConfiguration.Release;
                    else
                        configuration = Il2CppCompilerConfiguration.Master;

                    CheckConfigurationChange();
                }
                else
                {
                    CheckConfigurationChange(true);
                }
            }
            else
            {
                if (DependencyController.HasSection(SectionName))
                    DependencyController.DeleteSection(SectionName);
            }
        }

        private static async void CheckConfigurationChange(bool force = false)
        {
            for (int i = 0; i < delay; i++) await Task.Yield();

            if ((ScriptingImplementation.IL2CPP == GetScriptingBackend() &&
                 configuration != GetIl2CppCompilerConfiguration()) || force)
            {
                configuration = GetIl2CppCompilerConfiguration();
                switch (configuration)
                {
                    case Il2CppCompilerConfiguration.Debug:
                        SetDefine(Il2CppDebug);
                        break;
                    case Il2CppCompilerConfiguration.Release:
                        SetDefine(Il2CppRelease);
                        break;
                    case Il2CppCompilerConfiguration.Master:
                        SetDefine(Il2CppMaster);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void SetDefine(in string define)
        {
            DependencyController.AddDefines(new[] { define }, SectionName);
        }

        private static ScriptingImplementation GetScriptingBackend() =>
            PlayerSettings.GetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup);

        private static Il2CppCompilerConfiguration GetIl2CppCompilerConfiguration() =>
            PlayerSettings.GetIl2CppCompilerConfiguration(EditorUserBuildSettings.selectedBuildTargetGroup);
    }
}