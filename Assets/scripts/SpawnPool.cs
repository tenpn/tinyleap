
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class SpawnPool<T> where T : Component
{
    public SpawnPool(GameObject prototype, Transform poolHolder)
    {
        Assert.IsNotNull(prototype,
                         "no prototype for spawn pool");
        m_prototype = prototype;
        m_poolHolder = poolHolder;
    }

    public T Spawn()
    {
        T newT = null;
        
        if (m_pool.Any())
        {
            newT = m_pool.Dequeue();
            newT.gameObject.SetActive(true);
            newT.transform.parent = null;
        }
        else
        {
            var newObj = GameObject.Instantiate(m_prototype) as GameObject;
            newT = newObj.GetComponentInChildren<T>();
            Assert.True(newT != null,
                        "no component of type " + typeof(T) + " in prototype " + m_prototype);
        }

        Assert.True(newT != null,
                    "could not spawn new " + typeof(T));

        Assert.True(m_instances.Contains(newT) == false,
                    "newly-spawned " + typeof(T) 
                    + " should not already be in instance list");
        m_instances.Add(newT);
        newT.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

        return newT;
    }

    public void Despawn(T instance)
    {
        Assert.True(m_instances.Contains(instance),
                    "instance wasn't spawned by us");
        instance.SendMessage("OnDespawn", SendMessageOptions.DontRequireReceiver);
        instance.gameObject.SetActive(false);
        m_instances.Remove(instance);
        m_pool.Enqueue(instance);
        instance.transform.parent = m_poolHolder;
    }

    public void DespawnAll()
    {
        // because we can't shrink the m_instances list while we iterate it...
        var allInstances = new List<T>(m_instances);

        foreach(var instance in allInstances)
        {
            Despawn(instance);
        }
    }

    //////////////////////////////////////////////////

    private HashSet<T> m_instances = new HashSet<T>();
    private Queue<T> m_pool = new Queue<T>();
    private GameObject m_prototype;
    private Transform m_poolHolder;

    //////////////////////////////////////////////////
}