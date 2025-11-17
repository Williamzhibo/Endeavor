using System;
using System.Collections;
using System.Collections.Generic;

class Entity
{
    public Bounds2 hitbox;
    //allows you to both get and set the colors, velocity, and position. 
    public Entity(float x, float y, float length, float width, Color color)
    {
        position = new Vector2(x, y);
        size = new Vector2(length, width);
        this.color = color;
    } 

    public float friction = (float) .5;
    public Color color;
    public Vector2 position;
    public Vector2 velocity = new Vector2(0,0);
    public Vector2 previousv;
    public Vector2 size;
    public Boolean dead = false;


    public void update()
    {
        hitbox = new Bounds2(position, size);
        position += velocity;
        
        if (Game.cam.inCamera(position,size) && !dead)
        {
            draw();
        }

    }

    public virtual void draw()
    {
        //Engine.DrawRectSolid(new Bounds2(new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), size), color);
    }
}
class Wall : Entity
{
    // FIX THIS, TEMPORARY PLACEMENT
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");
    //Bounds2 floorBounds = new Bounds2(32 * 6, 0, 32, 32);
    float x;
    float y;

    public Wall(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        this.x = x;
        this.y = y;
    }

    public override void draw()
    {
        Engine.DrawRectSolid(new Bounds2(new Vector2(0, 0), new Vector2(32, 32)), Color.Yellow);
    }
}

class Floor : Entity
{
    // FIX THIS, TEMPORARY PLACEMENT
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");
    Bounds2 floorBounds = new Bounds2(32 * 4, 0, 32, 32);
    
    float x;
    float y;
    public Floor(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        this.x = x;
        this.y = y;
    }

    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: floorBounds, size: new Vector2(size.X, size.Y));
    }
}

class Platform : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 platformBounds = new Bounds2(32 * 6, 0, 32, 32);
    TextureMirror mirrorPlatform = TextureMirror.None;


    public float frictionCoefficient = 1; 
    public Platform(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }
    public new void update()
    {
        hitbox = new Bounds2(position, size);
        position += velocity;

        if (Game.cam.inCamera(position, size))
        {
            draw();
        }

    }

    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: platformBounds, size: new Vector2(size.X, size.Y), mirror: mirrorPlatform);
    }
}

class FrictionPlatform : Platform
{
    public FrictionPlatform(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }
}

class MovingPlatform : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 movPlatBounds = new Bounds2(32 * 8, 0, 32, 32);
    TextureMirror mirrorMovPlat = TextureMirror.None;


    private Vector2 initialPos;
    public MovingPlatform(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        friction = (float) 0.5;
        velocity.X = 2;
        initialPos = new Vector2(x, y);
    }

    public void update()
    {
        if (position.X >= initialPos.X + 60 && velocity.X > 0)
        {
            velocity.X = -(float)0.5;
        }
        if (position.X <= initialPos.X - 60 && velocity.X < 0)
        {
            velocity.X = (float)0.5;
        }
       
        hitbox = new Bounds2(position, size);
        position += velocity;

        if (Game.cam.inCamera(position, size))
        {
            draw();
        }

    }

    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y) , source: movPlatBounds, size : new Vector2(size.X, size.Y), mirror : mirrorMovPlat);
    }
          
}

class Ladder : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 ladderBounds = new Bounds2(32 * 7, 0, 32, 32);

    TextureMirror mirrorLadder = TextureMirror.None;
    Boolean colliding;
    public Ladder(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }
    public Boolean getIfColliding()
    {
        return colliding;
    }
    public void setColliding(Boolean b)
    {
        colliding = b;
    }
    public void update()
    {
        hitbox = new Bounds2(position, size);
        position += velocity;

        if (Game.cam.inCamera(position, size))
        {
            draw();
        }

    }

    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: ladderBounds, size: new Vector2(size.X, size.Y), mirror: mirrorLadder);
    }
}
class Portal : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 portalBounds = new Bounds2(32 * 5, 0, 32, 32);
    TextureMirror mirrorPortal = TextureMirror.None;

    public Portal to;

    public Portal(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        to = null;
    }
    public Portal getTo()
    {
        return to;
    }
    public void setTo(Portal p2)
    {
        to = p2;
    }
    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: portalBounds, size: new Vector2(size.X, size.Y), mirror: mirrorPortal);
    }
}

class PortalPair : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 portalBounds = new Bounds2(32 * 5, 0, 32, 32);
    TextureMirror mirrorPortal = TextureMirror.None;
    public PortalPair(float x, float y, float length, float width, Color color, Portal p1, Portal p2) : base(x, y, length, width, color)
    {
        p1.setTo(p2);
        p2.setTo(p1);
    }
    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: portalBounds, size: new Vector2(size.X, size.Y), mirror: mirrorPortal);
    }

}

class WinLoseBlock : Entity
{
    public WinLoseBlock(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }
}

class WinningBlock : WinLoseBlock
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 winningBlockBounds = new Bounds2(32 * 2, 0, 32, 32);
    TextureMirror mirrorWinningBlock = TextureMirror.None;

    public WinningBlock(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }

    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: winningBlockBounds, size: new Vector2(size.X, size.Y), mirror: mirrorWinningBlock);
    }

}

class LosingBlock : WinLoseBlock
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 losingBounds = new Bounds2(32 * 3, 32, 32, 32);

    public LosingBlock(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }

    public void update()
    {
        if (Game.cam.inCamera(position, size))
        {
            draw();
        }
    }

    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: losingBounds, size: new Vector2(size.X, size.Y));
    }

}
class Laser : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");
    public int stateCount = 0;
    public Boolean laserOn = true;
    Bounds2 laserBounds = new Bounds2(32 * 1, 0, 32, 32);
    Bounds2 laserOffBounds = new Bounds2(32 * 5, 32, 32, 32);
    TextureMirror mirrorLaser = TextureMirror.None;
    public Laser(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }

    public void update()
    {
        hitbox = new Bounds2(position, size);
        position += velocity;

        stateCount++;
        if (stateCount > 130)
        {
            stateCount = 0;
            if (laserOn)
            {
                laserOn = false;
            }
            else
            {
                laserOn = true;
            }
        }

        if (Game.cam.inCamera(position, size))
        {
            draw();
        }
    }

    public override void draw()
    {
        if (laserOn)
        {
            Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: laserBounds, size: new Vector2(size.X, size.Y), mirror: mirrorLaser);
        }
        else
        {
            Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: laserOffBounds, size: new Vector2(size.X, size.Y), mirror: mirrorLaser);
        }
    }

}

class ImpenetrablePlatform : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");
    Bounds2 impenetrableBounds = new Bounds2(32 * 0, 32, 32, 32);
    //impenetrable properties set in bullet class
    public ImpenetrablePlatform(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }

    public void update()
    {
        if (Game.cam.inCamera(position, size))
        {
            draw();
        }
    }

    public void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: impenetrableBounds, size: new Vector2(size.X, size.Y));
    }
}

class Booster : Entity
{
    public Boolean consumed = false;
    public Boolean armor = false;
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");
    Bounds2 boosterBounds = new Bounds2(32 * 1, 32, 32, 32);
    Bounds2 armorBounds = new Bounds2(32 * 4, 32, 32, 32);
    public Booster(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
    }

    public void update()
    {
        if (Game.cam.inCamera(position, size))
        {
            draw();
        }
    }

    public void draw()
    {
        if (armor)
        {
            Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: armorBounds, size: new Vector2(size.X, size.Y));
        } else
        {
            Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: boosterBounds, size: new Vector2(size.X, size.Y));
        }
    }
}

class Armor : Booster
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");
    public Armor(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        armor = true;
    }

    public void update()
    {
        if (Game.cam.inCamera(position, size))
        {
            draw();
        }
    }
}