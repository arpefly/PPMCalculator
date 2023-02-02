using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form2 : Form
    {
        readonly Form1 target_form;

        public Form2(Form1 _target)
        {
            InitializeComponent();
            target_form = _target;
        }

        public static List<Gas> gasesList= new List<Gas>();

        void Label5_Click(object sender, EventArgs e)   // Запись в csv файл
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox3.Text.Contains(",,"))  // Разрешаетс ввод тоько одной запятй
                SmoothAppearance("Введите верные параметры");
            else
            {
                File.AppendAllText("Gas.csv", "\n" + textBox2.Text + ";" + textBox1.Text + ";" + textBox3.Text, Encoding.GetEncoding(1251));
                target_form.AddGasToCombo();

                SmoothAppearance("Записанно");
            }

        }

        void TextBox3_KeyPress(object sender, KeyPressEventArgs e)  // Разрешен ввод только чи  сел, Backspace, запятая ","
        {
            if (e.KeyChar == 1073 || e.KeyChar == 1041 || e.KeyChar == 60 || e.KeyChar == 1102 || e.KeyChar == 1070 || e.KeyChar == 46 || e.KeyChar == 62 || e.KeyChar == 63 || e.KeyChar == 47 || e.KeyChar == 92 || e.KeyChar == 124)
                e.KeyChar = ',';

            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != ',' || (textBox3.Text.Contains(",") && e.KeyChar == ','))
                e.Handled = true;
        }

        private void Label5_MouseDown(object sender, MouseEventArgs e) => label5.BackColor = Color.FromArgb(142, 142, 142); // label5 меняет цвет что бы имитировать кноку
        private void Label5_MouseMove(object sender, MouseEventArgs e)  // label5 меняет цвет что бы имитировать кноку и при наведение на label5 он меняет текст на "Записать"
        {
            label5.Text = "Записать";
            label5.BackColor = Color.FromArgb(76, 76, 76);
        }
        private void Label5_MouseLeave(object sender, EventArgs e) => label5.BackColor = Color.FromArgb(65, 65, 65);    // label5 меняет цвет что бы имитировать кноку
        async void SmoothAppearance(string text)    // Плавное паявлени текста на кнопке
        {
            for (byte r = 255, g = 255, b = 255; r >= 65 & g >= 65 & b >= 65; r -= 10, g -= 10, b -= 10, await Task.Delay(10))
                label5.ForeColor = Color.FromArgb(r, g, b);

            label5.Text = text;
            for (byte r = 65, g = 65, b = 65; r < 255 & g < 255 & b < 255; r += 10, g += 10, b += 10, await Task.Delay(10))
                label5.ForeColor = Color.FromArgb(r, g, b);
        }
        void Button2_Click(object sender, EventArgs e) => Close();  // Закрытие формы
        void Form2_MouseDown(object sender, MouseEventArgs e)   // Перетаскивание формы
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }


    }
}