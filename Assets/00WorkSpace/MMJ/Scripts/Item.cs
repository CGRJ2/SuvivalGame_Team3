using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public bool isStackable;

    public virtual void Use()
    {
        Effect();
    }

    protected abstract void Effect();
}
