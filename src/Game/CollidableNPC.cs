using System;
using System.Collections.Generic;
using System.Text;

class CollidableNPC : Entity
{
    public Vector2 initialPos;

    public CollidableNPC(float x, float y, float length, float width, Color color) : base(x, y, length, width, color)
    {
        velocity.X = 1;
        initialPos = new Vector2(x, y);
    }

    public new void update()
    {
        if (position.X >= initialPos.X + 20 && velocity.X > 0)
        {
            velocity.X = -(float)0.25;
        }
        if (position.X <= initialPos.X - 20 && velocity.X < 0)
        {
            velocity.X = (float)0.25;
        }

        base.update();
    }
}
