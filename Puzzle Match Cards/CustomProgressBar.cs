using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Puzzle_Match_Cards
{
    class CustomProgressBar : ProgressBar
    {
        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = this.ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);
            rect.Inflate(-3, -3);

            if (this.Value > 0)
            {
                // Çubuğun dolu kısmını hesapla
                Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)this.Value / this.Maximum) * rect.Width), rect.Height);
                using (SolidBrush brush = new SolidBrush(GetProgressColor((float)this.Value / this.Maximum)))
                {
                    g.FillRectangle(brush, clip);
                }
            }
        }

        private Color GetProgressColor(float progressPercentage)
        {
            if (progressPercentage >= 0.6f)
            {
                // %100 ile %50 arasında yeşil
                return InterpolateColor(Color.FromArgb(255, 255, 128), Color.SpringGreen, (progressPercentage - 0.6f) / 0.4f);
            }
            else if (progressPercentage >= 0.4f)
            {
                // %50 ile %20 arasında turuncu
                return InterpolateColor(Color.FromArgb(255, 224, 192), Color.FromArgb(255, 255, 128), (progressPercentage - 0.4f) / 0.2f);
            }
            else if (progressPercentage >= 0.2f)
            {
                // %50 ile %20 arasında turuncu
                return InterpolateColor(Color.Orange, Color.FromArgb(255, 224, 192), (progressPercentage - 0.2f) / 0.2f);
            }
            else
            {
                // %20 ile %0 arası kırmızı
                return InterpolateColor(Color.Red, Color.Orange, progressPercentage / 0.2f);
            }
        }

        private Color InterpolateColor(Color color1, Color color2, float fraction)
        {
            float r = color1.R + (color2.R - color1.R) * fraction;
            float g = color1.G + (color2.G - color1.G) * fraction;
            float b = color1.B + (color2.B - color1.B) * fraction;
            return Color.FromArgb((int)r, (int)g, (int)b);
        }

    }
}
