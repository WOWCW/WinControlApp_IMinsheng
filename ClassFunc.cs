using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static WinControlApp.GlobalFunc;
using System.Runtime.CompilerServices;

namespace WinControlApp
{
  [Flags]
  public enum MouseEventFlags
  {
    LEFTDOWN = 0x00000002,
    LEFTUP = 0x00000004,
    RIGHTDOWN = 0x00000008,
    RIGHTUP = 0x00000010,
    MIDDLEDOWN = 0x00000020,
    MIDDLEUP = 0x00000040,
    XDOWN = 0x00000080,
    XUP = 0x00000100,
    WHEEL = 0x00000800,
    HWHEEL = 0x00001000,
    MOVE = 0x00000001,
    ABSOLUTE = 0x00008000
  }



  public class PinyinData
  {
    //拼音类读取
    public List<string> pinyinList { get; set; }
  }

  internal static class GlobalFunc
  {
    //本地程序路径
    public static string def_ExDirPath = $"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}",
                         def_JsonFile=(@$"{def_ExDirPath}\pinyin.json").Replace(@"\\", @"\");
    //上一次鼠标位置
    public static Point lastCursorPosition;
    // 定义右下角系统栏图标
    public static Icon icon1 = new Icon(Resource.ico1, 32, 32), iconbs = new Icon(Resource.icobs, 32, 32);
    //是否闪烁显示（已经启动执行任务，可以替代表示是否启动这个Flg）  //是否开始捕获鼠标  //静默期鼠标点击  //运行期人工干预  //是否正在执行
    public static bool isnotifyIconVisible = false, ismouseXYCapture = false, eventActFlg = false, doingActPauseFlg = false, isDoingFlg = false, mouseDoingFlg=false;
    public static DateTime lastActTime, eventActTime;
    public static string[] MouseClkitems = { "移动", "单击", "双击", "右键" };
    public static string tTmpLog = "", prname = "icmbc";
    public static List<string[]> dtlistItems = new List<string[]>();  //带指定时间的流程设计
    //随机
    private static Random random = new Random();
    //拼音表（用于生成随机的词组和句子）
    public static List<string> pinyinList = new List<string> { "" };
    //DLL------
    [DllImport("user32.dll", EntryPoint = "GetSystemMetrics", SetLastError = true)]
    public static extern int GetSystemMetrics(int nIndex);
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(ref Point lpPoint);
    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    //声明SetForegroundWindow函数
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    //声明IsIconic函数
    [DllImport("user32.dll")]
    public static extern bool IsIconic(IntPtr hWnd);

    //声明ShowWindow函数
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    //定义SW_RESTORE常量
    const int SW_RESTORE = 9;
    public static void ActiveWin(string psname)
    {
      //获取名为的进程
      Process[] processes = Process.GetProcessesByName(psname);
      //判断是否找到该进程
      if (processes.Length > 0)
      {
        //找到了该进程，遍历每个进程对象
        foreach (Process process in processes)
        {
          //获取主窗口句柄
          IntPtr handle = process.MainWindowHandle;
          //判断窗体是否被最小化
          if (IsIconic(handle))
          {
            //将窗体正常化
            ShowWindow(handle, SW_RESTORE);
          }
          else
          {
            //激活并前置该窗口
            SetForegroundWindow(handle);
          }
        }
      }
    }

    //读取Json
    public static void GreatJsonData()
    {
      if (File.Exists(def_JsonFile))
      {
        try
        {
          // 读取 JSON 文件内容
          string json = File.ReadAllText(def_JsonFile);
          // 将 JSON 转换为对象
          PinyinData data = JsonSerializer.Deserialize<PinyinData>(json);
          // 获取 pinyinList 属性的值
          pinyinList = data.pinyinList;
        }
        catch (Exception)
        {
          pinyinList = new List<string> { "" };
        }
      }
    }

    //生成随机的拼音
    public static string GenerateRandomPinyin(int len=10)
    {
      string pinyin = "";
      for (int i = 0; i < len; i++)
      {
        int index = random.Next(0, pinyinList.Count), spaceIndex = random.Next(1, 6)
          , paseTinx = random.Next(2, 9), enterTinx= random.Next(10, 40);
        if (index>=pinyinList.IndexOf("nihao"))
        {
          pinyin += $" {pinyinList[index]} ";
        }
        else
        {
          pinyin += pinyinList[index];
          if ((i + 1) % spaceIndex == 0) { pinyin += " "; }
          if ((i + 1) % paseTinx == 0) { pinyin += " ，"; }
          else if ((i + 1) % enterTinx == 0) { pinyin += " 。"+ ((char)1).ToString(); }
        }
      }
      pinyin += " ";
      return pinyin.Replace("  "," ");
    }

    //指定时间的RPA序列
    public static void ListCheckDoFrst(ref bool settoFlg)
    {
      try
      {
        //将满足时间要求的数据放入 新的List<string[]> listItems，然后调用 ExecuteListViewItems
        List<string[]> listItems = new List<string[]>();
        DateTime currentTime = DateTime.Now;
        for (int i = dtlistItems.Count - 1; i >= 0; i--)
        {
          try
          {
            string dateTimeStr = dtlistItems[i][3];
            DateTime dateTime;
            //将item[3]转成时间
            if (DateTime.TryParse(dateTimeStr, out dateTime) || DateTime.TryParseExact(dateTimeStr, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
              // 执行时间已经到了，但是需要在180秒之内，超过太久也不执行
              if (dateTime < currentTime && (currentTime - dateTime).TotalSeconds < 180)
              {
                listItems.Add(dtlistItems[i]);
                //执行完了就可以删除了，避免重复执行
                dtlistItems.RemoveAt(i);
                tTmpLog += $"{dateTime.ToString("HH:mm:ss")}计划任务已执行...\r\n";
              }
            }
            
          }
          catch (Exception)
          {
            throw;
          }
        }
        ExecuteListViewItems(listItems,ref settoFlg);
      }
      catch (Exception ex)
      {
        tTmpLog += $"{DateTime.Now.ToString("HH:mm:ss")}: {ex}\r\n";
      }
    }
    //执行RPA序列
    public static void ExecuteListViewItems(List<string[]> listItems, ref bool settoFlg)
    {
      if (listItems.Count==0) { return; }//没有数据就退出
      settoFlg = true;
      try
      {
        ActiveWin(prname); Thread.Sleep(1000);//激活i民生窗口（只在启动时进行一次激活）
        foreach (string[] item in listItems)
        {
          if (doingActPauseFlg) { break; }
          if (item.Length < 3) { continue; }
          string[] xy = item[0].Split("|");
          if (xy.Length == 3)
          {
            int x = int.Parse(xy[0]);
            int y = int.Parse(xy[1]);
            string clicktp = xy[2];
            int time = int.Parse(item[1]);
            string text = item[2].Replace( "\n", ((char)1).ToString());
            // 模拟鼠标左键单击
            SetMouseXY(x, y, clicktp, text);
            //记录到日志
            tTmpLog += $"{clicktp}({x}|{y}){(text.Length > 0 ? $">>{text}；" : "；")}\r\n";
            // 等待指定时间
            Thread.Sleep(time > 0 ? (time * 1000) : 1000);
          }
        }
      }
      catch (Exception ex)
      {
        tTmpLog += $"{DateTime.Now.ToString("HH:mm:ss")}: {ex}\r\n";
      }
      settoFlg = false;
    }

    // 模拟点击位置的坐标
    public static void SetMouseXY(int x, int y,string clicktp, string text = "")
    {
      mouseDoingFlg = true;
      try
      {
        // 移动鼠标到指定位置
        Cursor.Position = new System.Drawing.Point(x, y);
        // 模拟鼠标左键按下和释放的操作
        switch (clicktp)
        {
          case "单击":
            mouse_event((int)(MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP), x, y, 0, 0); break;
          case "右键":
            mouse_event((int)(MouseEventFlags.RIGHTDOWN | MouseEventFlags.RIGHTUP), x, y, 0, 0);break;
          case "双击":
            mouse_event((int)(MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP), x, y, 0, 0); Thread.Sleep(100);
            mouse_event((int)(MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP), x, y, 0, 0); break;
          default:
            // 获取屏幕的宽度和高度
            int screenWidth = GetSystemMetrics(0);
            int screenHeight = GetSystemMetrics(1);
            // 将鼠标的目标位置转换为绝对坐标
            int sx = (int)(x * (65535.0f / screenWidth));
            int sy = (int)(y * (65535.0f / screenHeight));
            mouse_event(((int)MouseEventFlags.MOVE | (int)MouseEventFlags.ABSOLUTE), sx, sy, 0, 0);
            break;
        }
        lastCursorPosition = Cursor.Position;  //及时重置最后鼠标位置
        string linkKeystr = "";
        if (text.Length > 0)
        {
          //等待1秒后输入文本
          Thread.Sleep(1000);
          // 输入文本                
          //SendKeys.Send(text);
          //KeysHook.SimulateKeyPress(Keys.A | Keys.Shift);
          KeyInputs(text);
        }
        //读取字符开始输入
        void KeyPressin(string text, Keys k= Keys.None)
        {
          foreach (char c in text)
          {
            if (doingActPauseFlg) { break; }
            switch (c)
            {
              case ((char)1):
                KeysHook.SimulateKeyPress(Keys.Enter); Thread.Sleep(300);
                break;
              case ((char)2):
                KeysHook.SimulateKeyPress(Keys.Escape); Thread.Sleep(300);
                break;
              case ',':
                KeysHook.SimulateKeyPress(Keys.Oemcomma); Thread.Sleep(300);
                break;
              case '，':
                KeysHook.SimulateKeyPress(Keys.ShiftKey, Keys.Oemcomma); Thread.Sleep(300);
                break;
              case '.':
                KeysHook.SimulateKeyPress(Keys.OemPeriod); Thread.Sleep(300);
                break;
              case '。':
                KeysHook.SimulateKeyPress(Keys.ShiftKey, Keys.OemPeriod); Thread.Sleep(300);
                break;
              default:
                if (k != Keys.None) { KeysHook.SimulateKeyPress(k, KeysHook.ConvertStringToKey(c.ToString())); }
                else { KeysHook.SimulateKeyPress(KeysHook.ConvertStringToKey(c.ToString())); }
                Thread.Sleep(100);  //每次输入暂停0.1秒
                break;
            }
          }
        }
        void KeyInputs(string text)
        {
          string textin = text.Replace("|||", ((char)1).ToString()).Replace("~~~", ((char)2).ToString())
                        .Replace("(DateTime)", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        .Replace("(Date)", DateTime.Now.ToString("yyyy-MM-dd")).Replace("(Time)", DateTime.Now.ToString("HH:mm:ss")); 
          foreach (char c in textin)
          {
            if (doingActPauseFlg) { break; }
            switch (c)
            {
              case '(':
              case '{':
                linkKeystr = $"{c}"; 
                break;  //接下的需要同时输入
              case ')':
              case '}':
                //同时按键结束符号
                if (linkKeystr.StartsWith("("))
                { //结束并执行
                  linkKeystr = linkKeystr.Replace("(", "");
                  if (linkKeystr.StartsWith("F"))
                  {
                    int keyFnum = int.Parse(linkKeystr.Replace("F", ""));
                    if (keyFnum >= 1 && keyFnum <= 12) { KeysHook.SimulateKeyPress((Keys)(keyFnum + 111)); Thread.Sleep(500); }
                  }
                  else if (linkKeystr.StartsWith("^") && linkKeystr.Length==2)
                  {
                    KeyPressin(linkKeystr.Remove(0,1), Keys.ControlKey);
                  }
                  else if (linkKeystr.StartsWith("@"))
                  {
                    int inputslen = int.Parse(linkKeystr.Replace("@", ""));
                    string inputstr = GenerateRandomPinyin(inputslen);
                    KeyPressin(inputstr);
                  }
                }
                else if (linkKeystr.StartsWith("{"))
                { //Shift&
                  linkKeystr = linkKeystr.Replace("{", "");
                  if (linkKeystr.Length > 0)
                  {
                    KeyPressin(linkKeystr, Keys.ShiftKey);
                  }
                  else
                  {
                    KeysHook.SimulateKeyPress(Keys.LShiftKey); Thread.Sleep(500);
                  }
                }
                linkKeystr = ""; 
                break;
              default:
                if (linkKeystr.Length > 0)
                {
                  linkKeystr += $"{c}";  //连接
                }
                else { KeyPressin(c.ToString()); }
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        tTmpLog += $"Error:{ex}\r\n";
      }
      mouseDoingFlg = false;
    }
    public static void MouseMain()
    {
      try
      {
        //开始监听鼠标事件
        MouseHook.Start();
        //注册鼠标事件的处理方法
        MouseHook.MouseAction += new MouseHook.MouseEventHandler(Event);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"{ex}","启动鼠标监听失败",MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
      //停止监听鼠标事件
      //MouseHook.Stop();
    }
    //鼠标事件的处理方法，用于输出鼠标状态（是否人为的都会显示）
    private static void Event(object sender, MouseEventArgs e)
    {
      //tTmpLog += $"{e.Button}({e.X}|{e.Y})\r\n";
      eventActFlg=true; eventActTime=DateTime.Now;
    }

  }



  public class MouseHook
  {
    //定义鼠标钩子类型
    private const int WH_MOUSE_LL = 14;

    //定义鼠标事件类型
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_LBUTTONUP = 0x0202;
    private const int WM_RBUTTONDOWN = 0x0204;
    private const int WM_RBUTTONUP = 0x0205;

    //定义鼠标钩子结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseHookStruct
    {
      public Point pt; //鼠标坐标
      public int hwnd; //窗口句柄
      public int wHitTestCode; //命中测试值
      public int dwExtraInfo; //额外信息
    }

    //定义鼠标钩子委托类型
    public delegate int MouseHookProc(int nCode, int wParam, IntPtr lParam);
    //定义鼠标钩子句柄变量
    public static int hMouseHook = 0;
    //定义鼠标钩子委托实例
    public static MouseHookProc MouseHookProcedure;
    //定义鼠标事件委托类型
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);
    //定义鼠标事件事件类型
    public static event MouseEventHandler MouseAction;
    //导入SetWindowsHookEx函数，用于安装钩子
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(int idHook, MouseHookProc lpfn, IntPtr hInstance, int threadId);

    //导入UnhookWindowsHookEx函数，用于卸载钩子
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern bool UnhookWindowsHookEx(int idHook);

    //导入CallNextHookEx函数，用于传递消息给下一个钩子过程
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

    //导入GetModuleHandle函数，用于获取当前模块句柄
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string name);

    //开始监听鼠标事件的方法
    public static void Start()
    {
      if (hMouseHook == 0)
      {
        MouseHookProcedure = new MouseHookProc(MouseHookProcAct);
        hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
        if (hMouseHook == 0)
        {
          Stop();
          throw new Exception("SetWindowsHookEx failed.");
        }
      }
    }

    //停止监听鼠标事件的方法
    public static void Stop()
    {
      bool retMouse = true;
      if (hMouseHook != 0)
      {
        retMouse = UnhookWindowsHookEx(hMouseHook);
        hMouseHook = 0;
      }
      if (!(retMouse)) throw new Exception("UnhookWindowsHookEx failed.");
    }

    //鼠标钩子回调函数，用于处理鼠标事件
    public static int MouseHookProcAct(int nCode, int wParam, IntPtr lParam)
    {
      if (nCode >= 0 && MouseAction != null)
      {
        MouseButtons button = MouseButtons.None;
        switch (wParam)
        {
          case WM_LBUTTONDOWN:
            button = MouseButtons.Left;
            break;
          case WM_RBUTTONDOWN:
            button = MouseButtons.Right;
            break;
          case WM_LBUTTONUP:
            button = MouseButtons.Left;
            break;
          case WM_RBUTTONUP:
            button = MouseButtons.Right;
            break;
        }
        if (button != MouseButtons.None)
        {
          MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
          MouseEventArgs e = new MouseEventArgs(button, 0, MyMouseHookStruct.pt.X, MyMouseHookStruct.pt.Y, 0);
          MouseAction(null, e);
        }
      }
      return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
    }

    

  }

  //模拟按键输入
  public static class KeysHook
  {

    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const int KEYEVENTF_KEYUP = 0x0002;
    

    private static void ListAllKeys()
    {
      for (int i = 0; i < 256; i++)
      {
        if (IsKeyEnabled(i))
        {
          Console.WriteLine((Keys)i);
        }
      }
    }

    private static bool IsKeyEnabled(int keyCode)
    {
      return (GetKeyState(keyCode) & 0x8000) != 0;
    }

    [DllImport("user32.dll")]
    public static extern short GetKeyState(int nVirtKey);

    public static void SimulateKeyPress(Keys key)
    {
      byte keyCode = (byte)key;
      // 模拟按下键
      keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY, 0);
      // 模拟释放键
      keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }
    public static void SimulateKeyPress(Keys key1, Keys key2)
    {
      byte keyCode1 = (byte)key1;
      byte keyCode2 = (byte)key2;
      // 模拟按下第一个键
      keybd_event(keyCode1, 0, KEYEVENTF_EXTENDEDKEY, 0);
      // 延迟一定时间
      Thread.Sleep(100);
      // 模拟按下第二个键
      keybd_event(keyCode2, 0, KEYEVENTF_EXTENDEDKEY, 0);
      // 延迟一定时间
      Thread.Sleep(100);
      // 模拟释放第二个键
      keybd_event(keyCode2, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
      // 延迟一定时间
      Thread.Sleep(100);
      // 模拟释放第一个键
      keybd_event(keyCode1, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }

    //定义一个函数，读取string字符串，将a-z转换成Key.A-Key.Z，将0-9转成Key.0-Key.9，将空格转成Key.Space
    public static Keys ConvertStringToKey(string s)
    {
      //如果字符串为空或者长度大于1，返回None
      if (string.IsNullOrEmpty(s) || s.Length > 1)
      {
        return Keys.None;
      }
      //获取字符串中的第一个字符
      char c = s[0];
      //获取字符的ASCII码
      int ascii = (int)c;
      //如果字符是a-z之间的小写字母，
      if (ascii >= 97 && ascii <= 122)
      {
        return (Keys)(ascii - 32); //返回对应的Key.A-Key.Z（不管大小写都是输出大写A）
      }
      //如果字符是A-Z之间的大写字母，
      else if (ascii >= 65 && ascii <= 90)
      {
        return (Keys)ascii; //返回对应的Key.A-Key.Z
      }
      //如果字符是0-9之间的数字，返回对应的Key.0-Key.9
      else if (ascii >= 48 && ascii <= 57)
      {
        return (Keys)(ascii + 48);
      }
      //如果字符是空格，返回Key.Space
      else if (ascii == 32)
      {
        return Keys.Space;
      }
      else if (ascii == 13)
      {
        return Keys.Enter;
      }
      //其他情况，返回None
      else
      {
        return Keys.None;
      }
    }

  }


}
