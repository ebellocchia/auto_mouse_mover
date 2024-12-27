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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoMouseMoverForm));
            StartButton = new System.Windows.Forms.Button();
            MovingTimeLabel = new System.Windows.Forms.Label();
            MovingPixelLabel = new System.Windows.Forms.Label();
            MovingPixelBox = new System.Windows.Forms.NumericUpDown();
            MovingPeriodBox = new System.Windows.Forms.NumericUpDown();
            MovingTimeSecondLabel = new System.Windows.Forms.Label();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            StatusTextLabel = new System.Windows.Forms.ToolStripStatusLabel();
            CursorTimer = new System.Windows.Forms.Timer(components);
            TrayBarIcon = new System.Windows.Forms.NotifyIcon(components);
            TrayBarContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            TrayBarMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            TrayBarMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            StopButton = new System.Windows.Forms.Button();
            MinimizeToTrayBarBox = new System.Windows.Forms.CheckBox();
            ShowTrayBarIconBox = new System.Windows.Forms.CheckBox();
            MenuStrip = new System.Windows.Forms.MenuStrip();
            StripMenuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            StripMenuEnglish = new System.Windows.Forms.ToolStripMenuItem();
            StripMenuItalian = new System.Windows.Forms.ToolStripMenuItem();
            StripMenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)MovingPixelBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MovingPeriodBox).BeginInit();
            StatusStrip.SuspendLayout();
            TrayBarContextMenu.SuspendLayout();
            MenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // StartButton
            // 
            resources.ApplyResources(StartButton, "StartButton");
            StartButton.Name = "StartButton";
            StartButton.UseVisualStyleBackColor = true;
            StartButton.Click += StartButton_Click;
            // 
            // MovingTimeLabel
            // 
            resources.ApplyResources(MovingTimeLabel, "MovingTimeLabel");
            MovingTimeLabel.Name = "MovingTimeLabel";
            // 
            // MovingPixelLabel
            // 
            resources.ApplyResources(MovingPixelLabel, "MovingPixelLabel");
            MovingPixelLabel.Name = "MovingPixelLabel";
            // 
            // MovingPixelBox
            // 
            resources.ApplyResources(MovingPixelBox, "MovingPixelBox");
            MovingPixelBox.Maximum = new decimal(new int[] { 25, 0, 0, 0 });
            MovingPixelBox.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            MovingPixelBox.Name = "MovingPixelBox";
            MovingPixelBox.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // MovingPeriodBox
            // 
            resources.ApplyResources(MovingPeriodBox, "MovingPeriodBox");
            MovingPeriodBox.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            MovingPeriodBox.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            MovingPeriodBox.Name = "MovingPeriodBox";
            MovingPeriodBox.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // MovingTimeSecondLabel
            // 
            resources.ApplyResources(MovingTimeSecondLabel, "MovingTimeSecondLabel");
            MovingTimeSecondLabel.Name = "MovingTimeSecondLabel";
            // 
            // StatusStrip
            // 
            StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { StatusLabel, StatusTextLabel });
            resources.ApplyResources(StatusStrip, "StatusStrip");
            StatusStrip.Name = "StatusStrip";
            StatusStrip.SizingGrip = false;
            // 
            // StatusLabel
            // 
            StatusLabel.Name = "StatusLabel";
            resources.ApplyResources(StatusLabel, "StatusLabel");
            // 
            // StatusTextLabel
            // 
            StatusTextLabel.Name = "StatusTextLabel";
            resources.ApplyResources(StatusTextLabel, "StatusTextLabel");
            // 
            // CursorTimer
            // 
            CursorTimer.Tick += CursorTimer_Tick;
            // 
            // TrayBarIcon
            // 
            TrayBarIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(TrayBarIcon, "TrayBarIcon");
            TrayBarIcon.ContextMenuStrip = TrayBarContextMenu;
            TrayBarIcon.MouseDoubleClick += TrayBarIcon_MouseDoubleClick;
            // 
            // TrayBarContextMenu
            // 
            TrayBarContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            TrayBarContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { TrayBarMenuOpen, TrayBarMenuClose });
            TrayBarContextMenu.Name = "TrayBarContextMenu";
            resources.ApplyResources(TrayBarContextMenu, "TrayBarContextMenu");
            // 
            // TrayBarMenuOpen
            // 
            TrayBarMenuOpen.Name = "TrayBarMenuOpen";
            resources.ApplyResources(TrayBarMenuOpen, "TrayBarMenuOpen");
            TrayBarMenuOpen.Click += TrayBarMenuOpen_Click;
            // 
            // TrayBarMenuClose
            // 
            TrayBarMenuClose.Name = "TrayBarMenuClose";
            resources.ApplyResources(TrayBarMenuClose, "TrayBarMenuClose");
            TrayBarMenuClose.Click += TrayBarMenuClose_Click;
            // 
            // StopButton
            // 
            resources.ApplyResources(StopButton, "StopButton");
            StopButton.Name = "StopButton";
            StopButton.UseVisualStyleBackColor = true;
            StopButton.Click += StopButton_Click;
            // 
            // MinimizeToTrayBarBox
            // 
            resources.ApplyResources(MinimizeToTrayBarBox, "MinimizeToTrayBarBox");
            MinimizeToTrayBarBox.Checked = true;
            MinimizeToTrayBarBox.CheckState = System.Windows.Forms.CheckState.Checked;
            MinimizeToTrayBarBox.Name = "MinimizeToTrayBarBox";
            MinimizeToTrayBarBox.UseVisualStyleBackColor = true;
            MinimizeToTrayBarBox.CheckedChanged += MinimizeToTrayBarBox_CheckedChanged;
            // 
            // ShowTrayBarIconBox
            // 
            resources.ApplyResources(ShowTrayBarIconBox, "ShowTrayBarIconBox");
            ShowTrayBarIconBox.Checked = true;
            ShowTrayBarIconBox.CheckState = System.Windows.Forms.CheckState.Checked;
            ShowTrayBarIconBox.Name = "ShowTrayBarIconBox";
            ShowTrayBarIconBox.UseVisualStyleBackColor = true;
            // 
            // MenuStrip
            // 
            MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { StripMenuLanguage, StripMenuAbout });
            resources.ApplyResources(MenuStrip, "MenuStrip");
            MenuStrip.Name = "MenuStrip";
            MenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // StripMenuLanguage
            // 
            StripMenuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { StripMenuEnglish, StripMenuItalian });
            StripMenuLanguage.Name = "StripMenuLanguage";
            resources.ApplyResources(StripMenuLanguage, "StripMenuLanguage");
            // 
            // StripMenuEnglish
            // 
            StripMenuEnglish.Name = "StripMenuEnglish";
            resources.ApplyResources(StripMenuEnglish, "StripMenuEnglish");
            StripMenuEnglish.Click += StripMenuEnglish_Click;
            // 
            // StripMenuItalian
            // 
            StripMenuItalian.Name = "StripMenuItalian";
            resources.ApplyResources(StripMenuItalian, "StripMenuItalian");
            StripMenuItalian.Click += StripMenuItalian_Click;
            // 
            // StripMenuAbout
            // 
            StripMenuAbout.Name = "StripMenuAbout";
            resources.ApplyResources(StripMenuAbout, "StripMenuAbout");
            StripMenuAbout.Click += StripMenuAbout_Click;
            // 
            // AutoMouseMoverForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(ShowTrayBarIconBox);
            Controls.Add(MinimizeToTrayBarBox);
            Controls.Add(StopButton);
            Controls.Add(StatusStrip);
            Controls.Add(MenuStrip);
            Controls.Add(MovingTimeSecondLabel);
            Controls.Add(MovingPeriodBox);
            Controls.Add(MovingPixelBox);
            Controls.Add(MovingPixelLabel);
            Controls.Add(MovingTimeLabel);
            Controls.Add(StartButton);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MainMenuStrip = MenuStrip;
            MaximizeBox = false;
            Name = "AutoMouseMoverForm";
            FormClosing += AutoMouseMoverForm_FormClosing;
            Resize += AutoMouseMoverForm_Resize;
            ((System.ComponentModel.ISupportInitialize)MovingPixelBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)MovingPeriodBox).EndInit();
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            TrayBarContextMenu.ResumeLayout(false);
            MenuStrip.ResumeLayout(false);
            MenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripStatusLabel StatusTextLabel;
        private System.Windows.Forms.ToolStripMenuItem StripMenuLanguage;
        private System.Windows.Forms.ToolStripMenuItem StripMenuEnglish;
        private System.Windows.Forms.ToolStripMenuItem StripMenuItalian;
    }
}

