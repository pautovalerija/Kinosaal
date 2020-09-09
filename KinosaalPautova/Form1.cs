using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KinosaalPautova
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Label[,] _arr = new Label[4, 4];
        Label[] rida = new Label[4];

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < 4; i++)
            {
                rida[i] = new Label();
                rida[i].Text = "Rida " +(i+1);
                rida[i].Size = new Size(50,50);
                rida[i].Location = new Point(1, i *50);
                this.Controls.Add(rida[i]);
                for (int j = 0; j < 4; j++)
                {
                    _arr[i, j] = new Label();
                    _arr[i, j].BackColor = Color.LightGray;
                    _arr[i, j].Text = " Koht"+(j+1);
                    _arr[i, j].Size = new Size(50,50);
                    _arr[i, j].Location = new Point(j*50+50,i*50);
                    this.Controls.Add(_arr[i, j]);

                }
            }
        }
    }
}
