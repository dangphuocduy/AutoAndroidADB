using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoAndroidADB
{
    public partial class Form1 : Form
    {
        public bool isStop = false;
        #region Properties
        Bitmap TOP_UP_PNG;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            TOP_UP_PNG = (Bitmap)Image.FromFile("Data//TOP_UP.png");
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            Task t = new Task(() =>
            {
                isStop = false;
                Auto();
            });
            t.Start();
        }

        private void Auto()
        {
            // LẤY DANH SÁCH DEVICES  ID
            List<string> devices = new List<string>();
            devices = KAutoHelper.ADBHelper.GetDevices();

            foreach (var deviceId in devices)
            {
                // TẠO RA MỘT LUỒNG XỬ LÝ RIÊNG BIỆT
                Task t = new Task(() =>
                {
                    while (true)
                    {
                        // IF STOP THÌ DỪNG TOÀN BỘ TASK
                        //CLICK VÀO BROWSER
                        if (isStop)
                            return;
                        KAutoHelper.ADBHelper.TapByPercent(deviceId, 48.1, 22.9);
                        Delay(5);

                        //CLICK VÀO ĐƯỜNG DẪN BROWSER
                        if (isStop)
                            return;
                        KAutoHelper.ADBHelper.TapByPercent(deviceId, 52.0, 37.9);
                        Delay(1);

                        // NHẬP TEXT
                        if (isStop)
                            return;
                        KAutoHelper.ADBHelper.InputText(deviceId, "https://www.howkteam.vn/");
                        Delay(5);

                        // NHẤN ENTER
                        if (isStop)
                            return;
                        KAutoHelper.ADBHelper.Key(deviceId, KAutoHelper.ADBKeyEvent.KEYCODE_ENTER);
                        Delay(5);

                        //CLICK VÀO TRANG WEB
                        if (isStop)
                            return;
                        KAutoHelper.ADBHelper.TapByPercent(deviceId, 0, 0);
                        Delay(1);


                        //LƯỚT TỪ TRÊN XUỐNG DƯỚI
                        for (int i = 0; i < 5; i++)
                        {
                            if (isStop)
                                return;
                            KAutoHelper.ADBHelper.Swipe(deviceId, 0, 0, 0, 0);
                            Delay(1);
                        }

                        var screenShot = KAutoHelper.ADBHelper.ScreenShoot(deviceId);
                        var topUpPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screenShot, TOP_UP_PNG);
                        if (topUpPoint != null)
                        {
                            KAutoHelper.ADBHelper.Tap(deviceId, topUpPoint.Value.X, topUpPoint.Value.Y);
                        }

                        // LƯỚT NGANG LOẠI BỎ ỨNG DỤNG
                        if (isStop)
                            return;
                        KAutoHelper.ADBHelper.Key(deviceId,KAutoHelper.ADBKeyEvent.KEYCODE_APP_SWITCH);
                        Delay(5);

                        // SWIPE NGANG
                        if (isStop)
                            return;
                        KAutoHelper.ADBHelper.Swipe(deviceId, 0, 0, 0, 0);
                        Delay(2);
                    }
                });
                t.Start();
            }

        }
        private void Delay(int delay)
        {
            while (delay > 0)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                delay--;
                if (isStop)
                {
                    break;
                }
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            isStop = true;
        }
    }
}
