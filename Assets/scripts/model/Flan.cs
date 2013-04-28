
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
        
        if (m_resourceHeld == null)
        {
            m_resourceHeld = AttemptPickupResource(newCol, flanLaneIndex);
        }
    }   

    public bool IsGoingRight { get { return m_direction > 0; } }

    //////////////////////////////////////////////////

    private int m_direction = 1;
    private Type m_resourceHeld = null;

    //////////////////////////////////////////////////
    
    private void OnSpawn()
    {
        m_direction = 1;
    }

    private Type AttemptPickupResource(int columnIndex, int flanLaneIndex)
    {
        var cell = Grid.Instance[columnIndex, flanLaneIndex];

        if (cell.Building == null)
        {
            return null;
        }

        var resource = cell.Building as Resource;

        if (resource == null || resource.Count == 0)
        {
            return null;
        }

        resource.OnPickup();
        return resource.GetType();

    }
}