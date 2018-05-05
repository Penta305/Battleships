using SwinGameSDK;
using System.Collections.Generic;

namespace Battleship
{
    public static class GameResources {

        private static void LoadFonts() {
            SwinGame.LoadFontNamed("ArialLarge", "arial.ttf", 80);
            SwinGame.LoadFontNamed("Courier", "cour.ttf", 14);
            SwinGame.LoadFontNamed("CourierSmall", "cour.ttf", 8);
            SwinGame.LoadFontNamed("Menu", "ffaccess.ttf", 8);
        }

        private static void LoadImages() {
            // Backgrounds
            SwinGame.LoadBitmapNamed("Menu", "main_page.jpg");
            SwinGame.LoadBitmapNamed("Discovery", "discover.jpg");
            SwinGame.LoadBitmapNamed("Deploy", "deploy.jpg");
            // Deployment
            SwinGame.LoadBitmapNamed("LeftRightButton", "deploy_dir_button_horiz.png");
            SwinGame.LoadBitmapNamed("UpDownButton", "deploy_dir_button_vert.png");
            SwinGame.LoadBitmapNamed("SelectedShip", "deploy_button_hl.png");
            SwinGame.LoadBitmapNamed("PlayButton", "deploy_play_button.png");
            SwinGame.LoadBitmapNamed("RandomButton", "deploy_randomize_button.png");
            // Ships
            int i;
            for (i = 1; (i <= 5); i++) {
                SwinGame.LoadBitmapNamed(("ShipLR" + i), ("ship_deploy_horiz_"
                                + (i + ".png")));
                SwinGame.LoadBitmapNamed(("ShipUD" + i), ("ship_deploy_vert_"
                                + (i + ".png")));
            }

            // Explosions
            SwinGame.LoadBitmapNamed("Explosion", "explosion.png");
            SwinGame.LoadBitmapNamed("Splash", "splash.png");
        }

        private static void LoadSounds() {
            SwinGame.LoadSoundEffectNamed("Error", "error.wav");
            SwinGame.LoadSoundEffectNamed("Hit", "hit.wav");
            SwinGame.LoadSoundEffectNamed("Sink", "sink.wav");
            SwinGame.LoadSoundEffectNamed("Siren", "siren.wav");
            SwinGame.LoadSoundEffectNamed("Miss", "watershot.wav");
            SwinGame.LoadSoundEffectNamed("Winner", "winner.wav");
            SwinGame.LoadSoundEffectNamed("Lose", "lose.wav");
            SwinGame.LoadSoundEffectNamed("Easy", "easy.wav");
            SwinGame.LoadSoundEffectNamed("Medium", "medium.wav");
            SwinGame.LoadSoundEffectNamed("Hard", "hard.wav");
        }

        private static void LoadMusic() {
            SwinGame.LoadMusicNamed("Background", "moo.ogg");
        }

        // Gets a Font currently loaded in the Resources
        public static Font GameFont(string font) {
            //return _Fonts(font);
            return SwinGame.FontNamed(font);
        }

        // Gets an Image currently loaded in the Resources
        public static Bitmap GameImage(string image) {
            //return _Images(image);
            return SwinGame.BitmapNamed(image);
        }

        // Gets an sound currently loaded in the Resources
        public static SoundEffect GameSound(string sound) {
            //return _Sounds(sound);
            return SwinGame.SoundEffectNamed(sound);
        }

        // Gets the music loaded in the Resources
        public static Music GameMusic(string music) {
            return SwinGame.MusicNamed(music);
            //return _Music(music);
        }

        private static Dictionary<string, Bitmap> _Images = new Dictionary<string, Bitmap>();

        private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();

        private static Dictionary<string, SoundEffect> _Sounds = new Dictionary<string, SoundEffect>();

        private static Dictionary<string, Music> _Music = new Dictionary<string, Music>();

        private static Bitmap _Background;

        private static Bitmap _Animation;

        private static Bitmap _LoaderFull;

        private static Bitmap _LoaderEmpty;

        private static Font _LoadingFont;

        private static SoundEffect _StartSound;

        // The Resources Class stores all of the games media resources, such as images,
        // fonts, sounds, and music.
        public static void LoadResources() {
            int width;
            int height;
            width = SwinGame.ScreenWidth();
            height = SwinGame.ScreenHeight();
            SwinGame.ChangeScreenSize(800, 600);
            GameResources.ShowLoadingScreen();
            GameResources.ShowMessage("Loading fonts...", 0);
            GameResources.LoadFonts();
            SwinGame.Delay(100);
            GameResources.ShowMessage("Loading images...", 1);
            GameResources.LoadImages();
            SwinGame.Delay(100);
            GameResources.ShowMessage("Loading sounds...", 2);
            GameResources.LoadSounds();
            SwinGame.Delay(100);
            GameResources.ShowMessage("Loading music...", 3);
            GameResources.LoadMusic();
            SwinGame.Delay(100);
            SwinGame.Delay(100);
            GameResources.ShowMessage("Game loaded...", 5);
            SwinGame.Delay(100);
            GameResources.EndLoadingScreen(width, height);
        }

        private static void ShowLoadingScreen() {
            SwinGame.LoadBitmapNamed("loadBackground", SwinGame.PathToResource("SplashBack.png", ResourceKind.BitmapResource));
            SwinGame.DrawBitmap("loadBackground", 0, 0);

            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();

            _Animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
            SwinGame.LoadBitmapNamed("loadBitmap", SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
            //SwinGame.Animation

            SwinGame.LoadFontNamed("LoadingFont", "arial.ttf", 12);
            SwinGame.LoadSoundEffectNamed("StartSound", "SwinGameStart.ogg");

            SwinGame.LoadBitmapNamed("LoaderFull", "loader_full.png");
            SwinGame.LoadBitmapNamed("LoaderEmpty", "loader_empty.png");

            GameResources.PlaySwinGameIntro();
        }

        private static void PlaySwinGameIntro() {
            const int ANI_CELL_COUNT = 11;
            Audio.PlaySoundEffect("StartSound");
            SwinGame.Delay(200);
            int i;
            for (i = 0; (i
                        <= (ANI_CELL_COUNT - 1)); i++) {
                SwinGame.DrawBitmap("loadBackground", 0, 0);
                SwinGame.Delay(20);
                SwinGame.RefreshScreen();
                SwinGame.ProcessEvents();
            }

            SwinGame.Delay(1500);
        }

        private static void ShowMessage(string message, int number) {
            const int BG_Y = 453;
            int TX = 310;
            int TY = 493;
            int TW = 200;
            int TH = 25;
            int STEPS = 5;
            int BG_X = 279;
            int fullW;
            Rectangle toDraw = new Rectangle();
            fullW = (260 * number / STEPS);
            SwinGame.DrawBitmap(SwinGame.BitmapNamed("LoaderEmpty"), BG_X, BG_Y);
            //SwinGame.DrawCell(SwinGame.BitmapNamed("LoaderFull"), 0, BG_X, BG_Y);
            //SwinGame.DrawBitmap(SwinGame.BitmapNamed("LoaderFull"), 0, 0, fullW, 66, BG_X, BG_Y);
            /*
            DrawingOptions opt = new DrawingOptions();
            //opt = DrawingOptionsConfiguration.OptionPartBmp(BG_X, BG_Y, fullW, 200);
            //DrawingOptionsConfiguration.OptionPartBmp(BG_X, BG_Y, fullW, 200);
            Rectangle asdf = new Rectangle
            {
                X = BG_X,
                Y = BG_Y,
                Height = 200,
                Width = fullW
            };
            opt.Part = asdf;


            SwinGame.DrawBitmap("LoaderFull", BG_X, BG_Y, opt);
            */
            // Silly Implementation

            Rectangle easyLoad = new Rectangle
            {
                X = BG_X + 20,
                Y = BG_Y + 20,
                Height = 15,
                Width = (200 * number / STEPS)
            };
            SwinGame.FillRectangle(Color.Pink, easyLoad);
            SwinGame.Delay(200);

            toDraw.X = TX;
            toDraw.Y = TY;
            toDraw.Width = TW;
            toDraw.Height = TH;
            SwinGame.DrawText(message, Color.White, Color.Transparent, "LoadingFont", FontAlignment.AlignCenter, toDraw);
            //SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _LoadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH);
            SwinGame.RefreshScreen();
            SwinGame.ProcessEvents();
        }

        private static void EndLoadingScreen(int width, int height) {
            SwinGame.ProcessEvents();
            SwinGame.Delay(500);
            SwinGame.ClearScreen();
            SwinGame.RefreshScreen();

            SwinGame.FreeFont(SwinGame.FontNamed("LoadingFont"));
            SwinGame.FreeBitmap(SwinGame.BitmapNamed("LoadingBackground"));
            SwinGame.FreeBitmap(_Animation);
            SwinGame.FreeBitmap(_LoaderEmpty);
            SwinGame.FreeBitmap(_LoaderFull);
            Audio.FreeSoundEffect(_StartSound);

            SwinGame.ChangeScreenSize(width, height);
        }

        private static void NewFont(string fontName, string filename, int size) {
            _Fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));

        }

        private static void NewImage(string imageName, string filename) {
            _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));
        }

        private static void NewTransparentColorImage(string imageName, string fileName, Color transColor) {
            _Images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource)));
        }

        private static void NewTransparentColourImage(string imageName, string fileName, Color transColor) {
            GameResources.NewTransparentColorImage(imageName, fileName, transColor);
        }

        private static void NewSound(string soundName, string filename) {
            _Sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
        }

        private static void NewMusic(string musicName, string filename) {
            _Music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
        }

        private static void FreeFonts() {
            foreach (Font obj in _Fonts.Values) {
                SwinGame.FreeFont(obj);
            }

        }

        private static void FreeImages() {
            foreach (Bitmap obj in _Images.Values) {
                SwinGame.FreeBitmap(obj);
            }

        }

        private static void FreeSounds() {
            foreach (SoundEffect obj in _Sounds.Values) {
                Audio.FreeSoundEffect(obj);
            }

        }

        private static void FreeMusic() {
            foreach (Music obj in _Music.Values) {
                Audio.FreeMusic(obj);
            }

        }

        public static void FreeResources() {
            SwinGame.ReleaseAllResources();
            SwinGame.ProcessEvents();
        }
    }
}