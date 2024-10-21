using UnityEngine.UI;

public class GUI_Widget_Role : GUI_Widget_Base
{
    public Text RoleText;

    void Update()
    {
        if (m_Player == null)
            return;

        RoleText.text = m_Player.m_MyProfile.Role_Index == 1 ? "Guard" : "Thief";
    }
}
