
using UnityEngine;
using System;

public class Grid : MonoBehaviour
{    
    public struct Cell
    {
        public Building Building;
    }
    
    // read-only grid accessor
    public Cell this[int column, int flanLane]
    {
        get
        {
            return m_grid[column, flanLane];
        }
    }


    public int ColumnCount { get { return m_columnCount; } }
    public int FlanLaneCount { get { return m_flanLaneCount; } }

    public void Tick()
    {
        
    }

    public static Grid Instance
    {
        get
        {
            Assert.IsNotNull(s_singleton,
                             "no singleton of type " + typeof(Grid) + " yet in scene");
            return s_singleton;
        }
    }

    //////////////////////////////////////////////////

    private static Grid s_singleton;

    [RangeAttribute(1, 50)]
    [SerializeField] private int m_columnCount;
    [RangeAttribute(1, 10)]
    [SerializeField] private int m_flanLaneCount; 

    private BuildingFactory m_buildingFactory = null;
    
    private Cell[,] m_grid = null;

    //////////////////////////////////////////////////

    private void Awake() 
    {
        Assert.IsNull(s_singleton,
                      "already singleton of type " + typeof(Grid) + ", is obj " 
                      + (s_singleton == null ? "NULL" : s_singleton.gameObject.name));
        s_singleton = this as Grid;

        m_buildingFactory = FindObjectOfType(typeof(BuildingFactory)) as BuildingFactory;
        Assert.IsNotNull(m_buildingFactory, "could not find building factory");
    }

    private void Start()
    {
        CreateGrid();
    }

    private void OnDestroy()
    {
        Assert.True(s_singleton == this,
                    "expected singleton of type " + typeof(Grid) + " to be us (" 
                    + gameObject + "), but instead it's " + s_singleton.gameObject);
        s_singleton = null;
    }

    private void CreateGrid()
    {
        if (m_grid != null)
        {
            for(int flanLaneIndex = 0; flanLaneIndex < m_grid.GetLength(1); ++flanLaneIndex)
            {
                for(int colIndex = 0; colIndex < m_grid.GetLength(0); ++colIndex)
                {
                    var cell = m_grid[colIndex, flanLaneIndex];
                    if (cell.Building != null)
                    {
                        m_buildingFactory.Destroy(cell.Building);
                        cell.Building = null;
                    }
                }
            }
        }

        m_grid = new Cell[m_columnCount, m_flanLaneCount];

        for(int flanLaneIndex = 0; flanLaneIndex < m_flanLaneCount; ++flanLaneIndex)
        {
            m_grid[0,flanLaneIndex].Building = MakeNewBuilding<FlanHouse>();
        }
    }

    private void Update()
    {
        if (m_grid.GetLength(0) != m_columnCount
            || m_grid.GetLength(1) != m_flanLaneCount)
        {
            CreateGrid();
        }
    }

    private T MakeNewBuilding<T>() where T : Building
    {
        var newBuilding = m_buildingFactory.Create<T>();
        newBuilding.transform.parent = transform;
        return newBuilding as T;
    }

}