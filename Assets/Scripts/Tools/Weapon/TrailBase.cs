using System;
using UnityEngine;

public class TrailBase<T> : MonoBehaviour, IResettable
{
    public T currentObj;
    public float speed;
    //TrailRenderer trail;
    LineRenderer trail;
    [HideInInspector]
    public Vector3 rayPositon;
    //public bool Autodestruct { get; protected set; } = false;

    void Start()
    {
        trail = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (trail != null) // 트레일이 있을때만 연산하자
            UpdateTrailPositon();
    }

    public virtual void UpdateTrailPositon()
    {
        transform.position = Vector3.Lerp(transform.position, rayPositon, Time.deltaTime * speed);
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }
}