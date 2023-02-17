using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HalconDotNet;
namespace Learning_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //lsv.ItemsSource= Listsv;
        }
        
        private void window_display_HInitWindow(object sender, EventArgs e)
        {

        }

        private void window_display_HMouseMove(object sender, HalconDotNet.HSmartWindowControlWPF.HMouseEventArgsWPF e)
        {

        }

        public ImageData CurrentImage = new ImageData(new HImage(), 0, false);
        public object image_lock = new object();
        public OutputSource<ImageData> OutputImage = new OutputSource<ImageData>();
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();

            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Task.Run((Action)(() =>
                {
                    try
                    {
                        HImage grabbed = new HImage(open.FileName);
                        lock (image_lock)
                        {
                            //CurrentImage.Dispose();
                            //CurrentImage = new ImageData(grabbed.CopyImage(), CurrentImage.FrameId, false);
                            //OutputImage.OnNext(CurrentImage);
                            window_display.HalconWindow.AttachBackgroundToWindow(grabbed);
                        };
                    }
                    catch (Exception ex)
                    {

                    }

                }));
            }
        }
        public class ImageData
        {
            public HImage Image { get; set; }
            public ulong FrameId { get; set; }
            public double FPS { get; set; }
            public bool IsPlayback { get; set; }
            public ImageData(HImage Image, ulong FrameId, bool IsPlayback, double FPS = 0)
            {
                this.Image = Image;
                this.FrameId = FrameId;
                this.IsPlayback = IsPlayback;
                this.FPS = FPS;
            }
            public void Dispose()
            {
                this.Image.Dispose();
            }
        }
        public class OutputSource<T> : IObservable<T>
        {
            public List<IObserver<T>> obser = new List<IObserver<T>>();
            object obserlock = new object();
            public IDisposable Subscribe(IObserver<T> observer)
            {
                lock (obserlock)
                {
                    obser.Add(observer);
                }

                return new Unsubscriber<T>(obser, observer);
            }
            public T Data;
            public void OnNext(T data)
            {
                Data = data;
                lock (obserlock)
                {
                    foreach (var item in obser)
                    {

                        item.OnNext(data);

                    }
                }
            }
        }
        public class Unsubscriber<T> : IDisposable
        {
            private List<IObserver<T>> _observers;
            private IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
         //   Listsv.Add(new sinhvien { Id = Int32.Parse(tb_id.Text), Name = tb_name.Text });
        }
    }
    
}
