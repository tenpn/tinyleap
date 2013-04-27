
using System;
using UnityEngine;

public class FlanHouse : Building
{
    public override void Tick(int colIndex, int flanLaneIndex)
    {
        if (m_myFlan == null)
        {
            Debug.Log("making flan!");
            m_myFlan = Grid.Instance.CreateFlan(colIndex, flanLaneIndex);
        }
    }

    //////////////////////////////////////////////////
    
    private Flan m_myFlan = null;

    //////////////////////////////////////////////////

    private void OnDespawn()
    {
        m_myFlan = null;
    }
}