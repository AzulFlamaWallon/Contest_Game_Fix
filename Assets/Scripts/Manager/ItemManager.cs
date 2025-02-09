﻿using Greyzone.GUI;
using Network.Data;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemManager : SingleToneMonoBehaviour<ItemManager>
{
    protected float removeRange = 10.0f;

    [HideInInspector]
    public List<ItemBase> itemList = new List<ItemBase>();
    List<Item_Data> m_ServerItemDataList = new List<Item_Data>();
    int m_FieldItemCount;

    GameObject m_ItemPrefab;   
    readonly string m_kItemPath = "Prefabs/Items/Item@";
    readonly string m_kMemoryChipItemPath = "Prefabs/Items/Item@MemoryChip"; // 임시
    StringBuilder m_strItemPath;

    void Start()
    {
        m_ItemPrefab = Resources.Load<GameObject>(m_kMemoryChipItemPath);
        m_strItemPath = new StringBuilder(m_kItemPath);
        m_FieldItemCount = 0;
        itemList.Clear();
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
        m_ServerItemDataList.Clear();
        m_ServerItemDataList = new List<Item_Data>(_Items);
    }
    /// <summary>
    /// 서버에서 받아오는 아이템리스트를 오브젝트에 할당합니다.
    /// 일단 서버에서 보내는 OID가 메모리칩뿐이니 메모리칩만 로드
    /// </summary>
    public void AllocateItemFromServer()
    {
        m_strItemPath.Clear();
        GameObject temp = Resources.Load<GameObject>(m_strItemPath.Append(m_kItemPath).Append("MemoryChip").ToString());       
        Spawn(temp, m_ServerItemDataList.Count);
    }

    /// <summary>
    /// 만약 아이템 획득했음에도 불구하고 남아있을시 이녀석을 호출해서 지워줍시다.
    /// </summary>
    /// <param name="_Center"></param>
    public void RemoveDummyItems()
    {
        int item_Count = itemList.Count;
        for(int i =0; i<item_Count;i++)
        {
            DestroyItem(i);
        }
        itemList.Clear();
    }

    void Spawn(GameObject _Temp, int _Count)
    {
        itemList.Capacity = _Count;

        bool spawn = false;

        for (int i = 0; i < _Count; i++)
        {
            m_FieldItemCount++;
            if (m_FieldItemCount <= _Count)
            {
                spawn = true;
            }
            else if(m_FieldItemCount > _Count)
            {
                m_FieldItemCount--;
                spawn = false;
            }

            if(spawn)
            {
                GameObject temp_go = Instantiate(_Temp,
                     m_ServerItemDataList[i].Position,
                     Quaternion.Euler(m_ServerItemDataList[i].Rotation));

                if (temp_go.TryGetComponent(out ItemBase item_compo))
                {
                    item_compo.Init();
                    item_compo.itemData = m_ServerItemDataList[i];
                    itemList.Add(item_compo);
                }
            }
        }
    }

    public void OnGetItemInstID(int _ItemInstID)
    {
        int obj_lenth = itemList.Count;
        for (var i = 0; i < obj_lenth; ++i)
        {
            // obj 에게서 item_data 빼내기
            Item_Data itemDat = itemList[i].itemData;
            // 대조해서 맞으면 오브젝트를 제거
            if (itemDat.IID == _ItemInstID)
            {
                TooltipManager.Instance.InvokeTooltip(_ =>
                {
                    _.ShowMessage(MessageStyle.ON_SCREEN_UP_MSG, "먹은 아이템을 삭제합니다.");
                }, MessageStyle.ON_SCREEN_UP_MSG);
                DestroyItem(i);
                m_FieldItemCount--;
            }
        }
    }

    public void DestroyItem(int _Index)
    {
        Destroy(itemList[_Index].gameObject);
        itemList.Remove(itemList[_Index]);
    }
}

