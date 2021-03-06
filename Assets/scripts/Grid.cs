
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Grid : MonoBehaviour
{    
    public struct Cell
    {
        public Building Building;
        public Flan Flan;
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
        m_nextGrid = new Cell[ColumnCount, FlanLaneCount];

        for(int flanLaneIndex = 0; flanLaneIndex < m_grid.GetLength(1); ++flanLaneIndex)
        {
            for(int colIndex = 0; colIndex < m_grid.GetLength(0); ++colIndex)
            {
                m_nextGrid[colIndex, flanLaneIndex] = m_grid[colIndex, flanLaneIndex];
            }
        }


        for(int flanLaneIndex = 0; flanLaneIndex < m_grid.GetLength(1); ++flanLaneIndex)
        {
            for(int colIndex = 0; colIndex < m_grid.GetLength(0); ++colIndex)
            {
                TickCell(colIndex, flanLaneIndex);               
            }
        }

        m_grid = m_nextGrid;
        m_nextGrid = null;
    }

    public Flan CreateFlan(int colIndex, int flanLaneIndex)
    {
        var newFlan = MakeNewBuilding<Flan>();
        Assert.IsNotNull(newFlan, "always expected a flan");
        m_nextGrid[colIndex, flanLaneIndex].Flan = newFlan;
        return newFlan;
    }

    public void MoveFlan(int flanLaneIndex, int prevColIndex, int newColIndex)
    {
        var flan = m_nextGrid[prevColIndex, flanLaneIndex].Flan;
        Assert.IsNotNull(flan, "no flan found at " + prevColIndex + "," + flanLaneIndex);
        m_nextGrid[prevColIndex, flanLaneIndex].Flan = null;
        m_nextGrid[newColIndex, flanLaneIndex].Flan = flan;
    }

    public IEnumerable<Type> ResourceTypes
    {
        get
        {
            return m_buildingFactory.AllTypes
                .Where(bType => typeof(Resource).IsAssignableFrom(bType));
        }
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

    [RangeAttribute(0f, 1f)]
    [SerializeField] private float m_resourceChance = 0.4f;


    private BuildingFactory m_buildingFactory = null;
    
    private Cell[,] m_grid = null;
    private Cell[,] m_nextGrid = null;

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
                    m_buildingFactory.Destroy(cell.Building);
                    m_buildingFactory.Destroy(cell.Flan);
                }
            }
        }

        m_grid = new Cell[m_columnCount, m_flanLaneCount];

        var allResources = ResourceTypes;

        var resourcesRng = new System.Random();
        Func<Building> newRandomResource = () 
            => MakeNewBuilding(allResources.ElementAt(
                                   resourcesRng.Next(allResources.Count())));

        for(int flanLaneIndex = 0; flanLaneIndex < m_flanLaneCount; ++flanLaneIndex)
        {
            m_grid[0,flanLaneIndex].Building = MakeNewBuilding<FlanHouse>();

            for(int columnIndex = 1; columnIndex < m_columnCount; ++columnIndex)
            {
                if ((float)resourcesRng.NextDouble() >= m_resourceChance)
                {
                    continue;
                }

                m_grid[columnIndex, flanLaneIndex].Building = newRandomResource();
            }
        }
    }

    private void TickCell(int colIndex, int flanLaneIndex)
    {
        var cell = m_grid[colIndex, flanLaneIndex];

        if (cell.Building != null)
        {
            cell.Building.Tick(colIndex, flanLaneIndex);
        }       

        if (cell.Flan != null)
        {
            cell.Flan.Tick(colIndex, flanLaneIndex);
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
        return MakeNewBuilding(typeof(T)) as T;
    }

    private Building MakeNewBuilding(Type newBuildingType)
    {
        var newBuilding = m_buildingFactory.Create(newBuildingType);
        newBuilding.transform.parent = transform;
        return newBuilding;
    }

}