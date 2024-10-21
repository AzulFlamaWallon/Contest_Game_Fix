using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
{
    List<GameObject> spawnObjs = new List<GameObject>();
    GameObject spawnObj;
    int maxObjs = 10;

    // 기본 생성자
    public Spawner(GameObject objectToSpawn) : this(objectToSpawn, 1) { }

    // 오브젝트 및 개수 설정 생성자
    public Spawner(GameObject objectToSpawn, int count)
    {
        Create(objectToSpawn, count);
    }

    void Create(GameObject objectToSpawn, int count)
    {
        maxObjs = count;

        for (int i = 0; i <= maxObjs; i++)
        {
            GameObject temp = GameObject.Instantiate(objectToSpawn);
            spawnObj = temp;
            temp.SetActive(false);
            spawnObjs.Add(temp);
        }
    }


    public GameObject GetSpawnedObject()
    {
        foreach (var obj in spawnObjs)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
}
