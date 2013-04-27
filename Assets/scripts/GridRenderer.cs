
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

        float bottomScreenRow = 0.5f * (screenHeight - gridScreenHeight);
        float flanLanePixelSeperation = gridScreenHeight / (1.0f + (float)grid.FlanLaneCount);

        Func<float, float, Vector3> screenToWorld = (screenX, screenY) 
            => mainCam.ScreenToWorldPoint(
                new Vector3(screenX, screenY, mainCam.nearClipPlane));

        for(int flanLaneIndex = 0; flanLaneIndex < grid.FlanLaneCount; ++flanLaneIndex)
        {
            // draw flan lane
            float flanLaneRow = bottomScreenRow 
                + flanLanePixelSeperation * (float)flanLaneIndex;
            var flanLaneWorldStart = screenToWorld(leftScreenColumn, flanLaneRow);
            var flanLaneWorldEnd = screenToWorld(rightScreenColumn, flanLaneRow);

            Debug.DrawLine(flanLaneWorldStart, flanLaneWorldEnd, Color.green);

            // draw buildings
            for(int colIndex = 0; colIndex < grid.ColumnCount; ++colIndex)
            {
                
            }
        }
    }
}