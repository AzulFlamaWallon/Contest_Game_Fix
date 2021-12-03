using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon), true)]
//
// 무기 스크립트 인스펙터를 편하게 사용하기 위한 에디터 클래스
// 참고자료: https://kupaprogramming.tistory.com/30
public class CustomWeaponInsfector : Editor
{
    Weapon m_WeaponEdit;

    void OnEnable()
    {
        // target은 위의 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객체에 대한 레퍼런스
        // object형이므로 실제 사용할 타입으로 캐스팅 해 준다.
        m_WeaponEdit = target as Weapon;
        CustomEditorProperty.serializedObject = serializedObject;
    }

    /// <summary>
    /// 커스텀에디터 구현 함수 재 정의.
    /// 인스펙터에서 보여지는 정보를 여기서 고쳐줍니다.
    /// </summary>
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(new GUIContent("발사체를 보여줄지 여부"));
        m_WeaponEdit.isDrawRay = EditorGUILayout.Toggle(new GUIContent("IsDrawRay", "발사체의 경로를 보여줍니까?"), m_WeaponEdit.isDrawRay);

        EditorGUILayout.Space();
        CustomEditorProperty.UseProperty("WeaponCategory");

        switch (m_WeaponEdit.WeaponCategory.eWeaponDetectionType)
        {
            case WeaponDetectionType.HITSCAN:
                CustomEditorProperty.UseProperty("pellet");
                break;
            case WeaponDetectionType.ENERGY:
                break;
            case WeaponDetectionType.PROJECTILE:
                break;
        }      
        EditorGUILayout.Space();

        m_WeaponEdit.wponName     = EditorGUILayout.TextField(new GUIContent("WeaponName", "무기명"), m_WeaponEdit.wponName);
        m_WeaponEdit.shotInternal = EditorGUILayout.FloatField(new GUIContent("ShotInternal", "발사간격"), m_WeaponEdit.shotInternal);
        m_WeaponEdit.clip         = EditorGUILayout.IntField(new GUIContent("Clip", "장탄수"), m_WeaponEdit.clip);
        m_WeaponEdit.shotDistance = EditorGUILayout.FloatField(new GUIContent("ShotDist", "발사사거리"), m_WeaponEdit.shotDistance);
        m_WeaponEdit.traceFilter  = EditorGUILayout.LayerField(new GUIContent("TrFilter", "필터레이어"), m_WeaponEdit.traceFilter);

        CustomEditorProperty.UseProperty("m_Aim_Ani");
        CustomEditorProperty.UseProperty("ownerNoGunInArm");
        CustomEditorProperty.UseProperty("ownerArm");
        CustomEditorProperty.UseProperty("ownerMuzzle");

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }   
}
