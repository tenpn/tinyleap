
using UnityEngine;
using System;
using System.Collections;

public class GridTicker : MonoBehaviour
{
    

    //////////////////////////////////////////////////

    [SerializeField] float m_tickInterval = 2.0f;

    //////////////////////////////////////////////////

    private void Start()
    {
        StartCoroutine(TickGrid());
    }

    private IEnumerator TickGrid()
    {
        float timeSinceLastTick = 0.0f;
        
        while(true)
        {
            timeSinceLastTick += Time.deltaTime;
            while(timeSinceLastTick > m_tickInterval)
            {
                timeSinceLastTick -= m_tickInterval;
                Grid.Instance.Tick();
            }

            yield return null;
        }
    }
}