namespace SmartEditor
{
    partial class Form_Find
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Find));
            this.label1 = new System.Windows.Forms.Label();
            this.FindTextBox = new System.Windows.Forms.TextBox();
            this.FindNextButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.CaseSensitive = new System.Windows.Forms.CheckBox();
            this.DirectionGroupBox = new System.Windows.Forms.GroupBox();
            this.DownRadioButton = new System.Windows.Forms.RadioButton();
            this.UpRadioButton = new System.Windows.Forms.RadioButton();
            this.DirectionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(16, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "查找内容：";
            // 
            // FindTextBox
            // 
            this.FindTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FindTextBox.Location = new System.Drawing.Point(178, 34);
            this.FindTextBox.Name = "FindTextBox";
            this.FindTextBox.Size = new System.Drawing.Size(455, 39);
            this.FindTextBox.TabIndex = 1;
            this.FindTextBox.TextChanged += new System.EventHandler(this.FindTextBox_TextChanged);
            // 
            // FindNextButton
            // 
            this.FindNextButton.Enabled = false;
            this.FindNextButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FindNextButton.Location = new System.Drawing.Point(658, 25);
            this.FindNextButton.Name = "FindNextButton";
            this.FindNextButton.Size = new System.Drawing.Size(190, 48);
            this.FindNextButton.TabIndex = 2;
            this.FindNextButton.Text = "查找下一个";
            this.FindNextButton.UseVisualStyleBackColor = true;
            this.FindNextButton.Click += new System.EventHandler(this.FindNextButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CancelButton.Location = new System.Drawing.Point(658, 92);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(190, 48);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // CaseSensitive
            // 
            this.CaseSensitive.AutoSize = true;
            this.CaseSensitive.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CaseSensitive.Location = new System.Drawing.Point(23, 148);
            this.CaseSensitive.Name = "CaseSensitive";
            this.CaseSensitive.Size = new System.Drawing.Size(166, 35);
            this.CaseSensitive.TabIndex = 4;
            this.CaseSensitive.Text = "区分大小写";
            this.CaseSensitive.UseVisualStyleBackColor = true;
            // 
            // DirectionGroupBox
            // 
            this.DirectionGroupBox.Controls.Add(this.DownRadioButton);
            this.DirectionGroupBox.Controls.Add(this.UpRadioButton);
            this.DirectionGroupBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DirectionGroupBox.Location = new System.Drawing.Point(257, 92);
            this.DirectionGroupBox.Name = "DirectionGroupBox";
            this.DirectionGroupBox.Size = new System.Drawing.Size(376, 112);
            this.DirectionGroupBox.TabIndex = 5;
            this.DirectionGroupBox.TabStop = false;
            this.DirectionGroupBox.Text = "方向";
            // 
            // DownRadioButton
            // 
            this.DownRadioButton.AutoSize = true;
            this.DownRadioButton.Checked = true;
            this.DownRadioButton.Location = new System.Drawing.Point(199, 47);
            this.DownRadioButton.Name = "DownRadioButton";
            this.DownRadioButton.Size = new System.Drawing.Size(93, 35);
            this.DownRadioButton.TabIndex = 1;
            this.DownRadioButton.TabStop = true;
            this.DownRadioButton.Text = "向下";
            this.DownRadioButton.UseVisualStyleBackColor = true;
            // 
            // UpRadioButton
            // 
            this.UpRadioButton.AutoSize = true;
            this.UpRadioButton.Location = new System.Drawing.Point(21, 47);
            this.UpRadioButton.Name = "UpRadioButton";
            this.UpRadioButton.Size = new System.Drawing.Size(93, 35);
            this.UpRadioButton.TabIndex = 0;
            this.UpRadioButton.Text = "向上";
            this.UpRadioButton.UseVisualStyleBackColor = true;
            // 
            // Form_Find
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 224);
            this.Controls.Add(this.DirectionGroupBox);
            this.Controls.Add(this.CaseSensitive);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.FindNextButton);
            this.Controls.Add(this.FindTextBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Find";
            this.ShowIcon = false;
            this.Text = " 查找";
            this.Load += new System.EventHandler(this.Form_Find_Load);
            this.DirectionGroupBox.ResumeLayout(false);
            this.DirectionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FindTextBox;
        private System.Windows.Forms.Button FindNextButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.CheckBox CaseSensitive;
        private System.Windows.Forms.GroupBox DirectionGroupBox;
        private System.Windows.Forms.RadioButton DownRadioButton;
        private System.Windows.Forms.RadioButton UpRadioButton;
    }
}