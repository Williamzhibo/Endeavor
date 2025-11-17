using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


static class Grid
{
    public static Vector2 resolution = new Vector2(1370, 700);

    private static float gridFactor = 32;
    private static float xGridSize = resolution.X / gridFactor;
    private static float yGridSize = resolution.Y / gridFactor;

    public static void drawGrid()
    {
        for (float i = 0; i < resolution.X; i += xGridSize)
        {
            Engine.DrawLine(new Vector2(i, 0), new Vector2(i, resolution.Y), Color.Black);
        }
        for (float i = 0; i < resolution.Y; i += yGridSize)
        {
            Engine.DrawLine(new Vector2(0, i), new Vector2(resolution.X, i), Color.Black);
        }
    }

    public static Vector2 cords(Vector2 gridCords)
    {
        return new Vector2(xGridSize * gridCords.X, yGridSize * gridCords.Y);
    }

    public static float toCartesian(float cord)
    {
        return cord * gridFactor;
    }
}

