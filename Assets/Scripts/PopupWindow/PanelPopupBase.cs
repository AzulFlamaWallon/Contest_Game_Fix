using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PanelPopupBase : MonoBehaviour
{
    public RectTransform     RectTrf { get; private set; }
    public GameObject        PopupPanelObj { get; private set; }
    public bool              IsShow { get; private set; }
    public virtual PopupType PopupType => PopupType.None;

    void Initialize()
    {
        RectTrf = GetComponent<RectTransform>();
        PopupPanelObj = gameObject;
    }

    public void Show()
    {
        if (IsShow) 
            return;

        IsShow = true;

        Initialize();
        UpdatePopup();
        ShowPopup();
        PopupPanelObj.SetActive(true);
    }

    public void Hide()
    {
        if (!IsShow)
            return;

        IsShow = false;
        HidePopup();
        PopupPanelObj.SetActive(false);
    }


    public virtual void ShowPopup() { }
    public virtual void HidePopup() { }
    public virtual void UpdatePopup() { }
    
}

public enum PopupType
{
    None,
    LoginScenePopup,
    ErrorPopup,
    MatchPopup
}