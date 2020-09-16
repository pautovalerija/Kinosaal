using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
        StreamWriter to_file;
        Label[,] _arr = new Label[4, 4];
        Label[] rida = new Label[4];
        Button btn, btnk;
        bool ost = false;
        public string text;

        private void Form1_Load(object sender, EventArgs e)
        {
            string text = "";
            StreamWriter to_file;
            if (!File.Exists("Kino.txt"))
            {
                to_file = new StreamWriter("Kino.txt", false);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        text += i + "," + j + ",false;";
                    }
                    text += "\n";
                }
                to_file.Write(text);
                to_file.Close();
            }
            StreamReader from_file = new StreamReader("Kino.txt", false);
            string[] arr = from_file.ReadToEnd().Split('\n');
            from_file.Close();
            this.Size = new Size(300, 430);
            this.Text = "kino";


            for (int i = 0; i < 4; i++)
            {
                rida[i] = new Label();
                rida[i].Text = "Rida" + (i + 1);
                rida[i].Size = new Size(50, 50);

                rida[i].Location = new Point(1, i * 50);
                this.Controls.Add(rida[i]);

                for (int j = 0; j < 4; j++)
                {
                    _arr[i, j] = new Label();
                    string[] arv = arr[i].Split(';');
                    string[] ardNum = arv[j].Split(',');
                    if (ardNum[2] == "true")
                    {
                        _arr[i, j].BackColor = Color.Red;
                    }
                    else
                    {
                        _arr[i, j].BackColor = Color.LightGreen;
                    }
                    _arr[i, j].Text = "Koht" + (j + 1);
                    _arr[i, j].Size = new Size(50, 50);
                    _arr[i, j].BorderStyle = BorderStyle.Fixed3D;
                    _arr[i, j].Location = new Point(j * 50 + 50, i * 50);
                    this.Controls.Add(_arr[i, j]);
                    _arr[i, j].Tag = new int[] { i, j };
                    _arr[i, j].Click += new System.EventHandler(Form1_Click);
                }
            }
            btn = new Button();
            btn.Text = "Osta";
            btn.Location = new Point(176, 200);
            btn.Click += Btn_Click;
            this.Controls.Add(btn);
            btnk = new Button();
            btnk.Text = "Kinni";
            btnk.Location = new Point(1, 200);
            this.Controls.Add(btnk);
            btnk.Click += Btnk_Click; 
            
        }

        public void Btnk_Click(object sender, EventArgs e)
        {
            string text = "";
            to_file = new StreamWriter("Kino.txt", false);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_arr[i, j].BackColor == Color.LightYellow )
                    {
                        Btn_Click_Func();
                    }
                }
                
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_arr[i, j].BackColor == Color.Red)
                    {
                        text += i + "," + j + ",true;";
                    }
                    else
                    {
                        text += i + "," + j + ",false;";
                    }
                }
                text += "\n";
            }
            to_file.Write(text);
            to_file.Close();
            this.Close();




        }
        public void Btn_Click_Func()
        {
            DialogResult result = MessageBox.Show("Kas te olete kindel, et soovite osta pilet?", "Pileti ostamine",
            MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes) {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arr[i, j].BackColor == Color.LightYellow)
                        {
                            _arr[i, j].BackColor = Color.Red;
                        }
                    }
                }

            }
            if (result == DialogResult.No)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (_arr[i, j].BackColor == Color.LightYellow) 
                        {
                            _arr[i, j].Text = "Koht" + (j + 1);
                            _arr[i, j].BackColor = Color.LightGreen;
                            ost = false;
                        }
                            
                    }
                }
            }
            else
            {
                DialogResult result2 = MessageBox.Show("Kas te soovite piletid emaili saada?", "Pileti ostamine",
                MessageBoxButtons.YesNoCancel);
                if (result2 == DialogResult.Yes)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("vlrptv@gmail.com");
                    mail.To.Add("vlrptv@gmail.com");
                    mail.Subject = "Teie pilet. Kõik head!";
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].BackColor == Color.Red)
                            {
                                text += "Rida: " + (i + 1) + "; Koht: " + (j + 1) + "<br>";
                            }

                        }
                    }
                    mail.Body = text;
                    mail.IsBodyHtml = true;
                   

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("vlrptv@gmail.com", "duke1973");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    MessageBox.Show("Mail saadetud");

                }
                if (result2 == DialogResult.No)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].BackColor == Color.LightYellow)
                            {
                                _arr[i, j].Text = "Koht" + (j + 1);
                                _arr[i, j].BackColor = Color.LightGreen;
                                ost = false;
                            }

                        }
                    }
                }
            }
                
        }
        public void Btn_Click(object sender, EventArgs e)
        {
            Btn_Click_Func();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            var label = (Label)sender;
            var tag = (int[])label.Tag;
            if (_arr[tag[0], tag[1]].BackColor == Color.LightGreen)
            {
                _arr[tag[0], tag[1]].Text = "kinni";
                _arr[tag[0], tag[1]].BackColor = Color.LightYellow;
                ost = true;
                
            }
            if (_arr[tag[0], tag[1]].BackColor == Color.Red)
            
            {
                string message = "See koht juba ostatud";
                string title = "Error";
                MessageBox.Show(message, title);

            }

        }
    }
}
