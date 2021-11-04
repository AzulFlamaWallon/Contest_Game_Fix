using Greyzone.GUI;
using Network.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemManager : SingleToneMonoBehaviour<ItemManager>
{
    [Header("아이템 풀링 설정")]
    public int maxItem = 2;

    SpawnerEx<ItemBase> m_Spawner;
    List<Item_Data> m_ServerItemDataList = new List<Item_Data>();
    [HideInInspector]
    public List<GameObject> itemPrefabList = new List<GameObject>();

    readonly string m_kItemPath = "Prefabs/Items/Item@";
    readonly string m_kMemoryChipItemPath = "Prefabs/Items/Item@MemoryChip"; // 임시

    GameObject m_ItemPrefab;

    void Start()
    {
        m_ItemPrefab = Resources.Load<GameObject>(m_kMemoryChipItemPath);
    }

    public List<Item_Data> GetServerItemList()
    {
        return m_ServerItemDataList;
    }

    /// <summary>
    /// 아이템 리스트를 서버아이템리스트로 할당합니다.
    /// </summary>
    /// <param name="_Items"></param>
    public void AllocateItemListFromServer(Item_Data[] _Items)
    {
        m_ServerItemDataList = new List<Item_Data>(_Items);
    }
    /// <summary>
    /// 서버에서 받아오는 아이템리스트를 오브젝트에 할당합니다.
    /// 일단 서버에서 보내는 OID가 메모리칩뿐이니 메모리칩만 로드
    /// </summary>
    public void AllocateItemFromServer()
    {
        int obj_lenth = itemPrefabList.Count;
        for (var i = 0; i < obj_lenth; ++i)
        {
            ClearItem(i);
        }

        maxItem = m_ServerItemDataList.Count;

        if (itemPrefabList.Count <= maxItem) // 중복 스폰 방지
        {
            StringBuilder strPath = new StringBuilder(m_kItemPath);

            GameObject temp = Resources.Load<GameObject>(strPath.Append("MemoryChip").ToString());
            if (temp == null)
            {
                m_Spawner = new SpawnerEx<ItemBase>(new PrefabSpawnFactory<ItemBase>(m_ItemPrefab, m_ItemPrefab.name, _IsAddCompo: true), maxItem);
            }

            m_Spawner = new SpawnerEx<ItemBase>(new PrefabSpawnFactory<ItemBase>(temp, temp.name, _IsAddCompo: false), maxItem);
            ItemBase tempItem = temp.GetComponent<ItemBase>();
            Spawn(tempItem);
        }       
    }

    public void Spawn()
    {
        ItemBase item = m_Spawner.Allocate();
        m_Spawner.DeActive(item);
        item.gameObject.SetActive(true);
    }

    public void Spawn(ItemBase _Obj)
    {
        _Obj = m_Spawner.Allocate();
        for (int i = 0; i < maxItem; i++)
        {
            _Obj.transform.position = m_ServerItemDataList[i].Position;
            _Obj.transform.rotation = Quaternion.Euler(m_ServerItemDataList[i].Rotation);
            _Obj.SetItemData(m_ServerItemDataList[i]);
            m_Spawner.DeActive(_Obj);
            _Obj.gameObject.SetActive(true);
            itemPrefabList.Add(_Obj.gameObject);
        }
        
    }

    public void OnGetItemInstID(int _ItemInstID)
    {
        int obj_lenth = itemPrefabList.Count;
        Item_Data[] itemDat = new Item_Data[obj_lenth];
        for (var i = 0; i < obj_lenth; ++i)
        {
            if (itemPrefabList[i].activeInHierarchy) // 오브젝트가 활성화되어있을때만
            {
                // obj 에게서 item_data 빼내기
                itemDat[i] = itemPrefabList[i].GetComponentInChildren<ItemBase>().item.item_data;
                // 대조해서 맞으면 오브젝트를 제거
                if (itemDat[i].IID == _ItemInstID)
                {
                    TooltipManager.Instance.InvokeTooltip(_ =>
                    {
                        _.ShowMessage(MessageStyle.ON_SCREEN_UP_MSG, "먹은 아이템을 삭제합니다.");
                    }, MessageStyle.ON_SCREEN_UP_MSG);
                    ClearItem(i);
                }
            }
        }
    }

    public void ClearItem(int _Index)
    {
        itemPrefabList[_Index].GetComponent<ItemBase>().DeActive();
    }

    public void DestroyItem(int _Index)
    {
        Destroy(itemPrefabList[_Index].gameObject);
        itemPrefabList.RemoveAt(_Index);
    }
}

