
using System;
using UnityEngine;
using System.Collections.Generic;

public class Factory<M> : MonoBehaviour where M : MonoBehaviour
{
    public T Create<T>() where T : M
    {
        return Create(typeof(T)) as T;
    }

    public M Create(Type mType)
    {
        Assert.True(m_spawnPools.ContainsKey(mType),
                    typeof(M) + " " + mType + " not found in factory");
        var pool = m_spawnPools[mType];
        return pool.Spawn();
    }

    public void Destroy(M b)
    {
        if (b == null)
        {
            return;
        }

        var mType = b.GetType();
        Assert.True(m_spawnPools.ContainsKey(mType),
                    typeof(M) + " " + mType + " not found in factory");
        m_spawnPools[mType].Despawn(b);
    }

    public IEnumerable<Type> AllTypes { get { return m_spawnPools.Keys; } }

    //////////////////////////////////////////////////

    private Dictionary<Type, SpawnPool<M>> m_spawnPools
        = new Dictionary<Type, SpawnPool<M>>();

    //////////////////////////////////////////////////

    private void Awake()
    {
        var spawnMStore = new GameObject("pool").transform;
        spawnMStore.parent = transform;

        var allMs = GetComponentsInChildren<Building>();

        foreach(var mType in allMs)
        {
            Assert.True(m_spawnPools.ContainsKey(mType.GetType()) == false,
                        "mType " + mType.gameObject + " found twice in prototype list");
            m_spawnPools[mType.GetType()] 
                = new SpawnPool<M>(mType.gameObject, spawnMStore);
        }
    }

}