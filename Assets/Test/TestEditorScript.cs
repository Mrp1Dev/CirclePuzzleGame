#if UNITY_EDITOR && false
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NaughtyAttributes;
using UnityEngine;

public class Field : DropdownAttribute
{
    public MonoBehaviour monoBehaviour;
    public Field
    public Field(string monobehaviour) : base("")
    {
    }

    public DropdownList<FieldInfo> GetFields() =>
        new DropdownList<FieldInfo>
        {
        };
}

[ExecuteInEditMode]
public class TestEditorScript : MonoBehaviour
{
    private void Update()
    {
        var mbs = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        foreach (var mb in mbs)
        {
            var fields = mb.GetType().GetFields();
            foreach (var field in fields)
            {
                var attrib = field.GetType().GetCustomAttribute<Field>();
                if (attrib != null)
                {
                }
            }
        }
    }
}

#endif
