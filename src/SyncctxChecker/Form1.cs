using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyncctxChecker
{
    public partial class Form1 : Form
    {
        static readonly HttpClient Client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private static string _log;
        private async void button1_Click(object sender, EventArgs e)
        {
            Disable();
            _log = "";
            LogBefore(nameof(button1_Click));
            await M3();
            LogBefore(nameof(button1_Click));
            MessageBox.Show(_log);
            Enable();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Disable();
            _log = "";
            LogBefore(nameof(button2_Click));
            await CpuMethod();
            LogBefore(nameof(button2_Click));
            MessageBox.Show(_log);
            Enable();
        }

        static async Task<string> M3()
        {
            LogBefore(nameof(M3));
            var str = await M2();
            LogAfter(nameof(M3));
            return str;
        }

        static async Task<string> M2()
        {
            LogBefore(nameof(M2));
            var str = await IoMethod();
            LogAfter(nameof(M2));
            return str;
        }

        static async Task<string> IoMethod()
        {
            LogBefore(nameof(IoMethod));
            var str = await Client.GetStringAsync("http://mtkachenko.me")
                .ConfigureAwait(false);
            LogAfter(nameof(IoMethod));
            return str;
        }

        static async Task CpuMethod()
        {
            LogBefore(nameof(CpuMethod));
            await Task.Run(() =>
            {
                LogBefore(nameof(Task.Run));
                Thread.Sleep(100);
                LogAfter(nameof(Task.Run));
            }).ConfigureAwait(false);
            LogAfter(nameof(CpuMethod));
        }

        static void LogBefore(string method) => Log($"before {method}");

        static void LogAfter(string method) => Log($"after {method}");

        static void Log(string prefix)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var type = Thread.CurrentThread.IsThreadPoolThread ? "threadpool" : "main";
            var ctx = SynchronizationContext.Current == null ? "no sync.ctx" : "yes sync.ctx";
            _log += $"{prefix}: {threadId} - {type} - {ctx} {Environment.NewLine}";
        }

        private void Disable()
        {
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void Enable()
        {
            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
