using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ObjectManager:MonoBehaviour
{
    public Spawner Spawner;
    public GameObject Prefab;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        var obj = Instantiate(Prefab);
        obj.transform.position = Spawner.SpawnPosition();
    }
}