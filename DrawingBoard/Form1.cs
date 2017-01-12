using DrawingBoard.CalculateGeometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingBoard
{
    public partial class Form1 : Form
    {
        FigureManager figureManager;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            figureManager = new FigureManager(pictureBox1);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            figureManager.MouseDown(e.X, e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            figureManager.MouseMove(e.X, e.Y);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            figureManager.MouseUp(e.X, e.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            figureManager.CurrentCreateMode = FigureManager.CreateMode.Point;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            figureManager.CurrentCreateMode = FigureManager.CreateMode.Nothing;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            figureManager.CurrentCreateMode = FigureManager.CreateMode.Line;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string message = "共有" + figureManager.Figures.Count + "个元素";
            foreach (Figure figure in figureManager.Figures)
            {
                message += Environment.NewLine + 
                    figure.ToString();
            }

            MessageBox.Show(message);
            Console.WriteLine(message);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            figureManager.CurrentCreateMode = FigureManager.CreateMode.Circle;
        }
    }
}
