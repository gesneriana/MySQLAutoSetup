using System;
using System.Collections.Generic;
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

namespace MySQLAutoSetup
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 生成my.ini配置文件的按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_auto_my_ini_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(textBox_baseDir.Text.ToString()))
            {
                MessageBox.Show("不存在的mysql数据库根目录");
                return;
            }
            string baseDir = textBox_baseDir.Text.ToString();
            try
            {
                var utf8 = new UTF8Encoding(false);    // 设置无BOM

                StringBuilder sb = new StringBuilder();
                sb.Append("[client]\r\n");
                sb.Append("port=3305\r\n");
                sb.Append("default-character-set=utf8\r\n");
                sb.Append("#客户端字符类型，与服务端一致就行，建议utf8\r\n");
                sb.Append("[mysqld]\r\n");
                sb.Append("port=3305\r\n");
                sb.Append("character_set_server=utf8\r\n");
                sb.Append("innodb_file_per_table=1\r\n");
                sb.Append("#服务端字符类型，建议utf8\r\n");
                sb.Append("basedir=" + baseDir + "\r\n");
                sb.Append("#解压根目录\r\n");
                sb.Append("datadir=" + baseDir + "\\data\r\n");
                sb.Append("#解压根目录\\data\r\n");
                sb.Append("sql_mode=NO_ENGINE_SUBSTITUTION,STRICT_TRANS_TABLES\r\n");
                sb.Append("[WinMySQLAdmin]\r\n");
                sb.Append(baseDir + "\\bin\\mysqld.exe\r\n");
                sb.Append("#解压根目录\\bin\\mysqld.exe\r\n");
                sb.Append("#以上是复制内容，这行可不复制");
                File.WriteAllText("my.ini", sb.ToString(), utf8);
                MessageBox.Show("my.ini配置文件生成成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 生成自动安装MySql服务的脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_auto_setup_server_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("my.ini")) { MessageBox.Show("my.ini配置文件不存在"); return; }
            if (!Directory.Exists("bin")) { MessageBox.Show("找不到bin目录"); return; }
            FileStream fs = new FileStream("bin\\auto_setup_server.cmd", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                sw.Write("mysqld --install");     // "mysqld --install"
                MessageBox.Show("安装服务的命令脚本生成成功,\n打开当前路径下的bin目录,\n右键点击auto_setup_server.cmd文件,\n以管理员身份运行");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }
    }
}