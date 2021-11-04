using UnityEngine;
using Network.Data;

public class ItemBase : MonoBehaviour, IResettable
{
    public ItemInfo item;

    public virtual void Init()
    {

    }
    public virtual void Init(GameObject _Obj)
    {

    }

    public void SetItemData(Item_Data _ItemData)
    {
        item.item_data = _ItemData;
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }
}
