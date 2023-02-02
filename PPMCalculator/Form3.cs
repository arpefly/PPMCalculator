using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form3 : Form
    {
        readonly Form1 target_form;

        public Form3(Form1 _target)
        {
            InitializeComponent();
            target_form = _target;
            label2.Text = "0";
        }


        public static List<Gas> SearchArr = new List<Gas>();

        int Search = 0; // Число совпадений
        int ButtonClickCounter = 0;


        private void Label11_MouseLeave(object sender, EventArgs e) => label11.BackColor = Color.FromArgb(65, 65, 65);    // label11 меняет цвет что бы имитировать кноку
        private void Label11_MouseDown(object sender, MouseEventArgs e) => label11.BackColor = Color.FromArgb(142, 142, 142); // label11 меняет цвет что бы имитировать кноку
        private void Label11_MouseMove(object sender, MouseEventArgs e) // label11 меняет цвет что бы имитировать кноку и при наведение на label11 он меняет текст на "Записать"
        {
            label11.Text = "Удалить";
            label11.BackColor = Color.FromArgb(76, 76, 76);
        }
        void Form3_MouseDown(object sender, MouseEventArgs e)   // Перетаскивание формы
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }
        void Button3_Click(object sender, EventArgs e) => Close();  // Закрытие формы
        void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            label11.Text = "Удалить";
        }
        void TextBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            label11.Text = "Удалить";
            SearchFunction();
        }


        /// <summary>
        /// Поиск газов
        /// </summary>
        void SearchFunction()
        {
            string[] gas = File.ReadAllLines("Gas.csv", Encoding.GetEncoding(1251));


            if (textBox1.Text == "")
            {
                label2.Text = "0";
                label6.Text = "";
                label7.Text = "";
                label8.Text = "";
                label9.Text = "";
                label6.Text = "";
                label7.Text = "";
                label8.Text = "";
                ButtonClickCounter = 0;
            }
            else
            {
                try
                {
                    label11.Text = "Удалить";
                    Search = 0; // Число совпадений

                    for (int i = gas.GetLowerBound(0); i <= gas.GetUpperBound(0); i++)
                    {
                        if (gas[i].Contains(textBox1.Text))
                            Search += 1;
                    }
                    label2.Text = Search.ToString();    // Число совпадений

                    if (Search == 0)
                    {
                        label2.Text = "0";
                        label6.Text = "";
                        label7.Text = "";
                        label8.Text = "";
                        label9.Text = "";
                        label6.Text = "";
                        label7.Text = "";
                        label8.Text = "";
                        ButtonClickCounter = 0;
                    }

                    string[] FoundedItems = new string[Search];

                    for (int k = 0, i = gas.GetLowerBound(0); i <= gas.GetUpperBound(0); i++)
                    {
                        if (gas[i].Contains(textBox1.Text))
                        {
                            FoundedItems[k] = gas[i];
                            k++;
                        }
                    }

                    for (int i = 0; i < Search; i++)
                    {
                        string[] elements = FoundedItems[i].Split(';');
                        SearchArr.Add(new Gas());
                        SearchArr[i].GasFormula = elements[0];
                        SearchArr[i].GasName = elements[1];
                        SearchArr[i].GasMolarMass = Convert.ToSingle(elements[2]);
                    }

                    ButtonClickCounter = 0;
                    label9.Text = "";

                    label6.Text = SearchArr[ButtonClickCounter].GasFormula;
                    label7.Text = SearchArr[ButtonClickCounter].GasName;
                    label8.Text = SearchArr[ButtonClickCounter].GasMolarMass.ToString();
                    label9.Text = Convert.ToString(ButtonClickCounter + 1);

                }
                catch (Exception)
                {
                    SmoothAppearance("Введите верные параметры");
                }
            }
        }
        async void SmoothAppearance(string text)
        {
            for (byte r = 255, g = 255, b = 255; r >= 65 & g >= 65 & b >= 65; r -= 10, g -= 10, b -= 10, await Task.Delay(10))
                label11.ForeColor = Color.FromArgb(r, g, b);

            label11.Text = text;
            for (byte r = 65, g = 65, b = 65; r < 255 & g < 255 & b < 255; r += 10, g += 10, b += 10, await Task.Delay(10))
                label11.ForeColor = Color.FromArgb(r, g, b);
        }


        void Button1_Click(object sender, EventArgs e)
        {
            button4.Text = "Удалить";

            if (textBox1.Text != "")
            {
                try
                {
                    if (ButtonClickCounter < Search - 1)
                        ButtonClickCounter++;

                    label6.Text = SearchArr[ButtonClickCounter].GasFormula;
                    label7.Text = SearchArr[ButtonClickCounter].GasName;
                    label8.Text = SearchArr[ButtonClickCounter].GasMolarMass.ToString();
                }
                catch { }

                if (textBox1.Text != "")
                    label9.Text = Convert.ToString(1 + ButtonClickCounter);
                else
                    label9.Text = "";
            }

        }
        void Button2_Click(object sender, EventArgs e)
        {
            button4.Text = "Удалить";

            if (textBox1.Text != "")
            {
                try
                {
                    if (ButtonClickCounter > 0)
                        ButtonClickCounter--;

                    label6.Text = SearchArr[ButtonClickCounter].GasFormula;
                    label7.Text = SearchArr[ButtonClickCounter].GasName;
                    label8.Text = SearchArr[ButtonClickCounter].GasMolarMass.ToString();
                }
                catch { }

                if (textBox1.Text != "")
                    label9.Text = Convert.ToString(1 + ButtonClickCounter);
                else
                    label9.Text = "";
            }

        }


        void Label11_Click(object sender, EventArgs e)
        {
            string[] gas = File.ReadAllLines("Gas.csv", Encoding.GetEncoding(1251));
            string[] UpdatedGasList = new string[gas.Length - 1];

            if (comboBox1.SelectedIndex > -1)
            {
                try
                {
                    textBox1.Text = "";

                    gas[comboBox1.SelectedIndex] = "";

                    comboBox1.SelectedIndex = -1;

                    for (int k = 0, i = 0; i < gas.Length - 1;)
                    {
                        if (gas[k] == "")
                            k++;
                        else
                        {
                            UpdatedGasList[i] = gas[k];
                            i++;
                            k++;
                        }
                    }

                    File.Delete("Gas.csv");

                    for (int i = 0; i < UpdatedGasList.Length; i++)
                    {
                        if (i == 0)
                            File.AppendAllText("Gas.csv", UpdatedGasList[i], Encoding.GetEncoding(1251));
                        else
                            File.AppendAllText("Gas.csv", "\n" + UpdatedGasList[i], Encoding.GetEncoding(1251));
                    }

                    target_form.AddGasToCombo();
                    AddNewToComboForm3();
                    SearchFunction();

                    SmoothAppearance("Удаленно");
                }
                catch { }
            }
            else
            {
                try
                {
                    int[] IndexOfFoundedItemIngasArray = new int[Search];

                    for (int k = 0, i = gas.GetLowerBound(0); i <= gas.GetUpperBound(0); i++)
                    {
                        if (gas[i].Contains(textBox1.Text))
                        {
                            IndexOfFoundedItemIngasArray[k] = i;
                            k++;
                        }
                    }

                    gas[IndexOfFoundedItemIngasArray[ButtonClickCounter]] = "";

                    for (int k = 0, i = 0; i < gas.Length - 1;)
                    {
                        if (gas[k] == "")
                            k++;
                        else
                        {
                            UpdatedGasList[i] = gas[k];
                            i++;
                            k++;
                        }
                    }

                    File.Delete("Gas.csv");

                    for (int i = 0; i < UpdatedGasList.Length; i++)
                    {
                        if (i == 0)
                            File.AppendAllText("Gas.csv", UpdatedGasList[i], Encoding.GetEncoding(1251));
                        else
                            File.AppendAllText("Gas.csv", "\n" + UpdatedGasList[i], Encoding.GetEncoding(1251));
                    }

                    target_form.AddGasToCombo();
                    AddNewToComboForm3();
                    SearchFunction();

                    SmoothAppearance("Удаленно");
                }
                catch { }

            }
        }
        

        void AddNewToComboForm3()    // Заполнение comboBox'а
        {
            try
            {
                string[] gas = File.ReadAllLines("Gas.csv", Encoding.GetEncoding(1251));    // Чтение csv файла

                string[] ForCombo = new string[gas.Length];

                for (int i = 0; i < gas.Length; i++)
                {
                    string[] elements = gas[i].Split(';');  // Split csv файла
                    SearchArr.Add(new Gas());
                    SearchArr[i].GasFormula = elements[0];                      // Заполнение параметров газа
                    SearchArr[i].GasName = elements[1];                         //
                    SearchArr[i].GasMolarMass = Convert.ToSingle(elements[2]);  //

                    ForCombo[i] = SearchArr[i].GasName + " (" + SearchArr[i].GasFormula + ")";  // Заполнение массива для ComboBox'а
                }

                comboBox1.Items.Clear();    // Отчистка ComboBox'а перед заполнением и записью
                comboBox1.Items.AddRange(ForCombo); // Заполнение ComboBox'а
            }
            catch (Exception ex)
            {
                MessageBox.Show("csv файл записан не правильно.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        void Form3_Load(object sender, EventArgs e) => AddNewToComboForm3();

    }
}