
using UnityEngine;
using System;

public class PathFactory : MonoBehaviour
{
    public Transform CreateFlanPathTile()
    {
        var newTile = m_flanPathTilePool.Spawn();
        newTile.parent = m_activeTileStore;
        return newTile;
    }

    public void Reset()
    {
        m_flanPathTilePool.DespawnAll();
    }

    //////////////////////////////////////////////////

    [SerializeField] private GameObject m_flanPathTileTemplate = null;
    private SpawnPool<Transform> m_flanPathTilePool;
    
    private Transform m_activeTileStore;

    //////////////////////////////////////////////////

    private void Awake()
    {
        var pathTilePoolStore = new GameObject("path tile store").transform;
        pathTilePoolStore.parent = transform;

        m_activeTileStore = new GameObject("active tile store").transform;
        m_activeTileStore.parent = transform;

        Assert.IsNotNull(m_flanPathTileTemplate, "no flan path tile template");
        m_flanPathTilePool = new SpawnPool<Transform>(
            m_flanPathTileTemplate, pathTilePoolStore);
    }
}