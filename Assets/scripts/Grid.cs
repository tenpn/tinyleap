
using UnityEngine;
using System;

public class Grid : MonoSingleton<Grid>
{    

    public int ColumnCount { get { return m_columnCount; } }
    public int FlanLaneCount { get { return m_flanLaneCount; } }

    //////////////////////////////////////////////////

    [RangeAttribute(1, 50)]
    [SerializeField] private int m_columnCount;
    [RangeAttribute(1, 10)]
    [SerializeField] private int m_flanLaneCount; 

    //////////////////////////////////////////////////

}