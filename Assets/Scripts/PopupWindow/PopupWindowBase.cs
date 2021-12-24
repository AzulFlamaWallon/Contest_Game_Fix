using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupWindowBase : PanelPopupBase
{
    [Header("버튼 오브젝트")]
    public Button obj_Ok;
    public Button obj_Cancel;

    [Header("텍스트")]
    public Text title;
    public Text desc;

    public Text txt_Ok;
    public Text txt_Cancel;

    [Header("애니메이터")]
    public Animator animator;

    private Action<PopUpResult> m_PopupAction;

    public bool IsShowCancel { get; private set; }
    public PopUpResult OutResult { get; private set; }

    public PopupWindowBase Ok(string _Text = null, Action<PopUpResult> _Callback = null)
    {        
        txt_Ok.text = _Text;
        m_PopupAction = _Callback;
        obj_Ok.onClick.AddListener(OnPressOKButton);
        obj_Ok.gameObject.SetActive(true);
        return this;
    }
    public PopupWindowBase Cancel(string _Text =  null, Action<PopUpResult> _Callback = null)
    {
        txt_Cancel.text = _Text;
        m_PopupAction = _Callback;
        obj_Cancel.onClick.AddListener(OnPressCancelButton);
        IsShowCancel = true;
        return this;
    }
    public PopupWindowBase Title(string _Text = null)
    {
        title.text = _Text;

        return this;
    }

    public void Show(string _Title, string _Desc)
    {
        title.text = _Title;
        desc.text = _Desc;
        obj_Cancel.gameObject.SetActive(IsShowCancel);
        Show();
    }

    public void Show(string _Desc)
    {
        desc.text = _Desc;
        obj_Cancel.gameObject.SetActive(IsShowCancel);
        Show();
    }

    public override void ShowPopup()
    {
        animator.SetTrigger("Activate");
    }

    public override void HidePopup()
    {
        animator.SetTrigger("Deactivate");
    }

    public void OnPressOKButton()
    {
        OutResult = PopUpResult.OK;
        m_PopupAction?.Invoke(OutResult);
        Hide();
    }

    public void OnPressCancelButton()
    {
        OutResult = PopUpResult.Cancel;
        m_PopupAction?.Invoke(OutResult);
        Hide();
    }


}

/// <summary>
/// 버튼결과
/// </summary>
public enum PopUpResult
{
    None,
    OK,
    Cancel,
    Abort,
    Retry,
    Ignore,
    Yes,
    No
}
