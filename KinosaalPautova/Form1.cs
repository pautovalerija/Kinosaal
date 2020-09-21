using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ContentAlignment = System.Drawing.ContentAlignment;

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
        Image imgred = Image.FromFile("red.png");
        Image imggreen = Image.FromFile("green.png");
        Image imgyellow = Image.FromFile("yellow.png");


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
            this.Size = new Size(600, 430);
            this.Text = "kino";


            for (int i = 0; i < 4; i++)
            {
                rida[i] = new Label();
                rida[i].AutoSize = false;
                rida[i].Font = new Font("Tobota", 14, FontStyle.Bold);
                rida[i].ForeColor = Color.White;
                rida[i].BackColor = Color.Transparent;
                rida[i].Text = "" + (i + 1);
                rida[i].TextAlign= ContentAlignment.MiddleCenter;
                rida[i].Size = new Size(50, 50);

                rida[i].Location = new Point(60, i * 50);
                this.Controls.Add(rida[i]);

                for (int j = 0; j < 4; j++)
                {
                    _arr[i, j] = new Label();
                    _arr[i, j].Font = new Font("Tobota", 14, FontStyle.Bold);
                    _arr[i, j].TextAlign = ContentAlignment.TopCenter;
                    _arr[i, j].BackColor = Color.Transparent;
                    string[] arv = arr[i].Split(';');
                    string[] ardNum = arv[j].Split(',');
                    if (ardNum[2] == "true")
                    {
                        _arr[i, j].Image = imgred;
                    }
                    else
                    {
                        _arr[i, j].Image = imggreen;
                    }
                    _arr[i, j].Text = "" + (j + 1);
                    _arr[i, j].Image=imggreen;
                    _arr[i, j].Size = new Size(50, 50);
                    _arr[i, j].Location = new Point(j * 100 + 110, i * 50);
                    this.Controls.Add(_arr[i, j]);
                    _arr[i, j].Tag = new int[] { i, j };
                    _arr[i, j].Click += new System.EventHandler(Form1_Click);
                }
            }
            btn = new Button();
            btn.Text = "Osta";
            btn.Location = new Point(400, 210);
            btn.Click += Btn_Click;
            this.Controls.Add(btn);
            btnk = new Button();
            btnk.Text = "Kinni";
            btnk.Location = new Point(100, 210);
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
                    if (_arr[i, j].Image == imgyellow )
                    {
                        Btn_Click_Func();
                    }
                }
                
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_arr[i, j].Image == imgred)
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
                        if (_arr[i, j].Image == imgyellow)
                        {
                            _arr[i, j].Image = imgred;
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
                        if (_arr[i, j].Image == imgyellow) 
                        {
                            _arr[i, j].Text = "Koht" + (j + 1);
                            _arr[i, j].Image = imggreen;
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

                    string maill = "";
                    ShowInputDialog(ref maill);

                    mail.From = new MailAddress("vlrptv@gmail.com");

                    mail.To.Add(maill);
                    mail.Subject = "Teie pilet. Kõik head!";
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (_arr[i, j].Image == imgred)
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
                            if (_arr[i, j].Image == imgyellow)
                            {
                                _arr[i, j].Text = "Koht" + (j + 1);
                                _arr[i, j].Image = imggreen;
                                ost = false;
                            }

                        }
                    }
                }
            }
                
        }
        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Email";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
        public void Btn_Click(object sender, EventArgs e)
        {
            Btn_Click_Func();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            var label = (Label)sender;
            var tag = (int[])label.Tag;
            if (_arr[tag[0], tag[1]].Image == imggreen)
            {
                _arr[tag[0], tag[1]].Text = "X";
                _arr[tag[0], tag[1]].Image = imgyellow;
                ost = true;
                
            }
            if (_arr[tag[0], tag[1]].Image == imgred)
            
            {
                string message = "See koht juba ostatud";
                string title = "Error";
                MessageBox.Show(message, title);

            }

        }
    }
}
