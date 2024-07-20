using System.Drawing;

namespace Puya.Captcha
{
    public class SimpleCaptchaOptions
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public Font Font { get; set; }
        public string Characters { get; set; }
        public Brush FontColor { get; set; }
        public Color LinesColor { get; set; }
        public Color NoiseColor { get; set; }
        public SimpleCaptchaOptions()
        {
            Height = 45;
            Width = 160;
            Length = 6;
            Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            FontColor = Brushes.Black;
            LinesColor = Color.Gray;
            NoiseColor = Color.Silver;
            Font = new Font("Times New Roman", 24, FontStyle.Italic);
        }
    }
}
