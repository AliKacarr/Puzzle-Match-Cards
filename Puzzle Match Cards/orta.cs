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
using System.Drawing.Drawing2D;

namespace Puzzle_Match_Cards
{
    public partial class Orta : Form
    {
        private int level;
        private string username;
        private double timeLeft;
        private double originalTime;
        private List<string> imagePaths;
        private List<PictureBox> hiddenPictureBoxes;
        private List<PictureBox> visiblePictureBoxes;
        private List<Tuple<PictureBox, bool>> temporarilyToggledPictureBoxes;
        private Dictionary<string, SoundPlayer> soundPlayers;
        private PictureBox firstClicked, secondClicked;
        private Random random = new Random();
        private OleDbConnection connection;
        private bool isHintActive = false;
        private Panel bilgiPanel;
        private bool isVolume = true;
        bool islose = false;

        public Orta()
        {
            InitializeComponent();
            InitializeSoundPlayers();
            InitializebilgiPanel();
        }

        public Orta(int level, string username)
        {
            InitializeComponent();
            InitializeSoundPlayers();
            InitializebilgiPanel();
            this.level = level;
            this.username = username;
            connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=cards.mdb");
        }

        private void Orta_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            if (username == "username")
            {
                button1.Visible = false;
            }
            else
            {
                SetButton1(PuanAl(username).ToString());
                pictureBox47.Visible = true;
            }

            InitializeGame();
            customProgressBar1.Maximum = (int)(originalTime * 10 - originalTime * 9 / 10);
            customProgressBar1.Value = (int)(originalTime * 10 - originalTime * 9 / 10);
            PlaySound("CardShuffling");
        }

        private void InitializeSoundPlayers()
        {
            soundPlayers = new Dictionary<string, SoundPlayer>
        {
            { "CardShuffling", new SoundPlayer("sound1.Wav") },
            { "CardSound", new SoundPlayer("sound2.Wav") },
            { "LoseCard", new SoundPlayer("sound3.Wav") },
            { "WinCard", new SoundPlayer("sound4.Wav") },
            { "FormClose", new SoundPlayer("sound5.Wav") },
            { "NextLevel", new SoundPlayer("sound6.Wav") }
        };
        }
        private void PlaySound(string sound)
        {
            if (isVolume && soundPlayers.ContainsKey(sound))
            {
                soundPlayers[sound].Play();
            }
        }
        private void InitializeGame()
        {
            string folderPath = string.Empty;
            // Level'e göre başlangıç süresini ve resim listesini ayarla
            switch (level)
            {
                case 1:
                    timeLeft = 75;
                    originalTime = 75;
                    folderPath = "car";
                    break;
                case 2:
                    timeLeft = 70;
                    originalTime = 70;
                    folderPath = "mosque";
                    break;
                case 3:
                    timeLeft = 65;
                    originalTime = 65;
                    folderPath = "sport";
                    break;
                case 4:
                    timeLeft = 60;
                    originalTime = 60;
                    folderPath = "instrument";
                    break;
                case 5:
                    timeLeft = 55;
                    originalTime = 55;
                    folderPath = "desserts";
                    break;
                case 6:
                    timeLeft = 50;
                    originalTime = 50;
                    folderPath = "animal";
                    break;
                case 7:
                    timeLeft = 45;
                    originalTime = 45;
                    folderPath = "flower";
                    break;
                case 8:
                    timeLeft = 40;
                    originalTime = 40;
                    folderPath = "job";
                    break;
                case 9:
                    timeLeft = 35;
                    originalTime = 35;
                    folderPath = "heroe";
                    break;
                case 10:
                    timeLeft = 30;
                    originalTime = 30;
                    folderPath = "fruit";
                    break;
                default:
                    timeLeft = 50;
                    originalTime = 50;
                    folderPath = "mosque";
                    break;
            }

            // Resim dosyalarını klasör yoluyla birleştir
            imagePaths = new List<string>();
            for (int i = 1; i <= 15; i++)
            {
                imagePaths.Add($"{folderPath}/{folderPath}{i}.jpg");
            }

            textBox2.Text = timeLeft.ToString();

            // Hidden ve visible PictureBox listesi oluşturma ve tıklama olayını ekleme
            hiddenPictureBoxes = new List<PictureBox>
            {
                pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5,
                pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10,
                pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15,
                pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20,
            };

            visiblePictureBoxes = new List<PictureBox>
            {
                pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25,
                pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30,
                pictureBox31, pictureBox32, pictureBox33, pictureBox34, pictureBox35,
                pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40
            };

            foreach (var pictureBox in hiddenPictureBoxes)
            {
                pictureBox.Click += HiddenPictureBox_Click;
            }

            foreach (var pictureBox in visiblePictureBoxes)
            {
                pictureBox.Visible = false;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            AssignImagesToPictureBoxes();
        }

        private void AssignImagesToPictureBoxes()
        {
            label3.Visible = false;
            textBox1.Visible = false;
            List<string> selectedImages = imagePaths.OrderBy(x => random.Next()).Take(10).ToList();
            selectedImages.AddRange(selectedImages);
            selectedImages = selectedImages.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < visiblePictureBoxes.Count; i++)
            {
                visiblePictureBoxes[i].Tag = selectedImages[i];
                visiblePictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void HiddenPictureBox_Click(object sender, EventArgs e)
        {
            if (isHintActive) return;
            PictureBox clickedBox = sender as PictureBox;
            int index = hiddenPictureBoxes.IndexOf(clickedBox);
            if ((double)customProgressBar1.Value / customProgressBar1.Maximum * 100 == 100)
            {
                timer5.Start();
                timer1.Start();
            }

            if (index >= 0 && index < visiblePictureBoxes.Count)
            {
                PictureBox correspondingBox = visiblePictureBoxes[index];

                if (!correspondingBox.Visible && firstClicked == null && secondClicked == null)
                {
                    correspondingBox.Image = Image.FromFile((string)correspondingBox.Tag);
                    correspondingBox.Visible = true;
                    firstClicked = correspondingBox;
                    PlaySound("CardSound");
                }
                else if (!correspondingBox.Visible && secondClicked == null)
                {
                    correspondingBox.Image = Image.FromFile((string)correspondingBox.Tag);
                    correspondingBox.Visible = true;
                    secondClicked = correspondingBox;
                    PlaySound("CardSound");

                    if (firstClicked.Tag == secondClicked.Tag)
                    {
                        firstClicked = null;
                        secondClicked = null;

                        if (visiblePictureBoxes.All(p => p.Visible) && textBox2.Text != "0")
                        {
                            timer1.Stop();
                            timer5.Stop();
                            PlaySound("WinCard");
                            int starCount = 0;
                            Form form1 = Application.OpenForms["Form1"];
                            if (form1 != null && form1 is Form1)
                            {
                                Form1 form1Instance = (Form1)form1;
                                form1Instance.SetVisiblePictureBox(level + 41);
                                if (pictureBox46.Visible)
                                {
                                    form1Instance.SetStars((level + 70).ToString(), 3);
                                    starCount = 3;
                                }
                                else if (pictureBox45.Visible)
                                {
                                    form1Instance.SetStars((level + 70).ToString(), 2);
                                    starCount = 2;
                                }
                                else if (pictureBox44.Visible)
                                {
                                    form1Instance.SetStars((level + 70).ToString(), 1);
                                    starCount = 1;
                                }
                            }
                            if (username != "username")
                            {
                                int earnedScore = 150 - Convert.ToInt32(originalTime - timeLeft) * 2;
                                UpdateUserScore(username, earnedScore);
                                SetButton1(PuanAl(username).ToString());
                                Form1 form1Instance = (Form1)form1;
                                form1Instance.SetPuanValue(PuanAl(username).ToString());
                                UpdateOrtaSeviye(level + 1);
                                UpdateOrtaStar(username, level, starCount);
                                textBox1.Text = earnedScore + " Puan Kazandınız!";
                                textBox1.Visible = true;
                            }
                            if (level != 10)
                            {
                                pictureBox48.Visible = true;
                            }
                            pictureBox49.Visible = true;
                            if (level != 1)
                            {
                                pictureBox50.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        timer2.Interval = 750;
                        timer2.Start();
                    }
                }
            }
        }

        public void SetButton1(string value)
        {
            // Butonun yazısını ayarla
            button1.Text = "Score: " + value;

            // Graphics nesnesi oluştur
            using (Graphics graphics = button1.CreateGraphics())
            {
                // Metnin genişliğini ölç
                SizeF textSize = graphics.MeasureString(button1.Text, button1.Font);

                // Butonun genişliğini ölçülen metin genişliğine göre ayarla
                int newWidth = Math.Max(165, (int)textSize.Width + 20); // Ekstra 20 piksel genişlik ekleniyor

                button1.Size = new Size(newWidth, button1.Height);
            }

            // Metni ortala
            button1.TextAlign = ContentAlignment.MiddleCenter;

        }
        private void UpdateOrtaSeviye(int newLevel)
        {
            if (newLevel == 11)
            {
                return;
            }

            connection.Open();

            // Mevcut OrtaSeviye'yi al
            OleDbCommand selectCommand = new OleDbCommand("SELECT OrtaSeviye FROM Register WHERE KullaniciAdi = @username", connection);
            selectCommand.Parameters.AddWithValue("@username", username);
            object result = selectCommand.ExecuteScalar();

            // Result null ise default olarak 0 atayalım
            int currentLevel = result != null ? Convert.ToInt32(result) : 0;


            // Eğer newLevel, currentLevel'den büyükse güncelle
            if (newLevel > currentLevel)
            {
                OleDbCommand updateCommand = new OleDbCommand("UPDATE Register SET OrtaSeviye = @newLevel WHERE KullaniciAdi = @username", connection);
                updateCommand.Parameters.AddWithValue("@newLevel", newLevel);
                updateCommand.Parameters.AddWithValue("@username", username);
                updateCommand.ExecuteNonQuery();
            }

            connection.Close();
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 1)
            {
                timeLeft--;
                textBox2.Text = timeLeft.ToString();
            }
            else
            {
                timeLeft--;
                textBox2.Text = "0";
                timer1.Stop();
                pictureBox44.Visible = false;
                pictureBox49.Visible = true;
                if (level != 1)
                {
                    pictureBox50.Visible = true;
                }
                label3.Visible = true;
                if (!islose)
                {
                    PlaySound("LoseCard");
                    islose = true;
                }
                Form form1 = Application.OpenForms["Form1"];
                Form1 form1Instance = (Form1)form1;
                if (!form1Instance.IsPictureBoxVisible(level + 41)&& level!=10)
                {
                    // pictureBox48 görünürlüğünü aç
                    pictureBox48.Visible = true;
                }
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            if (firstClicked != null)
                firstClicked.Visible = false;
            if (secondClicked != null)
                secondClicked.Visible = false;
            firstClicked = null;
            secondClicked = null;
        }
        private void pictureBox49_Click(object sender, EventArgs e)
        {
            GameSelect();
            islose = false;
            PlaySound("CardShuffling");
        }
        private void GameSelect()
        {

            InitializeGame();
            foreach (var pictureBox in visiblePictureBoxes)
            {
                pictureBox.Visible = false;
                pictureBox.Image = null;
            }

            AssignImagesToPictureBoxes();
            customProgressBar1.Maximum = (int)(originalTime * 10 - originalTime * 9 / 10);
            customProgressBar1.Value = (int)(originalTime * 10 - originalTime * 9 / 10);
            textBox2.Text = originalTime.ToString();
            pictureBox46.Visible = true;
            pictureBox45.Visible = true;
            pictureBox44.Visible = true;
            pictureBox48.Visible = false;
            pictureBox49.Visible = false;
            pictureBox50.Visible = false;
            firstClicked = null; // İlk tıklanan resmi sıfırla
            secondClicked = null; // İkinci tıklanan resmi sıfırla
            firstClicked = null; // İlk tıklanan resmi sıfırla
            secondClicked = null; // İkinci tıklanan resmi sıfırla
        }
        private void pictureBox48_Click(object sender, EventArgs e)
        {
            level += 1;
            GameSelect();
            PlaySound("NextLevel");
            islose = false;
        }
        private void UpdateUserScore(string username, int score)
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand("UPDATE Register SET Puan = Puan + @score WHERE KullaniciAdi = @username", connection);
            command.Parameters.AddWithValue("@score", score);
            command.Parameters.AddWithValue("@username", username);
            command.ExecuteNonQuery();
            connection.Close();
        }
        private void Orta_FormClosing(object sender, FormClosingEventArgs e)
        {
            PlaySound("FormClose");
        }
        private void InitializebilgiPanel()
        {
            // Paneli oluştur
            bilgiPanel = new Panel
            {
                Size = new Size(255, 28),
                BackColor = Color.White,
                Visible = false,
                Location = new Point(365, 85),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(bilgiPanel);
            bilgiPanel.BringToFront();

            Label copyLabel = new Label
            {
                Text = "50 Puan karşılığı gizli kartları göster",
                ForeColor = Color.Black,
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 12),
                Location = new Point((bilgiPanel.Width - TextRenderer.MeasureText(Text, new Font("Arial", 10)).Width) - 221, (bilgiPanel.Height - TextRenderer.MeasureText(Text, new Font("Arial", 10)).Height) / 2 - 4),
            };
            bilgiPanel.Controls.Add(copyLabel);

        }
        private void pictureBox47_Click(object sender, EventArgs e)
        {
            if (originalTime.ToString() == textBox2.Text && firstClicked == null)
            {
                MessageBox.Show("Gizli kartları görmek için önce oyuna başlayın");
                return;
            }
            if (!isHintActive)  // Eğer ipucu aktif değilse
            {
                int currentScore = PuanAl(username);
                if (currentScore >= 50)  // Kullanıcının puanı 50'den büyükse
                {
                    // Puanı 50 düşür
                    UpdateUserScore(username, -50);
                    SetButton1(PuanAl(username).ToString());
                    Form form1 = Application.OpenForms["Form1"];
                    Form1 form1Instance = (Form1)form1;
                    form1Instance.SetPuanValue(PuanAl(username).ToString());

                    // Görünür olmayan resim kutularının görünürlüğünü aç ve görünür olanların görünürlüğünü kapat
                    ToggleVisibilityOfAllCards();
                    isHintActive = true;  // İpucu aktif durumda

                    // Timer başlat ve 1 saniye sonra resimleri eski hallerine getir
                    timer3.Start();
                }
                else
                {
                    MessageBox.Show("Gizli kartları görmek için 50 Puan kazanmalısınız");
                }
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            RestoreVisibilityOfAllCards();
            isHintActive = false;
        }
        private void ToggleVisibilityOfAllCards()
        {
            temporarilyToggledPictureBoxes = new List<Tuple<PictureBox, bool>>();

            foreach (var pictureBox in visiblePictureBoxes)
            {
                if (firstClicked != null && pictureBox == firstClicked)
                {
                    continue;
                }
                if (firstClicked != null && pictureBox == firstClicked)
                {
                    pictureBox.Visible = false;
                }
                else if (secondClicked != null && pictureBox == secondClicked)
                {
                    pictureBox.Visible = false;
                }
                temporarilyToggledPictureBoxes.Add(Tuple.Create(pictureBox, pictureBox.Visible));
                pictureBox.Visible = !pictureBox.Visible;
                if (pictureBox.Visible)
                {
                    pictureBox.Image = Image.FromFile((string)pictureBox.Tag);
                }
                else
                {
                    pictureBox.Image = null;
                }
            }
        }

        private void RestoreVisibilityOfAllCards()
        {
            foreach (var item in temporarilyToggledPictureBoxes)
            {
                var pictureBox = item.Item1;
                bool wasVisible = item.Item2;
                pictureBox.Visible = wasVisible;
                if (wasVisible)
                {
                    pictureBox.Image = Image.FromFile((string)pictureBox.Tag);
                }
                else
                {
                    pictureBox.Image = null;
                }
            }

            temporarilyToggledPictureBoxes.Clear();
        }
        private bool isMouseOverPictureBox47 = false;
        private void pictureBox47_MouseEnter(object sender, EventArgs e)
        {
            isMouseOverPictureBox47 = true;
            timer4.Start();
        }

        private void pictureBox47_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverPictureBox47 = false;
            timer4.Stop();
            bilgiPanel.Visible = false;
        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            timer4.Stop();
            if (isMouseOverPictureBox47)
            {
                bilgiPanel.Visible = true;
            }
        }
        private void timer5_Tick(object sender, EventArgs e)
        {
            if (customProgressBar1.Value > 0)
            {
                customProgressBar1.Value--;
            }
            else
            {
                timer5.Stop(); // Zamanlayıcıyı durdur
                customProgressBar1.Value = 0; // ProgressBar'ı sıfırla
            }

            double dolulukOrani = (double)customProgressBar1.Value / customProgressBar1.Maximum * 100;
            if (dolulukOrani <= 50 && dolulukOrani >= 45 && pictureBox46.Visible)
            {
                pictureBox46.Visible = false;
            }
            else if (dolulukOrani <= 30 && dolulukOrani >= 25 && pictureBox45.Visible)
            {
                pictureBox45.Visible = false;
            }
        }
        private void pictureBox42_Click(object sender, EventArgs e)
        {
            pictureBox43.Visible = true;
            pictureBox42.Visible = false;
            isVolume = false;
        }
        private void pictureBox43_Click(object sender, EventArgs e)
        {
            pictureBox42.Visible = true;
            pictureBox43.Visible = false;
            isVolume = true;
        }
        private void pictureBox41_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }
        private void pictureBox50_Click(object sender, EventArgs e)
        {
            level -= 1;
            GameSelect();
            PlaySound("NextLevel");
            islose = false;
        }
        private void UpdateOrtaStar(string username, int level, int starCount)
        {
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("SELECT OrtaStar FROM Register WHERE KullaniciAdi = @username", connection);
                command.Parameters.AddWithValue("@username", username);

                string currentOrtaStar = (string)command.ExecuteScalar();
                if (currentOrtaStar != null && currentOrtaStar.Length > level - 1)
                {
                    char currentStar = currentOrtaStar[level - 1];
                    char newStar = starCount.ToString()[0];

                    // Mevcut yıldız değeri, güncellenmek istenen yıldız değerinden büyük veya eşitse güncellemeyi atla
                    if (currentStar >= newStar)
                    {
                        return;
                    }

                    // Gelen starCount değeri ile ilgili karakteri güncelle
                    char[] OrtaStarChars = currentOrtaStar.ToCharArray();
                    OrtaStarChars[level - 1] = newStar;
                    string newOrtaStar = new string(OrtaStarChars);

                    // Güncellenmiş değeri veritabanına kaydet
                    OleDbCommand updateCommand = new OleDbCommand("UPDATE Register SET OrtaStar = @newOrtaStar WHERE KullaniciAdi = @username", connection);
                    updateCommand.Parameters.AddWithValue("@newOrtaStar", newOrtaStar);
                    updateCommand.Parameters.AddWithValue("@username", username);
                    updateCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating OrtaStar: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        
    }
}
