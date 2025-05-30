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
using static Puzzle_Match_Cards.Form2;

namespace Puzzle_Match_Cards
{
    public partial class Form4 : Form
    {

        public Form4()
        {
            InitializeComponent();
        }
        public string Label2Text
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        public string Label1Text
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public string GetKullaniciAdi()
        {
            return label1.Text;
        }
        public void SetPuanValue(string value)
        {
            label4.Text = value;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label1.Text = GirisBilgileri.KullaniciAdi;
            string kullaniciAdi = label1.Text; // label1'den kullanıcı adını al

            // Kullanıcı adına göre eposta, puan ve SecilenResim değerlerini al
            Tuple<string, int> kullaniciBilgileri = KullaniciBilgileriniAl(kullaniciAdi);

            // label2'ye eposta değerini yazdır
            label2.Text = kullaniciBilgileri.Item1;

            // label4'e puan değerini yazdır
            label4.Text = kullaniciBilgileri.Item2.ToString();

            AdjustFormWidthBasedOnLabelText();

        }
        public void AdjustFormWidthBasedOnLabelText()
        {
            // Label'ların metin genişliğini ölç
            int label1Width = TextRenderer.MeasureText(label1.Text, label1.Font).Width;
            int label2Width = TextRenderer.MeasureText(label2.Text, label2.Font).Width;

            // En geniş olan label'ı seç
            int maxWidth = Math.Max(label1Width, label2Width);

            // Eğer metin genişliği belirlenen form genişliğinden büyükse form genişliğini ayarla
            int minWidth = 433; // Belirlenen minimum form genişliği
            int newWidth = Math.Max(minWidth, maxWidth + 150); 

            // Form genişliğini ayarla
            this.Width = newWidth;
            this.CenterToScreen();
        }



        private Tuple<string, int> KullaniciBilgileriniAl(string kullaniciAdi)
        {
            string eposta = "";
            int puan = 0;

            // Kullanıcı adına göre eposta, puan ve SecilenResim değerlerini veritabanından almak için gerekli sorgu
            string sorgu = "SELECT Eposta, Puan FROM Register WHERE KullaniciAdi = @kullaniciAdi";

            using (OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb"))
            {
                using (OleDbCommand komut = new OleDbCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                    baglanti.Open();

                    using (OleDbDataReader okuyucu = komut.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            // Eposta değerini al
                            eposta = okuyucu["Eposta"].ToString();

                            // Puan değerini al
                            puan = Convert.ToInt32(okuyucu["Puan"]);
                            
                        }
                    }
                }
            }

            // Tuple kullanarak üç değeri bir arada döndür
            return new Tuple<string, int>(eposta, puan);
        }

      


        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
            Form form1 = Application.OpenForms["Form1"];
            if (form1 != null && form1 is Form1)
            {
                Form1 form1Instance = (Form1)form1;
                form1Instance.SetDeleteAcccount();
                this.Hide();
            }
        }

        

        private void label6_Click(object sender, EventArgs e)
        {
            Form9 aç = new Form9();
            aç.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form10 aç = new Form10();
            aç.Show();
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

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.ForeColor = Color.DarkBlue;
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            label5.ForeColor = Color.DarkBlue;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Black;
        }


        private void LabelMouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.ForeColor = Color.DarkBlue;
            }
        }

        private void LabelMouseHover(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.ForeColor = Color.DarkBlue;
            }
        }

        private void LabelMouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
            {
                label.ForeColor = Color.Black;
            }
        }
    }
}
