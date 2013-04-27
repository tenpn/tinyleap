
using UnityEngine;
using System;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance
    {
        get
        {
            Assert.IsNotNull(s_singleton,
                             "no singleton of type " + typeof(T) + " yet in scene");
            return s_singleton;
        }
    }

    protected virtual void Awake() 
    {
        Assert.IsNull(s_singleton,
                      "already singleton of type " + typeof(T) + ", is obj " 
                      + s_singleton.gameObject);
        s_singleton = this as T;
    }

    protected virtual void OnDestroy()
    {
        Assert.True(s_singleton == this,
                    "expected singleton of type " + typeof(T) + " to be us (" 
                    + gameObject + "), but instead it's " + s_singleton.gameObject);
        s_singleton = null;
    }


    //////////////////////////////////////////////////

    private static T s_singleton;

    //////////////////////////////////////////////////

}