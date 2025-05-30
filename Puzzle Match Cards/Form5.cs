using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Puzzle_Match_Cards
{
    public partial class Form5 : Form
    {
        private string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + Application.StartupPath + "\\cards.mdb";


        public Form5()
        {
            InitializeComponent();
        }

        private void button1_click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string yeniSifre = textBox2.Text;
            label4.Visible = false;
            label3.Visible = false;
            // Kullanıcı adını tablodan kontrol et
            if (KullaniciAdiMevcut(kullaniciAdi))
            {
                if (textBox2.Text.Length < 6)
                {
                    label4.Visible = true;
                }
                else
                {
                    string epostaAdresi = "koddene23@hotmail.com";
                    string konu = "Yeni Şifre Talebi";
                    string mesaj = $"Şifremi değiştirmek istiyorum. Kullanıcı adım: {kullaniciAdi}, yeni şifrem: {yeniSifre}";

                    // Mailto URL'sini oluştur
                    string mailtoUrl = $"mailto:{epostaAdresi}?subject={Uri.EscapeDataString(konu)}&body={Uri.EscapeDataString(mesaj)}";

                    // Mailto URL'sini varsayılan e-posta istemcisinde aç
                    Process.Start(new ProcessStartInfo(mailtoUrl) { UseShellExecute = true });

                    MessageBox.Show("E-posta uygulaması açıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Kullanıcı adı mevcut değil, label8'i aktif et
                label3.Visible = true;
            }
        }


        private bool KullaniciAdiMevcut(string kullaniciAdi)
        {
            // Kullanıcı adını tablodan kontrol et
            string sorgu = "SELECT COUNT(*) FROM Register WHERE KullaniciAdi = @kullaniciAdi";

            using (OleDbConnection baglanti = new OleDbConnection(connectionString))
            {
                using (OleDbCommand komut = new OleDbCommand(sorgu, baglanti))
                {
                    komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);

                    baglanti.Open();

                    int kullaniciAdiSayisi = (int)komut.ExecuteScalar();

                    return kullaniciAdiSayisi > 0;
                }
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
    }
}

