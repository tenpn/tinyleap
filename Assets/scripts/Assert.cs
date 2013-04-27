
using UnityEngine;
using System;

public class Assert
{
    private const string s_assertGuard = "UNITY_EDITOR";

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, string message)
    {
        if(test == false)
        {
            if (Application.isEditor && Application.isPlaying == false)
            {
                // normal asserts don't work in the editor
                throw new UnityException(message);
            }
            else
            {
                Debug.LogError(message);
                Debug.Break();
            }
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsNotNull(System.Object obj, string msg)
    {
        Assert.True(obj != null, msg);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsNull(System.Object obj, string msg)
    {
        Assert.True(obj == null, msg);
    }


    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(float f, string msg)
    {
#pragma warning disable 1718
        Assert.True(f == f, msg);
#pragma warning restore 1718
    }

    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Vector3 v, string msg)
    {
        Assert.IsValid(v.x, msg);
        Assert.IsValid(v.y, msg);
        Assert.IsValid(v.z, msg);
    }

    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Quaternion q, string msg)
    {
        Assert.IsValid(q.x, msg);
        Assert.IsValid(q.y, msg);
        Assert.IsValid(q.z, msg);
        Assert.IsValid(q.w, msg);
    }

    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Transform t, string msg)
    {
        Assert.IsValid(t.position, msg);
        Assert.IsValid(t.rotation, msg);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void Fail(string message)
    {
        True(false, message);
    }
}





