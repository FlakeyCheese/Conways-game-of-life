using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphing
{
    public partial class Form1 : Form
    {
        static int width = 100;
        static int height = 60;
        Button[,] grid = new Button[width, height];
        System.Drawing.Color gridColour = Color.Blue;
        bool rainbow = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            
            for(int i=0;i<width;i=i+1)
            {
                for (int j=0;j<height;j=j+1)
                {
                    grid[i, j] = new Button
                    {
                        Text = "",
                        Size = new Size(10, 10),
                        Location = new Point(y, x),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.White,
                         ForeColor = Color.LightGray
                    };
                    (grid[i, j]).Click += new EventHandler(anyButton_Click);
                    this.Controls.Add(grid[i,j]);
                    
                    x += 10;
                }
                y += 10;
                x = 0;
            }
             
        }

        private void button1_Click(object sender, EventArgs e)//start button
        {
            timer1.Enabled = true; 
        }

        void anyButton_Click(object sender, EventArgs e) //handlers for all the cell buttons
        {
            var button = (sender as Button);
            if (button.BackColor == gridColour)
            { button.BackColor = Color.White; }
            else
            { button.BackColor = gridColour; }
        }

        private void timer1_Tick(object sender, EventArgs e)//timer 
        {
            int count = Int32.Parse(label2.Text); 
            count++;
            label2.Text = count.ToString();
            update(); // call method to calculate new positions
        }
        private void update()
        {
            int[,] intGrid = new int[width, height];//count the neighbours and store them in intGrid
            for (int i = 0; i < width; i = i + 1)
            {
                for (int j = 0; j < height; j = j + 1)
                {
                    int countNeighbours = 0;
                    int xl=i-1;
                    if (xl == -1) { xl = width-1; }
                    int xr = i + 1;
                    if (xr == width) { xr = 0; }
                    int yd = j - 1;
                    if (yd == -1) { yd = height-1; }
                    int yu = j + 1; 
                    if (yu == height) { yu = 0; }
                    // count the neighbours
                    if (grid[xl, yu].BackColor!= Color.White) { countNeighbours++; }
                    if (grid[xl, j].BackColor != Color.White) { countNeighbours++; }
                    if (grid[xl, yd].BackColor != Color.White) { countNeighbours++; }
                    if (grid[i, yu].BackColor != Color.White) { countNeighbours++; }
                    if (grid[i, yd].BackColor != Color.White) { countNeighbours++; }
                    if (grid[xr, yu].BackColor != Color.White) { countNeighbours++; }
                    if (grid[xr, j].BackColor != Color.White) { countNeighbours++; }
                    if (grid[xr, yd].BackColor != Color.White) { countNeighbours++; }
                    intGrid[i, j] = countNeighbours;
                }
            }
            for (int i = 0; i < width; i = i + 1) //check the rules for each cell
            {
                for (int j = 0; j < height; j = j + 1)
                {
                    if (rainbow)
                    {
                        Random rnd = new Random();
                        Color randomColor = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));

                        gridColour = randomColor;
                    }
                    //1. Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                    //2. Any live cell with two or three live neighbours lives on to the next generation.
                    //3. Any live cell with more than three live neighbours dies, as if by overpopulation.
                    //4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                    if (intGrid[i, j] < 2 && (grid[i, j].BackColor != Color.White)) { grid[i, j].BackColor = Color.White; }// rule 1
                    if (intGrid[i, j] == 3 && (grid[i, j].BackColor == Color.White)) { grid[i, j].BackColor = gridColour; }// rule 4
                    if (intGrid[i, j] > 3 && (grid[i, j].BackColor != Color.White)) { grid[i, j].BackColor = Color.White; }//rule 3
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //stop button
        {
            timer1.Enabled = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) //adjust speed
        {
            timer1.Interval = (int)(numericUpDown1.Value);
        }

        private void button3_Click(object sender, EventArgs e)// choose colour
        {
            colorDialog1.ShowDialog();
            gridColour = colorDialog1.Color;
            for (int i = 0; i < width; i = i + 1)
            {
                for (int j = 0; j < height; j = j + 1)
                {
                    if (grid[i,j].BackColor != Color.White)
                    { grid[i, j].BackColor = gridColour; }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)//random colour
        {
            rainbow = !rainbow;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            for (int i = 0; i < width; i = i + 1)
            {
                for (int j = 0; j < height; j = j + 1)
                {
                    grid[i, j].BackColor = Color.White;
                    
                }
            }
        }
    }
}
