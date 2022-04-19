using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.ComponentModel;
using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Silent_Void
{
    class Level
    {
        Game1 game;
        int enemies, OpEnemies, spiders, babies, boss;
        public string bg, music;
        int[] wave = new int[3];
        public List<int[]> wavePlural = new List<int[]>();
        public int waveNum = 0;
        public Level(IServiceProvider _serviceProvider, string path, Game1 game)
        {
            //Create a new content manager to load content used by this level.
            //content = new ContentManager(_serviceProvider, "Content");
            this.game = game;
            LoadLevel(path);
        }

        private void LoadLevel(string path)
        {
            //Load the level and ensure all of the lines are the same length.
            int numOfTilesAcross = 0;
            List<string> lines = new List<string>();
            try
            {
                //Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = reader.ReadLine();
                    numOfTilesAcross = line.Length;
                    while (line != null)
                    {
                        lines.Add(line);  //Saves the text file data in the List

                        //items.Add(line.Split(' '));
                        Debug.WriteLine(line);
                        line = reader.ReadLine();
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Debug.WriteLine("works!");
            ReadLevel(lines);
            //Allocate the tile grid based on the size of the world derived from the file.
        }

        private void ReadLevel(List<String> items)
        {
            //Debug.WriteLine(items[0][0]);
            bg = items[0];

            Debug.Print(bg);

            if (items[1].Equals("null"))
            {
                music = "none";
                Console.WriteLine("no freaking music!");
            }
            
            for (int i = 0; i < items.Count(); i++)
            {
                String[] word;
                word = items[i].Split(' ');


                if (word[0].Equals("en1"))
                {

                    enemies += int.Parse(word[1]);
                }

                if (word[0].Equals("op1"))
                {

                    OpEnemies += int.Parse(word[1]);
                }
                if (word[0].Equals("sp1"))
                {

                    spiders += int.Parse(word[1]);
                }

                if (word[0].Equals("ba1"))
                {

                    babies += int.Parse(word[1]);
                }
                if (word[0].Equals("boss1"))
                {

                    boss += int.Parse(word[1]);
                }

                if (items[i].Equals("."))
                {
                    wave = new int[]{ enemies, OpEnemies, spiders, babies, boss };
                    wavePlural.Add(wave);
                    enemies = 0;
                    OpEnemies = 0;
                    spiders = 0;
                    babies = 0;
                    boss = 0;

                }

                Debug.WriteLine(enemies + ", " + OpEnemies);

            }
            

        }

        public void startWave()
        {
            for (int i = 0; i < wavePlural[waveNum][1]; i++)
            {
                game.planes.Add(new OpEnemy(new Vector2(800, 0), 1f));
            }
            for (int i = 0; i < wavePlural[waveNum][0]; i++)
            {
                game.planes.Add(new Enemy(new Vector2(800, 0), 1f));
            }
            for (int i = 0; i < wavePlural[waveNum][2]; i++)
            {
                game.planes.Add(new MamaSpider(new Vector2(800, 0), 1f));
            }
            for (int i = 0; i < wavePlural[waveNum][3]; i++)
            {
                game.planes.Add(new Spider(new Vector2(800, 0), 1f));
            }
            for (int i = 0; i < wavePlural[waveNum][4]; i++)
            {
                game.planes.Add(new SpiderBoss(new Vector2(800, 0), 1f));
            }
            for (int i = 0; i < game.planes.Count; i++)
            {
                game.planes[i].loadSfx(game.sfxShot);
            }
            waveNum++;
        }
    }
}
