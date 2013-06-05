namespace DEVotion
{
	partial class frmProjectProperties
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
			this.ddResolution = new System.Windows.Forms.ComboBox();
			this.optStandard = new System.Windows.Forms.RadioButton();
			this.optManual = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.txtWidth = new System.Windows.Forms.TextBox();
			this.txtHeight = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optUnspecified = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.txtAuthor = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtVersion = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chkFullscreen = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.chkVSync = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.chkDisplayAuto = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.ddFSAABuffers = new System.Windows.Forms.ComboBox();
			this.txtIcon = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.picFindIcon = new System.Windows.Forms.PictureBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFindIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// ddResolution
			// 
			this.ddResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddResolution.FormattingEnabled = true;
			this.ddResolution.Items.AddRange(new object[] {
            "640x480",
            "800x600",
            "1024x768",
            "1280x1024"});
			this.ddResolution.Location = new System.Drawing.Point(109, 18);
			this.ddResolution.Name = "ddResolution";
			this.ddResolution.Size = new System.Drawing.Size(126, 21);
			this.ddResolution.TabIndex = 11;
			// 
			// optStandard
			// 
			this.optStandard.AutoSize = true;
			this.optStandard.Checked = true;
			this.optStandard.Location = new System.Drawing.Point(18, 19);
			this.optStandard.Name = "optStandard";
			this.optStandard.Size = new System.Drawing.Size(68, 17);
			this.optStandard.TabIndex = 8;
			this.optStandard.TabStop = true;
			this.optStandard.Text = "Standard";
			this.optStandard.UseVisualStyleBackColor = true;
			this.optStandard.CheckedChanged += new System.EventHandler(this.optStandard_CheckedChanged);
			// 
			// optManual
			// 
			this.optManual.AutoSize = true;
			this.optManual.Location = new System.Drawing.Point(18, 43);
			this.optManual.Name = "optManual";
			this.optManual.Size = new System.Drawing.Size(60, 17);
			this.optManual.TabIndex = 9;
			this.optManual.Text = "Manual";
			this.optManual.UseVisualStyleBackColor = true;
			this.optManual.CheckedChanged += new System.EventHandler(this.optManual_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(165, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(14, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "X";
			// 
			// txtWidth
			// 
			this.txtWidth.Enabled = false;
			this.txtWidth.Location = new System.Drawing.Point(109, 45);
			this.txtWidth.Name = "txtWidth";
			this.txtWidth.Size = new System.Drawing.Size(50, 20);
			this.txtWidth.TabIndex = 12;
			this.txtWidth.Text = "1024";
			// 
			// txtHeight
			// 
			this.txtHeight.Enabled = false;
			this.txtHeight.Location = new System.Drawing.Point(185, 45);
			this.txtHeight.Name = "txtHeight";
			this.txtHeight.Size = new System.Drawing.Size(50, 20);
			this.txtHeight.TabIndex = 13;
			this.txtHeight.Text = "768";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.optUnspecified);
			this.groupBox1.Controls.Add(this.optStandard);
			this.groupBox1.Controls.Add(this.txtHeight);
			this.groupBox1.Controls.Add(this.ddResolution);
			this.groupBox1.Controls.Add(this.txtWidth);
			this.groupBox1.Controls.Add(this.optManual);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 225);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(254, 94);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Default Resolution";
			// 
			// optUnspecified
			// 
			this.optUnspecified.AutoSize = true;
			this.optUnspecified.Location = new System.Drawing.Point(18, 66);
			this.optUnspecified.Name = "optUnspecified";
			this.optUnspecified.Size = new System.Drawing.Size(89, 17);
			this.optUnspecified.TabIndex = 10;
			this.optUnspecified.Text = "Not Specified";
			this.optUnspecified.UseVisualStyleBackColor = true;
			this.optUnspecified.CheckedChanged += new System.EventHandler(this.optUnspecified_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(30, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Title:";
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(91, 15);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(175, 20);
			this.txtTitle.TabIndex = 0;
			this.txtTitle.Text = "My Project";
			// 
			// txtAuthor
			// 
			this.txtAuthor.Location = new System.Drawing.Point(91, 41);
			this.txtAuthor.Name = "txtAuthor";
			this.txtAuthor.Size = new System.Drawing.Size(175, 20);
			this.txtAuthor.TabIndex = 1;
			this.txtAuthor.Text = "Me";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Author:";
			// 
			// txtVersion
			// 
			this.txtVersion.Location = new System.Drawing.Point(91, 67);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.Size = new System.Drawing.Size(80, 20);
			this.txtVersion.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 13);
			this.label4.TabIndex = 11;
			this.label4.Text = "LÖVE Version:";
			// 
			// chkFullscreen
			// 
			this.chkFullscreen.AutoSize = true;
			this.chkFullscreen.Location = new System.Drawing.Point(91, 121);
			this.chkFullscreen.Name = "chkFullscreen";
			this.chkFullscreen.Size = new System.Drawing.Size(15, 14);
			this.chkFullscreen.TabIndex = 4;
			this.chkFullscreen.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 122);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(58, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Fullscreen:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 144);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 13);
			this.label6.TabIndex = 16;
			this.label6.Text = "VSync:";
			// 
			// chkVSync
			// 
			this.chkVSync.AutoSize = true;
			this.chkVSync.Location = new System.Drawing.Point(91, 143);
			this.chkVSync.Name = "chkVSync";
			this.chkVSync.Size = new System.Drawing.Size(15, 14);
			this.chkVSync.TabIndex = 5;
			this.chkVSync.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 166);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(69, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "Display Auto:";
			// 
			// chkDisplayAuto
			// 
			this.chkDisplayAuto.AutoSize = true;
			this.chkDisplayAuto.Checked = true;
			this.chkDisplayAuto.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDisplayAuto.Enabled = false;
			this.chkDisplayAuto.Location = new System.Drawing.Point(91, 165);
			this.chkDisplayAuto.Name = "chkDisplayAuto";
			this.chkDisplayAuto.Size = new System.Drawing.Size(15, 14);
			this.chkDisplayAuto.TabIndex = 6;
			this.chkDisplayAuto.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 192);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(37, 13);
			this.label8.TabIndex = 19;
			this.label8.Text = "FSAA:";
			// 
			// ddFSAABuffers
			// 
			this.ddFSAABuffers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddFSAABuffers.FormattingEnabled = true;
			this.ddFSAABuffers.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "4",
            "8"});
			this.ddFSAABuffers.Location = new System.Drawing.Point(91, 189);
			this.ddFSAABuffers.Name = "ddFSAABuffers";
			this.ddFSAABuffers.Size = new System.Drawing.Size(53, 21);
			this.ddFSAABuffers.TabIndex = 7;
			// 
			// txtIcon
			// 
			this.txtIcon.Enabled = false;
			this.txtIcon.Location = new System.Drawing.Point(91, 93);
			this.txtIcon.Name = "txtIcon";
			this.txtIcon.Size = new System.Drawing.Size(156, 20);
			this.txtIcon.TabIndex = 3;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 96);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(31, 13);
			this.label9.TabIndex = 21;
			this.label9.Text = "Icon:";
			// 
			// picFindIcon
			// 
			this.picFindIcon.Image = global::DEVotion.Properties.Resources.find;
			this.picFindIcon.InitialImage = global::DEVotion.Properties.Resources.find;
			this.picFindIcon.Location = new System.Drawing.Point(253, 96);
			this.picFindIcon.Name = "picFindIcon";
			this.picFindIcon.Size = new System.Drawing.Size(16, 16);
			this.picFindIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picFindIcon.TabIndex = 23;
			this.picFindIcon.TabStop = false;
			this.picFindIcon.Click += new System.EventHandler(this.picFindIcon_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnCancel.Location = new System.Drawing.Point(197, 325);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnOK.Location = new System.Drawing.Point(116, 325);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 14;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// frmProjectProperties
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(278, 356);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.picFindIcon);
			this.Controls.Add(this.txtIcon);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.ddFSAABuffers);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.chkDisplayAuto);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.chkVSync);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.chkFullscreen);
			this.Controls.Add(this.txtVersion);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtAuthor);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmProjectProperties";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Project Settings";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFindIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox ddResolution;
		private System.Windows.Forms.RadioButton optStandard;
		private System.Windows.Forms.RadioButton optManual;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtWidth;
		private System.Windows.Forms.TextBox txtHeight;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.TextBox txtAuthor;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtVersion;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox chkFullscreen;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chkVSync;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox chkDisplayAuto;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox ddFSAABuffers;
		private System.Windows.Forms.TextBox txtIcon;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.PictureBox picFindIcon;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.RadioButton optUnspecified;
	}
}