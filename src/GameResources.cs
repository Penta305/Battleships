using SwinGameSDK;
using System.Collections.Generic;

namespace Battleship
{
    public class GameResources
    {
        private Dictionary<string, Bitmap> _Images = new Dictionary<string, Bitmap>();

        private Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();

        private Dictionary<string, SoundEffect> _Sounds = new Dictionary<string, SoundEffect>();

        private Dictionary<string, Music> _Music = new Dictionary<string, Music>();

        private Bitmap _Background;

        private Bitmap _Animation;

        private Bitmap _LoaderFull;

        private Bitmap _LoaderEmpty;

        private Font _LoadingFont;

        private SoundEffect _StartSound;
        private static void LoadFonts()
        {
            NewFont("ArialLarge", "arial.ttf", 80);
            NewFont("Courier", "cour.ttf", 14);
            NewFont("CourierSmall", "cour.ttf", 8);
            NewFont("Menu", "ffaccess.ttf", 8);
        }

        private static void LoadImages()
        {

            // Backgrounds
            NewImage("Menu", "main_page.jpg");
            NewImage("Discovery", "discover.jpg");
            NewImage("Deploy", "deploy.jpg");

            // Deployment
            NewImage("LeftRightButton", "deploy_dir_button_horiz.png");
            NewImage("UpDownButton", "deploy_dir_button_vert.png");
            NewImage("SelectedShip", "deploy_button_hl.png");
            NewImage("PlayButton", "deploy_play_button.png");
            NewImage("RandomButton", "deploy_randomize_button.png");

            // Ships
            int i;
            for (i = 1; (i <= 5); i++)
            {
                NewImage(("ShipLR" + i), ("ship_deploy_horiz_"
                                + (i + ".png")));
                NewImage(("ShipUD" + i), ("ship_deploy_vert_"
                                + (i + ".png")));
            }

            // Explosions
            NewImage("Explosion", "explosion.png");
            NewImage("Splash", "splash.png");
        }

        private static void LoadSounds()
        {
            NewSound("Error", "error.wav");
            NewSound("Hit", "hit.wav");
            NewSound("Sink", "sink.wav");
            NewSound("Siren", "siren.wav");
            NewSound("Miss", "watershot.wav");
            NewSound("Winner", "winner.wav");
            NewSound("Lose", "lose.wav");
        }

        private static void LoadMusic()
        {
            NewMusic("Background", "horrordrone.mp3");
        }

        // Gets a Font currently loaded in the Resources

        // CHECK
        // <param name="font">Name of Font</param>
        public static Font GameFont(string font)
        {
            GameResources _Fonts = new GameResources();
            return _Fonts._Fonts[font];
        }

        // Gets an Image currently loaded in the Resources

        // CHECK
        // <param name="image">Name of image</param>
        public static Bitmap GameImage(string image)
        {
            GameResources _Images = new GameResources();
            return _Images._Images[image];
        }

        // Gets an sound currently loaded in the Resources

        // CHECK
        // <param name="sound">Name of sound</param>
        public static SoundEffect GameSound(string sound)
        {
            GameResources _Sounds = new GameResources();
            return _Sounds._Sounds[sound];
        }

        // '' Gets the music currently loaded in the Resources

        // CHECK
        // '' <param name="music">Name of music</param>
        public static Music GameMusic(string music)
        {
            GameResources _Music = new GameResources();
            return _Music._Music[music];
        }



        // The Resources Class stores all of the games media resources, such as images,
        // fonts, sounds, and music.

        public static void LoadResources()
        {
            int width;
            int height;
            width = SwinGame.ScreenWidth();
            height = SwinGame.ScreenHeight();
            SwinGame.ChangeScreenSize(800, 600);
            ShowLoadingScreen();
            ShowMessage("Loading fonts...", 0);
            LoadFonts();
            SwinGame.Delay(100);
            ShowMessage("Loading images...", 1);
            LoadImages();
            SwinGame.Delay(100);
            ShowMessage("Loading sounds...", 2);
            LoadSounds();
            SwinGame.Delay(100);
            ShowMessage("Loading music...", 3);
            LoadMusic();
            SwinGame.Delay(100);
            SwinGame.Delay(100);
            ShowMessage("Game loaded...", 5);
            SwinGame.Delay(100);
            EndLoadingScreen(width, height);
        }

        private static void ShowLoadingScreen()
        {
            GameResources _LoadingScreen = new GameResources();

            _LoadingScreen._Background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack.png", ResourceKind.BitmapResource));
            SwinGame.DrawBitmap(_LoadingScreen._Background, 0, 0);
            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();
            _LoadingScreen._Animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
            _LoadingScreen._LoadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);
            _LoadingScreen._StartSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.ogg", ResourceKind.SoundResource));
            _LoadingScreen._LoaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));
            _LoadingScreen._LoaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));
            GameResources.PlaySwinGameIntro();
        }

        private static void PlaySwinGameIntro()
        {
            GameResources _PlaySwinGameIntro = new GameResources();
            const int ANI_CELL_COUNT = 11;
            Audio.PlaySoundEffect(_PlaySwinGameIntro._StartSound);
            SwinGame.Delay(200);
            int i;
            for (i = 0; (i
                        <= (ANI_CELL_COUNT - 1)); i++)
            {
                SwinGame.DrawBitmap(_PlaySwinGameIntro._Background, 0, 0);
                SwinGame.Delay(20);
                SwinGame.RefreshScreen();
                SwinGame.ProcessEvents();
            }

            SwinGame.Delay(1500);
        }

        private static void ShowMessage(string message, int number)
        {
            GameResources _ShowMessage = new GameResources();
            const int BG_Y = 453;
            int TX = 310;
            int TY = 493;
            int TW = 200;
            int TH = 25;
            int STEPS = 5;
            int BG_X = 279;
            int fullW;

            // Rectangle toDraw;
            // fullW = (260 * number);
            // STEPS;
            // SwinGame.DrawBitmap(_LoaderEmpty, BG_X, BG_Y);
            // SwinGame.DrawCell(_LoaderFull, 0, BG_X, BG_Y);
            // CHECK

            Rectangle toDraw = new Rectangle();
            fullW = (260 * number) / STEPS;

            SwinGame.DrawBitmap(_ShowMessage._LoaderEmpty, BG_X, BG_Y);
            SwinGame.DrawCell(_ShowMessage._LoaderFull, 0, BG_X, BG_Y);

            //  SwinGame.DrawBitmapPart(_LoaderFull, 0, 0, fullW, 66, BG_X, BG_Y)
            toDraw.X = TX;
            toDraw.Y = TY;
            toDraw.Width = TW;
            toDraw.Height = TH;

            //SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, toDraw);


            SwinGame.DrawText(message, Color.White, Color.Transparent, _ShowMessage._LoadingFont, FontAlignment.AlignCenter, toDraw);
            //SwinGame.DrawText(message, Color.White, Color.Transparent, _ShowMessage._LoadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH);

            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();
        }

        private static void EndLoadingScreen(int width, int height)
        {
            GameResources _EndLoadingScreen = new GameResources();
            SwinGame.ProcessEvents();
            SwinGame.Delay(500);
            SwinGame.ClearScreen();
            SwinGame.RefreshScreen();
            SwinGame.FreeFont(_EndLoadingScreen._LoadingFont);
            SwinGame.FreeBitmap(_EndLoadingScreen._Background);
            SwinGame.FreeBitmap(_EndLoadingScreen._Animation);
            SwinGame.FreeBitmap(_EndLoadingScreen._LoaderEmpty);
            SwinGame.FreeBitmap(_EndLoadingScreen._LoaderFull);
            Audio.FreeSoundEffect(_EndLoadingScreen._StartSound);
            SwinGame.ChangeScreenSize(width, height);
        }

        private static void NewFont(string fontName, string filename, int size)
        {
            GameResources _Fonts = new GameResources();
            _Fonts._Fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));
        }

        private static void NewImage(string imageName, string filename)
        {
            GameResources _NewImage = new GameResources();
            _NewImage._Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));
        }

        private static void NewTransparentColorImage(string imageName, string fileName, Color transColor)
        {
            GameResources _NewTransparentColorImage = new GameResources();
            _NewTransparentColorImage._Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource)));
        }


        private static void NewSound(string soundName, string filename)
        {
            GameResources _Sounds = new GameResources();
            _Sounds._Sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
        }

        private static void NewMusic(string musicName, string filename)
        {
            GameResources _NewMusic = new GameResources();
            _NewMusic._Music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
        }

        private static void FreeFonts()
        {
            GameResources _FreeFonts = new GameResources();
            foreach (Font obj in _FreeFonts._Fonts.Values)
            {
                SwinGame.FreeFont(obj);
            }

        }

        private static void FreeImages()
        {
            GameResources _FreeImages = new GameResources();
            foreach (Bitmap obj in _FreeImages._Images.Values)
            {
                SwinGame.FreeBitmap(obj);
            }

        }

        private static void FreeSounds()
        {
            GameResources _FreeSounds = new GameResources();
            foreach (SoundEffect obj in _FreeSounds._Sounds.Values)
            {
                Audio.FreeSoundEffect(obj);
            }

        }

        private static void FreeMusic()
        {
            GameResources _FreeMusic = new GameResources();
            foreach (Music obj in _FreeMusic._Music.Values)
            {
                Audio.FreeMusic(obj);
            }

        }

        public static void FreeResources()
        {
            FreeFonts();
            FreeImages();
            FreeMusic();
            FreeSounds();
            SwinGame.ProcessEvents();
        }
    }
}