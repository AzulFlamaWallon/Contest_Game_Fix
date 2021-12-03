using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Weapon_StunGun), true)]
public class CustomPelletInsfector : Editor
{
    Weapon_StunGun m_ShotGunInsf;

    void OnEnable()
    {
        // target은 위의 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객체에 대한 레퍼런스
        // object형이므로 실제 사용할 타입으로 캐스팅 해 준다.
        m_ShotGunInsf = target as Weapon_StunGun;
    }

    /// <summary>
    /// 커스텀에디터 구현 함수 재 정의.
    /// 인스펙터에서 보여지는 정보를 여기서 고쳐줍니다.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(m_ShotGunInsf.m_IsDebug)
        {
            EditorGUILayout.LabelField("스턴 딜레이", m_ShotGunInsf.stunDuration.ToString());
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}