using System.Configuration;
using System.IO;
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

            string filePathOut = ConfigurationManager.AppSettings.Get("outputFilePath");
            using (BinaryWriter br = new BinaryWriter(File.Open(filePathOut, FileMode.OpenOrCreate)))
            {
                foreach (DateFrame df in dateFrames)
                {
                    br.Write(df.DaysFrom1900);
                    br.Write(df.Milliseconds);
                    br.Write(df.Frame);
                }
            }
        }

        // ищем количество переносов строки в input - столько у нас фреймов
        private static int NumberOfFrames(string data)
        {
            return new Regex("\n").Matches(data).Count;
        }
    }
}
