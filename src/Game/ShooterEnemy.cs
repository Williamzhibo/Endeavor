using System;
using System.Collections.Generic;
using System.Text;

class ShooterEnemy : Enemy
{
    int countTicksReleased;
    protected Player player;

    public ShooterEnemy(float x, float y, float length, float width, Color color, Player player) : base(x, y, length, width, color)
    {
        countTicksReleased = 0;
        this.player = player;
    }

    public void update(Player player)
    {

        //find x distance between player and enemy
        float xDistance = player.position.X - (position.X + size.X / 2);
        //find y distance between player and enemy
        float yDistance = player.position.Y - (position.Y + size.Y / 2);
        float magnitude = (float)Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));

        if (countTicksReleased > 100)
        {
            if (magnitude < 300)
            {
                float posX = position.X + size.X / 2;
                float posY = position.Y + size.Y / 2;
                float velX = (xDistance / magnitude) * 4;
                float velY = (yDistance / magnitude) * 4;
                Game.enemybullets.Add(new Bullet(posX, posY, 5, 5, velX, velY, Color.Black, true, true));
                countTicksReleased = 0;

                playShootSound();
            }

        }
        countTicksReleased++;


        base.update();
    }

}