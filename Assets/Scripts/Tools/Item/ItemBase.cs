using UnityEngine;
using Network.Data;

public class ItemBase : MonoBehaviour, IResettable
{
    public ItemInfo item; // 이건 공통적으로 들어가는 아이템 데이터
    public Item_Data itemData; // 개별적으로 들어가는 아이템 위치 데이터
    public virtual void Init()
    {
        itemData.Init();
    }
    public virtual void Init(GameObject _Go)
    {

    }
    public void DeActive()
    {
        gameObject.SetActive(false);
    }
}
