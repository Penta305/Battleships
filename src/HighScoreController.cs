using System.IO;
using SwinGameSDK;
using static SwinGameSDK.SwinGame; // requires mcs version 4+, 
// using SwinGameSDK.SwinGame; // requires mcs version 4+, 

namespace Battleship
{
    static class HighScoreController
    {
        private const int NAME_WIDTH = 3;
        private const int SCORES_LEFT = 490;

        private struct Score : IComparable
        {
            public string Name;
            public int Value;
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
        private void LoadScores()
        {
            string filename;
            filename = SwinGame.PathToResource("highscores.txt");
            StreamReader input;
            input = new StreamReader(filename);
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

        public void HandleHighScoreInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.EscapeKey) || SwinGame.KeyTyped(KeyCode.ReturnKey))
            {
                EndCurrentState();
            }
        }

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
//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by Refactoring Essentials.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
