using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Spawner : SingleToneMonoBehaviour<Spawner>
{
    [HideInInspector]
    public List<GameObject> spawnObjs;
    public GameObject spawnObj;
    public int maxObjs;

    void Start()
    {
        spawnObjs = new List<GameObject>();
        GameObject temp;
        for(int i = 0; i < maxObjs;i++)
        {
            temp = Instantiate(spawnObj);
            temp.SetActive(false);
            spawnObjs.Add(temp);
        }
    }

    public void SlowSpawn()
    {
        if(spawnObjs != null)
            spawnObjs.Clear();

        spawnObjs = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < maxObjs; i++)
        {
            temp = Instantiate(spawnObj);
            temp.SetActive(false);
            spawnObjs.Add(temp);
        }
    }

    public GameObject GetSpawnedObject()
    {
        for(int i = 0; i < maxObjs; i++)
        {
            if(!spawnObjs[i].activeInHierarchy)
            {
                return spawnObjs[i];
            }
        }
        return null;
    }

    public void SlowSelfDeactivateObject(GameObject _Obj, float _Time)
    {
        StartCoroutine(Deactivate(_Obj, _Time));
    }

    public void SelfDeactivateObject(ref GameObject _Obj)
    {
        _Obj.SetActive(false);
    }

    IEnumerator Deactivate(GameObject _Obj, float _Time)
    {
        yield return new WaitForSeconds(_Time);
        _Obj.SetActive(false);
        yield return null;
    }
}
