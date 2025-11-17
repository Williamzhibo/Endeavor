using System;
using System.Collections.Generic;
using System.Text;

class Enemy : CollidableNPC
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Sound enemyDeathSound = Engine.LoadSound("enemyDeath.wav");
    Sound enemyShootSound = Engine.LoadSound("enemyShoot.wav");

    Bounds2 enemyBounds = new Bounds2(32 * 0, 0, 32, 32);
    TextureMirror mirrorEnemy = TextureMirror.None;
    public Boolean dead;
    private int countTicksHurt;

    public Enemy(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        dead = false;
        countTicksHurt = 0;
    }

    public void playShootSound()
    {
        Engine.PlaySound(enemyShootSound);
    }

    public void playDeathSound()
    {
        Engine.PlaySound(enemyDeathSound);
    }

    public void update()
    {

        foreach (Bullet bullet in Game.bullets)
        {
            if (bullet.released && hitbox.Overlaps(bullet.hitbox))
            {
                dead = true;
                bullet.delete();
            }
        }

        if (!dead)
        {
            if (countTicksHurt > 0)
            {
                color = Color.Pink;
                countTicksHurt++;
                if (countTicksHurt > 5)
                {
                    dead = true;
                    playDeathSound();
                }
            }

            if (color.Equals(Color.Black))
            {
                foreach (Bullet bullet in Game.bullets)
                {
                    if (bullet.released && hitbox.Overlaps(bullet.hitbox))
                    {
                        countTicksHurt++;
                        bullet.delete();
                    }
                }
            }


            base.update();
        }
    }
    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: enemyBounds, size: new Vector2(size.X, size.Y), mirror: mirrorEnemy);
    }
}