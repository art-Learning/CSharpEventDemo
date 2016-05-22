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
            // 使用Delegate完成Observer pattern
            Console.WriteLine("Delegate Demo");
            var tempatureMonitorDelegate = new TempatureMonitorUsingDelegate();
            var desktopApp = new DesktopApp();
            var mobileApp = new MobileApp();
            tempatureMonitorDelegate.OnTempatureChanged += desktopApp.OnTempatureChanged;
            tempatureMonitorDelegate.OnTempatureChanged += mobileApp.OnTempatureChanged;

            Console.WriteLine("設定溫度，現在是30.5度");
            tempatureMonitorDelegate.Tempature = 30.5;

            Console.WriteLine("設定溫度，現在依然是30.5度");
            tempatureMonitorDelegate.Tempature = 30.5;

            Console.WriteLine("設定溫度，現在是28.6度");
            tempatureMonitorDelegate.Tempature = 28.6;

            Console.WriteLine("mobileApp不再想觀察了");
            tempatureMonitorDelegate.OnTempatureChanged -= mobileApp.OnTempatureChanged;

            Console.WriteLine("設定溫度，現在是27.6度");
            tempatureMonitorDelegate.Tempature = 27.6;
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public partial class TempatureMonitorUsingDelegate
    {
        public delegate void TempatureChangedHandler(double tempature);

        public TempatureChangedHandler OnTempatureChanged;

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
                    OnTempatureChanged.Invoke(value);
                }
            }
        }

        public TempatureMonitorUsingDelegate()
        {
            // 使用delegate必須給定一個初始的委派方法
            OnTempatureChanged = tempatureChanged;
        }

        private void tempatureChanged(double tempature)
        {
            Console.WriteLine($"---委派方法內偵測到溫度發生變化了...{tempature}");
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
            Console.WriteLine($"------Desktop App被通知溫度變化了: {tempature}");
        }
    }

    // MobileApp.cs
    public class MobileApp : ITempatureMonitorObserver
    {
        public void OnTempatureChanged(double tempature)
        {
            Console.WriteLine($"------Mobile App被通知溫度變化了: {tempature}");
        }
    }
}
