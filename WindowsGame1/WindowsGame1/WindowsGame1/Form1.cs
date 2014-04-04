using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect;

namespace WindowsGame1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
