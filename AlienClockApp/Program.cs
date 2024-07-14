using System;

namespace AlienClockApp
{
    class Program
    {
        // Constants for Alien time conversion
        const double EarthToAlienRatio = 0.5;
        const int AlienSecondsInMinute = 90;
        const int AlienMinutesInHour = 90;
        const int AlienHoursInDay = 36;
        const int AlienMonthsInYear = 18;
        static readonly int[] AlienDaysPerMonth = { 44, 42, 48, 40, 48, 44, 40, 44, 42, 40, 40, 42, 44, 48, 42, 40, 44, 38 };

        // Variables for Alien time
        static int alienYear = 2804;
        static int alienMonth = 18;
        static int alienDay = 31;
        static int alienHour = 2;
        static int alienMinute = 2;
        static int alienSecond = 88;

        // Variables for Alarm
        static bool alarmSet = false;
        static int alarmYear, alarmMonth, alarmDay, alarmHour, alarmMinute, alarmSecond;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Alien Clock App!");

            // Start a timer to update Alien clock every second
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000; // 1 second
            timer.Elapsed += TimerElapsed;
            timer.Start();

            // User interaction loop
            while (true)
            {
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Set Earth Time");
                Console.WriteLine("2. Display Alien Time");
                Console.WriteLine("3. Display Earth Time");
                Console.WriteLine("4. Set Alarm");
                Console.WriteLine("5. Exit");

                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        SetEarthTime();
                        break;
                    case "2":
                        DisplayAlienTime();
                        break;
                    case "3":
                        DisplayEarthTime();
                        break;
                    case "4":
                        SetAlarm();
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Method to update Alien clock every second
        private static void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Update Alien time based on Earth time elapsed
            double earthSecondsElapsed = e.SignalTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            double alienSecondsElapsed = earthSecondsElapsed / EarthToAlienRatio;

            // Calculate Alien time components
            alienSecond = (int)(alienSecondsElapsed % AlienSecondsInMinute);
            alienMinute = (int)((alienSecondsElapsed / AlienSecondsInMinute) % AlienMinutesInHour);
            alienHour = (int)(((alienSecondsElapsed / AlienSecondsInMinute) / AlienMinutesInHour) % AlienHoursInDay);
            alienDay = 1 + (int)(((alienSecondsElapsed / AlienSecondsInMinute) / AlienMinutesInHour) / AlienHoursInDay);

            // Handle month and year overflow
            while (alienDay > AlienDaysPerMonth[alienMonth - 1])
            {
                alienDay -= AlienDaysPerMonth[alienMonth - 1];
                alienMonth++;
                if (alienMonth > AlienMonthsInYear)
                {
                    alienMonth = 1;
                    alienYear++;
                }
            }

            // Check for alarm
            if (alarmSet && alienYear == alarmYear && alienMonth == alarmMonth && alienDay == alarmDay &&
                alienHour == alarmHour && alienMinute == alarmMinute && alienSecond == alarmSecond)
            {
                Console.WriteLine("ALARM! ALIEN TIME REACHED: {0}-{1}-{2}, {3}:{4}:{5}", alarmYear, alarmMonth, alarmDay, alarmHour, alarmMinute, alarmSecond);
                alarmSet = false; // Reset alarm
            }
        }

        // Method to set Earth time
        private static void SetEarthTime()
        {
            Console.Write("Enter Earth Date and Time (yyyy-MM-dd HH:mm:ss): ");
            string input = Console.ReadLine();
            DateTime earthTime;
            if (DateTime.TryParse(input, out earthTime))
            {
                // Convert Earth time to Alien time
                double earthSeconds = earthTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                double alienSeconds = earthSeconds / EarthToAlienRatio;

                // Update Alien time components
                alienSecond = (int)(alienSeconds % AlienSecondsInMinute);
                alienMinute = (int)((alienSeconds / AlienSecondsInMinute) % AlienMinutesInHour);
                alienHour = (int)(((alienSeconds / AlienSecondsInMinute) / AlienMinutesInHour) % AlienHoursInDay);
                alienDay = 1 + (int)(((alienSeconds / AlienSecondsInMinute) / AlienMinutesInHour) / AlienHoursInDay);

                // Handle month and year overflow
                while (alienDay > AlienDaysPerMonth[alienMonth - 1])
                {
                    alienDay -= AlienDaysPerMonth[alienMonth - 1];
                    alienMonth++;
                    if (alienMonth > AlienMonthsInYear)
                    {
                        alienMonth = 1;
                        alienYear++;
                    }
                }

                Console.WriteLine($"Alien Time updated to: {alienYear}-{alienMonth}-{alienDay}, {alienHour}:{alienMinute}:{alienSecond}");
            }
            else
            {
                Console.WriteLine("Invalid Earth time format. Please enter in yyyy-MM-dd HH:mm:ss format.");
            }
        }

        // Method to display Alien time
        private static void DisplayAlienTime()
        {
            Console.WriteLine($"Current Alien Time: {alienYear}-{alienMonth}-{alienDay}, {alienHour}:{alienMinute}:{alienSecond}");
        }

        // Method to display Earth time based on Alien time
        private static void DisplayEarthTime()
        {
            double alienTotalSeconds = alienSecond +
                                       alienMinute * AlienSecondsInMinute +
                                       alienHour * AlienMinutesInHour * AlienSecondsInMinute +
                                       (alienDay - 1) * AlienHoursInDay * AlienMinutesInHour * AlienSecondsInMinute;

            for (int i = 0; i < alienMonth - 1; i++)
            {
                alienTotalSeconds += AlienDaysPerMonth[i] * AlienHoursInDay * AlienMinutesInHour * AlienSecondsInMinute;
            }

            alienTotalSeconds += (alienYear - 2804) * AlienDaysPerMonth.Length * AlienHoursInDay * AlienMinutesInHour * AlienSecondsInMinute;
            double earthTotalSeconds = alienTotalSeconds * EarthToAlienRatio;

            DateTime earthTime = new DateTime(1970, 1, 1).AddSeconds(earthTotalSeconds);
            Console.WriteLine($"Equivalent Earth Time: {earthTime:yyyy-MM-dd HH:mm:ss}");
        }

        // Method to set an alarm based on Alien time
        private static void SetAlarm()
        {
            Console.Write("Enter Alien Time for Alarm (yyyy-MM-dd HH:mm:ss): ");
            string input = Console.ReadLine();
            DateTime alarmTime;
            if (DateTime.TryParse(input, out alarmTime))
            {
                // Extract Alien time components
                alarmYear = alarmTime.Year;
                alarmMonth = alarmTime.Month;
                alarmDay = alarmTime.Day;
                alarmHour = alarmTime.Hour;
                alarmMinute = alarmTime.Minute;
                alarmSecond = alarmTime.Second;

                alarmSet = true;
                Console.WriteLine($"Alarm set for Alien Time: {alarmYear}-{alarmMonth}-{alarmDay}, {alarmHour}:{alarmMinute}:{alarmSecond}");
            }
            else
            {
                Console.WriteLine("Invalid Alien time format. Please enter in yyyy-MM-dd HH:mm:ss format.");
            }
        }
    }
}

