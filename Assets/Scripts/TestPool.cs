using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public GameObject Prefab;

    private void Start()
    {
        PoolManager.Instance.CreatePool(Prefab, 3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PoolManager.Instance.ReuseObject(Prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
