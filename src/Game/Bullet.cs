using System;
using System.Collections.Generic;
using System.Text;


class Bullet : Entity
{
    private float velocityX;
    private float velocityY;
    private float startX;
    private float startY;
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 bulletBounds = new Bounds2(32 * 2, 32, 32, 32);

    //allows you to both get and set the colors, velocity, and position. 
    public Bullet(float x, float y, float length, float width, float velocityX, float velocityY, Color color, Boolean released) : base(x, y, length, width, color)
    {
        enemy = false;
        this.released = released;
        this.velocityX = velocityX;
        this.velocityY = velocityY;
        startX = x;
        startY = y;
    }

    public Bullet(float x, float y, float length, float width, float velocityX, float velocityY, Color color, Boolean released, Boolean enemy) : base(x, y, length, width, color)
    {
        this.enemy = enemy;
        this.released = released;
        this.velocityX = velocityX;
        this.velocityY = velocityY;
        startX = x;
        startY = y;
    }

    public Boolean enemy;
    public Boolean released;
    public void update(Camera c, List<ImpenetrablePlatform> impenetrablePlatforms)
    {
        hitbox = new Bounds2(position, size);
        position.X += velocityX;
        position.Y += velocityY;

        if (released && c.inCamera(position, size))
        {
            draw();
        }

        double xDistance = position.X - startX;
        double yDistance = position.Y - startY;
        double magnitude = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);

        if (!Game.cam.inCamera(position,size) && !enemy)
        {
            delete();
        } else
        {
            foreach (ImpenetrablePlatform impenetrablePlatform in impenetrablePlatforms)
            {
                if (hitbox.Overlaps(impenetrablePlatform.hitbox) && !enemy)
                {
                    delete();
                    break;
                }
            }
        }
    }

    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: bulletBounds, size: new Vector2(size.X, size.Y));
    }
    public void delete()
    {
        released = false;
        velocity.X = 0;
        velocity.Y = 0;
    }
}