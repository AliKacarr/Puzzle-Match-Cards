using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Media;

namespace Puzzle_Match_Cards
{
    public partial class Form2 : Form
        {
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb");
        private SoundPlayer loginSound;

        public Form2()
        {
            InitializeComponent();
            loginSound = new SoundPlayer("soundLogin.Wav");      // giriş sesi
        }

        private void PlayCardSound()
        {
            loginSound.Play();
        }
        public static class GirisBilgileri
        {
            public static string KullaniciAdi { get; set; }
            public static string Eposta { get; set; }

        }
        public string GelenButtonText { get; set; }

        public string GidenButtonText { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string sifre = textBox2.Text;
            GirisBilgileri.KullaniciAdi = kullaniciAdi;
            
            if (GirisYap(kullaniciAdi, sifre))
            {
                PlayCardSound();
                Form form1 = Application.OpenForms["Form1"];
                if (form1 != null && form1 is Form1)
                {
                    Form1 form1Instance = (Form1)form1;
                    form1Instance.SetPuanValue(PuanAl(kullaniciAdi).ToString());
                    form1Instance.SetLabelValue(kullaniciAdi);
                    form1Instance.LoadStars();
                    form1Instance.LoadPictureBox(kullaniciAdi);
                    this.Hide();
                }

            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private int PuanAl(string kullaniciAdi)
        {
            int puan = 0;

            // Veritabanından kullanıcı adına göre Puan değerini almak için sorgu
            string sorgu = "SELECT Puan FROM Register WHERE KullaniciAdi = @kullaniciAdi";

            using (OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb"))
            {
                using (OleDbCommand komut = new OleDbCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                    baglanti.Open();

                    // Puan değerini al
                    object puanObj = komut.ExecuteScalar();

                    if (puanObj != null)
                    {
                        puan = Convert.ToInt32(puanObj);
                    }
                }
            }

            return puan;
        }
        private bool GirisYap(string kullaniciAdi, string sifre)
        {
            string sorgu = "SELECT * FROM Register WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre";

            using (OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb"))
            {
                using (OleDbCommand komut = new OleDbCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    komut.Parameters.AddWithValue("@sifre", sifre);

                    baglanti.Open();

                    using (OleDbDataReader okuyucu = komut.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            // Giriş başarılı
                            return true;
                        }
                        else
                        {
                            // Giriş başarısız
                            return false;
                        }
                    }
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form3 Kaydol = new Form3();
            Kaydol.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form5 ac = new Form5();
            ac.Show();
        }

        private void LabelMouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.ForeColor = Color.BlueViolet;
            }
        }

        private void LabelMouseHover(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.ForeColor = Color.BlueViolet;
            }
        }

        private void LabelMouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.ForeColor = Color.FromArgb(0, 0, 192);
            }
        }
        private void AdjustFormWidthBasedOnLabelText()
        {
            using (Graphics g = this.CreateGraphics())
            {
                // TextBox'taki metnin genişliğini ölç
                int maxWidth = (int)g.MeasureString(textBox1.Text, textBox1.Font).Width;

                // Eğer metin genişliği belirlenen form genişliğinden büyükse form genişliğini ayarla
                int minWidth = 616; // Belirlenen minimum form genişliği
                int newWidth = Math.Max(minWidth, maxWidth + 335);

                // Form genişliğini ayarla
                this.Width = newWidth;
                this.CenterToScreen();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AdjustTextBoxWidth(textBox1);
            UpdateProgressBar();
            AdjustFormWidthBasedOnLabelText();
        }

        private void AdjustTextBoxWidth(TextBox textBox)
        {
            // Metnin genişliğini ölçmek için Graphics nesnesi oluştur
            using (Graphics graphics = textBox.CreateGraphics())
            {
                // Metnin genişliğini ölç
                SizeF textSize = graphics.MeasureString(textBox.Text, textBox.Font);

                // TextBox'ın genişliğini ölçülen metin genişliği ile ayarla
                textBox.Width = Math.Max(160, (int)textSize.Width + 10);
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateProgressBar();
        }
        private void UpdateProgressBar()
        {
            int filledTextBoxCount = 0;

            if (!string.IsNullOrEmpty(textBox1.Text))
                filledTextBoxCount++;
            if (!string.IsNullOrEmpty(textBox2.Text))
                filledTextBoxCount++;
            if (filledTextBoxCount == 0)
                progressBar1.Value = 0;
            else if (filledTextBoxCount == 1)
                progressBar1.Value = 50;
            else
            {
                progressBar1.Value = 100;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Enter tuşunun işlenmesini durdur
                textBox2.Focus();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Enter tuşunun işlenmesini durdur
                button1.PerformClick(); // Button1'in tıklama işlemini gerçekleştir
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
                textBox2.PasswordChar = '\0';
                pictureBox2.Visible = false;
                pictureBox1.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '•';
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }
    }
    }
