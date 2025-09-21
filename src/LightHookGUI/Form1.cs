using System.Diagnostics;
using System.Net;
using System.Text;

namespace LightHook
{
    public partial class Form1 : Form
    {
        EncBinder Server;
        List<EncSocket> ActiveProcessConnection = new List<EncSocket>();
        public Form1()
        {
            InitializeComponent();
        }

        Queue<EncSocket> InputQueue = new Queue<EncSocket>();
        private void RecvFromTargetProcess(EncSocket enc)
        {
            ActiveProcessConnection.Add(enc);
            try
            {
                var code = File.ReadAllText(Application.StartupPath + "\\inject.js");
                enc.Send(System.Text.Encoding.Unicode.GetBytes(code), 1); //发送初始化脚本
                while (true)
                {
                    byte flag = 0;
                    var data = enc.Recv(ref flag);
                    if (flag == 1)
                    {
                        this.Invoke(() =>
                        {
                            //MessageBox.Show("脚本执行成功");
                        });
                    }
                    else if (flag == 2)
                    {
                        this.Invoke(() =>
                        {
                            richTextBox1.AppendText(Encoding.Unicode.GetString(data) + "\r\n");
                        });
                    }
                    else if (flag == 3) //远程方调用readln()请求输入
                    {
                        InputQueue.Enqueue(enc); //压入队列
                        this.Invoke(() =>
                        {
                            userInput.Enabled = true;
                        });
                    }
                    else if (flag == 4) //请求注入
                    {
                        int pid = BitConverter.ToInt32(data, 0);
                        Inject(pid);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                ActiveProcessConnection.Remove(enc);
            }
        }
        private void AcceptSocket(EncSocket enc)
        {
            Task.Run(() =>
            {
                RecvFromTargetProcess(enc);
            });
        }

        static void Inject(int pid)
        {
            if (WindowsNativeFunc.IsWin64(Process.GetProcessById(pid)))
            {
                var p = Process.Start(Application.StartupPath + "\\Injector64.exe", pid.ToString());
                p.WaitForExit();
                MessageBox.Show(p.ExitCode.ToString());
            }
            else
            {
                DllInject.Inject(pid, Application.StartupPath + "\\LightHookCore32.dll");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                int processId = int.Parse(textBox1.Text);
                Inject(processId);
            }
            else
            {
                var od = new OpenFileDialog();
                if(od.ShowDialog() == DialogResult.OK)
                {
                    int pid = Process.Start(od.FileName).Id;
                    Inject(pid);
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Server = new EncBinder(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 19924), AcceptSocket);
            var sock = new EncSocket();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var od = new OpenFileDialog();
            if (od.ShowDialog() == DialogResult.OK)
            {
                var code = File.ReadAllText(od.FileName);
                foreach (var con in ActiveProcessConnection)
                {
                    con.Send(Encoding.Unicode.GetBytes(code), 1);
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var enc = InputQueue.Dequeue();
                enc.Send(Encoding.Unicode.GetBytes(userInput.Text), 3);
                if (InputQueue.Count == 0)
                {
                    userInput.Enabled = false;
                }
                userInput.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
