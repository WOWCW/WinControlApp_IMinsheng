namespace WinControlApp
{
  partial class FrmMain
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
      buttonSetFoc = new Button();
      buttonGetFoc = new Button();
      richTextBoxLog = new RichTextBox();
      groupBox = new GroupBox();
      textBoxPlan = new TextBox();
      comboBoxActList = new ComboBox();
      buttonActTryXY = new Button();
      textBInTimer = new TextBox();
      textBoxWait = new TextBox();
      buttonActGetXY = new Button();
      buttonActAdd = new Button();
      label3 = new Label();
      textBInCont = new RichTextBox();
      textBInY = new TextBox();
      textBInX = new TextBox();
      label2 = new Label();
      textBoxSplt = new TextBox();
      label1 = new Label();
      statusStripFoot = new StatusStrip();
      toolStripStatusLabel = new ToolStripStatusLabel();
      checkBoxWait = new CheckBox();
      label5 = new Label();
      label4 = new Label();
      listViewActlist = new ListView();
      labelTimer = new Label();
      label = new Label();
      groupBox.SuspendLayout();
      statusStripFoot.SuspendLayout();
      SuspendLayout();
      // 
      // buttonSetFoc
      // 
      buttonSetFoc.BackColor = Color.SeaGreen;
      buttonSetFoc.FlatStyle = FlatStyle.Popup;
      buttonSetFoc.Font = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point);
      buttonSetFoc.ForeColor = Color.White;
      buttonSetFoc.Location = new Point(5, 8);
      buttonSetFoc.Name = "buttonSetFoc";
      buttonSetFoc.Size = new Size(55, 30);
      buttonSetFoc.TabIndex = 0;
      buttonSetFoc.Text = "▶";
      buttonSetFoc.UseVisualStyleBackColor = false;
      buttonSetFoc.Click += buttonSetFoc_Click;
      // 
      // buttonGetFoc
      // 
      buttonGetFoc.BackColor = Color.Brown;
      buttonGetFoc.FlatStyle = FlatStyle.Popup;
      buttonGetFoc.Font = new Font("微软雅黑", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
      buttonGetFoc.ForeColor = Color.White;
      buttonGetFoc.Location = new Point(69, 8);
      buttonGetFoc.Name = "buttonGetFoc";
      buttonGetFoc.Size = new Size(55, 30);
      buttonGetFoc.TabIndex = 1;
      buttonGetFoc.Text = "\U0001f6d1";
      buttonGetFoc.UseVisualStyleBackColor = false;
      buttonGetFoc.Click += buttonGetFoc_Click;
      // 
      // richTextBoxLog
      // 
      richTextBoxLog.BackColor = Color.WhiteSmoke;
      richTextBoxLog.BorderStyle = BorderStyle.None;
      richTextBoxLog.Location = new Point(204, 275);
      richTextBoxLog.Name = "richTextBoxLog";
      richTextBoxLog.ReadOnly = true;
      richTextBoxLog.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
      richTextBoxLog.Size = new Size(349, 156);
      richTextBoxLog.TabIndex = 2;
      richTextBoxLog.Text = "";
      // 
      // groupBox
      // 
      groupBox.Controls.Add(textBoxPlan);
      groupBox.Controls.Add(comboBoxActList);
      groupBox.Controls.Add(buttonActTryXY);
      groupBox.Controls.Add(textBInTimer);
      groupBox.Controls.Add(textBoxWait);
      groupBox.Controls.Add(buttonActGetXY);
      groupBox.Controls.Add(buttonActAdd);
      groupBox.Controls.Add(label3);
      groupBox.Controls.Add(textBInCont);
      groupBox.Controls.Add(textBInY);
      groupBox.Controls.Add(textBInX);
      groupBox.Controls.Add(label2);
      groupBox.Controls.Add(textBoxSplt);
      groupBox.Controls.Add(label1);
      groupBox.Controls.Add(statusStripFoot);
      groupBox.Controls.Add(checkBoxWait);
      groupBox.Controls.Add(label5);
      groupBox.Controls.Add(label4);
      groupBox.Location = new Point(-1, 56);
      groupBox.Name = "groupBox";
      groupBox.Size = new Size(204, 378);
      groupBox.TabIndex = 3;
      groupBox.TabStop = false;
      groupBox.Text = "流程化定制";
      // 
      // textBoxPlan
      // 
      textBoxPlan.Location = new Point(28, 242);
      textBoxPlan.MaxLength = 50;
      textBoxPlan.Name = "textBoxPlan";
      textBoxPlan.Size = new Size(87, 23);
      textBoxPlan.TabIndex = 17;
      // 
      // comboBoxActList
      // 
      comboBoxActList.DropDownStyle = ComboBoxStyle.DropDownList;
      comboBoxActList.FlatStyle = FlatStyle.Popup;
      comboBoxActList.FormattingEnabled = true;
      comboBoxActList.Location = new Point(119, 66);
      comboBoxActList.Name = "comboBoxActList";
      comboBoxActList.Size = new Size(75, 25);
      comboBoxActList.TabIndex = 14;
      // 
      // buttonActTryXY
      // 
      buttonActTryXY.BackColor = Color.DarkSlateGray;
      buttonActTryXY.FlatStyle = FlatStyle.Popup;
      buttonActTryXY.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
      buttonActTryXY.ForeColor = Color.Snow;
      buttonActTryXY.Location = new Point(148, 280);
      buttonActTryXY.Name = "buttonActTryXY";
      buttonActTryXY.Size = new Size(49, 34);
      buttonActTryXY.TabIndex = 12;
      buttonActTryXY.Text = "模拟";
      buttonActTryXY.UseVisualStyleBackColor = false;
      buttonActTryXY.Click += buttonActTryXY_Click;
      // 
      // textBInTimer
      // 
      textBInTimer.Location = new Point(167, 242);
      textBInTimer.MaxLength = 2;
      textBInTimer.Name = "textBInTimer";
      textBInTimer.Size = new Size(21, 23);
      textBInTimer.TabIndex = 11;
      textBInTimer.Text = "3";
      // 
      // textBoxWait
      // 
      textBoxWait.Location = new Point(126, 33);
      textBoxWait.MaxLength = 3;
      textBoxWait.Name = "textBoxWait";
      textBoxWait.Size = new Size(33, 23);
      textBoxWait.TabIndex = 8;
      textBoxWait.Text = "30";
      // 
      // buttonActGetXY
      // 
      buttonActGetXY.BackColor = Color.MidnightBlue;
      buttonActGetXY.FlatStyle = FlatStyle.Popup;
      buttonActGetXY.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
      buttonActGetXY.ForeColor = Color.Snow;
      buttonActGetXY.Location = new Point(86, 280);
      buttonActGetXY.Name = "buttonActGetXY";
      buttonActGetXY.Size = new Size(49, 34);
      buttonActGetXY.TabIndex = 7;
      buttonActGetXY.Text = "捕获";
      buttonActGetXY.UseVisualStyleBackColor = false;
      buttonActGetXY.Click += buttonActGetXY_Click;
      // 
      // buttonActAdd
      // 
      buttonActAdd.FlatStyle = FlatStyle.Popup;
      buttonActAdd.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
      buttonActAdd.Location = new Point(5, 280);
      buttonActAdd.Name = "buttonActAdd";
      buttonActAdd.Size = new Size(73, 34);
      buttonActAdd.TabIndex = 6;
      buttonActAdd.Text = "添加动作";
      buttonActAdd.UseVisualStyleBackColor = true;
      buttonActAdd.Click += buttonActAdd_Click;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.FlatStyle = FlatStyle.Popup;
      label3.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
      label3.Location = new Point(2, 138);
      label3.Name = "label3";
      label3.Size = new Size(20, 68);
      label3.TabIndex = 5;
      label3.Text = "输\r\n入\r\n内\r\n容";
      // 
      // textBInCont
      // 
      textBInCont.BackColor = Color.White;
      textBInCont.BorderStyle = BorderStyle.FixedSingle;
      textBInCont.Location = new Point(25, 114);
      textBInCont.Name = "textBInCont";
      textBInCont.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
      textBInCont.Size = new Size(169, 117);
      textBInCont.TabIndex = 4;
      textBInCont.Text = "";
      // 
      // textBInY
      // 
      textBInY.Location = new Point(76, 67);
      textBInY.MaxLength = 4;
      textBInY.Name = "textBInY";
      textBInY.Size = new Size(37, 23);
      textBInY.TabIndex = 4;
      textBInY.Text = "200";
      // 
      // textBInX
      // 
      textBInX.Location = new Point(23, 67);
      textBInX.MaxLength = 4;
      textBInX.Name = "textBInX";
      textBInX.Size = new Size(37, 23);
      textBInX.TabIndex = 2;
      textBInX.Text = "220";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.FlatStyle = FlatStyle.Popup;
      label2.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
      label2.Location = new Point(3, 69);
      label2.Name = "label2";
      label2.Size = new Size(76, 19);
      label2.TabIndex = 3;
      label2.Text = "X           Y ";
      // 
      // textBoxSplt
      // 
      textBoxSplt.Location = new Point(40, 33);
      textBoxSplt.MaxLength = 3;
      textBoxSplt.Name = "textBoxSplt";
      textBoxSplt.Size = new Size(33, 23);
      textBoxSplt.TabIndex = 0;
      textBoxSplt.Text = "30";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.FlatStyle = FlatStyle.Popup;
      label1.Font = new Font("Microsoft YaHei UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
      label1.Location = new Point(4, 35);
      label1.Name = "label1";
      label1.Size = new Size(191, 19);
      label1.TabIndex = 9;
      label1.Text = "间隔               静默           分钟";
      // 
      // statusStripFoot
      // 
      statusStripFoot.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
      statusStripFoot.Location = new Point(3, 353);
      statusStripFoot.Name = "statusStripFoot";
      statusStripFoot.Size = new Size(198, 22);
      statusStripFoot.TabIndex = 10;
      statusStripFoot.Text = "statusStrip1";
      // 
      // toolStripStatusLabel
      // 
      toolStripStatusLabel.ForeColor = Color.Black;
      toolStripStatusLabel.Name = "toolStripStatusLabel";
      toolStripStatusLabel.Size = new Size(20, 17);
      toolStripStatusLabel.Text = "   ";
      // 
      // checkBoxWait
      // 
      checkBoxWait.AutoSize = true;
      checkBoxWait.Checked = true;
      checkBoxWait.CheckState = CheckState.Checked;
      checkBoxWait.FlatStyle = FlatStyle.Popup;
      checkBoxWait.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
      checkBoxWait.Location = new Point(120, 12);
      checkBoxWait.Name = "checkBoxWait";
      checkBoxWait.Size = new Size(73, 21);
      checkBoxWait.TabIndex = 13;
      checkBoxWait.Text = "静默延时";
      checkBoxWait.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.FlatStyle = FlatStyle.Popup;
      label5.Font = new Font("Microsoft YaHei UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point);
      label5.Location = new Point(7, 97);
      label5.Name = "label5";
      label5.Size = new Size(192, 14);
      label5.TabIndex = 16;
      label5.Text = "Enter->|||；Esc->~~~； Shift->{}；(^c/^v)";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.FlatStyle = FlatStyle.Popup;
      label4.Font = new Font("Microsoft YaHei UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
      label4.Location = new Point(-3, 244);
      label4.Name = "label4";
      label4.Size = new Size(208, 19);
      label4.TabIndex = 15;
      label4.Text = "计划                      执行后停     秒";
      // 
      // listViewActlist
      // 
      listViewActlist.BackColor = Color.Cornsilk;
      listViewActlist.BorderStyle = BorderStyle.None;
      listViewActlist.Location = new Point(204, 2);
      listViewActlist.Name = "listViewActlist";
      listViewActlist.Size = new Size(349, 270);
      listViewActlist.TabIndex = 4;
      listViewActlist.UseCompatibleStateImageBehavior = false;
      // 
      // labelTimer
      // 
      labelTimer.BackColor = Color.Transparent;
      labelTimer.FlatStyle = FlatStyle.Popup;
      labelTimer.ForeColor = Color.IndianRed;
      labelTimer.Location = new Point(121, 4);
      labelTimer.Name = "labelTimer";
      labelTimer.Size = new Size(87, 50);
      labelTimer.TabIndex = 5;
      labelTimer.Text = "🔴🔴🔴\r\n       ";
      labelTimer.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // label
      // 
      label.AutoSize = true;
      label.FlatStyle = FlatStyle.Popup;
      label.Font = new Font("Microsoft YaHei UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
      label.Location = new Point(118, 215);
      label.Name = "label";
      label.Size = new Size(21, 19);
      label.TabIndex = 11;
      label.Text = "__";
      label.Visible = false;
      // 
      // FrmMain
      // 
      AutoScaleDimensions = new SizeF(7F, 17F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(552, 433);
      Controls.Add(listViewActlist);
      Controls.Add(richTextBoxLog);
      Controls.Add(label);
      Controls.Add(buttonGetFoc);
      Controls.Add(buttonSetFoc);
      Controls.Add(groupBox);
      Controls.Add(labelTimer);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      Icon = (Icon)resources.GetObject("$this.Icon");
      MaximizeBox = false;
      Name = "FrmMain";
      Load += FrmMain_Load;
      groupBox.ResumeLayout(false);
      groupBox.PerformLayout();
      statusStripFoot.ResumeLayout(false);
      statusStripFoot.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button buttonSetFoc;
    private Button buttonGetFoc;
    private RichTextBox richTextBoxLog;
    private GroupBox groupBox;
    private TextBox textBoxSplt;
    private TextBox textBInY;
    private TextBox textBInX;
    private Label label2;
    private Label label3;
    private RichTextBox textBInCont;
    private Button buttonActAdd;
    private Button buttonActGetXY;
    private ListView listViewActlist;
    private TextBox textBoxWait;
    private Label label1;
    private Label labelTimer;
    private Label label;
    private TextBox textBInTimer;
    private Button buttonActTryXY;
    private StatusStrip statusStripFoot;
    private ToolStripStatusLabel toolStripStatusLabel;
    private CheckBox checkBoxWait;
    private ComboBox comboBoxActList;
    private Label label4;
    private Label label5;
    private TextBox textBoxPlan;
  }
}