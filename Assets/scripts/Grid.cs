
using UnityEngine;
using System;

public class Grid : MonoSingleton<Grid>
{    

    //////////////////////////////////////////////////

    [RangeAttribute(1, 50)]
    [SerializeField] private int m_columns;
    [RangeAttribute(1, 10)]
    [SerializeField] private int m_flanLanes; 

    //////////////////////////////////////////////////

}