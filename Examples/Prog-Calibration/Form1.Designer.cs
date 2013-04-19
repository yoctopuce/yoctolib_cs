namespace WindowsFormsApplication1
{
    partial class Form1
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
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
          this.label1 = new System.Windows.Forms.Label();
          this.label2 = new System.Windows.Forms.Label();
          this.label3 = new System.Windows.Forms.Label();
          this.ValueDisplay = new System.Windows.Forms.Label();
          this.ValueDisplayUnits = new System.Windows.Forms.Label();
          this.devicesList = new System.Windows.Forms.ComboBox();
          this.functionsList = new System.Windows.Forms.ComboBox();
          this.label4 = new System.Windows.Forms.Label();
          this.RawLabel = new System.Windows.Forms.Label();
          this.CalibratedLabel = new System.Windows.Forms.Label();
          this.R0 = new System.Windows.Forms.TextBox();
          this.R1 = new System.Windows.Forms.TextBox();
          this.R2 = new System.Windows.Forms.TextBox();
          this.R3 = new System.Windows.Forms.TextBox();
          this.R4 = new System.Windows.Forms.TextBox();
          this.C4 = new System.Windows.Forms.TextBox();
          this.C3 = new System.Windows.Forms.TextBox();
          this.C2 = new System.Windows.Forms.TextBox();
          this.C1 = new System.Windows.Forms.TextBox();
          this.C0 = new System.Windows.Forms.TextBox();
          this.saveBtn = new System.Windows.Forms.Button();
          this.nosensorfunction = new System.Windows.Forms.Label();
          this.unsupported_warning = new System.Windows.Forms.Label();
          this.timer1 = new System.Windows.Forms.Timer(this.components);
          this.RawValueDisplay = new System.Windows.Forms.Label();
          this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
          this.statusStrip1 = new System.Windows.Forms.StatusStrip();
          this.cancelBtn = new System.Windows.Forms.Button();
          this.statusStrip1.SuspendLayout();
          this.SuspendLayout();
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(12, 21);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(267, 13);
          this.label1.TabIndex = 0;
          this.label1.Text = "Connect one or more Yocto-devices featuring a sensor:";
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(31, 53);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(44, 13);
          this.label2.TabIndex = 1;
          this.label2.Text = "Device:";
          // 
          // label3
          // 
          this.label3.AutoSize = true;
          this.label3.Location = new System.Drawing.Point(31, 80);
          this.label3.Name = "label3";
          this.label3.Size = new System.Drawing.Size(48, 13);
          this.label3.TabIndex = 2;
          this.label3.Text = "Function";
          // 
          // ValueDisplay
          // 
          this.ValueDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.ValueDisplay.Location = new System.Drawing.Point(44, 135);
          this.ValueDisplay.Name = "ValueDisplay";
          this.ValueDisplay.Size = new System.Drawing.Size(215, 51);
          this.ValueDisplay.TabIndex = 3;
          this.ValueDisplay.Text = "N/A";
          this.ValueDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
          // 
          // ValueDisplayUnits
          // 
          this.ValueDisplayUnits.AutoSize = true;
          this.ValueDisplayUnits.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.ValueDisplayUnits.Location = new System.Drawing.Point(254, 135);
          this.ValueDisplayUnits.Name = "ValueDisplayUnits";
          this.ValueDisplayUnits.Size = new System.Drawing.Size(37, 51);
          this.ValueDisplayUnits.TabIndex = 4;
          this.ValueDisplayUnits.Text = "-";
          // 
          // devicesList
          // 
          this.devicesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.devicesList.Enabled = false;
          this.devicesList.FormattingEnabled = true;
          this.devicesList.Location = new System.Drawing.Point(92, 50);
          this.devicesList.Name = "devicesList";
          this.devicesList.Size = new System.Drawing.Size(292, 21);
          this.devicesList.TabIndex = 0;
          this.devicesList.SelectedIndexChanged += new System.EventHandler(this.devicesList_SelectedIndexChanged);
          // 
          // functionsList
          // 
          this.functionsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.functionsList.Enabled = false;
          this.functionsList.FormattingEnabled = true;
          this.functionsList.Location = new System.Drawing.Point(92, 77);
          this.functionsList.Name = "functionsList";
          this.functionsList.Size = new System.Drawing.Size(292, 21);
          this.functionsList.TabIndex = 1;
          this.functionsList.SelectedIndexChanged += new System.EventHandler(this.functionsList_SelectedIndexChanged);
          // 
          // label4
          // 
          this.label4.Location = new System.Drawing.Point(41, 226);
          this.label4.Name = "label4";
          this.label4.Size = new System.Drawing.Size(372, 55);
          this.label4.TabIndex = 7;
          this.label4.Text = resources.GetString("label4.Text");
          // 
          // RawLabel
          // 
          this.RawLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Grip;
          this.RawLabel.AutoSize = true;
          this.RawLabel.Location = new System.Drawing.Point(12, 303);
          this.RawLabel.Name = "RawLabel";
          this.RawLabel.Size = new System.Drawing.Size(29, 13);
          this.RawLabel.TabIndex = 8;
          this.RawLabel.Text = "Raw";
          // 
          // CalibratedLabel
          // 
          this.CalibratedLabel.AutoSize = true;
          this.CalibratedLabel.Location = new System.Drawing.Point(12, 329);
          this.CalibratedLabel.Name = "CalibratedLabel";
          this.CalibratedLabel.Size = new System.Drawing.Size(54, 13);
          this.CalibratedLabel.TabIndex = 9;
          this.CalibratedLabel.Text = "Calibrated";
          // 
          // R0
          // 
          this.R0.Location = new System.Drawing.Point(75, 300);
          this.R0.Name = "R0";
          this.R0.Size = new System.Drawing.Size(50, 20);
          this.R0.TabIndex = 2;
          this.R0.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // R1
          // 
          this.R1.Location = new System.Drawing.Point(131, 300);
          this.R1.Name = "R1";
          this.R1.Size = new System.Drawing.Size(50, 20);
          this.R1.TabIndex = 4;
          this.R1.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // R2
          // 
          this.R2.Location = new System.Drawing.Point(186, 300);
          this.R2.Name = "R2";
          this.R2.Size = new System.Drawing.Size(50, 20);
          this.R2.TabIndex = 6;
          this.R2.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // R3
          // 
          this.R3.Location = new System.Drawing.Point(242, 300);
          this.R3.Name = "R3";
          this.R3.Size = new System.Drawing.Size(50, 20);
          this.R3.TabIndex = 8;
          this.R3.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // R4
          // 
          this.R4.Location = new System.Drawing.Point(298, 300);
          this.R4.Name = "R4";
          this.R4.Size = new System.Drawing.Size(50, 20);
          this.R4.TabIndex = 10;
          this.R4.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // C4
          // 
          this.C4.Location = new System.Drawing.Point(298, 326);
          this.C4.Name = "C4";
          this.C4.Size = new System.Drawing.Size(50, 20);
          this.C4.TabIndex = 11;
          this.C4.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // C3
          // 
          this.C3.Location = new System.Drawing.Point(242, 326);
          this.C3.Name = "C3";
          this.C3.Size = new System.Drawing.Size(50, 20);
          this.C3.TabIndex = 9;
          this.C3.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // C2
          // 
          this.C2.Location = new System.Drawing.Point(186, 326);
          this.C2.Name = "C2";
          this.C2.Size = new System.Drawing.Size(50, 20);
          this.C2.TabIndex = 7;
          this.C2.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // C1
          // 
          this.C1.Location = new System.Drawing.Point(131, 326);
          this.C1.Name = "C1";
          this.C1.Size = new System.Drawing.Size(50, 20);
          this.C1.TabIndex = 5;
          this.C1.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // C0
          // 
          this.C0.Location = new System.Drawing.Point(75, 326);
          this.C0.Name = "C0";
          this.C0.Size = new System.Drawing.Size(50, 20);
          this.C0.TabIndex = 3;
          this.C0.Leave += new System.EventHandler(this.CalibrationChange);
          // 
          // saveBtn
          // 
          this.saveBtn.Location = new System.Drawing.Point(354, 326);
          this.saveBtn.Name = "saveBtn";
          this.saveBtn.Size = new System.Drawing.Size(75, 23);
          this.saveBtn.TabIndex = 13;
          this.saveBtn.Text = "Save";
          this.saveBtn.UseVisualStyleBackColor = true;
          this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
          // 
          // nosensorfunction
          // 
          this.nosensorfunction.AutoSize = true;
          this.nosensorfunction.ForeColor = System.Drawing.Color.Red;
          this.nosensorfunction.Location = new System.Drawing.Point(21, 379);
          this.nosensorfunction.Name = "nosensorfunction";
          this.nosensorfunction.Size = new System.Drawing.Size(215, 13);
          this.nosensorfunction.TabIndex = 22;
          this.nosensorfunction.Text = "No supported sensor function on this device";
          // 
          // unsupported_warning
          // 
          this.unsupported_warning.AutoSize = true;
          this.unsupported_warning.ForeColor = System.Drawing.Color.Red;
          this.unsupported_warning.Location = new System.Drawing.Point(21, 366);
          this.unsupported_warning.Name = "unsupported_warning";
          this.unsupported_warning.Size = new System.Drawing.Size(330, 13);
          this.unsupported_warning.TabIndex = 23;
          this.unsupported_warning.Text = "This device does not support calibration,  firmware  upgrade needed.";
          // 
          // timer1
          // 
          this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
          // 
          // RawValueDisplay
          // 
          this.RawValueDisplay.AutoSize = true;
          this.RawValueDisplay.Location = new System.Drawing.Point(329, 184);
          this.RawValueDisplay.Name = "RawValueDisplay";
          this.RawValueDisplay.Size = new System.Drawing.Size(10, 13);
          this.RawValueDisplay.TabIndex = 25;
          this.RawValueDisplay.Text = "-";
          // 
          // toolStripStatusLabel1
          // 
          this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
          this.toolStripStatusLabel1.Size = new System.Drawing.Size(234, 17);
          this.toolStripStatusLabel1.Text = "Plug a Yoctopuce device featuring a sensor";
          // 
          // statusStrip1
          // 
          this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
          this.statusStrip1.Location = new System.Drawing.Point(0, 412);
          this.statusStrip1.Name = "statusStrip1";
          this.statusStrip1.Size = new System.Drawing.Size(440, 22);
          this.statusStrip1.TabIndex = 24;
          this.statusStrip1.Text = "statusStrip1";
          // 
          // cancelBtn
          // 
          this.cancelBtn.Location = new System.Drawing.Point(354, 300);
          this.cancelBtn.Name = "cancelBtn";
          this.cancelBtn.Size = new System.Drawing.Size(75, 23);
          this.cancelBtn.TabIndex = 26;
          this.cancelBtn.Text = "Cancel";
          this.cancelBtn.UseVisualStyleBackColor = true;
          this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
          // 
          // Form1
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(440, 434);
          this.Controls.Add(this.cancelBtn);
          this.Controls.Add(this.RawValueDisplay);
          this.Controls.Add(this.statusStrip1);
          this.Controls.Add(this.unsupported_warning);
          this.Controls.Add(this.nosensorfunction);
          this.Controls.Add(this.saveBtn);
          this.Controls.Add(this.C4);
          this.Controls.Add(this.C3);
          this.Controls.Add(this.C2);
          this.Controls.Add(this.C1);
          this.Controls.Add(this.C0);
          this.Controls.Add(this.R4);
          this.Controls.Add(this.R3);
          this.Controls.Add(this.R2);
          this.Controls.Add(this.R1);
          this.Controls.Add(this.R0);
          this.Controls.Add(this.CalibratedLabel);
          this.Controls.Add(this.RawLabel);
          this.Controls.Add(this.label4);
          this.Controls.Add(this.functionsList);
          this.Controls.Add(this.devicesList);
          this.Controls.Add(this.ValueDisplayUnits);
          this.Controls.Add(this.ValueDisplay);
          this.Controls.Add(this.label3);
          this.Controls.Add(this.label2);
          this.Controls.Add(this.label1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.Name = "Form1";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          this.Text = "Calibration settings";
          this.statusStrip1.ResumeLayout(false);
          this.statusStrip1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ValueDisplay;
        private System.Windows.Forms.Label ValueDisplayUnits;
        private System.Windows.Forms.ComboBox devicesList;
        private System.Windows.Forms.ComboBox functionsList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label RawLabel;
        private System.Windows.Forms.Label CalibratedLabel;
        private System.Windows.Forms.TextBox R0;
        private System.Windows.Forms.TextBox R1;
        private System.Windows.Forms.TextBox R2;
        private System.Windows.Forms.TextBox R3;
        private System.Windows.Forms.TextBox R4;
        private System.Windows.Forms.TextBox C4;
        private System.Windows.Forms.TextBox C3;
        private System.Windows.Forms.TextBox C2;
        private System.Windows.Forms.TextBox C1;
        private System.Windows.Forms.TextBox C0;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Label nosensorfunction;
        private System.Windows.Forms.Label unsupported_warning;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label RawValueDisplay;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button cancelBtn;
    }
}

