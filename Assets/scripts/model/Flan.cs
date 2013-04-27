
using UnityEngine;
using System;

public class Flan : Building
{
    public override void Tick(int currentColIndex, int flanLaneIndex)
    {
        int newCol = currentColIndex + m_direction;
        if (newCol >= Grid.Instance.ColumnCount)
        {
            m_direction = -1;
            newCol -= 2;
        }
        else if (newCol < 0)
        {
            m_direction = 1;
            newCol = 1;
        }

        Grid.Instance.MoveFlan(flanLaneIndex, currentColIndex, newCol);
    }   

    public bool IsGoingRight { get { return m_direction > 0; } }

    //////////////////////////////////////////////////

    private int m_direction = 1;

    //////////////////////////////////////////////////
    
    private void OnSpawn()
    {
        m_direction = 1;
    }
}