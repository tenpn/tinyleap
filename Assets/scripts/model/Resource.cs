
using System;
using UnityEngine;

public abstract class Resource : Building
{
    public string ResourceType { get { return GetType().ToString(); } }

    public int Count { get { return m_count; } }

    //////////////////////////////////////////////////

    private int m_count;
    [SerializeField] private int m_capacity;

    //////////////////////////////////////////////////

    private void OnSpawn()
    {
        m_count = m_capacity;
    }
}