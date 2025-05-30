using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.OleDb;
using static Puzzle_Match_Cards.Form2;

namespace Puzzle_Match_Cards
{
    public partial class Form1 : Form
    {
        private OleDbConnection connection;

        public Form1()
        {
            InitializeComponent();

            connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cards.mdb");
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            for (int i = 1; i <= 30; i++)
            {
                Controls["pictureBox" + i].Click += new EventHandler(pictureBox_Click);
            }
            LoadPictureBox(label1.Text);
        }
          
        private void button3_Click(object sender, EventArgs e)
        {
            Form3 registerForm = new Form3();
            registerForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 loginForm = new Form2();
            loginForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Form4 profileForm = new Form4();
            profileForm.Show();
        }
        
        public void SetDeleteAcccount()
        {
            label1.Text = "username";
            button2.Visible = true;
            button3.Visible = true;
            button5.Visible = false;
            button4.Visible = false;
            for (int i = 32; i <= 60; i++)
            {
                if (i == 41||i==51) continue;
                Controls["pictureBox" + i].Visible = true;
            }
            for (int i = 61; i <= 90; i++)
            {
                Controls["pictureBox" + i].Visible = false;
            }
        }
        public void SetPuanValue(string value)
        {

            // Butonun yazısını ayarla
            button4.Text = "Score: " + value;

            // Graphics nesnesi oluştur
            using (Graphics graphics = button4.CreateGraphics())
            {
                // Metnin genişliğini ölç
                SizeF textSize = graphics.MeasureString(button4.Text, button4.Font);

                // Butonun genişliğini ölçülen metin genişliğine göre ayarla
                int newWidth = Math.Max(165, (int)textSize.Width + 20); // Ekstra 20 piksel genişlik ekleniyor

                button4.Size = new Size(newWidth, button4.Height);
            }

            // Metni ortala
            button4.TextAlign = ContentAlignment.MiddleCenter;
        }
        
        public void SetVisiblePictureBox(int value)
        {
            int pictureBoxNumber = value;
            PictureBox pictureBox = this.Controls.Find("pictureBox" + pictureBoxNumber, true).FirstOrDefault() as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.Visible = false;
            }
        }

        public void LoadPictureBox(string value)
        {
            if (label1.Text != "username")
            {
                LoadUserData();
            }
        }
        public void SetLabelValue(string value)
        {
            // Form1'deki etikete değeri ata
            label1.Text = value;
            button4.Visible = true;
            button5.Visible = true;
            button2.Visible = false;
            button3.Visible = false;
        }
        private void LoadUserData()
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand("SELECT KolaySeviye, OrtaSeviye, ZorSeviye FROM Register WHERE KullaniciAdi = @KullaniciAdi", connection);
            command.Parameters.AddWithValue("@KullaniciAdi", label1.Text);
            OleDbDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string kolaySeviyeStr = reader.GetString(0);
                string ortaSeviyeStr = reader.GetString(1);
                string zorSeviyeStr = reader.GetString(2);

                int kolaySeviye = int.Parse(kolaySeviyeStr);
                int ortaSeviye = int.Parse(ortaSeviyeStr);
                int zorSeviye = int.Parse(zorSeviyeStr);

                SetPictureBoxVisibility(kolaySeviye, ortaSeviye, zorSeviye);
            }

            connection.Close();
        }

        private void SetPictureBoxVisibility(int kolaySeviye, int ortaSeviye, int zorSeviye)
        {
            for (int i = 1; i <= 10; i++)
            {
                Controls["pictureBox" + (30 + i)].Visible = i > kolaySeviye;
            }

            for (int i = 11; i <= 20; i++)
            {
                Controls["pictureBox" + (30 + i)].Visible = i > 10 + ortaSeviye;
            }

            for (int i = 21; i <= 30; i++)
            {
                Controls["pictureBox" + (30 + i)].Visible = i > 20 + zorSeviye;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedBox = sender as PictureBox;
            int index = int.Parse(clickedBox.Name.Substring(10));

            if (index <= 10)
            {
                Kolay easyForm = new Kolay(index,label1.Text);
                easyForm.Show();
            }
            else if (index <= 20)
            {
                Orta mediumForm = new Orta(index - 10, label1.Text);
                mediumForm.Show();
            }
            else if (index <= 30)
            {
                Zor hardForm = new Zor(index - 20, label1.Text);
                hardForm.Show();
            }
        }
        public void LoadStars()
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand("SELECT KolayStar, OrtaStar, ZorStar FROM Register WHERE KullaniciAdi = @KullaniciAdi", connection);
            command.Parameters.AddWithValue("@KullaniciAdi", label1.Text);
            OleDbDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string kolayStar = reader.GetString(0);
                string ortaStar = reader.GetString(1);
                string zorStar = reader.GetString(2);

                SetStarPictures(kolayStar, 61);
                SetStarPictures(ortaStar, 71);
                SetStarPictures(zorStar, 81);
            }

            connection.Close();
        }

        public void SetStarPictures(string starValues, int startPictureBoxIndex)
        {
            for (int i = 0; i < starValues.Length; i++)
            {
                PictureBox pictureBox = this.Controls.Find("pictureBox" + (startPictureBoxIndex + i), true).FirstOrDefault() as PictureBox;
                if (pictureBox != null)
                {
                    switch (starValues[i])
                    {
                        case '0':
                            pictureBox.Visible = false;
                            pictureBox.Image = null;
                            pictureBox.Tag = null;
                            break;
                        case '1':
                            pictureBox.Image = Image.FromFile("oneStar.png");
                            pictureBox.Tag = "oneStar";
                            pictureBox.Visible = true;
                            break;
                        case '2':
                            pictureBox.Image = Image.FromFile("twoStar.png");
                            pictureBox.Tag = "twoStar";
                            pictureBox.Visible = true;
                            break;
                        case '3':
                            pictureBox.Image = Image.FromFile("threeStar.png");
                            pictureBox.Tag = "threeStar";
                            pictureBox.Visible = true;
                            break;
                    }
                }
            }
        }
        public void SetStars(string no, int starCount)
        {
            PictureBox pictureBox = this.Controls.Find("pictureBox" + no, true).FirstOrDefault() as PictureBox;
            if (pictureBox != null)
            {
                // Mevcut yıldız resmini kontrol et
                string currentImageName = pictureBox.Tag?.ToString() ?? string.Empty;

                switch (starCount)
                {
                    case 0:
                        pictureBox.Visible = false;
                        pictureBox.Image = null;
                        pictureBox.Tag = null;
                        break;
                    case 1:
                        if (currentImageName != "twoStar" && currentImageName != "threeStar")
                        {
                            pictureBox.Image = Image.FromFile("oneStar.png");
                            pictureBox.Tag = "oneStar";
                            pictureBox.Visible = true;
                        }
                        break;
                    case 2:
                        if (currentImageName != "threeStar")
                        {
                            pictureBox.Image = Image.FromFile("twoStar.png");
                            pictureBox.Tag = "twoStar";
                            pictureBox.Visible = true;
                        }
                        break;
                    case 3:
                        pictureBox.Image = Image.FromFile("threeStar.png");
                        pictureBox.Tag = "threeStar";
                        pictureBox.Visible = true;
                        break;
                }
            }
        }
        public bool IsPictureBoxVisible(int pictureBoxNumber)
        {
            PictureBox pictureBox = this.Controls.Find("pictureBox" + pictureBoxNumber, true).FirstOrDefault() as PictureBox;
            return pictureBox != null && pictureBox.Visible;
        }

    }
}
