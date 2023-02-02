using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static List<Gas> gasesList = new List<Gas>();

        /// <summary>
        /// Чтобы после записи новых газов не сбрасывалось значение ComboBox1'а
        /// </summary>
        int comboBox1LastSelectedIndex;

        double temperature;     // Темепартура °K
        float soughtUnits,      // Числовое значение концентрации в искомых единицах
              molarMass,        // Молекулярная масса газа
              totalGasPressure; // Общее давление газовой смеси


        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)  // Заменяет б Б ю Ю . < > / | \ ? на ',' и разрешает ввод тоько цифр
        {
            if (e.KeyChar == '<' || e.KeyChar == 'б' || e.KeyChar == 'Б' || e.KeyChar == 'ю' || e.KeyChar == 'Ю' || e.KeyChar == 'Э' || e.KeyChar == 'ж' || e.KeyChar == 'Ж' || e.KeyChar == 'э' || e.KeyChar == '.' || e.KeyChar == '>' || e.KeyChar == '?' || e.KeyChar == '/' || e.KeyChar == '"' || e.KeyChar == ';' || e.KeyChar == ':' || e.KeyChar == '\'' || e.KeyChar == '\\' || e.KeyChar == '|')
                e.KeyChar = ',';

            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-' || (textBox1.Text.Contains(",") && e.KeyChar == ',') || (textBox1.Text.Contains("-") && e.KeyChar == '-'))
                e.Handled = true;
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)   // Перетаскивание формы
        {
            Capture = false;
            Message m = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }
        private void Button3_Click(object sender, EventArgs e)  // Открытие второй формы (запись новых газов)
        {
            Form2 f2 = new Form2(this);
            f2.Show();
        }
        private void Button8_Click(object sender, EventArgs e)  // Открытие третьей формы (Удаление газов)
        {
            Form3 f3 = new Form3(this);
            f3.Show();
        }
        private void Button2_Click(object sender, EventArgs e) => Application.Exit();  // Кнопка закрыти формы
        private void Button4_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized; // Сворачивание формы 
        async void SmoothAppearance(string text)    // Плавное паявлени текста на кнопке
        {
            for (byte r = 255, g = 255, b = 255; r >= 65 & g >= 65 & b >= 65; r -= 10, g -= 10, b -= 10, await Task.Delay(10))
                label11.ForeColor = Color.FromArgb(r, g, b);

            label11.Text = text;
            for (byte r = 65, g = 65, b = 65; r < 255 & g < 255 & b < 255; r += 10, g += 10, b += 10, await Task.Delay(10))
                label11.ForeColor = Color.FromArgb(r, g, b);
        }



        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e) // Пересчет значений автоматически (при изменении содержания texBox'ов (1, 2, 3) и ComboBox'ов (1, 2)
        {

            try    // texBox'ы должны быть заполнены
            {

                soughtUnits = Convert.ToSingle(textBox3.Text);                                  // Присваивание значений
                totalGasPressure = Convert.ToSingle(textBox2.Text) * 1000;                      //
                temperature = Convert.ToDouble(textBox1.Text) + 273.15;                         //
                molarMass = Convert.ToSingle(gasesList[comboBox1.SelectedIndex].GasMolarMass);      //
                label10.Text = Convert.ToString(gasesList[comboBox1.SelectedIndex].GasMolarMass);   // Вывод молярной массы текущего газа
                comboBox1LastSelectedIndex = comboBox1.SelectedIndex;                           // Последний выбраный элимент Combobox1'а. Что-бы после запитиси новых гозов занчание Combobox1'а не сбрасывалось
                label11.Text = "";
            }
            catch
            { 
                SmoothAppearance("Введите верные параметры");
                label7.Text = "";
                label8.Text = "";
                label9.Text = "";
                return;
            }

            label4.ForeColor = Color.White;
            label5.ForeColor = Color.White;
            label6.ForeColor = Color.White;

            label7.ForeColor = Color.White;
            label8.ForeColor = Color.White;
            label9.ForeColor = Color.White;

            // Конвертертация единиц концентрации газов 
            if (comboBox2.SelectedIndex == 0)      // ppm
            {
                label4.ForeColor = Color.Gray;
                label7.ForeColor = Color.Gray;

                label7.Text = textBox3.Text;  //pmm > pmm
                label8.Text = (0.12 * 0.001 * soughtUnits * molarMass * totalGasPressure / temperature).ToString("0.####"); // ppm > мг/м3
                label9.Text = String.Format("{0:0.0000}", soughtUnits * 0.0001);   	
            }
            else if (comboBox2.SelectedIndex == 1) // мг/м3
            {
                label5.ForeColor = Color.Gray;
                label8.ForeColor = Color.Gray;

					label7.Text = (8312.6 * soughtUnits * temperature / (molarMass * totalGasPressure)).ToString("0.####");  // мг/м3 > ppm
					label8.Text = textBox3.Text;  // мг/м3 > мг/м3
					label9.Text = String.Format("{0:0.0000}", 8312.6 * 0.0001 * soughtUnits * temperature / (molarMass * totalGasPressure));   // мг/м3 > % 
            }
            else if (comboBox2.SelectedIndex == 2)  // %
            {
                label6.ForeColor = Color.Gray;
                label9.ForeColor = Color.Gray;

                label7.Text = (10000 * soughtUnits).ToString("#.####");    // % > ppm
                label8.Text = (0.12 * 0.1 * soughtUnits * molarMass * totalGasPressure / temperature).ToString("#.####");  // % > мг/м3
                label9.Text = textBox3.Text;  // % > %
            }
        }


        private void Label12_DoubleClick(object sender, EventArgs e)    
        {
            label12.BringToFront();
            label1.BringToFront();
            label2.BringToFront();
            label3.BringToFront();

            label12.Location = new Point(0, 0);
            label12.Size = new Size(300, 300);

            label12.Text = "О программе";
            label1.Text = "Калькулятор единиц концентрации газов v 1.3";
            label2.Text = "Разработчик: Глухов Арсений 2020г\nugarvlog@gmail.com";
            label3.Text = "\nНазад";
        }// О программе
        private void Label3_Click(object sender, EventArgs e)           
        {
            label1.Text = "Температура °C";
            label2.Text = "Давление мПа";
            label3.Text = "Концентрация";
            label12.Text = "калькулятор";

            label12.Location = new Point(5, 5);
            label12.Size = new Size(153, 25);
        }//


        /// <summary>
        /// SQL запрос
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<object> SqlQuery(string query)   // Запрос в Элму
        {
            //SqlDataReader reader = null;
            string connectionString = "connectionString";
            string sql = query;
            List<object> listRead = new List<object>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                                listRead.Add(reader[i].ToString());
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к ЭЛМА.\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listRead = null;
            }
            return listRead;
        }
        private void Button6_Click(object sender, EventArgs e)  // Данные из Элмы
        {
            string SQL = "query";
            List<object> result = SqlQuery(SQL);
            if (result != null)
            {
                if (result.Count != 0)
                {
                    textBox1.Text = result[0].ToString();
                    textBox2.Text = result[1].ToString();
                    return;
                }
            }
            MessageBox.Show("В базе данных не установлены параметры окружающей среды на текущий день.", "Внимание");
        }
        private void Button5_Click_1(object sender, EventArgs e)    // Данные при н.у.
        {
            textBox1.Text = "20";
            textBox2.Text = "101";
            textBox3.Text = "50";
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;    // Значение ComboBox1'а по-умолчанию
            comboBox2.SelectedIndex = 0;    // Значение ComboBox2'а по-умолчанию
            textBox1.Text = "20";           // Значения texBox'ов по-умолчанию
            textBox2.Text = "101";          //
            textBox3.Text = "50";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists($@"{Environment.CurrentDirectory}\Gas.csv"))
            {
                DialogResult dialogResult = MessageBox.Show("Не удалось найти исходный файл формата .csv\r\n\r\n" +
                    "Создайте исходный файл в формате .csv и поместите его в корневую папку\r\n" +
                    "Формат записи .csv файла: «Формула_газа»;«Название газа»;«Моляраня,масса»\r\n\r\n" +
                    "Если в корневой папке уже существует исходный файл переименуйте его в \"Gas.csv\"\r\n\r\n" +
                    "Перейти в корневую папку ?", "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo { FileName = "explorer", Arguments = $@"/n, /select, {Assembly.GetExecutingAssembly().Location}" });

                Close();
                return;
            }

            try
            {
                AddGasToCombo();
            }
            catch (Exception ex)
            {
                DialogResult dialogResult = MessageBox.Show($"{ex.Message}\r\n\r\nИсходный файл .csv записан не правильно.\r\n" +
                    "Формат записи.csv файла: «Формула_газа»;«Название газа»;«Моляраня,масса»\r\n\r\n" +
                    "Переименуйте исходный файл .csv в \"Gas.csv\"\r\n\r\n" +
                    "Перейти к файлу ?", "Ошибка",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                if (dialogResult == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo { FileName = "explorer", Arguments = $@"/n, /select, {Environment.CurrentDirectory}\Gas.csv" });

                Close();
                return;
            }
        }
        public void AddGasToCombo()    // Чтение csv файла и заполнение ComboBox'а
        {
            string[] gas = File.ReadAllLines("Gas.csv", Encoding.GetEncoding(1251));    // Чтение csv файла 
            string[] ForCombo = new string[gas.Length];	

            for (int i = 0; i < gas.Length; i++)
            {
                string[] elements = gas[i].Split(';');  // Split csv файла
                gasesList.Add(new Gas());   
                gasesList[i].GasFormula = elements[0];                      // Заполнение параметров газа
                gasesList[i].GasName = elements[1];                         //
                gasesList[i].GasMolarMass = Convert.ToSingle(elements[2]);  //

                ForCombo[i] = gasesList[i].GasName + " (" + gasesList[i].GasFormula + ")";  // Заполнение массива для ComboBox'а
            }

            comboBox1.Items.Clear();    // Отчистка ComboBox'а перед заполнением
            comboBox1.Items.AddRange(ForCombo); // Заполнение ComboBox'а
            comboBox1.SelectedIndex = comboBox1LastSelectedIndex;
        }

    }
}