using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event)]
public class ClearOnReloadAttribute : Attribute
{
    public readonly bool assignNewTypeInstance;
    public readonly object valueToAssign;

    /// <summary>
    ///     Marks field, property or event to be cleared on reload.
    /// </summary>
    public ClearOnReloadAttribute()
    {
        valueToAssign = null;
        assignNewTypeInstance = false;
    }

    /// <summary>
    ///     Marks field of property to be cleared and assigned given value on reload.
    /// </summary>
    /// <param name="valueToAssign">
    ///     Explicit value which will be assigned to field/property on reload. Has to match
    ///     field/property type. Has no effect on events.
    /// </param>
    public ClearOnReloadAttribute(object valueToAssign)
    {
        this.valueToAssign = valueToAssign;
        assignNewTypeInstance = false;
    }

    /// <summary>
    ///     Marks field of property to be cleared or re-initialized on reload.
    /// </summary>
    /// <param name="assignNewTypeInstance">
    ///     If true, field/property will be assigned a newly created object of its type on
    ///     reload. Has no effect on events.
    /// </param>
    public ClearOnReloadAttribute(bool assignNewTypeInstance = false)
    {
        valueToAssign = null;
        this.assignNewTypeInstance = assignNewTypeInstance;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ExecuteOnReloadAttribute : Attribute
{
}