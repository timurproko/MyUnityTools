#if FMOD
using System.Reflection;
using FMODUnity;
using HarmonyLib;
using UnityEditor;

[InitializeOnLoad]
public static class EventManagerPatcher
{
    static EventManagerPatcher()
    {
        var harmony = new Harmony("com.yourdomain.eventdialogpatch");
        harmony.PatchAll();
    }

    [HarmonyPatch]
    static class ShowEventsRenamedDialogPatch
    {
        static MethodBase TargetMethod()
        {
            var type = AccessTools.TypeByName("FMODUnity.EventManager"); 
            return AccessTools.Method(type, "ShowEventsRenamedDialog", null, null);
        }

        static bool Prefix()
        {
            MyEventReferenceUpdater.ShowWindow();
            return false;
        }
    }
}
#endif