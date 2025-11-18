using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

class Boss : ShooterEnemy
{
    Texture bossTexture = Engine.LoadTexture("Boss.png");
    public Sound bossDeathScreechSound = Engine.LoadSound("bossHurtScreech.wav");

    EnemyBullet bullet;
    int countTicksReleased;
    private int countTicksHurt;
    private int health = 100;
    public Boolean collidesWithBullet = false;

    public float bossFrameIndex = 0;
    public TextureMirror mirrorBoss;
    
    public Boss(float x, float y, float length, float width, Color color, Player player) : base(x, y, length, width, color, player)
    {
        bullet = new EnemyBullet(0, 0, 8, 8, 0, 0, Color.Plum, false);
        countTicksReleased = 0;
        dead = false;
        countTicksHurt = 0;
    }

    public new void update(Player player)
    {
        //boss only
        //shooter enemy

        //find x distance between player and enemy
        float xDistance = player.position.X - (position.X + size.X / 2);
        //find y distance between player and enemy
        float yDistance = player.position.Y - (position.Y + size.Y / 2);
        float magnitude = (float)Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));


        // enemy
        if (!dead)
        {
            if (health == 0)
            {
                dead = true;
                Engine.PlaySound(bossDeathScreechSound);
            }

            if (countTicksHurt > 0)
            {
                color = Color.Pink;
                countTicksHurt++;
                if (countTicksHurt > 5)
                {
                    //health--;
                    countTicksHurt = 0;
                }
            }

            foreach (Bullet bullet in Game.bullets)
            {
                if (bullet.released && hitbox.Overlaps(bullet.hitbox))
                {
                    countTicksHurt++;
                    health -= 10;
                    bullet.delete();

                    playDeathSound();
                } 
            }
            // draws the boss stuffs
            Engine.DrawRectSolid(new Bounds2(new Vector2(position.X - Game.cam.position.X, position.Y - 4 - Game.cam.position.Y), new Vector2(health / 2, 4)), Color.Green);
            Engine.DrawRectSolid(new Bounds2(new Vector2(position.X + health/2 - Game.cam.position.X, position.Y - 4 - Game.cam.position.Y), new Vector2((100 - health) / 2, 4)), Color.Red);

            if (countTicksReleased > 100)
            {
                if (magnitude < 300)
                {
                    float posX = position.X + size.X / 2;
                    float posY = position.Y + size.Y / 2;
                    float velX = (xDistance / magnitude) * 4;
                    float velY = (yDistance / magnitude) * 4;
                    Game.enemybullets.Add(new Bullet(posX, posY, 5, 5, velX, velY, Color.Black, true));
                    countTicksReleased = 0;
                }

            }
            countTicksReleased++;



            if (position.X >= initialPos.X + 20 && velocity.X > 0)
            {
                velocity.X = -(float)0.25;
            }
            if (position.X <= initialPos.X - 20 && velocity.X < 0)
            {
                velocity.X = (float)0.25;
            }

            hitbox = new Bounds2(position, size);
            position += velocity;

            if (Game.cam.inCamera(position, size))
            {
                draw();
            }
        }

    }

    public void shoot(float xDistance, float yDistance, float magnitude, float bulletSpeed)
    {
        bullet.position.X = position.X + size.X / 2;
        bullet.position.Y = position.Y + size.Y / 2;
        bullet.velocity.X = (xDistance / magnitude) * bulletSpeed;
        bullet.velocity.Y = (yDistance / magnitude) * bulletSpeed;
        bullet.released = true;

        countTicksReleased = 0;
        playShootSound();
    }

    public override void draw()
    {
        if (velocity.X > 0)
        {
            mirrorBoss = TextureMirror.Horizontal;
        }
        else if (velocity.X < 0)
        {
            mirrorBoss = TextureMirror.None;
        }

        Bounds2 bossFrameBounds;
        if (countTicksHurt > 0)
        {
            bossFrameBounds = new Bounds2(64, 64, 32, 32);
        }
        else
        {
            bossFrameIndex = (bossFrameIndex + Engine.TimeDelta * 1f) % 8.0f;
            bossFrameBounds = new Bounds2(32 * ((int)bossFrameIndex / 3), 32 * ((int)bossFrameIndex % 3), 32, 32);
        }
        Engine.DrawTexture(bossTexture, new Vector2(position.X - Game.cam.position.X, position.Y -  Game.cam.position.Y), source : bossFrameBounds, size : new Vector2(48, 48), mirror : mirrorBoss);
    }
}