namespace AutoMouseMover
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            ProductLabel = new System.Windows.Forms.Label();
            VersionLabel = new System.Windows.Forms.Label();
            CopyrightLabel = new System.Windows.Forms.Label();
            Copyright2Label = new System.Windows.Forms.Label();
            CloseButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // ProductLabel
            // 
            resources.ApplyResources(ProductLabel, "ProductLabel");
            ProductLabel.Name = "ProductLabel";
            // 
            // VersionLabel
            // 
            resources.ApplyResources(VersionLabel, "VersionLabel");
            VersionLabel.Name = "VersionLabel";
            // 
            // CopyrightLabel
            // 
            resources.ApplyResources(CopyrightLabel, "CopyrightLabel");
            CopyrightLabel.Name = "CopyrightLabel";
            // 
            // Copyright2Label
            // 
            resources.ApplyResources(Copyright2Label, "Copyright2Label");
            Copyright2Label.Name = "Copyright2Label";
            // 
            // CloseButton
            // 
            resources.ApplyResources(CloseButton, "CloseButton");
            CloseButton.Name = "CloseButton";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(CloseButton);
            Controls.Add(Copyright2Label);
            Controls.Add(CopyrightLabel);
            Controls.Add(VersionLabel);
            Controls.Add(ProductLabel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label ProductLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label CopyrightLabel;
        private System.Windows.Forms.Label Copyright2Label;
        private System.Windows.Forms.Button CloseButton;
    }
}