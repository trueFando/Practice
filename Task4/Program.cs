using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            // получаем путь к файлу входных данных
            string filePath = ConfigurationManager.AppSettings.Get("inputFilePath");

            DateFrame[] dateFrames;
            int numberOfFrames;

            // создаем поток для чтения
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                // размерность массива
                numberOfFrames = NumberOfFrames(streamReader.ReadToEnd());

                // сюда сохраняем все строчки
                string[] allData = new string[numberOfFrames];
                
                // из allData собираем массив своей структуры
                dateFrames = new DateFrame[numberOfFrames];

                // ставим указатель в начало
                streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < numberOfFrames; i++)
                {
                    // читаем строчку с датой+временем и кадром
                    allData[i] = streamReader.ReadLine();

                    // отделяем дату+время от кадра
                    string date = allData[i].Substring(0, allData[i].IndexOf(' '));
                    string frame = allData[i].Substring(allData[i].IndexOf(' ') + 1, allData[i].Length - date.Length - 1);

                    // проверяем на наличие игнорируемой последовательности
                    if (frame.Substring(0, 8) == "1ACFFC1D") 
                        frame = frame.Substring(8, frame.Length-8);

                    // записываем элемент в массив
                    dateFrames[i].SetDateAndFrame(date, frame);
                }
            }

            // тест
            Console.WriteLine($"Кадр 0, дней с 1900: {dateFrames[0].DaysFrom1900}, " +
                $"в двоичном виде: {Convert.ToString(dateFrames[0].DaysFrom1900, 2)}");
            Console.WriteLine($"Кадр 0, мс с начала суток: {dateFrames[0].Milliseconds}, " +
                $"в двоичном виде: {Convert.ToString(dateFrames[0].Milliseconds, 2)}");
            Console.WriteLine($"Кадр 0, ТМ кадр: {dateFrames[0].Frame}");
        }

        // ищем количество переносов строки в input - столько у нас фреймов
        private static int NumberOfFrames(string data)
        {
            return new Regex("\n").Matches(data).Count;
        }
    }
}
