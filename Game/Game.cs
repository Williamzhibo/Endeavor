using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class Game
{  
    Music backgroundMusic = Engine.LoadMusic("background.wav");

    //GUIDE: WHEN ADDING A NEW TYPE OF ENTITY
    //add class to Entities.cs file, and if there are new interactions between the Player and this new Entity:
    //1. In Player class (in Update) make sure to add the case for when player hits your entity type
    //2. Create a method for <YourEntity>Collision within player class
    //within this class also remember to:
    //1. Create ArrayList (in field)
    //2. Clear ArrayList at the start of each level (in method createLevel)
    //4. Add case to switch statement (in CreateLevel)
    //5. Create a foreach loop that updates every single entity of this type (in Update)
    //also make sure to add your object into the csv files to see concrete objects
    public static float time = 0;
    public static Vector2 gameCoordinates = new Vector2(1370, 700);
    public static Camera cam = new Camera(new Vector2(0, 40), gameCoordinates);
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = gameCoordinates;
    public static Font font = Engine.LoadFont("Retro Gaming.ttf", 11);
    string input = "";
    int level = 0;
    public int count = 0;

    public PlayerStats playerStats = new PlayerStats(980, 0, 120, 5, Color.Black);
    public Player player = new Player(50, 0, 10, 10, Color.SaddleBrown);

    public static List<Entity> stillEntities = new List<Entity>();
    public static List<MovingPlatform> movingEntities = new List<MovingPlatform>();
    public static List<Enemy> enemies = new List<Enemy>();
    public static List<WinLoseBlock> winLoseBlocks = new List<WinLoseBlock>();
    public static List<ShooterEnemy> shooterEnemies = new List<ShooterEnemy>();
    public static List<ImpenetrablePlatform> impenetrablePlatforms = new List<ImpenetrablePlatform>();
    public static List<Boss> bosses = new List<Boss>();
    public static List<Booster> boosters = new List<Booster>();
    public static List<Npc> npcs = new List<Npc>();
    public static List<String> npcText = new List<String>();
    public static List<Laser> lasers = new List<Laser>();

    //just set the x, y, length, width to 0 as these screens fill the entire board, color is their
    //background, and the boolean is the visibility status
    public startScreen startScreen = new startScreen(0, 0, 0, 0, Color.Red, true);
    public controlsScreen controlsScreen = new controlsScreen(0, 0, 0, 0, Color.Green, false);
    public creditsScreen creditsScreen = new creditsScreen(0, 0, 0, 0, Color.Blue, false);

    public scoreboardScreen scoreboardScreen = new scoreboardScreen(0, 0, 0, 0, Color.Blue, false);

    public loseScreen loseScreen = new loseScreen(0, 0, 0, 0, Color.Red, false);
    public winScreen winScreen = new winScreen(0, 0, 0, 0, Color.LightSkyBlue, false);
    public bossWinScreen bossWinScreen = new bossWinScreen(0, 0, 0, 0, Color.LightSkyBlue, false);

    public static clickableButton saveStats = new clickableButton(1180, 650, 200, 40, Color.Gray, "Click to save!");

    public int enemiesKilled = 0;
    public int totalDeaths = 0;
   // Bullet[] bullets = { bullet0, bullet1, bullet2, bullet3, bullet4, bullet5, bullet6, bullet7, bullet8, bullet9 };

    public static List<Bullet> bullets = new List<Bullet>();
    public static List<Bullet> enemybullets = new List<Bullet>();


    //this is the initializer, to make things
    public Game()
    {
        Engine.DrawRectSolid(new Bounds2(Vector2.Zero, Resolution), Color.Aqua);
        Engine.PlayMusic(backgroundMusic);
        createLevelWithTxt(level);

    }


    public StreamReader instantiatingLevel(int level)
    {
        StreamReader reader = new StreamReader(@"Resource\tutorial.txt");
        List<String> tutorialNPC = new List<string> { "Powerups give your various boosters", "This one will let you float...", 
            "Jump into portals", "Avoid lasers and shoot at enemies by tapping screen", "Some are unreachable", "Some platforms will move...",
            "Press <- and -> to move", "Press ^ to jump","Climb up the ladder","Pay attention to the top right of your screen", 
            "If you're shot your health decreases"};
        npcText = tutorialNPC;
        if (level == 1)
        {
            reader = new StreamReader(@"Resource\level1.txt");
            List<String> level1NPC = new List<string> { "Get across the city to fight the overlord and his minions", 
                "Kill as many of the minions as possible", "We are all robots that have had our souls trapped by the overlord", "It hurts...", 
                "You must stop the overlord" };
            npcText = level1NPC;
        }
        else if (level == 2)
        {
            reader = new StreamReader(@"Resource\level2.txt");
            List<String> level2NPC = new List<string> { "One more level until you reach the evil overlord", 
                "You are one step closer to freeing our souls to heaven" };
            npcText = level2NPC;
        }
        else if (level == 3)
        {
            reader = new StreamReader(@"Resource\level3.txt");
            List<String> level3NPC = new List<string> { "You're trainings have prepared you well. We believe in you! Make us proud!",
                "You've almost reached it...", "Are you prepared to fight the boss?", "Last level! You're almost there!" };
            npcText = level3NPC;
        }
        else if (level == 4)
        {
            reader = new StreamReader(@"Resource\boss.txt");
            List<String> bossNPC = new List<string> { "You have reached the final level! Defeat the boss and you will free our souls!" };
            npcText = bossNPC;
        }
        return reader;
    }

    public void createLevelWithTxt(int level)
    {
        Player player = new Player(50, 0, 64, 64, Color.SaddleBrown);
        PlayerStats playerStats = new PlayerStats(980, 0, 120, 5, Color.Black);

        //BACKGROUND DRAWING
        Engine.DrawRectSolid(new Bounds2(Vector2.Zero, Resolution), Color.Aqua);

        stillEntities.Clear();
        movingEntities.Clear();
        winLoseBlocks.Clear();
        enemies.Clear();
        shooterEnemies.Clear();
        impenetrablePlatforms.Clear();
        bosses.Clear();
        boosters.Clear();
        npcs.Clear();
        npcText.Clear();
        lasers.Clear();
        int npcNumber = 0;

        Dictionary<int, Portal> portalNames = new Dictionary<int, Portal>();
        portalNames.Clear();
        WinningBlock winningBlock = new WinningBlock(250, 40, 40, 40, Color.Green);
        LosingBlock losingBlock = new LosingBlock(560, 300, 40, 40, Color.Red);

        StreamReader reader = instantiatingLevel(level);
        using (reader)
        {
            String line = "";
            int rowNum = 0;
            while (!String.IsNullOrWhiteSpace(line = reader.ReadLine()))
            {
                Char[] charReader = line.ToCharArray();
                int columnNum = 0;
                for (int i = 0; i < charReader.Length; i++)
                {
                    Char c = charReader[i];
                    switch (c)
                    {
                        case 'A': // armor
                            Armor a = new Armor(columnNum * 32, rowNum * 32, 32, 32, Color.DarkGreen);
                            boosters.Add(a);
                            stillEntities.Add(a);
                            break;
                        case '#': // floor
                            Floor f = new Floor(columnNum * 32, rowNum * 32, 32, 32, Color.DarkGreen);
                            stillEntities.Add(f);
                            break;
                        case '*': // platform
                            Platform plat = new Platform(columnNum * 32, rowNum * 32, 32, 32, Color.BlueViolet);
                            stillEntities.Add(plat);
                            break;
                        case '-': //moving platform
                            MovingPlatform mov = new MovingPlatform(columnNum * 32, rowNum * 32, 32, 32, Color.Orange);
                            stillEntities.Add(mov);
                            movingEntities.Add(mov);
                            break;
                        case 'P': //Portal
                            Portal port = new Portal(columnNum * 32, rowNum * 32, 32, 32, Color.Black);
                            stillEntities.Add(port);
                            if (!portalNames.ContainsKey(1))
                            {
                                portalNames.Add(1, port);
                            }
                            else
                            {
                                portalNames.Add(2, port);
                            }
                            break;
                        case 'T': //ladder
                            Ladder l = new Ladder(columnNum * 32, rowNum * 32, 32, 32, Color.Black);
                            stillEntities.Add(l);
                            break;
                        case 'E': //shooter enemy
                            ShooterEnemy shooting = new ShooterEnemy(columnNum * 32, rowNum * 32, 32, 32, Color.Purple,player);
                            stillEntities.Add(shooting);
                            shooterEnemies.Add(shooting);
                            enemies.Add(shooting);
                            break;
                        case 'W': //winning block
                            winningBlock = new WinningBlock(columnNum * 32, rowNum * 32, 32, 32, Color.Green);
                            stillEntities.Add(winningBlock);
                            winLoseBlocks.Add(winningBlock);
                            break;
                        case 'L': //losing block
                            losingBlock = new LosingBlock(columnNum * 32, rowNum * 32, 32, 32, Color.Red);
                            stillEntities.Add(losingBlock);
                            winLoseBlocks.Add(losingBlock);
                            break;
                        case 'I': //impenetrable walls
                            ImpenetrablePlatform impenetrablePlatform = new ImpenetrablePlatform(columnNum * 32, rowNum * 32, 32, 32, Color.Gray);
                            stillEntities.Add(impenetrablePlatform);
                            impenetrablePlatforms.Add(impenetrablePlatform);
                            break;
                        case 'R': //boss
                            Boss boss = new Boss(columnNum * 32, rowNum * 32, 32, 32, Color.Gold,player);
                            bosses.Add(boss);
                            break;
                        case 'B': //booster
                            Booster booster = new Booster(columnNum * 32, rowNum * 32, 32, 32, Color.Gold);
                            stillEntities.Add(booster);
                            boosters.Add(booster);
                            break;
                        case 'N': //npc
                            Npc npc = new Npc(npcText[npcNumber], columnNum * 32, rowNum * 32, 32, 32, 50, 50, Color.Chocolate);
                            npcs.Add(npc);
                            npcNumber++;
                            break;
                        /*case 'A': //wall
                            Wall wall = new Wall(columnNum * 32, rowNum * 32, 32, 32, Color.Orange);
                            stillEntities.Add(wall);
                            break;*/
                        case '~': //laser
                            Laser laser = new Laser(columnNum * 32, rowNum * 32, 32, 32, Color.Orange);
                            stillEntities.Add(laser);
                            lasers.Add(laser);
                            break;
                    }
                    columnNum++;
                }
                rowNum++;
            }
            reader.Close();
        }
        if (portalNames.ContainsKey(1))
        {
            PortalPair portals = new PortalPair(0, 0, 0, 0, Color.BlueViolet,
                                    portalNames.GetValueOrDefault(1), portalNames.GetValueOrDefault(2));

        }
    }

    public void Update()
    {
        time += Engine.TimeDelta;
        cam.update(new Vector2(player.position.X - cam.getSize().X / 2, player.position.Y - cam.getSize().Y / 2 - 120));
        Engine.DrawRectSolid(new Bounds2(Vector2.Zero, Resolution), Color.Aqua);

        //opens game to start screen, if it isnt supposed to be showing, plays game like normal
        if (startScreen.startVisibility)
        {
            startScreen.Update();
        }
        else if (controlsScreen.controlsVisibility)
        {
            controlsScreen.Update();
        }
        else if (creditsScreen.creditsVisibility)
        {
            creditsScreen.Update();
        }
        else if (scoreboardScreen.scoreboardVisibility)
        {
            scoreboardScreen.Update();
        }
        else if (winScreen.winScreenVisibility)
        {
            winScreen.Update();
        }
        else if (bossWinScreen.bossWinScreenVisibility)
        {
            bossWinScreen.Update();
        }
        else if (loseScreen.loseScreenVisibility)
        {
            loseScreen.Update();
        }
        else if (loseScreen.loseScreenVisibility)
        {
            loseScreen.Update();
        }
        else if (saveStats.isClicked())
        {
            ScoreboardHelper.updateStats(enemiesKilled, player.totalJumps, player.bulletsFired, totalDeaths);
            enemiesKilled = 0;
            player.totalJumps = 0;
            player.bulletsFired = 0;
            totalDeaths = 0;

        }
        else
        {
            if (level == 4)
            {
                player.win = true; //setting winning to true;
                foreach (Boss b in bosses)
                {
                    if (b.dead == false) //if even one boss is not dead then player has not won 
                    {
                        player.win = false;
                    }
                }
                if (player.win)
                {
                    bossWinScreen.bossWinScreenVisibility = true;
                    player.win = false;
                    level = 0;
                    createLevelWithTxt(level);
                    player.position.X = 50;
                    player.position.Y = 0;
                }
            }
            if (player.win && level != 4)
            {
                level++;
                ScoreboardHelper.readFile();
                winScreen.winScreenVisibility = true;
                player.position.X = 50;
                player.position.Y = 0;
                player.win = false;
                createLevelWithTxt(level);
                return;
            }
            else if (player.lose)
            {
                loseScreen.loseScreenVisibility = true;
                player.lose = false;
                player.health = 100;
                player.position.X = 50;
                player.position.Y = 0;
                createLevelWithTxt(level);
                totalDeaths++;
                ScoreboardHelper.updateStats(enemiesKilled, player.totalJumps, player.bulletsFired, totalDeaths);
                enemiesKilled = 0;
                player.totalJumps = 0;
                player.bulletsFired = 0;
                totalDeaths = 0;
                return;
            }
            
            if (Engine.GetKeyHeld(Key.F))
            {
                if (!player.floating)
                {
                    input = "floating";
                }
            }
            if (Engine.GetKeyHeld(Key.Space))
            {
                input = "long jump";
            }
            if (Engine.GetKeyHeld(Key.Left)|| Engine.GetKeyHeld(Key.A))
            {
                input = "left";
            }
            else if (Engine.GetKeyHeld(Key.Right) || Engine.GetKeyHeld(Key.D))
            {
                input = "right";
            }
            if (Engine.GetKeyHeld(Key.Up) || Engine.GetKeyHeld(Key.W)) //jump
            {
                input = "jump!";
                
            }

            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                input = "mouse click";
                
            }
            
            /*
            //DEV TOOLS 
            if (Engine.GetKeyDown(Key.F1))
            {
                level = 0;
                createLevelWithTxt(level);
            }
            if (Engine.GetKeyDown(Key.F2))
            {
                level = 1;
                createLevelWithTxt(level);
            }
            if (Engine.GetKeyDown(Key.F3))
            {
                level = 2;
                createLevelWithTxt(level);
            }
            if (Engine.GetKeyDown(Key.F4))
            {
                level = 3;
                createLevelWithTxt(level);
            }
            if (Engine.GetKeyDown(Key.F5))
            {
                level = 4;
                createLevelWithTxt(level);
            }
            //DEV TOOL 
            */

            //update entitities
            player.update();
          
            foreach (MovingPlatform movingPlatform in movingEntities)
            {
                movingPlatform.update();
            }
            foreach (Entity entity in stillEntities)
            { 
                if (typeof(Laser) != entity.GetType() && typeof(Enemy) != entity.GetType() && typeof(ShooterEnemy) != entity.GetType())
                {
                    entity.update();
                }
            }
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                
                enemies[i].update();

                if (enemies[i].dead)
                {
                    enemiesKilled++;
                    stillEntities.RemoveAll(entity => entity == enemies[i]);

                    enemies.RemoveAt(i);
                }

            }

            for (int i = shooterEnemies.Count - 1; i >= 0; i--)
            {
                count++;
                if (!shooterEnemies[i].dead)
                {
                    shooterEnemies[i].update(player);
                }
                else
                {
                    enemiesKilled++;
                    stillEntities.RemoveAll(entity => entity == shooterEnemies[i]);
                    shooterEnemies.RemoveAt(i);
                }
            }
            
            

            
            foreach (ImpenetrablePlatform impenetrablePlatform in impenetrablePlatforms)
            {
                impenetrablePlatform.update();
            }

            foreach (Boss boss in bosses)
            {
                boss.update(player);
            }

            foreach (Laser laser in lasers)
            {
                laser.update();
            }

            for (int i = boosters.Count - 1; i >= 0; i--)
            {
                boosters[i].update();
                if (boosters[i].consumed)
                {
                    stillEntities.RemoveAll(entity => entity == boosters[i]);
                    boosters.RemoveAt(i);
                }
            }
          
            foreach (Npc npc in npcs)
            {
                npc.update(player);

                count++;
            }
            count = 0;

            playerStats.update(player);
            //end of update entities
            
            Engine.DrawString("Enemies Killed:" + ScoreboardHelper.stat[0] + "  Jumps:" + ScoreboardHelper.stat[1] + "  Bullets Fired:" + ScoreboardHelper.stat[2] + "  Total Deaths: " + ScoreboardHelper.stat[3], new Vector2(20, 660), Color.Black, font);
            saveStats.Draw();

            foreach (Bullet b in bullets) { b.update(cam, impenetrablePlatforms); }
            foreach (Bullet b in enemybullets) { b.update(cam, impenetrablePlatforms); }
        }
    }
}