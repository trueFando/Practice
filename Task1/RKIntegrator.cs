using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    static class RKIntegrator
    {
        private static void F(double t, double[] x, double[] dx)
        {
            dx[0] = x[3];
            dx[1] = x[4];
            dx[2] = x[5];

            double mr = Math.Sqrt(x[0] * x[0] + x[1] * x[1] + x[2] * x[2]);
            double mr3 = mr * mr * mr;
            double mu = 398600.4418;
            double a = -(mu / mr3);
            dx[3] = a * x[0];
            dx[4] = a * x[1];
            dx[5] = a * x[2];
        }

        public static void RK(double t, double[] y, double[] yk, double h)
        {
            double h2 = h / 2;
            double[] y1 = new double[6], y2 = new double[6], y3 = new double[6], 
                        y4 = new double[6], yy = new double[6];

            F(t, y, y1);
            for (int i = 0; i < 6; i++)
            {
                yy[i] = y[i] + h2 * y1[i];
            }

            F(t + h2, yy, y2);
            for (int i = 0; i < 6; i++)
            {
                yy[i] = y[i] + h2 * y2[i];
            }

            F(t + h2, yy, y3);
            for (int i = 0; i < 6; i++)
            {
                yy[i] = y[i] + h * y3[i];
            }

            F(t + h, yy, y4);

            for(int i = 0; i < 6; i++)
            {
                yk[i] = y[i] + h * (y1[i] + y4[i] + 2 * (y2[i] + y3[i])) / 6;
            }
        }
    }
}
