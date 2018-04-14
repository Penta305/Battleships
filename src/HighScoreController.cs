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

    public class HighScoreController
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

            // CHECK
            // <param name="obj">the object to compare to</param>
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

        private List<Score> _Scores = new List<Score>;

        // Loads the scores from the highscores text file

        private void LoadScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");

            StreamReader input;
            input = new StreamReader(filename);

            // Reads in the number of scores
            int numScores;
            numScores = Convert.ToInt32(input.ReadLine());

            _Scores.Clear();

            int i;

            for (i = 1; i <= numScores; i++)
            {
                Score s;
                string line;
                line = input.ReadLine();
                s.Name = line.Substring(0, NAME_WIDTH);
                s.Value = Convert.ToInt32(line.Substring(NAME_WIDTH));
                _Scores.Add(s);
            }

            input.Close();
        }

        // Saves the scores back into the highscores text file

        private void SaveScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");

            StreamWriter output;
            output = new StreamWriter(filename);

            output.WriteLine(_Scores.Count);

            foreach (Score s in _Scores)
            {
                output.WriteLine(s.Name + s.Value);
            }

            output.Close();
        }

        // Draws the high scores onto the screen

        public void DrawHighScores()
        {
            const int SCORES_HEADING = 40;
            const int SCORES_TOP = 80;
            const int SCORE_GAP = 30;

            if (_Scores.Count == 0)
                LoadScores();

            SwinGame.DrawText("   High Scores   ", Color.White, GameFont("Courier"), SCORES_LEFT, SCORES_HEADING);

            int i;
            for (i = 0; i <= _Scores.Count - 1; i++)
            {
                Score s;
                s = _Scores.Item(i);

                if (i < 9)
                {
                    SwinGame.DrawText(" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                }
                else
                {
                    SwinGame.DrawText(i + 1 + ":   " + s.Name + "   " + s.Value, Color.White, GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                }
            }
        }

        // Handles the user input during the top score screen

        public void HandleHighScoreInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.VK_ESCAPE) || SwinGame.KeyTyped(KeyCode.VK_RETURN))
            {
                EndCurrentState();
            }
        }

        // Reads the users name for their high score
        
        public void ReadHighScore(int value)
        {
            const int ENTRY_TOP = 500;
            if (_Scores.Count == 0)
                LoadScores();
            if (value > _Scores.Item(_Scores.Count - 1).Value)
            {
                Score s = new Score;
                s.Value = value;
                AddNewState(GameState.ViewingHighScores);
                int x;
                x = SCORES_LEFT + SwinGame.TextWidth(GameFont("Courier"), "Name: ");
                SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameFont("Courier"), x, ENTRY_TOP);
                while (SwinGame.ReadingText())
                {
                    SwinGame.ProcessEvents();
                    DrawBackground();
                    DrawHighScores();
                    SwinGame.DrawText("Name: ", Color.White, GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
                    SwinGame.RefreshScreen();
                }

                s.Name = SwinGame.TextReadAsASCII();
                if (s.Name.Length < 3)
                {
                    s.Name = s.Name + new string((char)" ", 3 - s.Name.Length);
                }

                _Scores.RemoveAt(_Scores.Count - 1);
                _Scores.Add(s);
                _Scores.Sort();
                EndCurrentState();
            }
        }
    }
}
