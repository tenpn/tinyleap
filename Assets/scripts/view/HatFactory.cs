
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HatFactory : MonoBehaviour
{
    public Transform GetHat(Type resourceType) 
    {
        var resourceName = resourceType.ToString();
        if (m_spawnPools.ContainsKey(resourceName) == false)
        {
            // no hat for this resource yet
            return null;
        }

        return m_spawnPools[resourceName].Spawn();
    }

    public void Reset()
    {
        foreach(var pool in m_spawnPools.Values)
        {
            pool.DespawnAll();
        }
    }

    //////////////////////////////////////////////////

    private Dictionary<string, SpawnPool<Transform>> m_spawnPools
        = new Dictionary<string, SpawnPool<Transform>>();

    //////////////////////////////////////////////////

    private void Start()
    {
        var spawnStore = new GameObject("pool").transform;
        spawnStore.parent = transform;

        var allResources = Grid.Instance.ResourceTypes
            .Select(res => res.ToString())
            .ToArray();

        foreach(Transform hat in transform)
        {
            if (allResources.Contains(hat.gameObject.name) == false)
            {
                continue;
            }

            var newPool = new SpawnPool<Transform>(hat.gameObject, spawnStore);
            m_spawnPools[hat.gameObject.name] = newPool;
        }
    }

}