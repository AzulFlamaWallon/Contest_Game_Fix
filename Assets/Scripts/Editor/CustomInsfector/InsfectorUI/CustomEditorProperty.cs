using UnityEngine;
using UnityEditor;

public static class CustomEditorProperty
{
    public static SerializedObject serializedObject;

    /// <summary>
    /// 해당 변수를 원래의 Public 형태로 사용하게 만들어줍니다.
    /// </summary>
    /// <param name="_PropName">대상 변수이름</param>
    public static void UseProperty(string _PropName)
    {
        SerializedProperty prop = serializedObject.FindProperty(_PropName);
        EditorGUI.BeginChangeCheck(); //값이 바뀌는지 검사시작
        EditorGUILayout.PropertyField(prop, true); // 배열까지 싸그리 필드생성되게끔

        if (EditorGUI.EndChangeCheck()) //만약 검사가 끝날무렵 필드에 변화가 생겼다면
        {
            serializedObject.ApplyModifiedProperties(); // 원래 변수에 값 적용
        }
    }

    public static void UseProperty(string _VarName, string _PropName, string _PropDesc)
    {
        SerializedProperty prop = serializedObject.FindProperty(_VarName);
        EditorGUI.BeginChangeCheck(); //값이 바뀌는지 검사시작
        EditorGUILayout.PropertyField(prop, new GUIContent(_PropName, _PropDesc), true); // 배열까지 싸그리 필드생성되게끔 필드는 커스터마이징

        if (EditorGUI.EndChangeCheck()) //만약 검사가 끝날무렵 필드에 변화가 생겼다면
        {
            serializedObject.ApplyModifiedProperties(); // 원래 변수에 값 적용
        }
    }
}
