using System;
using System.Collections.Generic;
using System.Text;

class Camera
{
    public Vector2 position;
    public Vector2 size;
    public Camera(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
    }

    public void update(Vector2 newPosition)
    {
        position = newPosition;
    }

    public Boolean inCamera(Vector2 EntityPosition, Vector2 EntitySize)
    {
        if (EntityPosition.X + EntitySize.X >= position.X && EntityPosition.X <= position.X + size.X)
        {
            if (EntityPosition.Y + EntitySize.Y >= position.Y && EntityPosition.Y <= position.Y + size.Y)
            {
                return true;
            }
        }
        
        return false;
    }

    public Vector2 getPosition()
    {
        return position;
    }

    public Vector2 getSize()
    {
        return size;
    }
}
