using System;
using System.IO;
using System.Collections.Generic;
using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+, 

namespace Battleship
{
    // Controls displaying and recording high score data, which is saved
    // to a file so that they can be viewed each time the game is loaded.
    public static class HighScoreController
    {

        private const int NAME_WIDTH = 3;
        private const int SCORES_LEFT = 490;

        // The score structure is used to keep the mame and score of
        // the top players together.
        private struct Score : IComparable
        {
            public string Name;
            public int Value;

            // Allows the scores to be compared to facilitate sorting
            public int CompareTo(object obj)
            {
                if (obj is Score)
                {
                    Score other = (Score)obj;
                    return other.Value - this.Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        private static List<Score> _Scores = new List<Score>();
        public static int Hahaha()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");
            StreamReader input;
            input = new StreamReader(filename);
            StreamReader input1;
            input1 = new StreamReader(filename);
            string temp;
            List<string> temp2 = new List<string>();
            while (input1.ReadLine()!= "")
            {
                temp = input.ReadLine();
                temp2.Add(temp);
            }
            
            
            return temp2.Count;
        }

        // Loads the scores from the highscores text file
        // private List<Score> _Scores = new List<Score>;
        private static void LoadScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");
            StreamReader input;
            input = new StreamReader(filename);
            StreamReader input1;
            input1 = new StreamReader(filename);
            StreamReader input2;
            input2 = new StreamReader(filename);
            string temp;
            List<string> temp2 = new List<string>();
            while (input1.ReadLine() != null)
            {
                temp = input.ReadLine();
                temp2.Add(temp);
            }

            // Reads in the number of scores
            int numScores = temp2.Count;
            _Scores.Clear();
            int i;
            for (i = 0; i < numScores; i++)
            {
                Score s;
                string line;
                line = input2.ReadLine();
                s.Name = line.Substring(0, line.IndexOf(":"));
                s.Value = Convert.ToInt32(line.Substring(line.IndexOf(":")+1));
                _Scores.Add(s);
            }

            input.Close();
            input1.Close();
            input2.Close();
        }

        // Saves the scores back into the highscores text file
        public static void SaveScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");
            StreamWriter output;
            output = new StreamWriter(filename);
            // output.WriteLine(_Scores.Count);
            foreach (Score s in _Scores)
            {
                output.WriteLine(s.Name +":" +s.Value);
            }

            output.Close();
        }

        // Draws the high scores onto the screen
        public static void DrawHighScores()
        {
            const int SCORES_HEADING = 40;
            const int SCORES_TOP = 80;
            const int SCORE_GAP = 30;
            if (_Scores.Count == 0)
                LoadScores();
            SwinGame.DrawText("   High Scores   ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_HEADING);
            int i;
            for (i = 0; i <= _Scores.Count - 1; i++)
            {
                Score s;
                s = _Scores[i];
                if (i < 9)
                {
                    SwinGame.DrawText(" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                }
                else
                {
                    SwinGame.DrawText(i + 1 + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                }
            }
        }

        // Handles the user input during the top score screen
        public static void HandleHighScoreInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.EscapeKey) || SwinGame.KeyTyped(KeyCode.ReturnKey))
            {
                GameController.EndCurrentState();
            }
        }

        // Reads the users name for their high score
        public static void ReadHighScore(int value)
        {
            const int ENTRY_TOP = 500;
            if (_Scores.Count == 0)
                LoadScores();
            if (value > _Scores[_Scores.Count - 1].Value)
            {
                Score s = new Score();
                s.Value = value;
                GameController.AddNewState(GameState.ViewingHighScores);
                int x;
                x = SCORES_LEFT + SwinGame.TextWidth(SwinGame.FontNamed("Courier"), "Name: ");
                SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameResources.GameFont("Courier"), x, ENTRY_TOP);
                while (SwinGame.ReadingText())
                {
                    SwinGame.ProcessEvents();
                    UtilityFunctions.DrawBackground();
                    DrawHighScores();
                    SwinGame.DrawText("Name: ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
                    SwinGame.RefreshScreen();
                }

                s.Name = SwinGame.TextReadAsASCII();
                if (s.Name.Length < 3)
                {
                    s.Name = s.Name + new string(Convert.ToChar(" "), 3 - s.Name.Length);
                }

                _Scores.RemoveAt(_Scores.Count - 1);
                _Scores.Add(s);
                _Scores.Sort();

                SaveScores();
                GameController.EndCurrentState();
            }
        }
    }
}