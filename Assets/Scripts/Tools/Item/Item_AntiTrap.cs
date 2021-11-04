using System.Linq;
using UnityEngine;

public class Item_AntiTrap : ItemBase
{
    public bool IsUseRange = false;
    public float Range;

    public void Awake()
    {
        item.itemID = 1002;
        item.itemType = ItemType.REINFORCE;
        item.itemName = "안티";
        item.itemCount = 0;
        item.itemDesc = "함정 무력화";
    }

    public override void Init(GameObject _Obj)
    {
        
    }
}