using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable{ void UseItem(); }

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public int itemID;

    public enum type
    {
        quest,
        usable,
        etc
    }

    public type itemType;
    public bool stackable;       // 개수 중첩이 가능한지
}


