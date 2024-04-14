using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public int itemID;
    public bool stackable;       // 개수 중첩이 가능한 아이템인지.
}

public interface IUsable
{
    void UseItem();
}