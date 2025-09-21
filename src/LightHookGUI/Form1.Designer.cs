namespace LightHook
{
    partial class Form1
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
            button1 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            richTextBox1 = new RichTextBox();
            button2 = new Button();
            label2 = new Label();
            userInput = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(898, 564);
            button1.Name = "button1";
            button1.Size = new Size(164, 54);
            button1.TabIndex = 0;
            button1.Text = "注入/启动";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 579);
            label1.Name = "label1";
            label1.Size = new Size(95, 24);
            label1.TabIndex = 1;
            label1.Text = "ProcessId:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(644, 576);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(228, 30);
            textBox1.TabIndex = 2;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.Black;
            richTextBox1.ForeColor = Color.Lime;
            richTextBox1.Location = new Point(3, 5);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            richTextBox1.Size = new Size(1083, 498);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // button2
            // 
            button2.Location = new Point(314, 571);
            button2.Name = "button2";
            button2.Size = new Size(144, 45);
            button2.TabIndex = 4;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 518);
            label2.Name = "label2";
            label2.Size = new Size(118, 24);
            label2.TabIndex = 5;
            label2.Text = "控制台输入：";
            // 
            // userInput
            // 
            userInput.Enabled = false;
            userInput.Location = new Point(136, 515);
            userInput.Name = "userInput";
            userInput.Size = new Size(926, 30);
            userInput.TabIndex = 6;
            userInput.KeyDown += textBox2_KeyDown;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1089, 628);
            Controls.Add(userInput);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(richTextBox1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "LightHookGUI";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private TextBox textBox1;
        private RichTextBox richTextBox1;
        private Button button2;
        private Label label2;
        private TextBox userInput;
    }
}
