
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class SpawnPool<T> where T : MonoBehaviour
{
    public SpawnPool(GameObject prototype)
    {
        Assert.IsNotNull(prototype,
                         "no prototype for spawn pool");
        m_prototype = prototype;
    }

    public T Spawn()
    {
        T newT = null;
        
        if (m_pool.Any())
        {
            newT = m_pool.Dequeue();
            newT.gameObject.SetActive(true);
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

        return newT;
    }

    public void Despawn(T instance)
    {
        Assert.True(m_instances.Contains(instance),
                    "instance wasn't spawned by us");
        instance.gameObject.SetActive(false);
        m_pool.Enqueue(instance);
    }

    //////////////////////////////////////////////////

    private HashSet<T> m_instances = new HashSet<T>();
    private Queue<T> m_pool = new Queue<T>();
    private GameObject m_prototype;

    //////////////////////////////////////////////////
}