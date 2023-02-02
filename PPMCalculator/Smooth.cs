using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPMCalculator
{
    public class Smooth
    {
        public async void SmoothAppearance(string text, Label label)
        {
            for (byte r = 255, g = 255, b = 255; r >= 65 & g >= 65 & b >= 65; r -= 10, g -= 10, b -= 10, await Task.Delay(10))
                label.ForeColor = Color.FromArgb(r, g, b);

            label.Text = text;
            for (byte r = 65, g = 65, b = 65; r < 255 & g < 255 & b < 255; r += 10, g += 10, b += 10, await Task.Delay(10))
                label.ForeColor = Color.FromArgb(r, g, b);
        }
    }
}