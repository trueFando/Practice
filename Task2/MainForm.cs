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

namespace Task2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        // коэффициенты закона управления
        private double kF = 0.3, kW = 12, Eps = 0.1;

        private void GetM(double[] At, double[] result)
        {
            result[0] = -kF * Math.Atan2(-At[5], At[4]) - kW * At[9];
            result[1] = -kF * Math.Atan2(-At[6], At[0]) - kW * At[10];
            result[2] = -kF * Math.Asin(At[3]) - kW * At[11];

            for (int i = 0; i < 3; i++)
            {
                if (Math.Abs(result[i]) > Eps)
                {
                    result[i] = Math.Sign(result[i]) * Eps;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double endTime = 10000;
            double h = 0.01;
            double[] At = {0.99900827050126, -0.0397815132178866, -0.0199976667683312, 
                                0.039987334736587, 0.999150147071977, 0.00999983333416666, 
                                0.0195828631907146, -0.0107895695994827, 0.999750017082826,
                                0, 0, 0};

            double time = 0;

            string filePath = "C://Texts/task2.txt";
            using (StreamWriter sr = new StreamWriter(filePath))
            {
                for (int i = 0; time < endTime; i++)
                {
                    double[] newAt = new double[12];
                    RKIntegrator.RK(time, At, newAt, h);

                    for (int j = 0; j < 12; j++) At[j] = newAt[j];

                    if (i % 100 == 0)
                    {
                        double[] controlM = new double[3];
                        GetM(At, controlM);
                        sr.WriteLine($"{controlM[0]} {controlM[1]} {controlM[2]}");
                    }
                    time += h;
                }
                
            }
        }

        
    }
}
