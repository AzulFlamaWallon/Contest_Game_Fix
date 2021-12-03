using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(CharacterController), true)]
public class CustomCharacterControllerInsfector : Editor
{
    CharacterController m_TargetVars;
 
    public bool isDebug;


    void OnEnable()
    {
        // target은 위의 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객체에 대한 레퍼런스
        // object형이므로 실제 사용할 타입으로 캐스팅 해 준다.
        m_TargetVars = target as CharacterController;
        CustomEditorProperty.serializedObject = serializedObject;
    }

    /// <summary>
    /// 커스텀에디터 구현 함수 재 정의.
    /// 인스펙터에서 보여지는 정보를 여기서 고쳐줍니다.
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUIStyle debugLabel = UILayout.CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black)));

        EditorGUILayout.LabelField(new GUIContent("디버깅 인스펙터 활성화 여부"), debugLabel);
 
        isDebug = EditorGUILayout.Toggle(new GUIContent("디버깅중입니까?", "디버깅할때만 나타낼 값을 보여줍니다"), isDebug);
        if(isDebug)
        {
            EditorGUILayout.LabelField(new GUIContent("나의 캐릭터 프로필값"), UILayout.CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black))));
            CustomEditorProperty.UseProperty("m_MyProfile");
            CustomEditorProperty.UseProperty("m_Is_Stunned");
        }

        EditorGUILayout.LabelField(new GUIContent("캐릭터 스탯 및 물리메테리얼"), UILayout.CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black))));
        m_TargetVars.battery = EditorGUILayout.IntField(new GUIContent("배터리", "배터리 수치"), m_TargetVars.battery);
        m_TargetVars.moveSpeed = EditorGUILayout.FloatField(new GUIContent("이동속도 (m/s)", "이동속도"), m_TargetVars.moveSpeed);
        CustomEditorProperty.UseProperty("noFriction", "마찰력 없음", "마찰력을 없애는 메테리얼");
        CustomEditorProperty.UseProperty("fullFriction", "마찰력 있음", "마찰력이 있는 메테리얼");

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(new GUIContent("캐릭터 무기 및 좌표"), UILayout.CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.black))));
        CustomEditorProperty.UseProperty("m_ToolAxis", "툴 좌표", "툴을 든 손의 좌표");
        CustomEditorProperty.UseProperty("m_Tools", "툴들", "툴들의 배열");
        CustomEditorProperty.UseProperty("m_CameraAxis", "카메라(시선) 좌표", "캐릭터가 바라볼 카메라의 위치");
        if(isDebug)
        {
            EditorGUILayout.Space();
            CustomEditorProperty.UseProperty("m_Before_Position", "이전 위치", "캐릭터의 이전에 있던 위치");
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(new GUIContent("아이템 인식 설정"), UILayout.CustomizeGUIStyle(new CustomLabel(FontStyle.Bold, 12, Color.white, new CustomTex2D(10, 5, Color.grey))));

        CustomEditorProperty.UseProperty("acquireDist", "아이템 인식거리", "아이템 인식거리");
        EditorGUILayout.Space();
        CustomEditorProperty.UseProperty("itemSearchDuration", "아이템 탐색 주기", "아이템 탐색 주기");
        CustomEditorProperty.UseProperty("ItemLayer", "아이템 레이어", "아이템 레이어");


        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

}