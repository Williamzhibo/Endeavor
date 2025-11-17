using System;

class Npc : Entity
{
    Texture WorldSprites = Engine.LoadTexture("WorldSprites.png");

    Bounds2 NPCBounds = new Bounds2(32 * 3, 0, 32, 32);
    TextureMirror mirrorNPC = TextureMirror.None;

    private String dialogue;
    private Vector2 talkSize;
    private Font font;
    public Npc(String dialogue, float x, float y, float sizeX, float sizeY, float talkSizeX, float talkSizeY, Color color) : base(x, y, sizeX, sizeY, color)
    {
        this.dialogue = dialogue;
        this.talkSize = new Vector2(talkSizeX, talkSizeY);
        this.font = Engine.LoadFont("Retro Gaming.ttf", 13);
    }

    public Boolean inTalkRange(Vector2 playerPos)
    {
        if (playerPos.X >= position.X - talkSize.X && playerPos.X <= position.X + size.X + talkSize.X)
        {
            if (playerPos.Y >= position.Y - talkSize.Y && playerPos.Y <= position.Y + size.Y + talkSize.Y)
            {
                return true;
            }
        }
        return false;
    }

    public void speak()
    {
        Engine.DrawString(dialogue, new Vector2(position.X - size.X * dialogue.Length/10  - Game.cam.position.X, position.Y - size.Y * 2 - Game.cam.position.Y), Color.White, font);
    }
    public override void draw()
    {
        Engine.DrawTexture(WorldSprites, new Vector2(position.X - Game.cam.position.X, position.Y - Game.cam.position.Y), source: NPCBounds, size: new Vector2(size.X, size.Y), mirror: mirrorNPC);
    }

    public void update(Player player)
    {
        hitbox = new Bounds2(player.position, player.size);
        position += velocity;

        if (Game.cam.inCamera(player.position, player.size) && !player.dead)
        {
            draw();
        }

        if (inTalkRange(player.position))
        {
            speak();
        }
    }
}