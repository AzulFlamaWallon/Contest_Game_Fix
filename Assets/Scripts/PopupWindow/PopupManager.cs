using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopupManager : SingleToneMonoBehaviour<PopupManager>
{   
    PopupWindowBase m_Popup;
    readonly string m_kPopupPath = "Prefabs/Title/DefaultPopup";
    public bool IsOnlyOneInvoke { get; set; }
    public static PopupWindowBase PrecachedPopup { get; private set; }
    public int PopupCount { get; protected set; }
    public static PopupWindowBase InvokePopup()
    {
        Instance.m_Popup = Instantiate(Resources.Load<PopupWindowBase>(Instance.m_kPopupPath), Instance.transform, false);
        Instance.m_Popup.transform.localRotation = Quaternion.identity;
        Instance.m_Popup.transform.localScale = Vector3.one;
        Instance.m_Popup.transform.localPosition = Vector3.zero;
        Instance.m_Popup.transform.SetAsLastSibling(); // 가장 마지막 배치로 먼저나오게       
        Instance.PopupCount++;
        return Instance.m_Popup;
    }

    public static void CreatePrecachePopup()
    {
        if(Instance.IsOnlyOneInvoke && Instance.PopupCount > 1)
        {
            return;
        }
        PrecachedPopup = Instantiate(Resources.Load<PopupWindowBase>(Instance.m_kPopupPath), Instance.transform, false);
        PrecachedPopup.transform.localRotation = Quaternion.identity;
        PrecachedPopup.transform.localScale = Vector3.one;
        PrecachedPopup.transform.localPosition = Vector3.zero;
        PrecachedPopup.transform.SetAsLastSibling(); // 가장 마지막 배치로 먼저나오게
        PrecachedPopup.gameObject.SetActive(false);
        Instance.PopupCount++;
    }

    public static void ShowPrecachePopup()
    {
        PrecachedPopup.gameObject.SetActive(true);
    }

    public static void Clear()
    {
        Instance.m_Popup = null;
        Instance.PopupCount--;
    }

    public static void ClearPrecachePopup()
    {
        PrecachedPopup = null;
        Instance.PopupCount--;
    }
}