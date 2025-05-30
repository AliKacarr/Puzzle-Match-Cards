using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using static Puzzle_Match_Cards.Form2;

namespace Puzzle_Match_Cards
{
    public partial class Form3 : Form
    {
        private OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb");
        private SoundPlayer loginSound;

        public Form3()
        {
            InitializeComponent();
            loginSound = new SoundPlayer("soundLogin.Wav");      // giriş sesi

        }

        private void PlayCardSound()
        {
            loginSound.Play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            // Tüm koşulları kontrol et
            if (KontrolleriKontrolEt())
            {
                // Veritabanına ekleme işlemi
                if (KullaniciEkle())
                {
                    PlayCardSound();
                    Form form1 = Application.OpenForms["Form1"];
                    if (form1 != null && form1 is Form1)
                    {
                        GirisBilgileri.KullaniciAdi = textBox1.Text;
                        Form1 form1Instance = (Form1)form1;
                        form1Instance.SetPuanValue("0");
                        form1Instance.SetLabelValue(kullaniciAdi);
                        form1Instance.LoadPictureBox(kullaniciAdi);
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Kayıt eklenirken bir hata oluştu.");
                }
            }
        }

        private bool KullaniciEkle()
        {
            try
            {
                connection.Open();
                DateTime uyelikTarihi = DateTime.Now;

                OleDbCommand command = new OleDbCommand("INSERT INTO Register (KullaniciAdi, Eposta, Sifre, UyelikTarihi) VALUES (@KullaniciAdi, @Eposta, @Sifre, @UyelikTarihi)", connection);
                command.Parameters.AddWithValue("@KullaniciAdi", textBox1.Text);
                command.Parameters.AddWithValue("@Eposta", textBox2.Text);
                command.Parameters.AddWithValue("@Sifre", textBox3.Text);
                command.Parameters.AddWithValue("@UyelikTarihi", uyelikTarihi.ToString("d MMMM, yyyy, HH:mm")); // Tarih formatını ayarla

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
            finally
            {
                connection.Close();
            }
        }
        

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                textBox4.PasswordChar = '\0';
                pictureBox4.Visible = false;
            }
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                textBox4.PasswordChar = '•';
                pictureBox4.Visible = true;
            }
        }

        private bool KullaniciAdiBenzersizMi(string kullaniciAdi)
        {
            try
            {
                connection.Open();

                OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM Register WHERE KullaniciAdi = @KullaniciAdi", connection);
                command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);

                int kullaniciAdiSayisi = (int)command.ExecuteScalar();

                return kullaniciAdiSayisi == 0;
            }
            finally
            {
                connection.Close();
            }
        }
        private bool KontrolleriKontrolEt()
        {
            // Kullanıcı adı uzunluğu kontrolü
            if (textBox1.Text.Length < 4)
            {
                label4.Visible = true;
                label5.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                return false;
            }

            // Kullanıcı adı benzersiz kontrolü
            if (!KullaniciAdiBenzersizMi(textBox1.Text))
            {
                label8.Visible = true;
                label4.Visible = false;
                label5.Visible = false;
                label7.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                return false;
            }
            if (!(textBox2.Text.EndsWith("@hotmail.com", StringComparison.OrdinalIgnoreCase) || textBox2.Text.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)))
            {
                label9.Visible = true;
                label4.Visible = false;
                label5.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label10.Visible = false;
                return false;
            }
            if (textBox1.Text == textBox3.Text)
            {
                label10.Visible = true;
                label7.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                return false;
            }
            // Şifre uzunluğu kontrolü
            if (textBox3.Text.Length < 6)
            {
                label5.Visible = true;
                label4.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                return false;
            }

            // Şifre tekrarı kontrolü
            if (textBox3.Text != textBox4.Text)
            {
                label7.Visible = true;
                label4.Visible = false;
                label5.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                return false;
            }

            // Diğer kontroller
            // ...

            return true;
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
        private void AdjustFormWidthBasedOnLabelText()
        {
            using (Graphics g = this.CreateGraphics())
            {
                // TextBox'ların metin genişliğini ölç
                int textBox1Width = (int)g.MeasureString(textBox1.Text, textBox1.Font).Width;
                int textBox2Width = (int)g.MeasureString(textBox2.Text, textBox2.Font).Width;

                // En geniş olan TextBox'ı seç
                int maxWidth = Math.Max(textBox1Width, textBox2Width);

                // Eğer metin genişliği belirlenen form genişliğinden büyükse form genişliğini ayarla
                int minWidth = 616; // Belirlenen minimum form genişliği
                int newWidth = Math.Max(minWidth, maxWidth + 330);

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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            AdjustTextBoxWidth(textBox2);
            UpdateProgressBar();
            AdjustFormWidthBasedOnLabelText();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            UpdateProgressBar();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
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
            if (!string.IsNullOrEmpty(textBox3.Text))
                filledTextBoxCount++;
            if (!string.IsNullOrEmpty(textBox4.Text))
                filledTextBoxCount++;

            if (filledTextBoxCount == 0)
                progressBar1.Value = 0;
            else if (filledTextBoxCount == 1)
                progressBar1.Value = 25;
            else if (filledTextBoxCount == 2)
                progressBar1.Value = 50;
            else if (filledTextBoxCount == 3)
                progressBar1.Value = 75;
            else
                progressBar1.Value = 100;
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
                textBox3.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Enter tuşunun işlenmesini durdur
                textBox4.Focus();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Enter tuşunun işlenmesini durdur
                button1.PerformClick(); // Button1'in tıklama işlemini gerçekleştir
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox3.PasswordChar = '\0';
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox3.PasswordChar = '•';
            pictureBox2.Visible = true;
            pictureBox1.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox4.PasswordChar = '•';
            pictureBox4.Visible = true;
            pictureBox3.Visible = false;

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox4.PasswordChar = '\0';
            pictureBox4.Visible = false;
            pictureBox3.Visible = true;

        }
    }
}
