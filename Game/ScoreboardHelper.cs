using System;
using System.IO;
using System.Text.RegularExpressions;

public class ScoreboardHelper
{
    //number represents total gamestats saved in the order of 
    //int allEnemiesKilled;
    //int totalJumps;
    //int allBulletsFired;
    //int totalDeaths;
    //staeCount represents the total count of gamestates
    public static int stateCount = 4;
    public static int[] stat = new int[stateCount];
    public static string[] statDescription = {"Enemies Killed: ", "Jumps: ", "Bullets Fired: ", "Deaths: "};
    //static String[] completeLines = new string[stateCount];

    //static string fileLoc = "../../../Resource/GameStats.txt";

    //MODIFIER METHODS FOR GAMESTATS, will be called at closing of game program
    public static void updateStats (int allEnemiesKilled, int totalJumps, int allBulletsFired, int totalDeaths)
    {
        stat[0] = stat[0] + allEnemiesKilled;
        stat[1] = stat[1] + totalJumps;
        stat[2] = stat[2] + allBulletsFired;
        stat[3] = stat[3] + totalDeaths;
        writeFile();
    }
    
    public static void readFile()
    {
        StreamReader reader = null;
        string line;
        try
        {
            reader = new StreamReader("../../../Resource/GameStats.txt");

            line = reader.ReadLine();
            int index = 0;
            //puts every line into my string array
            while(line != null)
            {
                int content = int.Parse(Regex.Match(line, @"\d+\.*\d*").Value);
                Console.WriteLine(content);
                stat[index] = content;
                line = reader.ReadLine();
                index++;
            }

            reader.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine($"A Reading Error Occured");
        }
    }


    public static void writeFile()
    {
        
        try
        {
            File.Create("../../../Resource/GameStats.txt").Close();
            using (StreamWriter sw = new StreamWriter("../../../Resource/GameStats.txt", true))
            {
                int index = 0;
                foreach (string line in statDescription)
                {
                    
                    sw.WriteLine(line + stat[index]);
                    
                    index++;
                }

                ;
                sw.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured:");
        }
        
    }
}
