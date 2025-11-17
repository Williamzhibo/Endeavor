using System;
using System.Collections.Generic;
using System.Numerics;

class Player : Entity
{
    Texture playerTexture = Engine.LoadTexture("player.png");
    public Sound jumpSound = Engine.LoadSound("jump.wav");
    Sound playerShootSound = Engine.LoadSound("playerShoot.wav");
    Sound playerDeathSound = Engine.LoadSound("playerDeath.wav");
    Sound playerHurtSound = Engine.LoadSound("playerHurtSound.wav");
    
    public float playerFrameIndex = 0;
    public int playerHurtIndex;

    Boolean canJump = true;
    Boolean onLadder = false;
    public Boolean hurt = false;
    public Bounds2 hitboxY;
    public Bounds2 hitboxYTop;
    public Bounds2 hitboxLeftX;
    public Bounds2 hitboxRightX;
    public Boolean collidesLose = false;
    public Boolean win;
    public Boolean lose;
    public Boolean floating;
    public float health = 100;
    public Boolean armor = false;
    public float armorDurability = 0;
    public float xIncrement = 3;
    public float xAcceleration = 1;
    public Boolean collidesWithPlat = false;

    public Boolean boosted = false;
    public int boostedCount = 0;

    public int totalJumps;
    public int bulletsFired;

    public double xDistance;
    public double yDistance;
    public double magnitude;
    public ParallaxScreen parallax1;
    public ParallaxScreen parallax2;
    public ParallaxScreen parallax3;
    

    public int jumpTicks = 0;

    public Player(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        parallax1 = new ParallaxScreen(0, -2);
        parallax2 = new ParallaxScreen(1, -3);
        parallax3 = new ParallaxScreen(2, -5);
        
    }
    
    public void update()

    {
        parallax1.Update(position);
        parallax2.Update(position);
        parallax3.Update(position);
        
        /*
        // health bar
        Engine.DrawRectSolid(new Bounds2(new Vector2(1000, 10), new Vector2(health, 20)), Color.Green);
        Engine.DrawRectSolid(new Bounds2(new Vector2(1000 + health, 10), new Vector2(100 - health, 20)), Color.Red);

        // armor bar
        if (armor) {
            Engine.DrawRectSolid(new Bounds2(new Vector2(1000, 30), new Vector2(armorDurability, 20)), Color.Gray);
        }
        */


        // checks if palyer is dead
        if (health <= 0)
        {
            lose = true;
            Engine.PlaySound(playerDeathSound);
        }

        //checks if player is boosted 
        if (boosted)
        {
            boostedCount++;
            if (boostedCount > 140)
            {
                boosted = false;
                boostedCount = 0;
            }
        }

        // establishes the hitboxes in order to implement collisions
        hitboxY = new Bounds2(new Vector2(position.X + 3, position.Y + size.Y - 3), new Vector2(size.X - 7, 5));
        hitboxYTop = new Bounds2(new Vector2(position.X + 3, position.Y - 5), new Vector2(size.X - 7, 5));
        hitboxLeftX = new Bounds2(new Vector2(position.X - 2, position.Y), new Vector2(2, size.Y - 5));
        hitboxRightX = new Bounds2(new Vector2(position.X + size.X + 4.5f, position.Y), new Vector2(3, size.Y - 5));

        Font font = Engine.LoadFont("Retro Gaming.ttf", 11);


        //START OF PLAYER MOVEMENT LOGIC
        //player tries moving left
        if (Engine.GetKeyHeld(Key.Left) || Engine.GetKeyHeld(Key.A))
        {
            velocity.X = -xIncrement;
        }
        //player tries moving right
        else if (Engine.GetKeyHeld(Key.Right) || Engine.GetKeyHeld(Key.D))
        {
            velocity.X = xIncrement;
        }
        //stops the player from moving
        else
        {
            velocity.X = 0;
        }
        
        if (boosted)
        {
            if (!floating) // floating turned on with this click
            {
                velocity.Y = 0;
                canJump = false;
                floating = true;
            }
            else //floating turned off
            {
                floating = false;
                canJump = false;
            }
        }
        //assuming the player is able to jump and not floating, performs a jump
        if ((Engine.GetKeyHeld(Key.Up) || Engine.GetKeyHeld(Key.W)) && canJump && !floating) //jump
        {
            totalJumps++;
            canJump = false;
            velocity.Y = -8;
            Engine.PlaySound(jumpSound);
        }
        if (!canJump)
        {
            jumpTicks++;
        }
        if (jumpTicks == 9 && (Engine.GetKeyHeld(Key.Up) || Engine.GetKeyHeld(Key.W)))
        {
            velocity.Y -= 2;
        }

        else if (!onLadder && !floating)
        {
            velocity.Y += (float)0.25;
            xIncrement = 3;
            canJump = false;
        }

        if (Engine.GetMouseButtonDown(MouseButton.Left))
        {
            bulletsFired++;
            //find x distance between mouse and player
            xDistance = Engine.MousePosition.X - (position.X + size.X / 2 - Game.cam.position.X);
            //find y distance between mouse and player
            yDistance = Engine.MousePosition.Y - (position.Y + size.Y / 2 - Game.cam.position.Y);
            magnitude = (float)Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
            float bulletUnitVelocityX = (float)(xDistance / magnitude) * 4;
            float bulletUnitVelocityY = (float)(yDistance / magnitude) * 4;
            float bulletSpeed = 50;

            Game.bullets.Add(new Bullet(position.X, position.Y, 5, 5, bulletUnitVelocityX, bulletUnitVelocityY, Color.Brown, true));
            Engine.PlaySound(playerShootSound);
        }
        //player is on a ladder, move up
        if ((Engine.GetKeyHeld(Key.Up) || Engine.GetKeyHeld(Key.W)) && onLadder)
        {
            velocity.Y = -3;
        }
        else if ((Engine.GetKeyHeld(Key.Down) || Engine.GetKeyHeld(Key.S)) && onLadder)
        {
            velocity.Y = 3;
            
        }
        else if (onLadder)
        {
            velocity.Y = 0;
            
        }
        //END OF PLAYER MOVEMENT LOGIC

        onLadder = false;
        foreach (Entity entity in Game.stillEntities)
        {
            if (!Game.cam.inCamera(entity.position, entity.size))
            {
                continue;
            }

            if (entity.GetType() == typeof(WinningBlock)) //if winning!
            {
                updateWin((WinningBlock)entity);
            }
            else if (entity.GetType() == typeof(LosingBlock)) //if losing
            {
                updateLose((LosingBlock)entity);
            }
            else if (entity.GetType() == typeof(Laser)) //if laser
            {
                laserCollision((Laser)entity);
                continue;
            }
            else if (entity.GetType() == typeof(Portal)) //if portal
            {
                portalCollision((Portal)entity);
            }
            else if (entity.GetType() == typeof(Ladder))
            {
                ladderCollision((Ladder)entity);
            }
            else if (entity.GetType() == typeof(CollidableNPC))
            {
                collidableNPCCollision((CollidableNPC)entity);
            }
            else if (entity.GetType() == typeof(Armor))
            {
                armorCollision((Armor)entity);
            }
            else if (entity.GetType() == typeof(Booster))
            {
                boosterCollision((Booster)entity);
            }

            if ((hitboxYTop.Overlaps(entity.hitbox)) && velocity.Y < 0){
                velocity.Y = 1;
                position.Y = entity.position.Y + entity.size.Y + 10;
                xIncrement = entity.friction * 3;
            } else if (hitboxY.Overlaps(entity.hitbox) && velocity.Y > 0)
            {
                jumpTicks = 0;
                canJump = true;
                velocity.Y = 0;
                position.Y = entity.position.Y - 10;
                xIncrement = entity.friction * 3;
                xAcceleration = -velocity.X / entity.friction;
                collidesWithPlat = true;
                if (entity.GetType() == typeof(MovingPlatform) || entity.GetType() == typeof(FrictionPlatform))
                {
                    xIncrement = (float) 1.5;
                }
                else
                {
                    xIncrement = 3;
                }
            }
            else
            {
                collidesWithPlat = false;
            }
            if (hitboxLeftX.Overlaps(entity.hitbox) && velocity.X < 0)
            {
                velocity.X += 3;
            }
            if (hitboxRightX.Overlaps(entity.hitbox) && velocity.X > 0)
            {
                velocity.X -= 3;
            }

        }

        foreach (Bullet bullet in Game.enemybullets)
        {
            if (bullet.released && hitbox.Overlaps(bullet.hitbox))
            {
                Engine.PlaySound(playerHurtSound);
                if (armor)
                {
                    health -= 5;
                    armorDurability -= 20;
                } else {
                    health -= 10;
                }
                bullet.delete();
                hurt = true;
            } else
            {
                hurt = false;
            }
        }

        //Break armor if durability is 0
        if( armorDurability <= 0)
        {
            armor = false;
        }
        
        //THIS IS ACTUALLY CHANGING OUR POSITION, MOVEMENT CAUSED, HITBOX UPDATED
        hitbox = new Bounds2(position, size);
        position += velocity;
        

        if (Game.cam.inCamera(position, size))
        {
            draw();
        }
    }

    public void draw()
    {
        //is framerate (10) correct?
        if (velocity.X == 0)
        {
            playerFrameIndex = 0;
        } else
        {
            playerFrameIndex = (playerFrameIndex + Engine.TimeDelta * 10) % 3.0f;
            if (velocity.X < 0)
            {
                playerFrameIndex += 3;
            }
        }

        Bounds2 playerFrameBounds = new Bounds2((int)playerFrameIndex * 32, 16, 32, 48);
        if (hurt)
        {
            Engine.DrawTexture(playerTexture, new Vector2(position.X - Game.cam.position.X - 8, position.Y -  Game.cam.position.Y - 35), size: new Vector2(32, 48), source: playerFrameBounds, color: Color.Maroon);
        } else
        {
            Engine.DrawTexture(playerTexture, new Vector2(position.X - Game.cam.position.X - 8, position.Y -  Game.cam.position.Y - 35), size: new Vector2(32, 48), source: playerFrameBounds);
        }
        
        Engine.DrawTexture(playerTexture, new Vector2(position.X - Game.cam.position.X, position.Y -  Game.cam.position.Y - 11), size: new Vector2(20, 20), source: new Bounds2(0, 0, 32, 32), rotation: -90 - (float)GetAngleBetweenPoints(new Vector2(position.X + size.X / 2 - Game.cam.position.X, position.Y + size.Y / 2 - Game.cam.position.Y), new Vector2(Engine.MousePosition.X, Engine.MousePosition.Y)));
        
    }

    public static double GetAngleBetweenPoints(Vector2 p1, Vector2 p2)
    {
        var angle = Math.Atan((p2.X - p1.X)/(p2.Y - p1.Y)) * (180/Math.PI);
        var quadrantX = p1.X > p2.X ? 0 : 1;
        var quadrantY = p1.Y < p2.Y ? 0 : 1;
        if (quadrantX > 0) angle += 180;
        else if (quadrantY > 0) angle += 360;
        return angle;
    }

    public void ladderCollision(Ladder ladder)
    {
        //when the player is colliding with the ladder from either the left or the right
        if ((hitboxLeftX.Overlaps(ladder.hitbox) && velocity.X < 0) || (hitboxRightX.Overlaps(ladder.hitbox) && velocity.X > 0))
        {
            //sets the onLadder function to be true
            onLadder = true;
            canJump = false;
        }
        
    }

    public void collidableNPCCollision(CollidableNPC e)
    {
        if (hitboxLeftX.Overlaps(e.hitbox) || hitboxRightX.Overlaps(e.hitbox))
        {
            hurt = true;
        }
        else
        {
            hurt = false;
        }
    }
    public void portalCollision(Portal p)
    {
        if (p.size.X > p.size.Y) //length > width, only enter from top and bottom
        {
            //if x value is on the portal & y-bottom value is also on portal-top or y-top value is also on portal-bottom
            if ((hitboxY.Overlaps(p.hitbox)) && (velocity.Y > 0))
            {

                position.X = p.to.position.X + 30; //added to position to avoid loop
                position.Y = p.to.position.Y + 10;
            }
        }
        else //width > length, only enter from left and right
        {
            //if y value is on the portal & x-left value is also on portal-right or x-bottom value is also on portal-left
            if ((hitboxLeftX.Overlaps(p.hitbox) && velocity.X < 0) || (hitboxRightX.Overlaps(p.hitbox) && velocity.X > 0))
            {
                position.X = p.to.position.X;
                position.Y = p.to.position.Y - 30;//added to position to avoid loop
                velocity.Y = -10;
            }
        }
    }

    //updates win and lose to true if colliding
    public void updateWin(WinningBlock w)
    {
        if ((hitboxY.Overlaps(w.hitbox)) && (velocity.Y > 0 || velocity.Y < 0))
        {
            win = true;
            health = 100;
        }
        if ((hitboxLeftX.Overlaps(w.hitbox) && velocity.X < 0) || (hitboxRightX.Overlaps(w.hitbox) && velocity.X > 0))
        {
            win = true;
            health = 100;
        }


    }

    public void updateLose(LosingBlock l)
    {
        if ((hitboxY.Overlaps(l.hitbox)))
        {
            if (!collidesLose)
            {
                health -= 30;
            }
            health -= 1;
            collidesLose = true;
        }
        else if ((hitboxLeftX.Overlaps(l.hitbox) || (hitboxRightX.Overlaps(l.hitbox))))
        {
            if (!collidesLose)
            {
                health -= 30;
            }
            health -= 1;
            collidesLose = true;
        }
        else
        {
            collidesLose = false;
        }


    }

    public void laserCollision(Laser l)
    {
        if (!l.laserOn)
        {
            return;
        }

        if (hitboxY.Overlaps(l.hitbox) || hitboxYTop.Overlaps(l.hitbox))
        {
            if (!collidesLose)
            {
                health -= 50;
            }
            collidesLose = true;
        }
        else if ((hitboxLeftX.Overlaps(l.hitbox) || (hitboxRightX.Overlaps(l.hitbox))))
        {
            if (!collidesLose)
            {
                health -= 50;
            }
            collidesLose = true;
        }
        else
        {
            collidesLose = false;
        }

    }

    public void boosterCollision(Booster b)
    {
        if (hitboxLeftX.Overlaps(b.hitbox) || hitboxRightX.Overlaps(b.hitbox) || hitboxY.Overlaps(b.hitbox))
        {

            b.consumed = true;
            boosted = true;

        }
    }

    public void armorCollision(Armor b)
    {
        if (hitboxLeftX.Overlaps(b.hitbox) || hitboxRightX.Overlaps(b.hitbox) || hitboxY.Overlaps(b.hitbox))
        {
            b.consumed = true;
            armor = true;
            armorDurability = 100;
        }
    }
}