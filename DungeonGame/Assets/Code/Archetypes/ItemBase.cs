using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    public string Title;
    public abstract string Info { get; }
    public int Price;
    public Sprite Icon;

    public abstract bool isWeapon { get; }
}