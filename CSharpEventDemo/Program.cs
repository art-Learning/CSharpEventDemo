using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpEventDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 使用一般Observer pattern
            Console.WriteLine("Observer Pattern Demo");
            var tempatureMonitor = new TempatureMonitorSubject();

            var desktopApp = new DesktopApp();
            var mobileApp = new MobileApp();

            tempatureMonitor.RegisterObserver(desktopApp);
            tempatureMonitor.RegisterObserver(mobileApp);

            Console.WriteLine("溫度變化了，現在是30.5度");
            tempatureMonitor.Tempature = 30.5;

            Console.WriteLine("溫度沒變化，現在依然是30.5度");
            tempatureMonitor.Tempature = 30.5;

            Console.WriteLine("溫度變化了，現在是28.6度");
            tempatureMonitor.Tempature = 28.6;

            Console.WriteLine("mobileApp不再想觀察了");
            tempatureMonitor.UnregisterObserver(mobileApp);

            Console.WriteLine("溫度變化了，現在是27.6度");
            tempatureMonitor.Tempature = 27.6;
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public class TempatureMonitorSubject : ITempatureMonitorSubject
    {
        private double tempature;

        public double Tempature
        {
            get { return tempature; }
            set
            {
                var oldTempature = tempature;
                if (oldTempature != value)
                {
                    tempature = value;
                    NotifyTempature();
                }
            }
        }

        private List<ITempatureMonitorObserver> observers;

        public TempatureMonitorSubject()
        {
            observers = new List<ITempatureMonitorObserver>();
            Console.WriteLine("開始偵測溫度");
        }

        public void RegisterObserver(ITempatureMonitorObserver observer)
        {
            observers.Add(observer);
        }

        public void UnregisterObserver(ITempatureMonitorObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyTempature()
        {
            foreach (var observer in observers)
            {
                observer.OnTempatureChanged(tempature);
            }
        }
    }
    // ITempatureMonitorSubject.cs
    public interface ITempatureMonitorSubject
    {
        void RegisterObserver(ITempatureMonitorObserver observer);

        void UnregisterObserver(ITempatureMonitorObserver observer);

        void NotifyTempature();
    }

    // ITempatureMonitorObserver.cs
    public interface ITempatureMonitorObserver
    {
        void OnTempatureChanged(double tempature);
    }
    public class DesktopApp : ITempatureMonitorObserver
    {
        public void OnTempatureChanged(double tempature)
        {
            Console.WriteLine($"Desktop App被通知溫度變化了: {tempature}");
        }
    }

    // MobileApp.cs
    public class MobileApp : ITempatureMonitorObserver
    {
        public void OnTempatureChanged(double tempature)
        {
            Console.WriteLine($"Mobile App被通知溫度變化了: {tempature}");
        }
    }
}
