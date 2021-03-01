using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace cell
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(args.Length==2 && args[0]== "/p")//选择
            {
                //Convert.ToInt32(args[1]);
                //Application.Run();
                Application.Exit();
            }
            else if(args.Length==1 && args[0]=="/s")//屏幕保护 或 预览
            {
                Application.Run(new Form1(args));
            }
            else if(args.Length==1&& args[0].Substring(0,2)=="/c")//设置
            {
                Application.Exit();
            }
            else
            {
                Application.Run(new Form1(args));
            }
        }
    }
}
