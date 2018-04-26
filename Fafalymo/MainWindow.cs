using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fafalymo
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool m_first = false;
        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (this.m_first) return;
            this.m_first = true;

            var ffxivlogs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Advanced Combat Tracker", "FFXIVLogs");
            if (Directory.Exists(ffxivlogs) && Directory.GetFiles(ffxivlogs, "*.log", SearchOption.TopDirectoryOnly).Length > 0)
                this.ofd.InitialDirectory = ffxivlogs;

            if (this.ofd.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }

            Task.Run(new Action(() => this.Converter(this.ofd.FileNames)));
        }

        private void SetLable(double pFile, double pLine, string format, params object[] args)
        {
            this.Invoke(new Action(() =>
                {
                    this.progFile.Value = (int)(this.progLine.Maximum * pFile);
                    this.progLine.Value = (int)(this.progLine.Maximum * pLine);
                    this.lblProgress.Text = string.Format(format, args);
                }));
        }

        private DialogResult ShowMessageBox(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return (DialogResult)this.Invoke(new Func<IWin32Window, string, string, MessageBoxButtons, MessageBoxIcon, DialogResult>(MessageBox.Show), this, text, this.Text, buttons, icon);
        }

        private void Converter(string[] filePaths)
        {
            int fileIndex = 0;

            long curLine = 0;
            long baseCur = 0;
            string[] lines = null;
            var lstLines = new List<string>();

            Task task;
            void RefreshLabel()
            {
                long cur;
                while (true)
                {
                    cur = Interlocked.Read(ref curLine);

                    var f = (double)fileIndex / filePaths.Length;
                    var l = (double)(baseCur + cur) / lines.Length / 2;

                    this.SetLable(f, l, "[{0} / {1}] {2:#,##0} / {3:#,##0} ({4:#0.0} %)", fileIndex + 1, filePaths.Length, cur, lines.Length, l * 100);

                    Thread.Sleep(100);

                    if (cur == lines.Length)
                        break;
                }
            }

            for (fileIndex = 0; fileIndex < filePaths.Length; ++fileIndex)
            {
                var path = filePaths[fileIndex];

                if (!File.Exists(path))
                    continue;

                // Read all lines;
                this.SetLable((double)fileIndex / filePaths.Length, 0, "[{0} / {1}] 파일 읽는 중", fileIndex + 1, filePaths.Length);
                
                try
                {
                    lines = File.ReadAllLines(path, Encoding.UTF8);
                }
                catch
                {
                    string newPath = null;

                    try
                    {
                        newPath = Path.GetTempFileName();
                        File.Delete(newPath);
                        File.Copy(path, newPath);

                        lines = File.ReadAllLines(newPath, Encoding.UTF8);
                    }
                    catch
                    {
                        this.ShowMessageBox("ACT 를 종료 후 다시 시도해주세요!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        this.Invoke(new Action(Application.Exit));
                    }
                    finally
                    {
                        if (newPath != null)
                            File.Delete(newPath);
                    }
                }

                curLine = 0;
                baseCur = 0;

                // Translate
                using (task = Task.Run(new Action(RefreshLabel)))
                {
                    Parallel.For(0, lines.Length, index =>
                    {
                        Interlocked.Increment(ref curLine);
                        lines[index] = LogConverter.Translate(lines[index]);
                    });
                    task.Wait();
                }

                curLine = 0;
                baseCur = lines.Length;

                using (task = Task.Run(new Action(RefreshLabel)))
                {
                    var dir = Path.Combine(Path.GetDirectoryName(path), "Fafalymo");
                    Directory.CreateDirectory(dir);

                    using (var file = File.OpenWrite(Path.Combine(dir, Path.GetFileNameWithoutExtension(path) + "-k2e" + Path.GetExtension(path))))
                    using (var writer = new StreamWriter(file, Encoding.UTF8))
                    {
                        writer.BaseStream.SetLength(0);

                        int line = 1;
                        for (int index = 0; index < lines.Length; ++index)
                        {
                            Interlocked.Increment(ref curLine);

                            if (lines[index] == null || lines[index].Length < 10)
                                continue;

                            if (lines[index].StartsWith("253|"))
                                line = 1;
                            // LogChangeZone
                            else if (lines[index].StartsWith("01|"))
                                line = 1;

                            writer.WriteLine(LogConverter.RecalcHash(lines[index], line++));
                        }

                        writer.Flush();
                    }

                    task.Wait();
                }
            }

            this.ShowMessageBox("변환을 완료했습니다!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Invoke(new Action(Application.Exit));
        }
    }
}
