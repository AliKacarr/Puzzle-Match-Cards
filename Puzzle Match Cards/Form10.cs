using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Puzzle_Match_Cards.Form1;
using System.Windows.Forms;
using System.Data.OleDb;
using static Puzzle_Match_Cards.Form2;

namespace Puzzle_Match_Cards
{
    public partial class Form10 : Form
    {
        private string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb";
        public Form10()
        {
            InitializeComponent();
        }
        private void Form10_Load(object sender, EventArgs e)
        {
            Form form1 = Application.OpenForms["Form1"];

            // Form8 bulunduysa
            if (form1 != null)
            {
                label1.Text = GirisBilgileri.KullaniciAdi;
            }
            string kullaniciAdi = label1.Text;
            string tarih = GetTarih(kullaniciAdi);

            // Label4'e Konum değerini yazdır
            label2.Text = tarih;

        }
        private string GetTarih(string kullaniciAdi)
        {
            string tarih = "";

            // Veritabanından kullanıcı adına göre Konum değerini almak için sorgu
            string sorgu = "SELECT UyelikTarihi FROM Register WHERE KullaniciAdi = @kullaniciAdi";

            using (OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb"))
            {
                using (OleDbCommand komut = new OleDbCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                    baglanti.Open();

                    // Konum değerini al
                    object tarihObj = komut.ExecuteScalar();

                    if (tarihObj != null)
                    {
                        tarih = tarihObj.ToString();
                    }
                }
            }

            return tarih;
        }
        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SilmeIslemi();
            this.Hide();
            Form form1 = Application.OpenForms["Form1"];
            if (form1 != null && form1 is Form1)
            {
                Form1 form1Instance = (Form1)form1;
                form1Instance.SetDeleteAcccount();
                this.Hide();
            }
        }

        private void SilmeIslemi()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Silme işlemi yapılacak komut
                    string silmeSorgusu = "DELETE FROM Register WHERE KullaniciAdi = @kullaniciAdi";

                    using (OleDbCommand komut = new OleDbCommand(silmeSorgusu, connection))
                    {
                        // Label'dan kullanıcı adını al
                        string kullaniciAdi = label1.Text;

                        // Parametre ekleyerek SQL injection saldırılarından korunma
                        komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                        // Komutu çalıştır
                        int etkilenenSatirSayisi = komut.ExecuteNonQuery();

                        if (etkilenenSatirSayisi > 0)
                        {
                            // Silme işlemi başarılıysa kullanıcıya mesaj göster
                            MessageBox.Show("Hesabınız başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // Silme işlemi başarısızsa kullanıcıya mesaj göster
                            MessageBox.Show("Silme işlemi başarısız oldu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj göster
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
