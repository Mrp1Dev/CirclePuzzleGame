using System;
using System.Reflection;
using UnityEngine;

namespace MUtility
{
    public class TrySelfInit : Attribute { }
    public class TryParentInit : Attribute { }

    public static class MAttributes
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnLoad()
        {
            var components = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
            foreach (var component in components)
            {
                foreach (var field in component.GetType().GetRuntimeFields())
                {
                    TryPerformSelfInit(component, field);
                    TryPerformParentInit(component, field);
                }
            }
        }

        private static void TryPerformSelfInit(MonoBehaviour component, FieldInfo field)
        {
            if (field.GetCustomAttribute(typeof(TrySelfInit)) == null) return;
            if (!field.FieldType.IsSubclassOf(typeof(Component))) return;
            if (!field.GetValue(component).Equals(null)) return;
            if (component.TryGetComponent(field.FieldType, out var wanted))
            {
                field.SetValue(component, wanted);
            }

        }
        private static void TryPerformParentInit(MonoBehaviour component, FieldInfo field)
        {
            if (component.transform.parent == null) return;
            if (field.GetCustomAttribute(typeof(TryParentInit)) == null) return;
            if (!field.FieldType.IsSubclassOf(typeof(Component))) return;
            if (!field.GetValue(component).Equals(null)) return;
            Component wanted = component.GetComponentInParent(field.FieldType);
            if (wanted)
            {
                field.SetValue(component, wanted);
            }
        }
    }

}
