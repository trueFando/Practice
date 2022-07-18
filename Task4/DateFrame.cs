using System;

namespace Task4
{
    struct DateFrame
    {
        public string DateAndTme
        {
            get;
            private set;
        }

        public string Frame
        {
            get;
            private set;
        }

        public DateTime Date
        {
            get
            {
                string date = DateAndTme.Substring(0, DateAndTme.IndexOf('T'));

                return DateTime.Parse(date);
            }
        }

        public int Milliseconds
        {
            get
            {
                string time = DateAndTme.Substring(DateAndTme.IndexOf('T') + 1,
                    DateAndTme.Length - DateAndTme.Substring(0, DateAndTme.IndexOf('T')).Length - 1);

                DateTime dt = DateTime.Parse(time).AddHours(-3);
                return (int) dt.TimeOfDay.TotalMilliseconds;
            }
        }

        public int DaysFrom1900
        {
            get
            {
                DateTime dt1900 = new DateTime(1900, 1, 1);
                return (int)(Date - dt1900).TotalDays;
            }
        }

        public void SetDateAndFrame(string date, string frame)
        {
            DateAndTme = date;
            Frame = frame;
        }
    }
}
