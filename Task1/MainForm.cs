using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 04.06.2023 15:19:13.100
            double[] x = {-14849.674121, -6184.115762, 5525.320326, 2.914687, -0.917413, -4.746170}; // вар3 
            //double[] x = { -1195.712, -829.495, -6818.185, 1.954065, 7.195319, -1.222097 }; // вар 1
            double[] xk = new double[6];
            double t = 0, tk = 86400, h = 0.1;
            
            string filePath = "E:\\out.txt";

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                int counter = 0;
                while (t <= tk)
                {
                    RKIntegrator.RK(t, x, xk, h);
                    for (int i = 0; i < 3; i++)
                    {
                        x[i] = xk[i];
                    }
                    if (counter % 1200 == 0)
                    {
                        WriteFile(x[0], x[1], x[2], streamWriter);
                    }
                    t += h;
                    counter++;
                }
            }
            
        }

        private static void WriteFile(double x0, double x1, double x2, StreamWriter writer)
        {
            writer.WriteLine($"{x0}, {x1}, {x2}");
        }
    }
}
