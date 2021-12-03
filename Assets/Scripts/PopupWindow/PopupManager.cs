using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : SingleToneMonoBehaviour<PopupManager>
{
    PopupWindowBase m_Popup;
    readonly string m_kPopupPath = "Prefabs/Title/DefaultPopup";
    public static PopupWindowBase InvokePopup()
    {
        Instance.m_Popup = Instantiate(Resources.Load<PopupWindowBase>(Instance.m_kPopupPath), Instance.transform, false);
        Instance.m_Popup.transform.localRotation = Quaternion.identity;
        Instance.m_Popup.transform.localScale = Vector3.one;
        Instance.m_Popup.transform.localPosition = Vector3.zero;
        Instance.m_Popup.transform.SetAsLastSibling(); // 가장 마지막 배치로 먼저나오게

        return Instance.m_Popup;
    }

    public static void Clear()
    {
        Instance.m_Popup = null;
    }
}