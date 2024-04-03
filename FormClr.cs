using WinControlApp;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using Timer = System.Windows.Forms.Timer;
using static WinControlApp.GlobalFunc;

namespace WinControlApp
{
  
  public partial class FormClr : Form
  {
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(ref Point lpPoint);
    public FormClr()
    {
      InitializeComponent();
      // 设置窗体的标题
      this.Text = "";
      // 设置窗体的背景色为透明
      this.BackColor = Color.White;
      // 设置窗体的边框样式为无边框
      this.FormBorderStyle = FormBorderStyle.None;
      // 设置窗体的大小为屏幕的大小
      this.Size = Screen.PrimaryScreen.Bounds.Size;
      // 设置窗体的位置为屏幕的左上角
      this.Location = new Point(0, 0);
      // 设置窗体的不透明度，即半透明
      this.Opacity = 0.1;
      // 设置窗体的 TopMost 属性为 true，即总是在最前
      this.TopMost = true;
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      // 如果正在捕获鼠标位置，就更新鼠标的位置
      lastCursorPosition = e.Location;
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      // 如果按下了鼠标右键，就停止捕获鼠标位置，并显示窗体
      if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
      {
        // 读出鼠标的位置
        //MessageBox.Show($"鼠标的位置是：({lastCursorPosition.X}, {lastCursorPosition.Y})");
        this.Close();
      }
    }
  }
}
