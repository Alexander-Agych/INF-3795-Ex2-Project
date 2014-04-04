using System;
using System.Collections.Generic;
using System.Linq;
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
        //public GameObject(float x_pos, float y_pos, float x_speed, float y_speed)
        //{
        //    X_pos = x_pos;
        //    Y_pos = y_pos;
        //    X_speed = x_speed;
        //    Y_speed = y_speed;
        //}
        public GameObject(int resolutionXtotal, int resolutionYtotal, Random rnd)
        //public void Init(int resolutionXtotal, int resolutionYtotal);
        {
            Radius = 20;
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
                Y_speed = -rnd.Next(12, 14) - (float)(rnd.NextDouble());
            }
            else
            {
                //Y_pos = resolutionYtotal - resolutionYtotal / 4;
                Y_pos = rnd.Next(resolutionYtotal / 4, resolutionYtotal - resolutionYtotal / 4);
                for (X_pos = resolutionXtotal / 2; X_pos == resolutionXtotal / 2; X_pos = resolutionXtotal / 2 + (resolutionXtotal / 2 + Radius) * rnd.Next(-1, 2)) ;
                if (X_pos > 0)
                {
                    X_speed = -rnd.Next(3, 6) - (float)(rnd.NextDouble());
                }
                else
                {
                    X_speed = rnd.Next(3, 6) + (float)(rnd.NextDouble());
                }
                if (Y_pos > resolutionYtotal / 2)
                {
                    Y_pos = resolutionYtotal - resolutionYtotal / 4;
                    Y_speed = -rnd.Next(9, 11) - (float)(rnd.NextDouble());
                }
                else
                {
                    Y_pos = resolutionYtotal / 4;
                    Y_speed = -rnd.Next(1, 4);
                    X_speed *= (float)(1 + rnd.NextDouble() / 2);
                }
                
            }
        }
    }


    public class Game1 : Microsoft.Xna.Framework.Game
    {

        private GraphicsDeviceManager graphics;
        private Skeleton[] skeleton;
        private SpriteBatch spriteBatch;
        private RenderTarget2D cameraTexture;
        private byte[] colorFrameData;
        private Effect colorSwapEffect;
        private static Random rnd = new Random();

        // 4:3 resolutions: 640 x 480 || 768 x 576 || 800 x 600 || 960 x 720 || 1024 x 768
        // 4:3 resolutions: 1152 x 864 || 1280 x 960 || 1200 x 1600
        private int resolutionXtotal = 800, resolutionYtotal = 600;

        private float gravity = 0.15f;
        private float sceletonThickness = 5.0f;

        // ALEXANDER // Retriving the connected Kinect-sensor
        KinectSensor sensor;

        GameObject gameObject, half1, half2;       

        public Game1()
        {

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            spriteBatch = new SpriteBatch(GraphicsDevice);
            cameraTexture = new RenderTarget2D(this.GraphicsDevice, 640, 480);
            colorSwapEffect = Content.Load<Effect>("ColorSwapEffect");
            //gameObject = new GameObject(-20.0f, resolutionYtotal / 2, 7.0f, -10.0f);
            RestartFruit();
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

            gameObject.Y_speed += gravity;
            gameObject.X_pos += gameObject.X_speed;
            gameObject.Y_pos += gameObject.Y_speed;
            if(half1 != null && half2 != null)
            {
                half1.Y_speed += gravity;
                half1.X_pos += half1.X_speed;
                half1.Y_pos += half1.Y_speed;
                half2.Y_speed += gravity;
                half2.X_pos += half2.X_speed;
                half2.Y_pos += half2.Y_speed;
                if (half1.Y_pos > resolutionYtotal + half1.Radius)
                {
                    half1 = null;
                }
                if (half2.Y_pos > resolutionYtotal + half2.Radius)
                {
                    half2 = null;
                }
            }
            if (gameObject.Y_pos > resolutionYtotal + gameObject.Radius)
            {
                RestartFruit();
            }
                

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, colorSwapEffect);
            spriteBatch.Begin();
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

            spriteBatch.DrawCircle(gameObject.X_pos, gameObject.Y_pos, gameObject.Radius, 20, Color.Black, 20);
            if(half1 != null && half2 != null)
            {
                spriteBatch.DrawCircle(half1.X_pos, half1.Y_pos, half1.Radius, 3, Color.Yellow, 20);
                spriteBatch.DrawCircle(half2.X_pos, half2.Y_pos, half2.Radius, 3, Color.Yellow, 20);
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
            spriteBatch.DrawCircle(i.X - sceletonThickness / 2, i.Y, sceletonThickness * 4, (int)(sceletonThickness * 4), Color.White, sceletonThickness * 4);

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
                spriteBatch.DrawLine(i1.X, i1.Y, i2.X + i2.X - i1.X, i2.Y + i2.Y - i1.Y, c, sceletonThickness / 2 * thickness_factor);
                if((gameObject.X_pos) > Math.Min(i1.X, i2.X + i2.X - i1.X) &&
                    (gameObject.X_pos) < Math.Max(i1.X, i2.X + i2.X - i1.X) &&
                    (gameObject.Y_pos) > Math.Min(i1.Y, i2.Y + i2.Y - i1.Y) &&
                    (gameObject.Y_pos) < Math.Max(i1.Y, i2.Y + i2.Y - i1.Y))
                {
                    RestartHalfs();
                    RestartFruit();
                }
            }
            else
            {
                c = Color.Green;
                spriteBatch.DrawLine(i1.X, i1.Y, i2.X, i2.Y, c, sceletonThickness / 2 * thickness_factor);
            }

            
        }

        private void RestartFruit()
        {
            gameObject = new GameObject(resolutionXtotal, resolutionYtotal, rnd);
            GC.Collect();
        }

        private void RestartHalfs()
        {
            half1 = new GameObject(resolutionXtotal, resolutionYtotal, rnd);
            half1.X_pos = gameObject.X_pos;
            half1.Y_pos = gameObject.Y_pos;
            half1.X_speed = gameObject.X_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2) + 2;
            half1.Y_speed = gameObject.Y_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2);
            half1.Radius = gameObject.Radius / 2;
            half2 = new GameObject(resolutionXtotal, resolutionYtotal, rnd);
            half2.X_pos = gameObject.X_pos;
            half2.Y_pos = gameObject.Y_pos;
            half2.X_speed = gameObject.X_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2) - 2;
            half2.Y_speed = gameObject.Y_speed + (float)(rnd.NextDouble()) * rnd.Next(-1, 2);
            half2.Radius = gameObject.Radius / 2;
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
