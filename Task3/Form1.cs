using System;
using System.Linq;
using System.Windows.Forms;

namespace Task3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // интервал времени
            double tStart = 0;
            double tFinish = 300;
            
            // шаг времени
            double h = 0.1;

            // размер таблицы
            int sizeX = (int) Math.Floor((tFinish - tStart) / h + 1), sizeY = 2; 

            // таблица 
            double[,] table = new double[sizeX, sizeY];
            double t = tStart;

            // среднеквадратичное отклонение
            double sigma = 0.01;

            // заполняем таблицу задаваемой функции
            for (int i = 0; i < sizeX; i++)
            {
                table[i, 0] = t;
                table[i, 1] = F(t) + GaussRandom(sigma);
                t += h;
            }

            double[] a = new double[sizeX - 10];
            double[] b = new double[sizeX - 10];
            int n = 10;

            // определяем коэффициенты линейного аппроксимирующего полинома
            for (int i = 0; i < sizeX - n; i++)
            {
                double tySum = 0;
                double tSum = 0;
                double t2Sum = 0;
                double ySum = 0;
                for (int j = i; j < i + n; j++)
                {
                    tySum += table[j, 0] * table[j, 1];
                    tSum += table[j, 0];
                    t2Sum += table[j, 0] * table[j, 0];
                    ySum += table[j, 1];
                }
                a[i] = (n * tySum - tSum * ySum) / (n * t2Sum - tSum * tSum);
                b[i] = (ySum - a[i] * tSum) / n;
                t += h;
            }

            double[] dK = new double[sizeX - 10];

            // находим значения невязок
            for (int i = 0; i < sizeX - 10; i++)
                dK[i] = table[i + 5, 1] - a[i] * table[i + 5, 0] + b[i];

            double dkSum = dK.Sum();

            // среднеквадратичное значение невязок
            double rootMeanSquare_dK = Math.Sqrt(Math.Abs(dkSum / (sizeX - 10)));

            // вывод в интерфейс
            RMSLabel.Text = rootMeanSquare_dK.ToString();
            DrawPlot(0, sizeX - 10, h, dK);
        }

        // f(t) по условию
        private static double F(double t)
        {
            return 0.2 * Math.Sin(t) + 1;
        }

        // Преобразование Бокса-Мюллера
        private static double GaussRandom(double sigma)
        {
            Random rand = new Random(); 
            double u1 = 1.0 - rand.NextDouble(); 
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); 
            double randNormal = 0 + sigma * randStdNormal;

            return randNormal;
        }

        // метод построения графика
        private void DrawPlot(double a, double b, double h, double[] points)
        {
            chart1.Series[0].Points.Clear();

            double x = a;
            foreach (double y in points)
            {
                chart1.Series[0].Points.AddXY(x, y);
                x += h;
            }
        }
    }
}
