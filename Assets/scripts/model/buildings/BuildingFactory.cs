
using System;
using UnityEngine;
using System.Collections.Generic;

public class BuildingFactory : MonoBehaviour
{
    public T Create<T>() where T : Building
    {
        var buildingType = typeof(T);
        Assert.True(m_spawnPools.ContainsKey(buildingType),
                    "building " + buildingType + " not found in factory");
        var pool = m_spawnPools[buildingType];
        return pool.Spawn() as T;
    }

    public void Destroy(Building b)
    {
        if (b == null)
        {
            return;
        }

        var buildingType = b.GetType();
        Assert.True(m_spawnPools.ContainsKey(buildingType),
                    "building " + buildingType + " not found in factory");
        m_spawnPools[buildingType].Despawn(b);
    }

    public IEnumerable<Type> AllBuildingTypes { get { return m_spawnPools.Keys; } }

    //////////////////////////////////////////////////

    private Dictionary<Type, SpawnPool<Building>> m_spawnPools
        = new Dictionary<Type, SpawnPool<Building>>();

    //////////////////////////////////////////////////

    private void Awake()
    {
        var spawnBuildingStore = new GameObject("pool").transform;
        spawnBuildingStore.parent = transform;

        var allBuildings = GetComponentsInChildren<Building>();

        foreach(var building in allBuildings)
        {
            Assert.True(m_spawnPools.ContainsKey(building.GetType()) == false,
                        "building " + building.gameObject + " found twice in prototype list");
            m_spawnPools[building.GetType()] 
                = new SpawnPool<Building>(building.gameObject, spawnBuildingStore);
        }
    }

}