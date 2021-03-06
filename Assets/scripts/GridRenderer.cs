
using UnityEngine;
using System;

public class GridRenderer : MonoBehaviour
{
    //////////////////////////////////////////////////

    [RangeAttribute(0.01f, 1.0f)]
    [SerializeField] private float m_gridToScreenWidth = 0.9f;
    [RangeAttribute(0.01f, 1.0f)]
    [SerializeField] private float m_gridToScreenHeight = 0.9f;
    [RangeAttribute(0.01f, 5.0f)]
    [SerializeField] private float m_buildingScaleRatio = 0.8f;

    private PathFactory m_pathFactory = null;
    private HatFactory m_hatFactory = null;

    //////////////////////////////////////////////////

    private void Awake()
    {
        m_hatFactory = GetComponentInChildren<HatFactory>();
        Assert.IsNotNull(m_hatFactory, "no hat factory found");

        m_pathFactory = GetComponentInChildren<PathFactory>();
        Assert.IsNotNull(m_pathFactory, "no path factory found");
    }

    private void Update()
    {
        var mainCam = Camera.main;
        float screenWidth = mainCam.pixelWidth;
        float screenHeight = mainCam.pixelHeight;

        var grid = Grid.Instance;

        float gridScreenWidth = screenWidth * m_gridToScreenWidth;
        float gridScreenHeight = screenHeight * m_gridToScreenHeight;

        Assert.True(gridScreenWidth > 0 && gridScreenWidth <= screenWidth,
                    "grid width " + gridScreenWidth 
                    + " can't fit on screen. grid to screen width should be 0 > " 
                    + m_gridToScreenWidth + " <= 1.0");
        Assert.True(gridScreenHeight > 0 && gridScreenHeight <= screenHeight,
                    "grid height " + gridScreenHeight 
                    + " can't fit on screen. grid to screen height should be 0 > " 
                    + m_gridToScreenWidth + " <= 1.0");

        float leftScreenColumn = 0.5f * (screenWidth - gridScreenWidth);
        float rightScreenColumn = leftScreenColumn + gridScreenWidth;
        float columnPixelSeperation = gridScreenWidth / (float)grid.ColumnCount;

        float topScreenRow = 0.5f * (screenHeight - gridScreenHeight) + gridScreenHeight;
        float lanePixelSeperation = gridScreenHeight 
            / (2.0f + 2.0f*(float)grid.FlanLaneCount);

        float renderDepth = mainCam.nearClipPlane + 1f;
        Func<float, float, Vector3> screenToWorld = (screenX, screenY) 
            => mainCam.ScreenToWorldPoint(
                new Vector3(screenX, screenY, renderDepth));

        m_hatFactory.Reset();
        m_pathFactory.Reset();

        for(int flanLaneIndex = 0; flanLaneIndex < grid.FlanLaneCount; ++flanLaneIndex)
        {
            // build lane -> flan lane. ends with one last resource lane.
            // draw flan lane. 0 is at the top of the screen.

            float buildLanePixelRow = topScreenRow
                - lanePixelSeperation * (1.0f + 2.0f * (float)flanLaneIndex);

            var buildLaneWorldStart = screenToWorld(leftScreenColumn, buildLanePixelRow);
            var buildLaneWorldEnd = screenToWorld(rightScreenColumn, buildLanePixelRow);

            Debug.DrawLine(buildLaneWorldStart, buildLaneWorldEnd, Color.blue);

            float flanLanePixelRow = buildLanePixelRow - lanePixelSeperation;

            var flanLaneWorldStart = screenToWorld(leftScreenColumn, flanLanePixelRow);
            var flanLaneWorldEnd = screenToWorld(rightScreenColumn, flanLanePixelRow);

            Debug.DrawLine(flanLaneWorldStart, flanLaneWorldEnd, Color.green);

            // draw buildings, etc
            for(int colIndex = 0; colIndex < grid.ColumnCount; ++colIndex)
            {
                var cell = grid[colIndex, flanLaneIndex];
                
                var cellPixelColumn = leftScreenColumn 
                    + columnPixelSeperation * ((float)colIndex + 0.5f);

                var flanLane = m_pathFactory.CreateFlanPathTile();
                flanLane.transform.position 
                    = screenToWorld(cellPixelColumn, flanLanePixelRow);
                flanLane.transform.localScale 
                    = new Vector3(m_buildingScaleRatio, m_buildingScaleRatio, 1.0f);

                if (cell.Building != null)
                {
                    var buildingWorldPos 
                        = screenToWorld(cellPixelColumn, buildLanePixelRow);

                    float resourceScalar = cell.Building is Resource 
                        ? Mathf.Lerp(0.35f, 1.0f, 
                                     (cell.Building as Resource).PercentageRemaining)
                        : 1.0f;

                    float buildingScalar = m_buildingScaleRatio * resourceScalar;

                    cell.Building.transform.position = buildingWorldPos;
                    cell.Building.transform.localScale 
                        = new Vector3(buildingScalar, buildingScalar, 1.0f);
                }
                
                if (cell.Flan != null)
                {
                    var flanWorldPos
                        = screenToWorld(cellPixelColumn, flanLanePixelRow);

                    cell.Flan.transform.position = flanWorldPos;
                    cell.Flan.transform.localScale 
                        = new Vector3(m_buildingScaleRatio, m_buildingScaleRatio, 1.0f);

                    float facingDir = cell.Flan.IsGoingRight ? 0.0f : 180.0f;
                    cell.Flan.transform.rotation 
                        = Quaternion.Euler(0.0f, facingDir, 0.0f);

                    if (cell.Flan.IsHoldingResource)
                    {
                        var hat = m_hatFactory.GetHat(cell.Flan.ResourceHeld);

                        if (hat != null)
                        {
                            hat.position = cell.Flan.transform.position;
                            hat.localScale = cell.Flan.transform.localScale;
                            hat.rotation = cell.Flan.transform.rotation;
                        }
                    }
                }
            }
        }
    }
}