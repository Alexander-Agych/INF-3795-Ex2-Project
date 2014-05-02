using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Threading;

namespace WindowsGame1
{
    public partial class Form1 : Form
    {
        //Meskerem:  create a new thread
        Thread gameThread = null;
       
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("Fullscreen");
            comboBox1.Items.Add("640 x 480");
            comboBox1.Items.Add("768 x 576");
            comboBox1.Items.Add("800 x 600");
            comboBox1.Items.Add("960 x 720");
            comboBox1.Items.Add("1024 x 768");
            comboBox1.Items.Add("1152 x 864");
            comboBox1.Items.Add("1280 x 960");
            comboBox1.Items.Add("1600 x 1200");

            comboBox2.Items.Add(0.1);
            comboBox2.Items.Add(0.3);
            comboBox2.Items.Add(0.5);
            comboBox2.Items.Add(1.0);
            comboBox2.Items.Add(1.5);
            comboBox2.Items.Add(2.0);
            comboBox2.Items.Add(2.5);
            comboBox2.Items.Add(3.0);
            comboBox2.Items.Add(3.5);
            comboBox2.Items.Add(4.0);
            comboBox2.Items.Add(4.5);
            comboBox2.Items.Add(5.0);
            comboBox2.Items.Add(5.5);
            comboBox2.Items.Add(6.0);
            comboBox2.Items.Add(7.0);
            comboBox2.Items.Add(8.0);
            comboBox2.Items.Add(9.0);
            comboBox2.Items.Add(10.0);
            comboBox2.Items.Add(12.0);
            comboBox2.Items.Add(15.0);

            comboBox3.Items.Add(50);
            comboBox3.Items.Add(100);
            comboBox3.Items.Add(150);
            comboBox3.Items.Add(200);
            comboBox3.Items.Add(250);
            comboBox3.Items.Add(300);
            comboBox3.Items.Add(350);
            comboBox3.Items.Add(400);
            comboBox3.Items.Add(450);
            comboBox3.Items.Add(500);
            comboBox3.Items.Add(550);
            comboBox3.Items.Add(600);
            comboBox3.Items.Add(700);
            comboBox3.Items.Add(800);
            comboBox3.Items.Add(900);
            comboBox3.Items.Add(1000);
            comboBox3.Items.Add(1200);
            comboBox3.Items.Add(1500);
            comboBox3.Items.Add(2000);
            comboBox3.Items.Add(2500);
            comboBox3.Items.Add(3000);

            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            comboBox1.SelectedItem = "Fullscreen";
            comboBox2.SelectedItem = 1.0;
            comboBox3.SelectedItem = 200;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Meskerem:  the new thread starts the game when start button clicked
            gameThread = new Thread(new ThreadStart(StartGame));
            gameThread.Start();
         }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Meskerem:  Add a background image to the usesr interface
            this.BackgroundImage = Image.FromFile(".\\Content\\Assets\\fruit_ninja_start.png");
        }

        public static void StartGame()
        {
            using (Game1 game = new Game1())
            {
                game.stopWatch.Start();//Starts the stopwatch when the game starts
                game.Run();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();// exit the game when Exit button clicked
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Text = " Welcome to The best Exergaming!";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState.Equals(1))
            {
                Settings1.Default.enableSounds = false;
            }
            else
            {
                Settings1.Default.enableSounds = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState.Equals(1))
            {
                Settings1.Default.enableMusic = false;
            }
            else
            {
                Settings1.Default.enableMusic = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState.Equals(1))
            {
                Settings1.Default.showColorCamera = false;
            }
            else
            {
                Settings1.Default.showColorCamera = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("Fullscreen"))
            {
                Settings1.Default.fullScreen = true;
            }
            else if (comboBox1.SelectedItem.Equals("640 x 480"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 640;
                Settings1.Default.resolutionY = 480;
            }
            else if (comboBox1.SelectedItem.Equals("768 x 576"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 768;
                Settings1.Default.resolutionY = 576;
            }
            else if (comboBox1.SelectedItem.Equals("800 x 600"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 800;
                Settings1.Default.resolutionY = 600;
            }
            else if (comboBox1.SelectedItem.Equals("960 x 720"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 960;
                Settings1.Default.resolutionY = 720;
            }
            else if (comboBox1.SelectedItem.Equals("1024 x 768"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 1024;
                Settings1.Default.resolutionY = 768;
            }
            else if (comboBox1.SelectedItem.Equals("1152 x 864"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 1152;
                Settings1.Default.resolutionY = 864;
            }
            else if (comboBox1.SelectedItem.Equals("1280 x 960"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 1280;
                Settings1.Default.resolutionY = 960;
            }
            else if (comboBox1.SelectedItem.Equals("1600 x 1200"))
            {
                Settings1.Default.fullScreen = false;
                Settings1.Default.resolutionX = 1600;
                Settings1.Default.resolutionY = 1200;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings1.Default.gameDuration = (double)comboBox2.SelectedItem;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings1.Default.minimumScore = (int)comboBox3.SelectedItem;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
     }
}
