﻿namespace LiveSplit.RedFaction
{
    partial class RedFactionSettings
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.gbEndSplits = new System.Windows.Forms.GroupBox();
			this.CBList_Splits = new System.Windows.Forms.CheckedListBox();
			this.gbStartSplits = new System.Windows.Forms.GroupBox();
			this.tlpStartSplits = new System.Windows.Forms.TableLayoutPanel();
			this.chkAutoReset = new System.Windows.Forms.CheckBox();
			this.chkAutoStart = new System.Windows.Forms.CheckBox();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.chkAllowRepeatedRuns = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.Cbox_Mod = new System.Windows.Forms.ComboBox();
			this.gbEndSplits.SuspendLayout();
			this.gbStartSplits.SuspendLayout();
			this.tlpStartSplits.SuspendLayout();
			this.tlpMain.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbEndSplits
			// 
			this.gbEndSplits.Controls.Add(this.CBList_Splits);
			this.gbEndSplits.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbEndSplits.Location = new System.Drawing.Point(3, 63);
			this.gbEndSplits.Name = "gbEndSplits";
			this.gbEndSplits.Size = new System.Drawing.Size(450, 385);
			this.gbEndSplits.TabIndex = 7;
			this.gbEndSplits.TabStop = false;
			this.gbEndSplits.Text = "Auto-splits";
			// 
			// CBList_Splits
			// 
			this.CBList_Splits.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CBList_Splits.FormattingEnabled = true;
			this.CBList_Splits.Location = new System.Drawing.Point(3, 16);
			this.CBList_Splits.Name = "CBList_Splits";
			this.CBList_Splits.Size = new System.Drawing.Size(444, 366);
			this.CBList_Splits.TabIndex = 0;
			this.CBList_Splits.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CBList_Splits_ItemCheck);
			// 
			// gbStartSplits
			// 
			this.gbStartSplits.AutoSize = true;
			this.gbStartSplits.Controls.Add(this.tlpStartSplits);
			this.gbStartSplits.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbStartSplits.Location = new System.Drawing.Point(3, 3);
			this.gbStartSplits.Name = "gbStartSplits";
			this.gbStartSplits.Size = new System.Drawing.Size(219, 48);
			this.gbStartSplits.TabIndex = 5;
			this.gbStartSplits.TabStop = false;
			this.gbStartSplits.Text = "Start Auto-splits";
			// 
			// tlpStartSplits
			// 
			this.tlpStartSplits.AutoSize = true;
			this.tlpStartSplits.BackColor = System.Drawing.Color.Transparent;
			this.tlpStartSplits.ColumnCount = 2;
			this.tlpStartSplits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpStartSplits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpStartSplits.Controls.Add(this.chkAutoReset, 1, 0);
			this.tlpStartSplits.Controls.Add(this.chkAutoStart, 0, 0);
			this.tlpStartSplits.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpStartSplits.Location = new System.Drawing.Point(3, 16);
			this.tlpStartSplits.Name = "tlpStartSplits";
			this.tlpStartSplits.RowCount = 1;
			this.tlpStartSplits.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpStartSplits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.tlpStartSplits.Size = new System.Drawing.Size(213, 29);
			this.tlpStartSplits.TabIndex = 4;
			// 
			// chkAutoReset
			// 
			this.chkAutoReset.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkAutoReset.AutoSize = true;
			this.chkAutoReset.Location = new System.Drawing.Point(109, 6);
			this.chkAutoReset.Name = "chkAutoReset";
			this.chkAutoReset.Size = new System.Drawing.Size(54, 17);
			this.chkAutoReset.TabIndex = 4;
			this.chkAutoReset.Text = "Reset";
			this.chkAutoReset.UseVisualStyleBackColor = true;
			// 
			// chkAutoStart
			// 
			this.chkAutoStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkAutoStart.AutoSize = true;
			this.chkAutoStart.Checked = true;
			this.chkAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAutoStart.Location = new System.Drawing.Point(3, 6);
			this.chkAutoStart.Name = "chkAutoStart";
			this.chkAutoStart.Size = new System.Drawing.Size(48, 17);
			this.chkAutoStart.TabIndex = 6;
			this.chkAutoStart.Text = "Start";
			this.chkAutoStart.UseVisualStyleBackColor = true;
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 1;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.Controls.Add(this.groupBox2, 0, 2);
			this.tlpMain.Controls.Add(this.gbEndSplits, 0, 1);
			this.tlpMain.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.tlpMain.Location = new System.Drawing.Point(0, 0);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 2;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
			this.tlpMain.Size = new System.Drawing.Size(456, 508);
			this.tlpMain.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(3, 454);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(450, 51);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Other";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Controls.Add(this.chkAllowRepeatedRuns, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(444, 32);
			this.tableLayoutPanel3.TabIndex = 4;
			// 
			// chkAllowRepeatedRuns
			// 
			this.chkAllowRepeatedRuns.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.chkAllowRepeatedRuns.AutoSize = true;
			this.chkAllowRepeatedRuns.Checked = true;
			this.chkAllowRepeatedRuns.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAllowRepeatedRuns.Location = new System.Drawing.Point(3, 7);
			this.chkAllowRepeatedRuns.Name = "chkAllowRepeatedRuns";
			this.chkAllowRepeatedRuns.Size = new System.Drawing.Size(119, 17);
			this.chkAllowRepeatedRuns.TabIndex = 6;
			this.chkAllowRepeatedRuns.Text = "Allow repeated runs";
			this.chkAllowRepeatedRuns.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.gbStartSplits, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(450, 54);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(228, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(219, 48);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Mod";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Controls.Add(this.Cbox_Mod, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(213, 29);
			this.tableLayoutPanel2.TabIndex = 4;
			// 
			// Cbox_Mod
			// 
			this.Cbox_Mod.Dock = System.Windows.Forms.DockStyle.Top;
			this.Cbox_Mod.FormattingEnabled = true;
			this.Cbox_Mod.Location = new System.Drawing.Point(3, 3);
			this.Cbox_Mod.Name = "Cbox_Mod";
			this.Cbox_Mod.Size = new System.Drawing.Size(207, 21);
			this.Cbox_Mod.TabIndex = 0;
			this.Cbox_Mod.SelectedIndexChanged += new System.EventHandler(this.Cbox_Mod_SelectedIndexChanged);
			// 
			// RedFactionSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlpMain);
			this.Name = "RedFactionSettings";
			this.Size = new System.Drawing.Size(456, 508);
			this.gbEndSplits.ResumeLayout(false);
			this.gbStartSplits.ResumeLayout(false);
			this.gbStartSplits.PerformLayout();
			this.tlpStartSplits.ResumeLayout(false);
			this.tlpStartSplits.PerformLayout();
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbEndSplits;
        private System.Windows.Forms.GroupBox gbStartSplits;
        private System.Windows.Forms.TableLayoutPanel tlpStartSplits;
        private System.Windows.Forms.CheckBox chkAutoReset;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
		private System.Windows.Forms.CheckBox chkAutoStart;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ComboBox Cbox_Mod;
		private System.Windows.Forms.CheckedListBox CBList_Splits;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.CheckBox chkAllowRepeatedRuns;
	}
}
