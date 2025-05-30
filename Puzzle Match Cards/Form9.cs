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
using static Puzzle_Match_Cards.Form2;

namespace Puzzle_Match_Cards
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms["Form1"];

            // Form8 bulunduysa
            if (form1 != null)
            {
                    label1.Text = GirisBilgileri.KullaniciAdi;
            }
            
            
        }
        
        private void UpdateForm8Label2Text(string newText)
        {
            // Form8'in örneğini al
            Form4 form4 = Application.OpenForms.OfType<Form4>().FirstOrDefault();

            // Form8 bulunduysa ve Label2Text property'si varsa, Label2Text'i güncelle

            form4.Label2Text = newText;

        }
        private void label6_Click(object sender, EventArgs e)
        {
            label9.Visible = true;
            textBox2.Visible = true;
            button2.Visible = true;
            label8.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            label8.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;
            label9.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Lines.Length == 0)
            {
                MessageBox.Show("Textbox boş. Eposta ekleyin.");
                return;
            }

            if (!(textBox1.Text.EndsWith("@hotmail.com", StringComparison.OrdinalIgnoreCase) || textBox1.Text.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Eposta, '@gmail.com' veya '@hotmail.com' ile bitmelidir.");
            }
            // Eposta güncelleme işlemini kontrol et
            else if (GuncelleEposta())
            {
                MessageBox.Show("Eposta başarıyla güncellendi.");
                UpdateForm8Label2Text(textBox1.Text);
                // Kontrolleri tekrar pasif hale getir
                label8.Visible = false;
                textBox1.Visible = false;
                button1.Visible = false;
                Form form4 = Application.OpenForms["Form4"];
                if (form4 != null && form4 is Form4)
                {
                    Form4 form4Instance = (Form4)form4;
                    form4Instance.Label2Text = textBox1.Text;
                    form4Instance.AdjustFormWidthBasedOnLabelText();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Eposta güncellenirken bir hata oluştu.");
            }
        }
        private bool GuncelleEposta()
        {
            OleDbConnection connection = null;

            try
            {
                connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb");
                connection.Open();

                string kullaniciAdi = label1.Text; // Kullanıcı adını label1'den al
                string yeniEposta = textBox1.Text;

                // Eposta güncelleme sorgusunu hazırla
                string updateQuery = "UPDATE Register SET Eposta = @Eposta WHERE KullaniciAdi = @KullaniciAdi";
                OleDbCommand command = new OleDbCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Eposta", yeniEposta);
                command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Lines.Length == 0)
            {
                MessageBox.Show("Textbox boş. Şifre ekleyin.");
                return;
            }

            string sonSatir = textBox2.Lines[textBox2.Lines.Length - 1];
            if (sonSatir == label1.Text)
            {
                MessageBox.Show("Şifre kullanıcı adıyla aynı olamaz.");
            }
            else if (GuncelleSifre())
            {
                MessageBox.Show("Şifre başarıyla güncellendi.");
                // Kontrolleri tekrar pasif hale getir
                label9.Visible = false;
                textBox2.Visible = false;
                button2.Visible = false;
                textBox2.Clear();
                Form form4 = Application.OpenForms["Form4"];
                if (form4 != null && form4 is Form4)
                {
                    Form4 form4Instance = (Form4)form4;
                    form4Instance.AdjustFormWidthBasedOnLabelText();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Şifre güncellenirken bir hata oluştu.");
            }
        }
        private bool GuncelleSifre()
        {
            OleDbConnection connection = null;

            try
            {
                connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb");
                connection.Open();

                string kullaniciAdi = label1.Text; // Kullanıcı adını label1'den al
                string yeniSifre = textBox2.Text;

                // Şifre güncelleme sorgusunu hazırla
                string updateQuery = "UPDATE Register SET Sifre = @Sifre WHERE KullaniciAdi = @KullaniciAdi";
                OleDbCommand command = new OleDbCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Sifre", yeniSifre);
                command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        
        

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.DarkBlue;
        }

        private void label6_MouseHover(object sender, EventArgs e)
        {
            label6.ForeColor = Color.DarkBlue;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Black;
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.DarkBlue;
        }

        private void label7_MouseHover(object sender, EventArgs e)
        {
            label7.ForeColor = Color.DarkBlue;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Black;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Enter tuşunun işlenmesini durdur
                button1.PerformClick(); // Button1'in tıklama işlemini gerçekleştir
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Enter tuşunun işlenmesini durdur
                button2.PerformClick(); // Button1'in tıklama işlemini gerçekleştir
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AdjustTextBoxWidth(textBox1);
            AdjustFormWidthBasedOnLabelText();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            AdjustTextBoxWidth(textBox2);
            AdjustFormWidthBasedOnLabelText();
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
                int minWidth = 513; // Belirlenen minimum form genişliği
                int newWidth = Math.Max(minWidth, maxWidth + 310);

                // Form genişliğini ayarla
                this.Width = newWidth;
                this.CenterToScreen();
            }
        }

        private void AdjustTextBoxWidth(TextBox textBox)
        {
            // Metnin genişliğini ölçmek için Graphics nesnesi oluştur
            using (Graphics graphics = textBox.CreateGraphics())
            {
                // Metnin genişliğini ölç
                SizeF textSize = graphics.MeasureString(textBox.Text, textBox.Font);

                // TextBox'ın genişliğini ölçülen metin genişliği ile ayarla
                textBox.Width = Math.Max(195, (int)textSize.Width + 10);
            }
        }
    }
}
