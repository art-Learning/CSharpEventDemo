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
            // 使用Event事件委派
            Console.WriteLine("Event Demo");
            var tempatureMonitorEvent = new TempatureMonitorUsingEvent();

            var desktopApp = new DesktopApp();
            var mobileApp = new MobileApp();
            // 額外自訂事件委派方法, 由於是宣告成事件委派, 輸入到+=時可以直接用TAB產生基本的程式碼
            tempatureMonitorEvent.OnTempatureChanged += TempatureMonitorEvent_OnTempatureChanged;
            tempatureMonitorEvent.OnTempatureChanged += desktopApp.OnTempatureChangedEvent;
            tempatureMonitorEvent.OnTempatureChanged += mobileApp.OnTempatureChangedEvent;

            Console.WriteLine("溫度變化了，現在是30.5度");
            tempatureMonitorEvent.Tempature = 30.5;

            Console.WriteLine("溫度沒變化，現在依然是30.5度");
            tempatureMonitorEvent.Tempature = 30.5;

            Console.WriteLine("溫度變化了，現在是28.6度");
            tempatureMonitorEvent.Tempature = 28.6;

            Console.WriteLine("mobileApp不再想觀察了");
            tempatureMonitorEvent.OnTempatureChanged -= mobileApp.OnTempatureChangedEvent;

            Console.WriteLine("溫度變化了，現在是27.6度");
            tempatureMonitorEvent.Tempature = 27.6;
            Console.WriteLine();
            Console.ReadKey();
        }
        private static void TempatureMonitorEvent_OnTempatureChanged(object sender, double e)
        {
            Console.WriteLine($"---自訂的委派方法得知溫度變化了: {e}");
        }
    }

    public class TempatureMonitorUsingEvent
    {
        // 使用EventHandler<T>來省去自訂delegate的麻煩
        public event EventHandler<double> OnTempatureChanged;

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
                    if (OnTempatureChanged != null)
                    {
                        OnTempatureChanged(this, value);
                    }
                }
            }
        }
    }

    
    public interface ITempatureMonitorEvent
    {
        void OnTempatureChangedEvent(object sender, double e);
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
    // DesktopApp.cs
    public class DesktopApp : ITempatureMonitorObserver, ITempatureMonitorEvent
    {
        public void OnTempatureChanged(double tempature)
        {
            Console.WriteLine($"---Desktop App被通知溫度變化了: {tempature}");
        }

        public void OnTempatureChangedEvent(object sender, double tempature)
        {
            Console.WriteLine($"---Desktop App使用事件委派方法得知溫度變化了: {tempature}");
        }
    }


    // MobileApp.cs
    public class MobileApp : ITempatureMonitorObserver, ITempatureMonitorEvent
    {
        public void OnTempatureChanged(double tempature)
        {
            Console.WriteLine($"---Mobile App被通知溫度變化了: {tempature}");
        }

        public void OnTempatureChangedEvent(object sender, double tempature)
        {
            Console.WriteLine($"---Mobile App使用事件委派方法得知溫度變化了: {tempature}");
        }
    }
}
