namespace AutoMouseMover
{
    partial class AutoMouseMoverForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoMouseMoverForm));
            this.StartButton = new System.Windows.Forms.Button();
            this.MovingTimeLabel = new System.Windows.Forms.Label();
            this.MovingPixelLabel = new System.Windows.Forms.Label();
            this.MovingPixelBox = new System.Windows.Forms.NumericUpDown();
            this.MovingPeriodBox = new System.Windows.Forms.NumericUpDown();
            this.MovingTimeSecondLabel = new System.Windows.Forms.Label();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.CursorTimer = new System.Windows.Forms.Timer(this.components);
            this.TrayBarIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayBarContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TrayBarMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayBarMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.StopButton = new System.Windows.Forms.Button();
            this.MinimizeToTrayBarBox = new System.Windows.Forms.CheckBox();
            this.ShowTrayBarIconBox = new System.Windows.Forms.CheckBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.StripMenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.MovingPixelBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MovingPeriodBox)).BeginInit();
            this.StatusStrip.SuspendLayout();
            this.TrayBarContextMenu.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(19, 184);
            this.StartButton.Margin = new System.Windows.Forms.Padding(4);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(100, 28);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // MovingTimeLabel
            // 
            this.MovingTimeLabel.AutoSize = true;
            this.MovingTimeLabel.Location = new System.Drawing.Point(16, 47);
            this.MovingTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MovingTimeLabel.Name = "MovingTimeLabel";
            this.MovingTimeLabel.Size = new System.Drawing.Size(166, 17);
            this.MovingTimeLabel.TabIndex = 3;
            this.MovingTimeLabel.Text = "Move when PC is idle for:";
            // 
            // MovingPixelLabel
            // 
            this.MovingPixelLabel.AutoSize = true;
            this.MovingPixelLabel.Location = new System.Drawing.Point(16, 79);
            this.MovingPixelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MovingPixelLabel.Name = "MovingPixelLabel";
            this.MovingPixelLabel.Size = new System.Drawing.Size(210, 17);
            this.MovingPixelLabel.TabIndex = 4;
            this.MovingPixelLabel.Text = "Number of pixel to move mouse:";
            // 
            // MovingPixelBox
            // 
            this.MovingPixelBox.Location = new System.Drawing.Point(235, 76);
            this.MovingPixelBox.Margin = new System.Windows.Forms.Padding(4);
            this.MovingPixelBox.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.MovingPixelBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MovingPixelBox.Name = "MovingPixelBox";
            this.MovingPixelBox.Size = new System.Drawing.Size(67, 22);
            this.MovingPixelBox.TabIndex = 5;
            this.MovingPixelBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // MovingPeriodBox
            // 
            this.MovingPeriodBox.Location = new System.Drawing.Point(235, 44);
            this.MovingPeriodBox.Margin = new System.Windows.Forms.Padding(4);
            this.MovingPeriodBox.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.MovingPeriodBox.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.MovingPeriodBox.Name = "MovingPeriodBox";
            this.MovingPeriodBox.Size = new System.Drawing.Size(67, 22);
            this.MovingPeriodBox.TabIndex = 6;
            this.MovingPeriodBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // MovingTimeSecondLabel
            // 
            this.MovingTimeSecondLabel.AutoSize = true;
            this.MovingTimeSecondLabel.Location = new System.Drawing.Point(309, 47);
            this.MovingTimeSecondLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MovingTimeSecondLabel.Name = "MovingTimeSecondLabel";
            this.MovingTimeSecondLabel.Size = new System.Drawing.Size(71, 17);
            this.MovingTimeSecondLabel.TabIndex = 7;
            this.MovingTimeSecondLabel.Text = "second(s)";
            // 
            // StatusStrip
            // 
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 230);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.StatusStrip.Size = new System.Drawing.Size(393, 25);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.TabIndex = 8;
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(74, 20);
            this.StatusLabel.Text = "Status: {0}";
            // 
            // CursorTimer
            // 
            this.CursorTimer.Tick += new System.EventHandler(this.CursorTimer_Tick);
            // 
            // TrayBarIcon
            // 
            this.TrayBarIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TrayBarIcon.BalloonTipText = "Automatic mouse mover is running in background";
            this.TrayBarIcon.BalloonTipTitle = "Automatic mouse mover";
            this.TrayBarIcon.ContextMenuStrip = this.TrayBarContextMenu;
            this.TrayBarIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayBarIcon.Icon")));
            this.TrayBarIcon.Text = "Automatic mouse mover running";
            this.TrayBarIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayBarIcon_MouseDoubleClick);
            // 
            // TrayBarContextMenu
            // 
            this.TrayBarContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.TrayBarContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayBarMenuOpen,
            this.TrayBarMenuClose});
            this.TrayBarContextMenu.Name = "TrayBarContextMenu";
            this.TrayBarContextMenu.Size = new System.Drawing.Size(115, 52);
            // 
            // TrayBarMenuOpen
            // 
            this.TrayBarMenuOpen.Name = "TrayBarMenuOpen";
            this.TrayBarMenuOpen.Size = new System.Drawing.Size(114, 24);
            this.TrayBarMenuOpen.Text = "Open";
            this.TrayBarMenuOpen.Click += new System.EventHandler(this.TrayBarMenuOpen_Click);
            // 
            // TrayBarMenuClose
            // 
            this.TrayBarMenuClose.Name = "TrayBarMenuClose";
            this.TrayBarMenuClose.Size = new System.Drawing.Size(114, 24);
            this.TrayBarMenuClose.Text = "Close";
            this.TrayBarMenuClose.Click += new System.EventHandler(this.TrayBarMenuClose_Click);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(274, 184);
            this.StopButton.Margin = new System.Windows.Forms.Padding(4);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(100, 28);
            this.StopButton.TabIndex = 10;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // MinimizeToTrayBarBox
            // 
            this.MinimizeToTrayBarBox.AutoSize = true;
            this.MinimizeToTrayBarBox.Checked = true;
            this.MinimizeToTrayBarBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MinimizeToTrayBarBox.Location = new System.Drawing.Point(20, 114);
            this.MinimizeToTrayBarBox.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeToTrayBarBox.Name = "MinimizeToTrayBarBox";
            this.MinimizeToTrayBarBox.Size = new System.Drawing.Size(238, 21);
            this.MinimizeToTrayBarBox.TabIndex = 11;
            this.MinimizeToTrayBarBox.Text = "Minimize to tray bar when started";
            this.MinimizeToTrayBarBox.UseVisualStyleBackColor = true;
            this.MinimizeToTrayBarBox.CheckedChanged += new System.EventHandler(this.MinimizeToTrayBarBox_CheckedChanged);
            // 
            // ShowTrayBarIconBox
            // 
            this.ShowTrayBarIconBox.AutoSize = true;
            this.ShowTrayBarIconBox.Checked = true;
            this.ShowTrayBarIconBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowTrayBarIconBox.Location = new System.Drawing.Point(20, 143);
            this.ShowTrayBarIconBox.Margin = new System.Windows.Forms.Padding(4);
            this.ShowTrayBarIconBox.Name = "ShowTrayBarIconBox";
            this.ShowTrayBarIconBox.Size = new System.Drawing.Size(250, 21);
            this.ShowTrayBarIconBox.TabIndex = 12;
            this.ShowTrayBarIconBox.Text = "Show tray bar icon when minimized";
            this.ShowTrayBarIconBox.UseVisualStyleBackColor = true;
            // 
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenuAbout});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.MenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MenuStrip.Size = new System.Drawing.Size(393, 28);
            this.MenuStrip.TabIndex = 13;
            // 
            // StripMenuAbout
            // 
            this.StripMenuAbout.Name = "StripMenuAbout";
            this.StripMenuAbout.Size = new System.Drawing.Size(62, 24);
            this.StripMenuAbout.Text = "About";
            this.StripMenuAbout.Click += new System.EventHandler(this.StripMenuAbout_Click);
            // 
            // AutoMouseMoverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 255);
            this.Controls.Add(this.ShowTrayBarIconBox);
            this.Controls.Add(this.MinimizeToTrayBarBox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.MenuStrip);
            this.Controls.Add(this.MovingTimeSecondLabel);
            this.Controls.Add(this.MovingPeriodBox);
            this.Controls.Add(this.MovingPixelBox);
            this.Controls.Add(this.MovingPixelLabel);
            this.Controls.Add(this.MovingTimeLabel);
            this.Controls.Add(this.StartButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "AutoMouseMoverForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Automatic Mouse Mover";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoMouseMoverForm_FormClosing);
            this.Resize += new System.EventHandler(this.AutoMouseMoverForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.MovingPixelBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MovingPeriodBox)).EndInit();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.TrayBarContextMenu.ResumeLayout(false);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label MovingTimeLabel;
        private System.Windows.Forms.Label MovingPixelLabel;
        private System.Windows.Forms.NumericUpDown MovingPixelBox;
        private System.Windows.Forms.NumericUpDown MovingPeriodBox;
        private System.Windows.Forms.Label MovingTimeSecondLabel;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.Timer CursorTimer;
        private System.Windows.Forms.NotifyIcon TrayBarIcon;
        private System.Windows.Forms.ContextMenuStrip TrayBarContextMenu;
        private System.Windows.Forms.ToolStripMenuItem TrayBarMenuOpen;
        private System.Windows.Forms.ToolStripMenuItem TrayBarMenuClose;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.CheckBox MinimizeToTrayBarBox;
        private System.Windows.Forms.CheckBox ShowTrayBarIconBox;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem StripMenuAbout;
    }
}

