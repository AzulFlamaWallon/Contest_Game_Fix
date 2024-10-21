using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 풀링 제네릭 버젼
/// https://github.com/LiamEderzeel/Pool
/// </summary>
/// <typeparam name="T"></typeparam>
public class SpawnerEx<T> : IEnumerable where T : IResettable
{
    public List<T> spawners = new List<T>();         // 스폰오브젝트 리스트
    public HashSet<T> deactiable = new HashSet<T>(); // 비활성화 가능 해쉬셋
    ISpawnFactory<T> factory;                       // 뿌릴 오브젝트 팩토리
    int maxCount;

    public SpawnerEx(ISpawnFactory<T> _SpawnFactory) : this(_SpawnFactory, 3) { } // 기본생성자는 3개만
    public SpawnerEx(ISpawnFactory<T> _SpawnFactory, int _MaxSpawnAmount)
    {
        factory = _SpawnFactory;
        maxCount = _MaxSpawnAmount;
        for (int i = 0; i< _MaxSpawnAmount; i++)
        {
            Spawn();
        }

        
    }

    /// <summary>
    /// 오브젝트를 할당해줍니다.
    /// </summary>
    /// <returns></returns>
    public T Allocate()
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            if (!deactiable.Contains(spawners[i]))
            {
                deactiable.Add(spawners[i]);
                return spawners[i];
            }
        }


        T newspawners = Spawn();
        deactiable.Add(newspawners);
        return newspawners;
    }

    public void DeActive(T _Spawner)
    {
        _Spawner.DeActive();
        deactiable.Remove(_Spawner);
    }

    T Spawn()
    {
        //T spawner = default;

        T spawner = factory.Spawn();
        spawners.Add(spawner);

        return spawner;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return spawners.GetEnumerator();
    }

   
}
