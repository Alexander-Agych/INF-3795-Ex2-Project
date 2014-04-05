using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using C3.XNA;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 

    class GameObject
    {
        public float X_pos, Y_pos, X_speed, Y_speed, Radius;
        public Texture2D Asset;
        //public GameObject(float x_pos, float y_pos, float x_speed, float y_speed)
        //{
        //    X_pos = x_pos;
        //    Y_pos = y_pos;
        //    X_speed = x_speed;
        //    Y_speed = y_speed;
        //}
        public GameObject(int resolutionXtotal, int resolutionYtotal, Random rnd, Texture2D asset)
        //public void Init(int resolutionXtotal, int resolutionYtotal);
        {
            Radius = 80;
            Asset = asset;
            Y_pos = rnd.Next(resolutionYtotal / 2, resolutionYtotal + resolutionYtotal / 2);
            if (Y_pos > resolutionYtotal)
            {
                Y_pos = resolutionYtotal + Radius;
                //X_pos = rnd.Next(-50, resolutionXtotal + 50);
                for (X_pos = resolutionXtotal / 2; X_pos == resolutionXtotal / 2; X_pos = resolutionXtotal / 2 + (resolutionXtotal / 3) * rnd.Next(-1, 2));
                if (X_pos > resolutionXtotal / 2)
                {
                    X_speed = -rnd.Next(2, 4) - (float)(rnd.NextDouble());
                }
                else
                {
                    X_speed = rnd.Next(2, 4) + (float)(rnd.NextDouble());
                }
                Y_speed = -rnd.Next(11, 13) - (float)(rnd.NextDouble());
            }
            else
            {
                //Y_pos = resolutionYtotal - resolutionYtotal / 4;
                Y_pos = rnd.Next(resolutionYtotal / 4, resolutionYtotal - resolutionYtotal / 4);
                for (X_pos = resolutionXtotal / 2; X_pos == resolutionXtotal / 2; X_pos = resolutionXtotal / 2 + (resolutionXtotal / 2 + Radius) * rnd.Next(-1, 2)) ;
                if (X_pos > 0)
                {
                    X_speed = -rnd.Next(4, 6) - (float)(rnd.NextDouble());
                }
                else
                {
                    X_speed = rnd.Next(4, 6) + (float)(rnd.NextDouble());
                }
                if (Y_pos > resolutionYtotal / 2)
                {
                    Y_pos = resolutionYtotal - resolutionYtotal / 4;
                    Y_speed = -rnd.Next(9, 11) - (float)(rnd.NextDouble());
                }
                else
                {
                    Y_pos = resolutionYtotal / 4;
                    Y_speed = -rnd.Next(4, 6) - (float)(rnd.NextDouble());
                    X_speed *= 0.75f + (float)(rnd.NextDouble() / 2);
                }
                
            }
        }
    }


    class GameObjectCollection
    {
        public GameObject full, half1, half2;
        public GameObjectCollection(int resolutionXtotal, int resolutionYtotal, Random rnd, Texture2D asset)
        {
            full = new GameObject(resolutionXtotal, resolutionYtotal, rnd, asset);
            half1 = null;
            half2 = null;
        }
    }


    public class Game1 : Microsoft.Xna.Framework.Game
    {

        private GraphicsDeviceManager graphics;
        private Skeleton[] skeleton;
        private SpriteBatch spriteBatch;
        private RenderTarget2D cameraTexture;
        private Texture2D ninjaHead;
        private Texture2D[] fruits;
        private Texture2D background;
        private String[] BG_set, NH_set, FR_set;
        private byte[] colorFrameData;
        private Effect colorSwapEffect;
        private static Random rnd = new Random();

        // 4:3 resolutions: 640 x 480 || 768 x 576 || 800 x 600 || 960 x 720 || 1024 x 768
        // 4:3 resolutions: 1152 x 864 || 1280 x 960 || 1200 x 1600
        private int resolutionXtotal = 800, resolutionYtotal = 600;

        private float gravity = 0.15f;
        private float sceletonThickness = 5.0f;

        private KinectSensor sensor;

        private GameObjectCollection[] goc;

        private int maxNumGameObjects = 4;
        private int numGameObjects;

        public Game1()
        {

            // ALEXANDER // Retriving the connected Kinect-sensor
            if (KinectSensor.KinectSensors.Count == 0)
            {
                Console.WriteLine("No Kinect Connected!");
                Environment.Exit(0);
            }
            sensor = KinectSensor.KinectSensors[0];

            graphics = new GraphicsDeviceManager(this);
            skeleton = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];

            graphics.PreferredBackBufferWidth = resolutionXtotal;
            graphics.PreferredBackBufferHeight = resolutionYtotal;
            //graphics.ToggleFullScreen();

            Content.RootDirectory = "Content";

            BG_set = new String[3];
            BG_set[0] = ".\\Content\\Assets\\Backgrounds\\background1.jpg";
            BG_set[1] = ".\\Content\\Assets\\Backgrounds\\background2.jpg";
            BG_set[2] = ".\\Content\\Assets\\Backgrounds\\background3.jpg";
            NH_set = new String[2];
            NH_set[0] = ".\\Content\\Assets\\Ninja\\ninja1.png";
            NH_set[1] = ".\\Content\\Assets\\Ninja\\ninja2.png";
            FR_set = new String[6];
            FR_set[0] = ".\\Content\\Assets\\Fruits\\banana-hd.png";
            FR_set[1] = ".\\Content\\Assets\\Fruits\\bomb-hd.png";
            FR_set[2] = ".\\Content\\Assets\\Fruits\\grapes-hd.png";
            FR_set[3] = ".\\Content\\Assets\\Fruits\\pineapple-hd.png";
            FR_set[4] = ".\\Content\\Assets\\Fruits\\strawberry-hd.png";
            FR_set[5] = ".\\Content\\Assets\\Fruits\\watermelon-hd.png";

            // ALEXANDER // Ensure the sensor exists
            // ALEXANDER // Activating the video camera and setting it to return images in the format specified
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.SkeletonStream.Enable();
            sensor.Start();

            //Show the cursor
            IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cameraTexture = new RenderTarget2D(this.GraphicsDevice, 640, 480);
            colorSwapEffect = Content.Load<Effect>("ColorSwapEffect");

            //background = Content.Load<Texture2D>("Assets/Backgrounds/background1.jpg");
            background = Texture2D.FromStream(GraphicsDevice, File.OpenRead(BG_set[rnd.Next(0, 3)]));
            ninjaHead = Texture2D.FromStream(GraphicsDevice, File.OpenRead(NH_set[rnd.Next(0, 2)]));
            fruits = new Texture2D[6];
            for (int i = 0; i < 6; i++)
            {
                fruits[i] = Texture2D.FromStream(GraphicsDevice, File.OpenRead(FR_set[i]));
            }

            goc = new GameObjectCollection[maxNumGameObjects];
            RestartCollection();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            /*
            if (sensor != null)
            {
                sensor.Stop();
                sensor.Dispose();
            }
            */
            GC.Collect();
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (sensor != null)
                {
                    sensor.Stop();
                    sensor.Dispose();
                }
                this.Exit();
            }

            // TODO: Add your update logic here

            using (SkeletonFrame skeletonFrame = KinectSensor.KinectSensors[0].SkeletonStream.OpenNextFrame(0)) // Open the Skeleton frame
            {
                if (skeletonFrame != null && this.skeleton != null) // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeleton); // get the skeletal information in this frame
                }
            }

            using (var frame = KinectSensor.KinectSensors[0].ColorStream.OpenNextFrame(0))
            {
                if (frame != null)
                {
                    if (colorFrameData == null || colorFrameData.Length != frame.PixelDataLength)
                    {
                        colorFrameData = new byte[frame.PixelDataLength];
                    }

                    frame.CopyPixelDataTo(colorFrameData);
                    GraphicsDevice.Textures[0] = null;
                    cameraTexture.SetData<byte>(colorFrameData);
                }
            }

            for (int i = 0; i < numGameObjects; i++)
            {
                if (goc[i] != null)
                {
                    if (goc[i] != null && goc[i].full != null)
                    {
                        goc[i].full.Y_speed += gravity;
                        goc[i].full.X_pos += goc[i].full.X_speed;
                        goc[i].full.Y_pos += goc[i].full.Y_speed;
                        if (goc[i].full.Y_pos > resolutionYtotal + goc[i].full.Radius)
                        {
                            goc[i].full = null;
                        }
                    }
                    else if (goc[i].half1 != null && goc[i].half2 != null)
                    {
                        goc[i].half1.Y_speed += gravity;
                        goc[i].half1.X_pos += goc[i].half1.X_speed;
                        goc[i].half1.Y_pos += goc[i].half1.Y_speed;
                        goc[i].half2.Y_speed += gravity;
                        goc[i].half2.X_pos += goc[i].half2.X_speed;
                        goc[i].half2.Y_pos += goc[i].half2.Y_speed;
                        if (goc[i].half1.Y_pos > resolutionYtotal + goc[i].half1.Radius)
                        {
                            goc[i].half1 = null;
                        }
                        if (goc[i].half2.Y_pos > resolutionYtotal + goc[i].half2.Radius)
                        {
                            goc[i].half2 = null;
                        }
                    }   
                    else
                    {
                        goc[i] = null;
                    }
                }
                if(!CheckIfEmpty())
                {
                    RestartCollection();
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.SkyBlue);
            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, (float)resolutionYtotal / (float)background.Height, SpriteEffects.None, 0f);
            //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, colorSwapEffect);
            float colorFrameScale = 0.25f + 0.25f * (resolutionYtotal / 480.0f - 1);
            ColorImagePoint i;
            spriteBatch.Draw(cameraTexture, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, colorFrameScale, SpriteEffects.None, 0f);

            foreach (Skeleton sk in this.skeleton)
            {
                if (sk != null)
                {
                    if (sk.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        DrawTrackedSkeletonJoints(sk.Joints);
                    }
                    else if (sk.TrackingState == SkeletonTrackingState.PositionOnly)
                    {
                        i = SkeletonCoordinatesScaleAndTranslate(sk.Position.X, sk.Position.Y);
                        spriteBatch.DrawCircle(i.X - sceletonThickness / 2, i.Y, sceletonThickness / 2, (int)sceletonThickness, Color.Yellow, sceletonThickness);
                    }
                }
            }

            for (int a = 0; a < numGameObjects; a++)
            {
                if(goc[a] != null)
                {
                    if (goc[a].full != null)
                    {
                        float scale = (float)goc[a].full.Radius / (float)goc[a].full.Asset.Height;
                        Vector2 v = new Vector2(goc[a].full.X_pos - (float)goc[a].full.Asset.Width * scale / 2,
                            goc[a].full.Y_pos - (float)goc[a].full.Asset.Height * scale / 2);
                        //spriteBatch.DrawCircle(goc[a].full.X_pos, goc[a].full.Y_pos, goc[a].full.Radius, 20, Color.Black, 20);
                        spriteBatch.Draw(goc[a].full.Asset, v, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    }
                    if (goc[a].half1 != null)
                    {
                        spriteBatch.DrawCircle(goc[a].half1.X_pos, goc[a].half1.Y_pos, goc[a].half1.Radius / 2, 3, Color.Yellow, 20);
                    }
                    if (goc[a].half2 != null)
                    {
                        spriteBatch.DrawCircle(goc[a].half2.X_pos, goc[a].half2.Y_pos, goc[a].half2.Radius / 2, 3, Color.Yellow, 20);
                    }
                }
            }
            base.Draw(gameTime);
            spriteBatch.End();
        }

        // ALEXANDER: // For drawing a skeleton
        private void DrawTrackedSkeletonJoints(JointCollection jointCollection)
        {

            ColorImagePoint i;

            // Render Head and Shoulders
            DrawBone(jointCollection[JointType.Head], jointCollection[JointType.ShoulderCenter]);
            DrawBone(jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderLeft]);
            DrawBone(jointCollection[JointType.ShoulderCenter], jointCollection[JointType.ShoulderRight]);
            i = SkeletonCoordinatesScaleAndTranslate((jointCollection[JointType.Head]).Position.X, (jointCollection[JointType.Head]).Position.Y);
            //Vector2 v = new Vector2(i.X - sceletonThickness / 2, i.Y);
            float headscale = (float)ninjaHead.Height / (float)resolutionYtotal;
            Vector2 v = new Vector2(i.X - (float)ninjaHead.Width * headscale / 2, i.Y - (float)ninjaHead.Height * headscale / 2);
            //spriteBatch.DrawCircle(i.X - sceletonThickness / 2, i.Y, sceletonThickness * 4, (int)(sceletonThickness * 4), Color.White, sceletonThickness * 4);
            spriteBatch.Draw(ninjaHead, v, null, Color.White, 0f, Vector2.Zero, headscale, SpriteEffects.None, 0f);
            // Render Left Arm
            DrawBone(jointCollection[JointType.ShoulderLeft], jointCollection[JointType.ElbowLeft]);
            DrawBone(jointCollection[JointType.ElbowLeft], jointCollection[JointType.WristLeft]);
            //DrawBone(jointCollection[JointType.WristLeft], jointCollection[JointType.HandLeft]);

            // Render Right Arm
            DrawBone(jointCollection[JointType.ShoulderRight], jointCollection[JointType.ElbowRight]);
            DrawBone(jointCollection[JointType.ElbowRight], jointCollection[JointType.WristRight]);
            //DrawBone(jointCollection[JointType.WristRight], jointCollection[JointType.HandRight]);

            // Render other bones...

            // Spine & Hip
            DrawBone(jointCollection[JointType.ShoulderCenter], jointCollection[JointType.Spine]);
            DrawBone(jointCollection[JointType.Spine], jointCollection[JointType.HipCenter]);
            DrawBone(jointCollection[JointType.HipCenter], jointCollection[JointType.HipLeft]);
            DrawBone(jointCollection[JointType.HipCenter], jointCollection[JointType.HipRight]);

            // Left Leg
            DrawBone(jointCollection[JointType.HipLeft], jointCollection[JointType.KneeLeft]);
            DrawBone(jointCollection[JointType.KneeLeft], jointCollection[JointType.AnkleLeft]);
            DrawBone(jointCollection[JointType.AnkleLeft], jointCollection[JointType.FootLeft]);

            // Right Leg
            DrawBone(jointCollection[JointType.HipRight], jointCollection[JointType.KneeRight]);
            DrawBone(jointCollection[JointType.KneeRight], jointCollection[JointType.AnkleRight]);
            DrawBone(jointCollection[JointType.AnkleRight], jointCollection[JointType.FootRight]);

            
            foreach (Joint j in jointCollection)
            {
                if (!j.JointType.Equals(JointType.HandRight) && !j.JointType.Equals(JointType.HandLeft) &&
                    !j.JointType.Equals(JointType.WristRight) && !j.JointType.Equals(JointType.WristLeft))
                {
                    i = SkeletonCoordinatesScaleAndTranslate(j.Position.X, j.Position.Y);
                    if (j.TrackingState == JointTrackingState.Tracked)
                        spriteBatch.DrawCircle(i.X - sceletonThickness / 2, i.Y, sceletonThickness / 2, (int)sceletonThickness, Color.White, sceletonThickness);
                    else if (j.TrackingState == JointTrackingState.Inferred)
                        spriteBatch.DrawCircle(i.X - sceletonThickness / 2, i.Y, sceletonThickness / 2, (int)sceletonThickness, Color.White, sceletonThickness);
                }
            }

        }


        private void DrawBone(Joint jointFrom, Joint jointTo)
        {

            if (jointFrom.TrackingState == JointTrackingState.NotTracked ||
            jointTo.TrackingState == JointTrackingState.NotTracked)
            {
                return; // nothing to draw, one of the joints is not tracked
            }

            Color c;
            ColorImagePoint i1;
            ColorImagePoint i2;
            float thickness_factor = 1.0f;
            //float sx, sy;

            if (jointFrom.TrackingState == JointTrackingState.Inferred ||
            jointTo.TrackingState == JointTrackingState.Inferred)
            {
                // DrawNonTrackedBoneLine(jointFrom.Position, jointTo.Position);  // Draw thin lines if either one of the joints is inferred
                //spriteBatch.DrawLine(i1.X, i1.Y, i2.X, i2.Y, c, sceletonThickness / 2);
                thickness_factor = 0.75f;
            }

            if (jointFrom.TrackingState == JointTrackingState.Tracked &&
            jointTo.TrackingState == JointTrackingState.Tracked)
            {
                // DrawTrackedBoneLine(jointFrom.Position, jointTo.Position);  // Draw bold lines if the joints are both tracked
                //spriteBatch.DrawLine(i1.X, i1.Y, i2.X, i2.Y, c, sceletonThickness);
                //Console.WriteLine(x1);
                //Console.WriteLine(y1);
                //Console.WriteLine(x2);
                //Console.WriteLine(y1);
                thickness_factor = 1.0f;
            }

            i1 = SkeletonCoordinatesScaleAndTranslate(jointFrom.Position.X, jointFrom.Position.Y);
            i2 = SkeletonCoordinatesScaleAndTranslate(jointTo.Position.X, jointTo.Position.Y);

            //if ((jointFrom.JointType.Equals(JointType.ElbowLeft) && jointTo.JointType.Equals(JointType.WristLeft)) ||
            //    (jointFrom.JointType.Equals(JointType.ElbowRight) && jointTo.JointType.Equals(JointType.WristRight)) || 
            //    (jointFrom.JointType.Equals(JointType.WristLeft) && jointTo.JointType.Equals(JointType.HandLeft)) ||
            //    (jointFrom.JointType.Equals(JointType.WristRight) && jointTo.JointType.Equals(JointType.HandRight)))
            if ((jointFrom.JointType.Equals(JointType.ElbowLeft) && jointTo.JointType.Equals(JointType.WristLeft)) ||
                (jointFrom.JointType.Equals(JointType.ElbowRight) && jointTo.JointType.Equals(JointType.WristRight)))
            {
                c = Color.Red;

                /*
                sx = i2.X + i2.X - i1.X;
                if (sx < i1.X)
                    sx = i1.X + resolutionXtotal / 10;
                else
                    sx = i1.X - resolutionXtotal / 10;
                sy = i2.Y + i2.Y - i1.Y;
                if (sx < i1.Y)
                    sy = i1.Y + resolutionYtotal / 10;
                else
                    sy = i1.Y - resolutionYtotal / 10;
                spriteBatch.DrawLine(i1.X, i1.Y, sx, sy, c, sceletonThickness / 2 * thickness_factor);
                */

                spriteBatch.DrawLine(i1.X, i1.Y, i2.X + i2.X - i1.X, i2.Y + i2.Y - i1.Y, c, sceletonThickness / 2 * thickness_factor);
                
                for (int a = 0; a < numGameObjects; a++)
                {
                    if(goc[a] != null && goc[a].full != null)
                    {
                        float scale = (float)goc[a].full.Radius / (float)goc[a].full.Asset.Height;
                        if ((goc[a].full.X_pos + (float)goc[a].full.Asset.Width * scale / 10) > Math.Min(i1.X, i2.X + i2.X - i1.X) &&
                            (goc[a].full.X_pos - (float)goc[a].full.Asset.Width * scale / 10) < Math.Max(i1.X, i2.X + i2.X - i1.X) &&
                            (goc[a].full.Y_pos + (float)goc[a].full.Asset.Width * scale / 10) > Math.Min(i1.Y, i2.Y + i2.Y - i1.Y) &&
                            (goc[a].full.Y_pos - (float)goc[a].full.Asset.Width * scale / 10) < Math.Max(i1.Y, i2.Y + i2.Y - i1.Y))
                        {
                            BeginHalfs(goc[a]);
                            goc[a].full = null;
                        }
                    }
                }
            }
            else
            {
                c = Color.Green;
                spriteBatch.DrawLine(i1.X, i1.Y, i2.X, i2.Y, c, sceletonThickness / 2 * thickness_factor);
            }

            
        }

        /*
        private void RestartFruit(GameObjectCollection oc)
        {
            oc.full = new GameObject(resolutionXtotal, resolutionYtotal, rnd);
            GC.Collect();
        }
        */

        private void BeginHalfs(GameObjectCollection oc)
        {
            oc.half1 = new GameObject(resolutionXtotal, resolutionYtotal, rnd, null);
            oc.half1.X_pos = oc.full.X_pos;
            oc.half1.Y_pos = oc.full.Y_pos;
            oc.half1.X_speed = oc.full.X_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2) + 2;
            oc.half1.Y_speed = oc.full.Y_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2);
            oc.half1.Radius = oc.full.Radius / 2;
            oc.half2 = new GameObject(resolutionXtotal, resolutionYtotal, rnd, null);
            oc.half2.X_pos = oc.full.X_pos;
            oc.half2.Y_pos = oc.full.Y_pos;
            oc.half2.X_speed = oc.full.X_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2) - 2;
            oc.half2.Y_speed = oc.full.Y_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2);
            oc.half2.Radius = oc.full.Radius / 2;
            GC.Collect();
        }

        private Boolean CheckIfEmpty()
        {
            for (int i = 0; i < numGameObjects; i++)
            {
                if (goc[i] != null)
                {
                    return true;
                }
            }
            return false;
        }

        private void RestartCollection()
        {
            numGameObjects = rnd.Next(1, maxNumGameObjects + 1);
            for (int i = 0; i < numGameObjects; i++)
            {
                goc[i] = new GameObjectCollection(resolutionXtotal, resolutionYtotal, rnd, fruits[rnd.Next(0, 6)]);
            }
            GC.Collect();
        }

        private ColorImagePoint SkeletonCoordinatesScaleAndTranslate(float x, float y)
        {
            SkeletonPoint p = new SkeletonPoint();
            p.X = x;
            p.Y = y;
            p.Z = 2.5f;
            ColorImagePoint i = this.sensor.CoordinateMapper.MapSkeletonPointToColorPoint(p, ColorImageFormat.RgbResolution640x480Fps30);
            i.X = i.X * resolutionYtotal / 480;
            i.Y = i.Y * resolutionYtotal / 480;
            return i;
        }
    }
}
