using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{

    [Header("Only gameplay")]
    public ItemType type;
    public ActionType actionType;

    [Header("Only UI")]
    public bool stackable;       // 개수 중첩이 가능한 아이템인지.
    public bool consumable;      // 소비 아이템인지.

    [Header("Both")]
    public Sprite image;


}
public enum ItemType
{
    Quest,
    posion,
}
public enum ActionType
{
    Healing,
    speedUp,
    required_in_Quest2
}
