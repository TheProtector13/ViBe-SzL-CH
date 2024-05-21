namespace Test1.Cmd {
    partial class Form1 {
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
            if (disposing && (components != null)) {
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
            groupBox1 = new GroupBox();
            cameraCheck = new CheckBox();
            outputBoxButton = new Button();
            inputBoxButton = new Button();
            outputBox = new TextBox();
            inputBox = new TextBox();
            groupBox2 = new GroupBox();
            splitContainer1 = new SplitContainer();
            omegaNum = new Label();
            label5 = new Label();
            omegaSlider = new TrackBar();
            radiusNum = new Label();
            label3 = new Label();
            radiusSlider = new TrackBar();
            bufferNum = new Label();
            label1 = new Label();
            bufferSlider = new TrackBar();
            maskingBox = new ComboBox();
            label2 = new Label();
            reinitNum = new Label();
            label7 = new Label();
            reinitSlider = new TrackBar();
            cardinalityNum = new Label();
            label9 = new Label();
            cardinalitySlider = new TrackBar();
            startButton = new Button();
            progressBar1 = new ProgressBar();
            remtimeLabel = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)omegaSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)radiusSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bufferSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)reinitSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cardinalitySlider).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cameraCheck);
            groupBox1.Controls.Add(outputBoxButton);
            groupBox1.Controls.Add(inputBoxButton);
            groupBox1.Controls.Add(outputBox);
            groupBox1.Controls.Add(inputBox);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(760, 104);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Fájl Elérés";
            // 
            // cameraCheck
            // 
            cameraCheck.AutoSize = true;
            cameraCheck.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            cameraCheck.Location = new Point(586, 30);
            cameraCheck.Name = "cameraCheck";
            cameraCheck.Size = new Size(78, 21);
            cameraCheck.TabIndex = 5;
            cameraCheck.Text = "Kamera?";
            cameraCheck.UseVisualStyleBackColor = true;
            cameraCheck.CheckedChanged += cameraCheck_CheckedChanged;
            // 
            // outputBoxButton
            // 
            outputBoxButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            outputBoxButton.Location = new Point(679, 63);
            outputBoxButton.Name = "outputBoxButton";
            outputBoxButton.Size = new Size(75, 25);
            outputBoxButton.TabIndex = 4;
            outputBoxButton.Text = "Tallózás...";
            outputBoxButton.UseVisualStyleBackColor = true;
            outputBoxButton.Click += outputBoxButton_Click;
            // 
            // inputBoxButton
            // 
            inputBoxButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            inputBoxButton.Location = new Point(679, 27);
            inputBoxButton.Name = "inputBoxButton";
            inputBoxButton.Size = new Size(75, 25);
            inputBoxButton.TabIndex = 3;
            inputBoxButton.Text = "Tallózás...";
            inputBoxButton.UseVisualStyleBackColor = true;
            inputBoxButton.Click += inputBoxButton_Click;
            // 
            // outputBox
            // 
            outputBox.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            outputBox.Location = new Point(6, 63);
            outputBox.Name = "outputBox";
            outputBox.PlaceholderText = "Kimeneti fájl...";
            outputBox.ReadOnly = true;
            outputBox.Size = new Size(567, 25);
            outputBox.TabIndex = 2;
            // 
            // inputBox
            // 
            inputBox.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            inputBox.Location = new Point(6, 28);
            inputBox.Name = "inputBox";
            inputBox.PlaceholderText = "Forrás fájl...";
            inputBox.ReadOnly = true;
            inputBox.Size = new Size(567, 25);
            inputBox.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(splitContainer1);
            groupBox2.Location = new Point(12, 122);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(760, 259);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Beállítások";
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(6, 25);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(omegaNum);
            splitContainer1.Panel1.Controls.Add(label5);
            splitContainer1.Panel1.Controls.Add(omegaSlider);
            splitContainer1.Panel1.Controls.Add(radiusNum);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(radiusSlider);
            splitContainer1.Panel1.Controls.Add(bufferNum);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(bufferSlider);
            splitContainer1.Panel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(maskingBox);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(reinitNum);
            splitContainer1.Panel2.Controls.Add(label7);
            splitContainer1.Panel2.Controls.Add(reinitSlider);
            splitContainer1.Panel2.Controls.Add(cardinalityNum);
            splitContainer1.Panel2.Controls.Add(label9);
            splitContainer1.Panel2.Controls.Add(cardinalitySlider);
            splitContainer1.Panel2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            splitContainer1.Size = new Size(748, 231);
            splitContainer1.SplitterDistance = 374;
            splitContainer1.TabIndex = 2;
            // 
            // omegaNum
            // 
            omegaNum.AutoSize = true;
            omegaNum.Location = new Point(342, 195);
            omegaNum.Name = "omegaNum";
            omegaNum.Size = new Size(29, 17);
            omegaNum.TabIndex = 8;
            omegaNum.Text = "100";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(3, 195);
            label5.Name = "label5";
            label5.Size = new Size(86, 17);
            label5.TabIndex = 7;
            label5.Text = "Friss. tényező";
            // 
            // omegaSlider
            // 
            omegaSlider.Location = new Point(89, 183);
            omegaSlider.Name = "omegaSlider";
            omegaSlider.Size = new Size(240, 45);
            omegaSlider.TabIndex = 6;
            omegaSlider.TickStyle = TickStyle.Both;
            omegaSlider.ValueChanged += Slider_ValueChanged;
            // 
            // radiusNum
            // 
            radiusNum.AutoSize = true;
            radiusNum.Location = new Point(342, 104);
            radiusNum.Name = "radiusNum";
            radiusNum.Size = new Size(29, 17);
            radiusNum.TabIndex = 5;
            radiusNum.Text = "100";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(3, 104);
            label3.Name = "label3";
            label3.Size = new Size(81, 17);
            label3.TabIndex = 4;
            label3.Text = "Szín k. sugár";
            // 
            // radiusSlider
            // 
            radiusSlider.Location = new Point(89, 92);
            radiusSlider.Name = "radiusSlider";
            radiusSlider.Size = new Size(240, 45);
            radiusSlider.TabIndex = 3;
            radiusSlider.TickStyle = TickStyle.Both;
            radiusSlider.ValueChanged += Slider_ValueChanged;
            // 
            // bufferNum
            // 
            bufferNum.AutoSize = true;
            bufferNum.Location = new Point(342, 15);
            bufferNum.Name = "bufferNum";
            bufferNum.Size = new Size(29, 17);
            bufferNum.TabIndex = 2;
            bufferNum.Text = "100";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(3, 15);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 1;
            label1.Text = "Buffer méret";
            // 
            // bufferSlider
            // 
            bufferSlider.Location = new Point(89, 3);
            bufferSlider.Name = "bufferSlider";
            bufferSlider.Size = new Size(240, 45);
            bufferSlider.TabIndex = 0;
            bufferSlider.TickStyle = TickStyle.Both;
            bufferSlider.ValueChanged += Slider_ValueChanged;
            // 
            // maskingBox
            // 
            maskingBox.DropDownStyle = ComboBoxStyle.DropDownList;
            maskingBox.FormattingEnabled = true;
            maskingBox.Items.AddRange(new object[] { "Csak Maszk", "Csak a hátteret maszkolja", "Csak az előteret maszkolja" });
            maskingBox.Location = new Point(93, 192);
            maskingBox.Name = "maskingBox";
            maskingBox.Size = new Size(240, 25);
            maskingBox.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(3, 195);
            label2.Name = "label2";
            label2.Size = new Size(69, 17);
            label2.TabIndex = 12;
            label2.Text = "Maszkolás";
            // 
            // reinitNum
            // 
            reinitNum.AutoSize = true;
            reinitNum.Location = new Point(338, 103);
            reinitNum.Name = "reinitNum";
            reinitNum.Size = new Size(29, 17);
            reinitNum.TabIndex = 11;
            reinitNum.Text = "100";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(3, 106);
            label7.Name = "label7";
            label7.Size = new Size(92, 13);
            label7.TabIndex = 10;
            label7.Text = "Újra inic. küszöb";
            // 
            // reinitSlider
            // 
            reinitSlider.Location = new Point(93, 92);
            reinitSlider.Name = "reinitSlider";
            reinitSlider.Size = new Size(240, 45);
            reinitSlider.TabIndex = 9;
            reinitSlider.TickStyle = TickStyle.Both;
            reinitSlider.ValueChanged += Slider_ValueChanged;
            // 
            // cardinalityNum
            // 
            cardinalityNum.AutoSize = true;
            cardinalityNum.Location = new Point(338, 15);
            cardinalityNum.Name = "cardinalityNum";
            cardinalityNum.Size = new Size(29, 17);
            cardinalityNum.TabIndex = 8;
            cardinalityNum.Text = "100";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(3, 15);
            label9.Name = "label9";
            label9.Size = new Size(84, 17);
            label9.TabIndex = 7;
            label9.Text = "Min. egyezés";
            // 
            // cardinalitySlider
            // 
            cardinalitySlider.Location = new Point(93, 3);
            cardinalitySlider.Name = "cardinalitySlider";
            cardinalitySlider.Size = new Size(240, 45);
            cardinalitySlider.TabIndex = 6;
            cardinalitySlider.TickStyle = TickStyle.Both;
            cardinalitySlider.ValueChanged += Slider_ValueChanged;
            // 
            // startButton
            // 
            startButton.Location = new Point(622, 399);
            startButton.Name = "startButton";
            startButton.Size = new Size(150, 50);
            startButton.TabIndex = 2;
            startButton.Text = "Futtatás";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 399);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(503, 50);
            progressBar1.TabIndex = 3;
            // 
            // remtimeLabel
            // 
            remtimeLabel.Location = new Point(521, 414);
            remtimeLabel.Name = "remtimeLabel";
            remtimeLabel.Size = new Size(95, 21);
            remtimeLabel.TabIndex = 4;
            remtimeLabel.Text = "00 : 00";
            remtimeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            ClientSize = new Size(784, 461);
            Controls.Add(remtimeLabel);
            Controls.Add(progressBar1);
            Controls.Add(startButton);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            HelpButton = true;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "VIBE vezérlő";
            FormClosing += Form1_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)omegaSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)radiusSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)bufferSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)reinitSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)cardinalitySlider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox outputBox;
        private TextBox inputBox;
        private Button inputBoxButton;
        private CheckBox cameraCheck;
        private Button outputBoxButton;
        private GroupBox groupBox2;
        private Label label1;
        private TrackBar bufferSlider;
        private SplitContainer splitContainer1;
        private Label bufferNum;
        private Label radiusNum;
        private Label label3;
        private TrackBar radiusSlider;
        private Label omegaNum;
        private Label label5;
        private TrackBar omegaSlider;
        private Label reinitNum;
        private Label label7;
        private TrackBar reinitSlider;
        private Label cardinalityNum;
        private Label label9;
        private TrackBar cardinalitySlider;
        private Label label2;
        private ComboBox maskingBox;
        private Button startButton;
        private ProgressBar progressBar1;
        private Label remtimeLabel;
    }
}
