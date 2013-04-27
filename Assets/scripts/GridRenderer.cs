
using UnityEngine;
using System;

public class GridRenderer : MonoBehaviour
{
    //////////////////////////////////////////////////

    [RangeAttribute(0.0f, 1.0f)]
    [SerializeField] private float m_gridToScreenWidth = 0.9f;
    [RangeAttribute(0.0f, 1.0f)]
    [SerializeField] private float m_gridToScreenHeight = 0.9f;

    //////////////////////////////////////////////////

    private void Update()
    {
        
    }
}