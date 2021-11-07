using System;
using UnityEngine;
using Network.Data;

[CreateAssetMenu(fileName = "Item", menuName = "GreyZoneTools/Create Item")]
[Serializable]
public class ItemInfo : ScriptableObject
{
    public ushort itemID;
    public ItemType itemType;
    public string itemName;
    public string itemDesc;
    public byte itemCount;
    public AudioSource[] itemSnds;
}

[Flags]
public enum ItemType
{
    NONE = 0,
    SYSTEM,
    REINFORCE,
    TRAP,
    SECURITY
}