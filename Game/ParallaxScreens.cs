
using System;
using System.Collections.Generic;
using System.Numerics;

class ParallaxScreen
{ 
    Texture WorldSprites = Engine.LoadTexture("ParallaxBackground_V2.png");

    Vector2 backgroundResolution = new Vector2(1390, 700);

    Bounds2 backgroundTextureBounds; //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;

    //these are the position functions for the texture boxes
    Vector2 position;
    Color color;
    //Vector2 position2;

    int texture;
    Vector2 velocity;

    //Initializes a parallax screen pair
    //texture is the image it wants to draw
    //order is the order that it should draw at. 
    public ParallaxScreen(int texture, float speed)
    {
        this.texture = texture;

        position = new Vector2(0, 0);
        
        velocity = new Vector2(speed, 0);

        
        backgroundTextureBounds = new Bounds2(0, texture * 700, 1390, 700);

    }

    
    public void Update(Vector2 playerPosition)
    {
        position = new Vector2(playerPosition.X * velocity.X * 0.1f, 0); 

        for (int i = -4; i < 20; i++)
        {
            if (i%2 == 0)
            {
                color = Color.AliceBlue;
            } else
            {
                color = Color.Green;
            }
            //Engine.DrawRectSolid(new Bounds2(new Vector2(position.X + (i*backgroundResolution.X), position.Y ), backgroundResolution), color);
            Engine.DrawTexture(WorldSprites, new Vector2(position.X + (i * backgroundResolution.X), position.Y), source: backgroundTextureBounds, size: backgroundResolution, mirror: mirrorbackground);
        }
     
    }

}
