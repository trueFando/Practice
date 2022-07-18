using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    static class RKIntegrator
    {

        // тензор инерции КА
        readonly static double[] J = new double[9] {1000, 0, 0, 
                                                                0, 1200, 0,  
                                                                0, 0, 1305};

        // инвертированный тензор инерции КА
        readonly static double[] Jinv = new double[9] {1/1000, 0, 0,
                                                                    0, 1/1200, 0,
                                                                    0, 0, 1/1305};
        // матрица ориентации
        static double[] M = new double[3];

        // перемножение матриц 3х3
        private static void MatrixMultiply3x3(double[] a, double[] b, double[] c)
        {
            c[0] = a[0] * b[0] + a[1] * b[3] + a[2] * b[6];
            c[1] = a[0] * b[1] + a[1] * b[4] + a[2] * b[7];
            c[2] = a[0] * b[2] + a[1] * b[5] + a[2] * b[8];

            c[3] = a[3] * b[0] + a[4] * b[3] + a[5] * b[6];
            c[4] = a[3] * b[1] + a[4] * b[4] + a[5] * b[7];
            c[5] = a[3] * b[2] + a[4] * b[5] + a[5] * b[8];

            c[3] = a[6] * b[0] + a[7] * b[3] + a[8] * b[6];
            c[4] = a[6] * b[1] + a[7] * b[4] + a[8] * b[7];
            c[5] = a[6] * b[2] + a[7] * b[5] + a[8] * b[8];
        }

        // перемножение матриц 3х1
        private static void MatrixMultiply3x1(double[] a, double[] b, double[] c)
        {
            c[0] = a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
            c[1] = a[3] * b[0] + a[4] * b[1] + a[5] * b[2];
            c[2] = a[6] * b[0] + a[7] * b[1] + a[8] * b[2];
        }

        // векторное произведение 
        private static void MatrixVec(double[] a, double[] b, double[] c)
        {
            c[0] = a[1] * b[2] - a[2] * b[1];
            c[1] = -a[0] * b[2] + a[2] * b[0];
            c[2] = a[0] * b[1] - a[1] * b[0];
        }

        // ф-я правых частей
        private static void F(double t, double[] x, double[] dx)
        {
            // 
            double[] aw = {0, -x[11], x[10],
                                x[11], 0, -x[9],
                                -x[10], x[9], 0};

            MatrixMultiply3x3(aw, x, dx);
            double[] tmp = new double[3], tmp2 = new double[3];
            double[] x9 = new double[3];
            Array.Copy(x, 9, x9, 0, 3);
            MatrixMultiply3x1(J, x9, tmp); // Jw
            MatrixVec(x9, tmp, tmp2);
            for (int i = 0; i < 3; i++) tmp[i] = M[i] - tmp2[i]; // M - w x Jw

            double[] dx9 = new double[3];
            Array.Copy(dx, 9, dx9, 0, 3);

            MatrixMultiply3x1(Jinv, tmp, dx9);
        }

        // интегрирование
        public static void RK(double t, double[] y, double[] yk, double h)
        {
            double h2 = h / 2;
            double[] y1 = new double[12], y2 = new double[12], y3 = new double[12],
                        y4 = new double[12], yy = new double[12];

            F(t, y, y1);
            for (int i = 0; i < 12; i++)
            {
                yy[i] = y[i] + h2 * y1[i];
            }

            F(t + h2, yy, y2);
            for (int i = 0; i < 12; i++)
            {
                yy[i] = y[i] + h2 * y2[i];
            }

            F(t + h2, yy, y3);
            for (int i = 0; i < 12; i++)
            {
                yy[i] = y[i] + h * y3[i];
            }

            F(t + h, yy, y4);

            for (int i = 0; i < 12; i++)
            {
                yk[i] = y[i] + h * (y1[i] + y4[i] + 2 * (y2[i] + y3[i])) / 6;
            }
        }
    }
}
