using System;
using System.Collections.Generic;
using System.Numerics;

//just set the x, y, length, width to 0 as these screens fill the entire board, color is their
//background, and the boolean is the visibility status
class winScreen : Entity
{
    Texture gameScreens = Engine.LoadTexture("gameScreens.png");

    Vector2 gameScreensResolution = new Vector2(1390, 700);

    Bounds2 gameScreensTextureBounds = new Bounds2(1390 * 2, 0, 1390, 700); //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;

    //makes button that is clickable
    private static clickableButton nextButton = new clickableButton(620, 465, 150, 50, Color.AliceBlue, "Next", Color.Black, 15);

    //determines if i can see this
    public static bool winScreenVisibility
    {
        get;
        set;
    }
    public winScreen(float x, float y, float length, float width, Color color, bool isVisible) : base(x, y, length, width, color)
    {

        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        winScreenVisibility = isVisible;
    }


    public void Update()
    {
        //when my button is clicked, 
        if (nextButton.isClicked())
        {
            //closes winScreen
            winScreenVisibility =  false;
        }
        if (winScreenVisibility)
        {
            Draw();
        }

    }

    public void Draw()
    {
        //draws the entire screen
        Engine.DrawTexture(gameScreens, new Vector2(0, 0), source: gameScreensTextureBounds, size: gameScreensResolution, mirror: mirrorbackground);

        //draws the escape button
        nextButton.Draw();

    }
}

class bossWinScreen : winScreen
{
    Texture gameScreens = Engine.LoadTexture("gameScreens.png");

    Vector2 gameScreensResolution = new Vector2(1390, 700);

    Bounds2 gameScreensTextureBounds = new Bounds2(1390 * 2, 0, 1390, 700); //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;

    private static clickableButton back = new clickableButton(620 - 125, 465, 400, 50, Color.AliceBlue, "Go Back To Start Screen", Color.Black, 15);
    public bossWinScreen(float x, float y, float length, float width, Color color, bool isVisible) : base(x,  y,  length,  width,  color,  isVisible)
    {

        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        bossWinScreenVisibility = isVisible;
    }
    public new void Update()
    {
        if (back.isClicked())
        {
            //closes winScreen
            bossWinScreenVisibility = false;
            startScreen.startVisibility = true;
        }
        if (bossWinScreenVisibility)
        {
            Draw();
        }

    }
    public new void Draw()
    {
        //draws the entire screen
        Engine.DrawTexture(gameScreens, new Vector2(0, 0), source: gameScreensTextureBounds, size: gameScreensResolution, mirror: mirrorbackground);
        back.Draw();
        
    }
    public static bool bossWinScreenVisibility
    {
        get;
        set;
    }
}



//just set the x, y, length, width to 0 as these screens fill the entire board, color is their
//background, and the boolean is the visibility status
class loseScreen : Entity
{
    Texture gameScreens = Engine.LoadTexture("gameScreens.png");

    Vector2 gameScreensResolution = new Vector2(1390, 700);

    Bounds2 gameScreensTextureBounds = new Bounds2(1390 * 1, 0, 1390, 700); //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;

    static Color ButtonColor = new Color(63, 70, 70);

    //makes button that is clickable

    private static clickableButton resetButton = new clickableButton(620, 465, 150, 50, ButtonColor, "Respawn");

    //determines if i can see this
    public static bool loseScreenVisibility
    {
        get;
        set;
    }
    public loseScreen(float x, float y, float length, float width, Color color, bool isVisible) : base(x, y, length, width, color)
    {

        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        loseScreenVisibility = isVisible;
    }

    public void Update()
    {
        //when my button is clicked, 
        if (resetButton.isClicked())
        {
            //closes loseScreen
            loseScreenVisibility = false;
        }
        if (loseScreenVisibility)
        {
            Draw();
        }

    }

    public void Draw()
    {
        //draws the entire screen
        //Engine.DrawRectSolid(new Bounds2(Vector2.Zero, Game.Resolution), Color.Aqua);
        Engine.DrawTexture(gameScreens, new Vector2(0,0), source: gameScreensTextureBounds, size: gameScreensResolution, mirror: mirrorbackground);

        //draws the escape button
        resetButton.Draw();


       
    }
}




//just set the x, y, length, width to 0 as these screens fill the entire board, color is their
//background, and the boolean is the visibility status
class creditsScreen : Entity
{
    Texture gameScreens = Engine.LoadTexture("gameScreens.png");

    Vector2 gameScreensResolution = new Vector2(1390, 700);

    Bounds2 gameScreensTextureBounds = new Bounds2(0, 0, 1390, 700); //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;
    static Color ButtonColor = new Color(63, 70, 70);
    //makes button that is clickablea
    private static clickableButton BackButton = new clickableButton(620, 465, 150, 50, ButtonColor, "Back", Color.White, 17);
    public Font creditsFont = Engine.LoadFont("Retro Gaming.ttf", 19);
    //determines if i can see this
    public static bool creditsVisibility
    {
        get;
        set;
    }
    public creditsScreen(float x, float y, float length, float width, Color color, bool isVisible) : base(x, y, length, width, color)
    {

        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        creditsVisibility = isVisible;
    }



    public void Update()
    {
        //when my button is clicked, 
        if (BackButton.isClicked())
        {
            //turns back on the start screen, and turns off the controls screen
            startScreen.startVisibility = true;
            creditsVisibility = false;
        }
        if (creditsVisibility)
        {
            Draw();
        }

    }

    public void Draw()
    {
        Engine.DrawTexture(gameScreens, new Vector2(0, 0), source: gameScreensTextureBounds, size: gameScreensResolution, mirror: mirrorbackground);
        BackButton.Draw();
        Engine.DrawString("Produced By:", new Vector2(695, 250), Color.Black, creditsFont, TextAlignment.Center);
        Engine.DrawString("Colin Xie", new Vector2(695, 275), Color.Black, creditsFont, TextAlignment.Center);
        Engine.DrawString("Prakshi Shukla", new Vector2(695, 300), Color.Black, creditsFont, TextAlignment.Center);
        Engine.DrawString("William Bo", new Vector2(695, 325), Color.Black, creditsFont, TextAlignment.Center);
        Engine.DrawString("Artur Sobol", new Vector2(695, 350), Color.Black, creditsFont, TextAlignment.Center);
    }
}







//rules screen
//just set the x, y, length, width to 0 as these screens fill the entire board, color is their
//background, and the boolean is the visibility status
class controlsScreen : Entity
{
    Texture gameScreens = Engine.LoadTexture("gameScreens.png");

    Vector2 gameScreensResolution = new Vector2(1390, 700);

    Bounds2 gameScreensTextureBounds = new Bounds2(0, 0, 1390, 700); //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;
    static Color ButtonColor = new Color(63, 70, 70);
    //makes button that is clickable
    private static clickableButton backButton = new clickableButton(620, 465, 150, 50, ButtonColor, "Back", Color.White, 17);

    public Font controlsFont = Engine.LoadFont("Retro Gaming.ttf", 18);

    //determines if i can see this
    public static bool controlsVisibility
    {
        get;
        set;
    }
    public controlsScreen(float x, float y, float length, float width, Color color, bool isVisible) : base(x, y, length, width, color)
    {

        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        controlsVisibility = isVisible;
    }

    public void Update()
    {
        //when my button is clicked, 
        if (backButton.isClicked())
        {
            //turns back on the start screen, and turns off the controls screen
            startScreen.startVisibility = true;
            controlsVisibility = false;
        }
        if (controlsVisibility)
        {
            Draw();
        }
            
    }

    public void Draw()
    {
        Engine.DrawTexture(gameScreens, new Vector2(0, 0), source: gameScreensTextureBounds, size: gameScreensResolution, mirror: mirrorbackground);
        backButton.Draw();
        Engine.DrawString("WASD to Move", new Vector2(695, 250), Color.Black, controlsFont, TextAlignment.Center);
        Engine.DrawString("<^v> to Move", new Vector2(695, 275), Color.Black, controlsFont, TextAlignment.Center);
        Engine.DrawString("Tap to shoot", new Vector2(695, 300), Color.Black, controlsFont, TextAlignment.Center);
    }
}


//just set the x, y, length, width to 0 as these screens fill the entire board, color is their
//background, and the boolean is the visibility status
class scoreboardScreen : Entity
{
    Texture gameScreens = Engine.LoadTexture("gameScreens.png");

    Vector2 gameScreensResolution = new Vector2(1390, 700);

    Bounds2 gameScreensTextureBounds = new Bounds2(0, 0, 1390, 700); //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;
    static Color ButtonColor = new Color(63, 70, 70);

    //makes button that is clickable
    private static clickableButton backButton = new clickableButton(620, 465, 150, 50, ButtonColor, "Back", Color.White, 17);
    public ScoreboardHelper scoreboardHelper = new ScoreboardHelper();
    public Font statisticsFont = Engine.LoadFont("Retro Gaming.ttf", 18);

    //determines if i can see this
    public static bool scoreboardVisibility
    {
        get;
        set;
    }
    public scoreboardScreen(float x, float y, float length, float width, Color color, bool isVisible) : base(x, y, length, width, color)
    {

        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        scoreboardVisibility = isVisible;
    }

    public void Update()
    {
        //when my button is clicked, 
        if (backButton.isClicked())
        {
            //closes scoreboard
            scoreboardVisibility = false;
            startScreen.startVisibility = true;
        }
        if (scoreboardVisibility)
        {
            ScoreboardHelper.readFile();
            Draw();
        }

    }

    public void Draw()
    {
        //draws the entire screen
        Engine.DrawTexture(gameScreens, new Vector2(0, 0), source: gameScreensTextureBounds, size: gameScreensResolution, mirror: mirrorbackground);

        //draws the escape button
        backButton.Draw();

        //gives you your scoreboard
        int y = 225;
        int index=0;
        foreach(string line in ScoreboardHelper.statDescription)
        {
            Engine.DrawString(line + ScoreboardHelper.stat[index], new Vector2(695, y), Color.Black, statisticsFont, TextAlignment.Center);
            y += 40;
            index++;
        }
        
        //Engine.DrawString("Your highscore is: ", new Vector2(100, 100), Color.Black, Game.font);
    }
}


class startScreen : Entity
{
    Texture gameScreens = Engine.LoadTexture("gameScreens.png");

    Vector2 gameScreensResolution = new Vector2(1390, 700);

    Bounds2 gameScreensTextureBounds = new Bounds2(0, 0, 1390, 700); //this allows us to read from the texture file so we can display the correct texture

    TextureMirror mirrorbackground = TextureMirror.None;
    static Color SSButtonColor = new Color(63, 70, 70);
    //makes button that is clickable
    private static clickableButton startButton = new clickableButton(620, 225, 150, 50, SSButtonColor, "Start", Color.White, 16);
    private static clickableButton controlsButton = new clickableButton(620, 305, 150, 50, SSButtonColor, "Controls", Color.White, 16);
    private static clickableButton creditsButton = new clickableButton(620, 385, 150, 50, SSButtonColor, "Credits", Color.White, 16);
    private static clickableButton scoreboardButton = new clickableButton(620, 465, 150, 50, SSButtonColor, "Statistics", Color.White, 16);
    
    


    //determines if i can see this
    public static bool startVisibility
    {
        get;
        set;
    }
    public startScreen(float x, float y, float length, float width, Color color, bool isVisible) : base(x, y, length, width, color)
    {
        
        this.hitbox = new Bounds2(new Vector2(x, y), new Vector2(length, width));
        startVisibility = isVisible;
    }


    public void Update()
    {

        //when my button is clicked, 
        if (startButton.isClicked())
        {
            startVisibility = false;

        } else if (controlsButton.isClicked())
        {
            controlsScreen.controlsVisibility = true;
            startVisibility = false;
        } else if (creditsButton.isClicked())
        {
            creditsScreen.creditsVisibility = true;
            startVisibility = false;
        } else if (scoreboardButton.isClicked())
        {
            scoreboardScreen.scoreboardVisibility = true;
            startVisibility = false;
        }
        if (startVisibility)
        {
            //if you can show the start screen, draw it
            Draw();
        }
    }
    public void Draw()
    {
        Engine.DrawTexture(gameScreens, new Vector2(0, 0), source: gameScreensTextureBounds, size: gameScreensResolution, mirror: mirrorbackground);
        startButton.Draw();
        controlsButton.Draw();
        creditsButton.Draw();
        scoreboardButton.Draw();
    }
}


