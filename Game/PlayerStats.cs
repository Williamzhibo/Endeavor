using System;
using System.Collections.Generic;
using System.Text;

class PlayerStats : Entity
{
    public PlayerStats(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {

    }

    public void update(Player player)
    {
        draw(player);
    }

    public void draw(Player player)
    {
        Engine.DrawRectSolid(new Bounds2(new Vector2(1180, 0), new Vector2(140,60)), color);
        // health bar
        Engine.DrawRectSolid(new Bounds2(new Vector2(1200, 10), new Vector2(player.health, 20)), Color.Green);
        Engine.DrawRectSolid(new Bounds2(new Vector2(1200 + player.health, 10), new Vector2(100 - player.health, 20)), Color.Red);

        // armor bar
        if (player.armor)
        {
            Engine.DrawRectSolid(new Bounds2(new Vector2(1200, 30), new Vector2(player.armorDurability, 20)), Color.Gray);
        }
    }
}
