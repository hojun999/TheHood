using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public int itemID;
    public bool stackable;       // ���� ��ø�� ������ ����������.
}

public interface IUsable
{
    void UseItem();
}