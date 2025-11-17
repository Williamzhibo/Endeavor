using System;
using System.Collections.Generic;
class clickableButton 
{
    string text;
    Bounds2 hitbox;
    Vector2 position;
    Vector2 size;
    Color color;
    Color fontColor = Color.White;
    public Font buttonFont;
    int fontSize;
    public clickableButton(float x, float y, float length, float width, Color color, string text)
    {
        this.text = text;
        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        this.position = new Vector2(x, y);
        this.size = new Vector2(length, width);
        this.color = color;
        this.buttonFont = Game.font;
    }

    public clickableButton(float x, float y, float length, float width, Color color, string text, Color fontColor, int fontSize)
    {
        this.text = text;
        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        this.position = new Vector2(x, y);
        this.size = new Vector2(length, width);
        this.color = color;
        this.fontColor = fontColor;
        this.buttonFont = Engine.LoadFont("Archeologicaps.ttf", fontSize);
    }


    public bool isClicked()
    {
        //if this button is clicked, it will return true
        return (Engine.GetMouseButtonDown(MouseButton.Left) && hitbox.Contains(Engine.MousePosition));
    }

    
    public void Draw()
    {
        Engine.DrawRectSolid(hitbox, color);
        Engine.DrawString(text, new Vector2(position.X + size.X/2, position.Y+ size.Y/4), fontColor, buttonFont, TextAlignment.Center);
    }
}

