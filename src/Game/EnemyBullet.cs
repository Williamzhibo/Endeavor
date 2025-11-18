using System;
using System.Collections.Generic;
using System.Text;

class EnemyBullet : Bullet
{
    public EnemyBullet(float x, float y, float length, float width, float velocityX, float velocityY, Color color, Boolean released) : base(x, y, length, width, velocityX, velocityY, color, released)
    {
    }
   

    public void update(Camera c, double xDistance, double yDistance)
    {

        hitbox = new Bounds2(position, size);
        position += velocity;

        if (released && c.inCamera(position, size))
        {
            draw(c);
        }


        double magnitude = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);

        if (magnitude > 300)
        {
            delete();
        }
    }

    public void draw(Camera c)
    {
        Engine.DrawRectSolid(new Bounds2(new Vector2(position.X - c.position.X, position.Y -  c.position.Y), size), color);
    }
}
