using WinControlApp;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using Timer = System.Windows.Forms.Timer;
using static WinControlApp.GlobalFunc;

namespace WinControlApp
{
  public partial class FrmMain : Form
  {
    // 定义一个右键菜单
    private ContextMenuStrip menu = new ContextMenuStrip();
    //private static ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
    private static NotifyIcon notifyIcon = new NotifyIcon();
    private static ListViewItem dragItem;
    private static Timer timerThread = new Timer(), timer = new();
    private static int scrollSpeed = 1, scrollDirection = -1; // 滚动速度// 滚动方向（1 表示从右到左，-1 表示从左到右）
    public FrmMain()
    {
      InitializeComponent();
      this.ForeColor = Color.Black;
      this.Text = " i民生活跃提醒小助理  😀😀😀";
      this.FormClosing += FrmMain_FormClosing;
      this.CenterToScreen();
      // 设置 View 属性为 View.Details  // 设置 ListView 的 LabelEdit 属性为 true
      listViewActlist.View = View.Details; listViewActlist.AllowDrop = true; listViewActlist.LabelEdit = true; listViewActlist.FullRowSelect = true;
      // 添加列标题
      listViewActlist.Columns.AddRange(new[] {
          new ColumnHeader { Text = "坐标", Width = 90 },
          new ColumnHeader { Text = "停秒", Width = 40 },
          new ColumnHeader { Text = "输入文本", Width = 150 },
          new ColumnHeader { Text = "时间", Width = 100 }
      });
      this.Resize += FrmMain_Resize;
      timerThread.Tick += timerThread_Tick;
      timer.Tick += Timer_Tick;
      textBoxPlan.LostFocus += TextBoxPlan_LostFocus;
      // 添加双击事件处理程序
      notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
      listViewActlist.DoubleClick += ListViewActlist_DoubleClick;
      listViewActlist.ItemDrag += ListViewActlist_ItemDrag;
      listViewActlist.DragEnter += ListViewActlist_DragEnter;
      listViewActlist.DragOver += ListViewActlist_DragOver;
      listViewActlist.DragDrop += ListViewActlist_DragDrop;
      listViewActlist.MouseDown += ListViewActlist_MouseDown;
      // 添加右键菜单项（简化写法）
      menu.Items.AddRange(new ToolStripItem[]
      {
        new ToolStripMenuItem("导出", null, (sender, e) => Export_Click(sender, e)),
        new ToolStripMenuItem("导入", null, (sender, e) => Import_Click(sender, e)),
        new ToolStripSeparator(),
        new ToolStripMenuItem("复制", null, (sender, e) => Copy_Click(sender, e)),
        new ToolStripMenuItem("删除", null, (sender, e) => ListView_DelItem(sender, e)),
        new ToolStripSeparator(),
        new ToolStripMenuItem("清空", null, (sender, e) => ListView_ClearItem(sender, e))
      });
      //原始写法***************************************
      //// 为右键菜单添加两个选项：导出和导入
      //menu.Items.Add("导出");
      //menu.Items.Add("导入");
      //menu.Items.Add(new ToolStripSeparator());
      //menu.Items.Add("复制");
      //menu.Items.Add("复制");
      //// 为右键菜单的选项添加单击事件处理方法
      //menu.Items[0].Click += Export_Click;
      //menu.Items[1].Click += Import_Click;
      //menu.Items[3].Click += Copy_Click;
      //menu.Items[4].Click += ListView_DelItem;
      //\\\\\\\\\***************************************
      // 为ListView添加鼠标事件处理方法，用于显示右键菜单
      listViewActlist.MouseUp += ListView_MouseUp;
      textBoxSplt.KeyUp += TextBoxSplt_KeyUp;
      textBoxWait.KeyUp += TextBoxWait_KeyUp;
      for (int i = 0; i < MouseClkitems.Length; i++) { comboBoxActList.Items.Add(MouseClkitems[i]); }
      comboBoxActList.SelectedIndex = 0;
      //读取Json
      GreatJsonData();
      //初始化鼠标
      MouseMain();
      if (pinyinList.Count < 10) { MessageBox.Show("拼音数据调取失败或源文件数据量太小...",this.Text); }
    }

    private void TextBoxPlan_LostFocus(object? sender, EventArgs e)
    {
      if (!CheckTextBoxPlan(textBoxPlan.Text)) { textBoxPlan.Text = ""; MessageBox.Show("请按照以下两种格式进行输入：\r\nyyyy-MM-dd HH:mm:ss 和 HH:mm:ss", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
    private bool CheckTextBoxPlan(string textin)
    {
      if (textin.Length == 0) { return true; }
      string[] formats = new string[] { "yyyy-MM-dd HH:mm:ss", "HH:mm:ss" };
      textin = textin.Replace("：", ":");
      return DateTime.TryParseExact(textin, formats, null, System.Globalization.DateTimeStyles.None, out DateTime date);
    }

    private void TextBoxWait_KeyUp(object? sender, KeyEventArgs e)
    {
      if (int.Parse(textBoxWait.Text) > int.Parse(textBoxSplt.Text))
      {
        textBoxWait.Text = textBoxSplt.Text;
      }
    }

    private void TextBoxSplt_KeyUp(object? sender, KeyEventArgs e)
    {
      if (int.Parse(textBoxSplt.Text) < 1)
      {
        textBoxSplt.Text = "1";
      }
      TextBoxWait_KeyUp(sender, e);
    }

    private void FrmMain_FormClosing(object? sender, FormClosingEventArgs e)
    {
      try
      {
        // 显示询问对话框，询问用户是否要退出
        DialogResult result = MessageBox.Show("确定要退出应用程序吗？", this.Text, MessageBoxButtons.YesNo);
        // 如果用户选择“否”，则取消窗体关闭事件
        if (result == DialogResult.No)
        {
          e.Cancel = true;
          return;
        }
        if (isnotifyIconVisible) { buttonGetFoc_Click(sender, e); Thread.Sleep(1000); }
        //停止监听鼠标事件
        MouseHook.Stop();
      }
      catch (Exception)
      {
        Process.GetCurrentProcess().Kill();
      }
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
      buttonSetFoc.Enabled = true; buttonGetFoc.Enabled = false;
      timerThread.Enabled = true; timerThread.Stop(); timerThread.Interval = 500; // 0.5秒
      timer.Enabled = true; timer.Stop(); timer.Interval = 10; // 设置滚动速度（以毫秒为单位）
      label.Top = this.Height - 61; label.Left = statusStripFoot.Width; label.ForeColor = Color.Red;
      label.Visible = true;
      timer.Start();

    }
    private void FrmMain_Resize(object? sender, EventArgs e)
    {
      // 当窗体最小化时，隐藏到系统栏
      if (this.WindowState == FormWindowState.Minimized)
      {
        this.Hide();
        notifyIcon.Visible = true;
      }
      else
      {
        notifyIcon.Visible = false;
      }
    }
    private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
    {
      // 双击系统栏图标时，显示窗体
      this.Show();
      this.WindowState = FormWindowState.Normal;
    }
    //50毫秒计数器
    private void Timer_Tick(object sender, EventArgs e)
    {
      TimeSpan timeDiffSplt = lastActTime - DateTime.Now; // 计算时间差（间隔期）
      if (isnotifyIconVisible)
      {
        label.Text = $"{lastActTime.ToString("HH:mm:ss")}: 还剩{((int)Math.Round(timeDiffSplt.TotalSeconds) > 60 ? (int)Math.Round(timeDiffSplt.TotalMinutes) + "分钟" : (int)Math.Round(timeDiffSplt.TotalSeconds) + "秒")}开始执行...";
      }
      else
      {
        label.Text = "本软件为RPA系统程序，使用过程中需要获取计算机管理员权限，请勿使用本软件进行任何不当行为...";
      }
      if (tTmpLog.Length > 0)
      {
        richTextBoxLog.Text = tTmpLog + richTextBoxLog.Text; tTmpLog = "";
        //只保留这么多
        if (richTextBoxLog.TextLength > 9999) { richTextBoxLog.Text = richTextBoxLog.Text.Substring(0, 9999); }
      }
      if (this.WindowState == FormWindowState.Normal)
      {
        // 滚动 Label 控件的文本
        label.Left += scrollSpeed * scrollDirection;
        // 当 Label 控件的 Left 属性超出窗体的右边缘时，重置为窗体的左边缘
        if (scrollDirection == 1 && label.Left >= statusStripFoot.Width) { label.Left = -label.Width; }
        // 当 Label 控件的 Right 属性小于窗体的左边缘时，重置为窗体的右边缘
        if (scrollDirection == -1 && label.Right <= 0) { label.Left = richTextBoxLog.Left; }
      }
      if (notifyIcon.Visible)
      {
        if (!isnotifyIconVisible) { notifyIcon.Icon = this.Icon; }  //非闪烁期常态化显示
        notifyIcon.Text = this.Text + $" --- {label.Text}";
      }
      if (mouseDoingFlg) { buttonActTryXY.Text = "停止"; }  else { buttonActTryXY.Text = "模拟"; }
    }
    //线程启动计数器
    private void timerThread_Tick(object? sender, EventArgs e)
    {
      labelTimer.Text = $"🔴🔴🔴\r\n{DateTime.Now.ToString("HH:mm:ss")}";
      Point currentCursorPosition = Cursor.Position;
      Application.DoEvents();
      //启动后5分钟之后再执行全局的计划任务，主要防止操作失误，反复执行
      if (eventActTime.AddMinutes(5) < DateTime.Now)
      {
        //有时间的按照时间执行，没有计划执行时间的按照周期（间隔时间执行）
        //执行指定时间的流程，开启子线程，但是必须等子线程运行完毕
        Thread mainThread = new Thread(() => ListCheckDoFrst(ref isDoingFlg));
        mainThread.Start(); mainThread.Join();// 等待子线程完成
      }
      if (listViewActlist.Items.Count == 0 && isnotifyIconVisible)
      {
        buttonGetFoc_Click(sender, e); Thread.Sleep(3000); return;  //没有执行列表，停止执行
      }
      //int result = DateTime.Compare(DateTime.Now, eventActTime); //比较当前时间和eventActTime的值（当前时间大于eventActTime）
      //TimeSpan timediff = DateTime.Now.Subtract(eventActTime);//计算当前时间和eventActTime之间的时间间隔
      if (isDoingFlg)
      {
        //运行期间，时间无限重置延长
        lastActTime = DateTime.Now.AddMinutes(Convert.ToInt16(textBoxSplt.Text));
        //运行期间检测到鼠标动作默认是系统操作
        if (eventActFlg)
        {
          //默认所有的操作都是系统执行的
          eventActFlg = false;
          //如果发现鼠标位置与上一次位置不同且发生了点击事件，就证明人工干预了，不继续执行了，但是本次RPA终止
          if (lastCursorPosition != currentCursorPosition) { doingActPauseFlg = true; tTmpLog += $"{DateTime.Now.ToString("HH:mm:ss")}: 人工介入，终止本次RPA任务...\r\n"; }
        }
      }
      else
      {
        //非运行期间
        //自动设置到最后的位置
        lastCursorPosition = currentCursorPosition;
        //未在运行才会开始执行RPA，如果已经在运行就不执行了
        if (lastActTime < DateTime.Now)
        {
          //读取RPA流程计划，开始执行
          //Thread thread = new Thread(new ThreadStart(ExecuteListViewItems));
          // 获取listView中的所有项
          List<string[]> listItems = new List<string[]>();
          foreach (ListViewItem item in listViewActlist.Items)
          {
            if (item.SubItems[3].Text == "") { listItems.Add(item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(s => s.Text).ToArray()); }
          }
          doingActPauseFlg = false;
          Thread thread = new Thread(() => ExecuteListViewItems(listItems, ref isDoingFlg)); thread.Start();
        }
        //非运行期间才判断是否有人工干预
        if (eventActFlg)
        {
          //叠加静默时间（如果DateTime.Now加上静默时间小于lastActTime就不加了，如果大于就加）
          if (checkBoxWait.Checked && DateTime.Now.AddMinutes(Convert.ToInt16(textBoxWait.Text)) > lastActTime)
          {
            lastActTime = DateTime.Now.AddMinutes(Convert.ToInt16(textBoxWait.Text));
            tTmpLog += $"静默期干预，重置时间{lastActTime.ToString("HH:mm:ss")}\r\n";
          }
          eventActFlg = false;
        }
      }
      if (isnotifyIconVisible)
      {
        if (isDoingFlg)
        {
          notifyIcon.Icon = notifyIcon.Icon != iconbs ? iconbs : icon1;
        }
        else
        {
          notifyIcon.Icon = notifyIcon.Icon != this.Icon ? this.Icon : icon1;
        }
      }
    }

    private void buttonSetFoc_Click(object? sender, EventArgs e)
    {
      if (listViewActlist.Items.Count == 0)
      {
        return;
      }
      //检测需要启动的程序
      Process[] processes = Process.GetProcessesByName(prname);
      if (!(processes.Length > 0))
      {
        if (MessageBox.Show("i民生未启动，是否继续执行？", this.Text, MessageBoxButtons.YesNo) == DialogResult.No)
        {
          return;
        }
      }
      richTextBoxLog.Text = "";
      //将有指定时间的RPA流程单独存放
      foreach (ListViewItem item in listViewActlist.Items)
      {
        if (item.SubItems[3].Text.Length > 4) { dtlistItems.Add(item.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(s => s.Text).ToArray()); }
      }
      try
      {
        buttonSetFoc.Enabled = false;
        lastCursorPosition = Cursor.Position;
        eventActTime = DateTime.Now; //启动时间
        lastActTime = eventActTime.AddMinutes(Convert.ToInt16(textBoxSplt.Text)); //周期计划开始时间
        timerThread.Start();
        buttonGetFoc.Enabled = true;
        labelTimer.ForeColor = buttonSetFoc.BackColor;
        this.WindowState = FormWindowState.Minimized;
      }
      catch (Exception)
      {
        buttonSetFoc.Enabled = true;
      }
      isnotifyIconVisible = true;
    }

    private void buttonGetFoc_Click(object? sender, EventArgs e)
    {
      try
      {
        buttonGetFoc.Enabled = false;
        timerThread.Stop();
        toolStripStatusLabel.Text = "";
        buttonSetFoc.Enabled = true;
        labelTimer.ForeColor = buttonGetFoc.BackColor;
      }
      catch (Exception)
      {
        buttonGetFoc.Enabled = true;
      }
      isnotifyIconVisible = false;
      //IntPtr foregroundWindow = GetForegroundWindow();
      //StringBuilder windowText = new StringBuilder(256);
      //GetWindowText(foregroundWindow, windowText, 256);

      //richTextBoxLog.Text += ("当前活动窗口标题: " + windowText.ToString() + "\r\n");
    }




    //捕获XY坐标
    private void buttonActGetXY_Click(object sender, EventArgs e)
    {
      if (isDoingFlg) { return; }
      ismouseXYCapture = true;
      this.Visible = false;
      FormClr formClr = new FormClr();
      formClr.ShowDialog();
      textBInX.Text = lastCursorPosition.X.ToString();
      textBInY.Text = lastCursorPosition.Y.ToString();
      this.Visible = true;
      ismouseXYCapture = false;
    }

    //添加动作
    private void buttonActAdd_Click(object sender, EventArgs e)
    {
      if (isnotifyIconVisible) { return; }
      if (!CheckTextBoxPlan(textBoxPlan.Text)) { textBoxPlan.Text = ""; }
      if (textBInCont.Text.Length > 0) { comboBoxActList.Text = "单击"; }
      ListViewItem item = new ListViewItem(new string[] {
        $"{textBInX.Text}|{textBInY.Text}|{comboBoxActList.Text}", textBInTimer.Text, textBInCont.Text.Replace("\r\n", "").Replace("\n", ""), textBoxPlan.Text});
      listViewActlist.Items.Add(item);
    }

    private void ListViewActlist_DoubleClick(object? sender, EventArgs e)
    {
      if (isnotifyIconVisible) { return; }
      if (listViewActlist.SelectedItems.Count > 0)
      {
        // 获取双击的行
        ListViewItem selectedItem = listViewActlist.SelectedItems[0];
        // 启用编辑模式
        selectedItem.BeginEdit();
        foreach (ListViewItem.ListViewSubItem subItem in selectedItem.SubItems)
        {
          // 将子项的 Selected 属性设置为 true
          //subItem.Selected = true;
        }
      }
    }
    private void ListViewActlist_ItemDrag(object? sender, ItemDragEventArgs e)
    {
      if (isnotifyIconVisible) { return; }
      dragItem = (ListViewItem)e.Item;
      listViewActlist.DoDragDrop(dragItem, DragDropEffects.Move);
    }
    private void ListViewActlist_DragEnter(object? sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.Move;
    }
    private void ListViewActlist_DragOver(object? sender, DragEventArgs e)
    {
      if (isnotifyIconVisible) { return; }
      Point dragPosition = listViewActlist.PointToClient(new Point(e.X, e.Y));
      ListViewItem hoverItem = listViewActlist.GetItemAt(dragPosition.X, dragPosition.Y);
      if (hoverItem != null && hoverItem != dragItem)
      {
        int hoverIndex = hoverItem.Index;
        int dragIndex = dragItem.Index;
        if (hoverIndex < dragIndex)
        {
          hoverIndex++;
        }
        listViewActlist.Items.Remove(dragItem);
        listViewActlist.Items.Insert(hoverIndex, dragItem);
      }
    }
    private void ListViewActlist_DragDrop(object? sender, DragEventArgs e)
    {
      dragItem = null;
    }
    private void ListViewActlist_MouseDown(object? sender, MouseEventArgs e)
    {
      if (isnotifyIconVisible) { return; }
      dragItem = null;
    }
    private void ListView_DelItem(object? sender, EventArgs e)
    {
      if (listViewActlist.SelectedItems.Count > 0)
      {
        listViewActlist.Items.Remove(listViewActlist.SelectedItems[0]);
      }
    }
    private void ListView_ClearItem(object? sender, EventArgs e)
    {
      if (MessageBox.Show("是否清空RPA任务列表？", this.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        listViewActlist.Items.Clear();
      }
    }
    //右键点击
    private void ListView_MouseUp(object? sender, MouseEventArgs e)
    {
      if (isnotifyIconVisible) { return; }
      // 如果鼠标右键被按下，并且有选中项，就显示右键菜单
      if (e.Button == MouseButtons.Right)
      {
        menu.Show((ListView)sender, e.Location);
      }
    }

    //模拟操作
    private void buttonActTryXY_Click(object sender, EventArgs e)
    {
      if (isnotifyIconVisible) { return; }
      if (textBInCont.Text.Length > 0) { comboBoxActList.Text = "单击"; }
      //this.WindowState = FormWindowState.Minimized;
      //Application.DoEvents();
      if (buttonActTryXY.Text == "模拟")
      {
        doingActPauseFlg = false;
        int tx = int.Parse(textBInX.Text), ty = int.Parse(textBInY.Text);
        string xact = comboBoxActList.Text, xin = textBInCont.Text;
        Thread.Sleep(1000);
        Thread simThread = new Thread(() => SetMouseXY(tx,ty,xact,xin));
        simThread.Start();
      }
      else
      {
        doingActPauseFlg = true;
      }
      Thread.Sleep(1000);
    }






    private void Export_Click(object? sender, EventArgs e)
    {
      // 获取选中的ListView控件
      ListView lv = (ListView)menu.SourceControl;
      if (lv.Items.Count <= 0)
      {
        return;
      }
      // 创建一个保存文件对话框，用于选择导出文件的路径和格式
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.DefaultExt = ".txt";
      sfd.Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv";
      if (sfd.ShowDialog() == DialogResult.OK)
      {
        // 获取选中的文件名和格式
        string fileName = sfd.FileName;
        string fileFormat = sfd.FilterIndex == 1 ? ".txt" : ".csv";
        // 创建一个文件流，用于写入数据到文件中
        using (FileStream fs = new FileStream(fileName, FileMode.Create))
        {
          using (StreamWriter sw = new StreamWriter(fs))
          {
            // 写入列标题，用制表符或逗号分隔，根据不同的格式
            foreach (ColumnHeader ch in lv.Columns)
            {
              sw.Write(ch.Text + (fileFormat == ".txt" ? "\t" : ","));
            }
            sw.WriteLine();
            // 写入选中的数据，用制表符或逗号分隔，根据不同的格式
            foreach (ListViewItem lvi in lv.Items)
            {
              foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
              {
                sw.Write(lvs.Text + (fileFormat == ".txt" ? "\t" : ","));
              }
              sw.WriteLine();
            }
          }
        }
        MessageBox.Show("导出成功");
      }
    }
    private void Import_Click(object? sender, EventArgs e)
    {
      // 创建一个打开文件对话框，用于选择导入文件的路径和格式
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.DefaultExt = ".txt";
      ofd.Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv";
      if (ofd.ShowDialog() == DialogResult.OK)
      {
        // 获取选中的ListView控件
        ListView lv = (ListView)menu.SourceControl;
        // 获取选中的文件名和格式
        string fileName = ofd.FileName;
        string fileFormat = ofd.FilterIndex == 1 ? ".txt" : ".csv";
        // 创建一个文件流，用于读取数据从文件中
        using (FileStream fs = new FileStream(fileName, FileMode.Open))
        {
          using (StreamReader sr = new StreamReader(fs))
          {
            // 跳过第一行，不读取列标题
            sr.ReadLine();
            lv.Items.Clear();
            // 读取剩余的数据，用制表符或逗号分隔，根据不同的格式
            while (!sr.EndOfStream)
            {
              string line = sr.ReadLine();
              string[] items = line.Split(fileFormat == ".txt" ? '\t' : ',');
              // 将数据添加到ListView中
              lv.Items.Add(new ListViewItem(items));
            }
          }
        }
      }
    }
    private void Copy_Click(object? sender, EventArgs e)
    {
      // 获取选中的ListView控件
      ListView lv = (ListView)menu.SourceControl;
      // 遍历选中的数据，复制到ListView的最后一行
      foreach (ListViewItem lvi in lv.SelectedItems)
      {
        // 创建一个新的ListViewItem，复制选中项的数据
        ListViewItem newItem = new ListViewItem(lvi.SubItems[0].Text);
        for (int i = 1; i < lvi.SubItems.Count; i++)
        {
          newItem.SubItems.Add(lvi.SubItems[i].Text);
        }
        // 将新的ListViewItem添加到ListView中
        lv.Items.Add(newItem);
      }
    }




    //#
  }

}