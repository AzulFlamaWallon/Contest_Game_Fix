using UnityEngine;

public class PrefabSpawnFactory<T> : ISpawnFactory<T> where T : MonoBehaviour
{
    GameObject m_Prefab;
    string m_Name;
    int m_Index = 0;
    bool IsAddComponent;

    public PrefabSpawnFactory(GameObject _Prefab) : this(_Prefab, _Prefab.name) { }

    public PrefabSpawnFactory(GameObject _Prefab, string _Name)
    {
        m_Prefab = _Prefab;
        m_Name = _Name;
    }

    public PrefabSpawnFactory(GameObject _Prefab, string _Name, bool _IsAddCompo)
    {
        m_Prefab = _Prefab;
        m_Name = _Name;
        IsAddComponent = _IsAddCompo;
    }

    public T Spawn()
    {
        GameObject tempGameObject = GameObject.Instantiate(m_Prefab);

        if (IsAddComponent)
        {
            tempGameObject.name = m_Name + m_Index.ToString();
            tempGameObject.AddComponent<T>();
        }
        else
        {
            tempGameObject.name = m_Name;
        }
        T objectOfType = tempGameObject.GetComponent<T>();
        m_Index++;
        return objectOfType;
    }
}