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
        // initialize necessary instance fields
        Game1 game;
        int enemies, OpEnemies;
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

        private void LoadLevel(string path) // method for loading level based on paramater of text file
        {
            int ns = 0;
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = reader.ReadLine();
                    ns = line.Length;
                    while (line != null)
                    {
                        lines.Add(line); // loading level using inside info

                        //items.Add(line.Split(' '));
                        Debug.WriteLine(line);
                        line = reader.ReadLine();
                    }
                    
                }
            }
            catch (Exception e) // catch exception in case it doesnt work
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Debug.WriteLine("works!");
            ReadLevel(lines);
        }

        private void ReadLevel(List<String> items) // reading level's information to produce level
        {
            //Debug.WriteLine(items[0][0]);
            bg = items[0];

            Debug.Print(bg);

            if (items[1].Equals("null")) // load music
            {
                music = "none";
                Console.WriteLine("no freaking music!");
            }
            
            for (int i = 0; i < items.Count(); i++) // load enemies - both normal and OP
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

                if(items[i].Equals(".")) // indicates new wave
                {
                    wave = new int[]{ enemies, OpEnemies };
                    wavePlural.Add(wave);
                    enemies = 0;
                    OpEnemies = 0;
                    
                }

                Debug.WriteLine(enemies + ", " + OpEnemies);

            }
            

        }

        public void startWave() // manage flow of waves
        {
            for (int i = 0; i < wavePlural[waveNum][1]; i++) // add mandated number of op enemies
            {
                game.planes.Add(new OpEnemy(new Vector2(800, 0), 1f));
            }
            for (int i = 0; i < wavePlural[waveNum][0]; i++)  // add mandated number of normal enemies
            {
                game.planes.Add(new Enemy(new Vector2(800, 0), 1f));
            }
            for (int i = 0; i < game.planes.Count; i++) // play shot noise
            {
                game.planes[i].loadSfx(game.sfxShot);
            }
            waveNum++; // increase wav num
        }
    }
}
