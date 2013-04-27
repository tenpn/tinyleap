
using UnityEngine;
using System;

public class GridRenderer : MonoBehaviour
{
    //////////////////////////////////////////////////

    [RangeAttribute(0.01f, 1.0f)]
    [SerializeField] private float m_gridToScreenWidth = 0.9f;
    [RangeAttribute(0.01f, 1.0f)]
    [SerializeField] private float m_gridToScreenHeight = 0.9f;

    //////////////////////////////////////////////////

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

        float topScreenRow = 0.5f * (screenHeight - gridScreenHeight) + gridScreenHeight;
        float lanePixelSeperation = gridScreenHeight 
            / (2.0f + 2.0f*(float)grid.FlanLaneCount);

        Func<float, float, Vector3> screenToWorld = (screenX, screenY) 
            => mainCam.ScreenToWorldPoint(
                new Vector3(screenX, screenY, mainCam.nearClipPlane));

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

            // draw buildings
            for(int colIndex = 0; colIndex < grid.ColumnCount; ++colIndex)
            {
                
            }
        }
    }
}