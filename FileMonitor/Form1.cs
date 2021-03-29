using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FileMonitor
{
    public partial class form1 : Form
    {
        string x = "C:\\";
        private System.IO.FileSystemWatcher watcher = null;
        public form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ShowInTaskbar = false;
            this.NotifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyIcon1.Icon = new System.Drawing.Icon(@"../../OIP.ico");
            this.NotifyIcon1.Visible = true;
            this.NotifyIcon1.Text = "フォルダ監視システム";
            this.NotifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            //Clickイベントハンドラを追加する
            this.NotifyIcon1.Click += new EventHandler(NotifyIcon1_Click);
            textBox1.Text += "2021 フォルダ監視システム";
            if (watcher != null) return;

            watcher = new System.IO.FileSystemWatcher();
            //監視するディレクトリを指定
            watcher.Path = @"C:\";
            //最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
            watcher.NotifyFilter =
                (System.IO.NotifyFilters.LastAccess
                | System.IO.NotifyFilters.LastWrite
                | System.IO.NotifyFilters.FileName
                | System.IO.NotifyFilters.DirectoryName);
            //すべてのファイルを監視
            watcher.Filter = "";
            //UIのスレッドにマーシャリングする
            //コンソールアプリケーションでの使用では必要ない
            watcher.SynchronizingObject = this;

            //イベントハンドラの追加
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);

            watcher.IncludeSubdirectories = false;
            //監視を開始する
            watcher.EnableRaisingEvents = true;
            textBox1.Text += "\r\n監視が開始されました。監視パス:「" + watcher.Path + "」";
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            textBox1.Text += "\r\nアプリを表示しました。";
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            textBox1.Text += "\r\nアプリを隠しました";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (watcher != null) return;

            watcher = new System.IO.FileSystemWatcher();
            //監視するディレクトリを指定
            watcher.Path = @"C:\";
            //最終アクセス日時、最終更新日時、ファイル、フォルダ名の変更を監視する
            watcher.NotifyFilter =
                (System.IO.NotifyFilters.LastAccess
                | System.IO.NotifyFilters.LastWrite
                | System.IO.NotifyFilters.FileName
                | System.IO.NotifyFilters.DirectoryName);
            //すべてのファイルを監視
            watcher.Filter = "";
            //UIのスレッドにマーシャリングする
            //コンソールアプリケーションでの使用では必要ない
            watcher.SynchronizingObject = this;

            //イベントハンドラの追加
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);

            //監視を開始する
            watcher.EnableRaisingEvents = true;
            textBox1.Text += "\r\n監視が開始されました。監視パス:「" + watcher.Path+"」";
        }

        private void watcher_Renamed(System.Object source,System.IO.RenamedEventArgs e)
        {
            textBox1.Text += "\r\nファイル「" + e.FullPath + "」の名前が変更されました。";
        }

        private void watcher_Changed(System.Object source,System.IO.FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case System.IO.WatcherChangeTypes.Changed:
                    textBox1.Text += "\r\nファイル「" + e.FullPath + "」が変更されました。";
                    break;
                case System.IO.WatcherChangeTypes.Created:
                    textBox1.Text += "\r\nファイル「" + e.FullPath + "」が作成されました。";
                    break;
                case System.IO.WatcherChangeTypes.Deleted:
                    textBox1.Text += "\r\nファイル「" + e.FullPath + "」が削除されました。";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //監視を終了
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                watcher = null;
                textBox1.Text += "\r\n監視終了";
            }
            catch
            {
                textBox1.Text += "\r\n監視停止中に終了できません";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを選択してください。\nデフォルト値は「C:\\」です。\n現在,値は「" + x + "」です。";
            // フォルダ選択ダイアログ表示
            fbd.ShowDialog();
            x = fbd.SelectedPath;
            watcher.Path = x;
            textBox1.Text += "\r\n監視フォルダを「" + watcher.Path + "」に変更しました";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text += "\r\nログを表示しました";
            this.Height = 480;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text += "\r\nログを隠しました";
            this.Height = 200;        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (watcher.IncludeSubdirectories == true)
            {
                watcher.IncludeSubdirectories = false;
                log("サブフォルダ監視の停止");
            }
            else
            {
                watcher.IncludeSubdirectories = true;
                log("サブフォルダ監視の開始");
            }
        }

        void log(string y)
        {
            textBox1.Text += "\r\n" + y;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // テキストファイルのパス
            string path = @"log.tokei";
            // StreamWriterオブジェクトのインスタンスを生成
            StreamWriter streamWriter = new StreamWriter(path, true);
            // Writeメソッドで文字列データを書き込む
            streamWriter.Write("\r\n"+textBox1.Text);
            // StreamWriterオブジェクトを閉じる
            streamWriter.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"log.tokei");
        }

        public static void SetCurrentVersionRun()
        {
            //Runキーを開く
            Microsoft.Win32.RegistryKey regkey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", true);
            //値の名前に製品名、値のデータに実行ファイルのパスを指定し、書き込む
            regkey.SetValue(Application.ProductName, Application.ExecutablePath);
            //閉じる
            regkey.Close();
        }

        private void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("終了してもいいですか？\n監視は停止されます。\n保存していないログは削除されます。", "確認",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ライセンスToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"log.tokei");
        }
    }
}
