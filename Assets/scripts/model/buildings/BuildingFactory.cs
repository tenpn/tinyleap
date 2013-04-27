
using System;
using UnityEngine;
using System.Collections.Generic;

public class BuildingFactory : MonoBehaviour
{
    public SpawnPool<Building> this[Type buildingType]
    {
        get
        {
            Assert.True(m_spawnPools.ContainsKey(buildingType),
                        "building " + buildingType + " not found in factory");
            return m_spawnPools[buildingType];
        }
    }

    //////////////////////////////////////////////////

    private Dictionary<Type, SpawnPool<Building>> m_spawnPools
        = new Dictionary<Type, SpawnPool<Building>>();

    //////////////////////////////////////////////////

    private void Start()
    {
        var allBuildings = GetComponentsInChildren<Building>();

        foreach(var building in allBuildings)
        {
            Assert.True(m_spawnPools.ContainsKey(building.GetType()) == false,
                        "building " + building.gameObject + " found twice in prototype list");
            m_spawnPools[building.GetType()] = new SpawnPool<Building>(building.gameObject);
        }
    }

}