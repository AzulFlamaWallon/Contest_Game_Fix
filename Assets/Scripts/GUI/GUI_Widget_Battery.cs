using UnityEngine.UI;

public class GUI_Widget_Battery : GUI_Widget_Base
{
    public Text m_Text;
    int battery = 0;

    void Update()
    {
        if (m_Player == null)
            return;

        battery = m_Player.m_MyProfile.Battery;
        m_Text.text = "" + battery;
    }
}
