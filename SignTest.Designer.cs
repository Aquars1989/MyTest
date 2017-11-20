namespace MyTest
{
    partial class SignTest
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
            this.btnSave1 = new System.Windows.Forms.Button();
            this.btnLoad1 = new System.Windows.Forms.Button();
            this.btnLoad2 = new System.Windows.Forms.Button();
            this.btnSave2 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave1
            // 
            this.btnSave1.Location = new System.Drawing.Point(10, 85);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(85, 25);
            this.btnSave1.TabIndex = 0;
            this.btnSave1.Text = "Save1";
            this.btnSave1.UseVisualStyleBackColor = true;
            this.btnSave1.Click += new System.EventHandler(this.btnSave1_Click);
            // 
            // btnLoad1
            // 
            this.btnLoad1.Location = new System.Drawing.Point(10, 115);
            this.btnLoad1.Name = "btnLoad1";
            this.btnLoad1.Size = new System.Drawing.Size(85, 25);
            this.btnLoad1.TabIndex = 1;
            this.btnLoad1.Text = "Load1";
            this.btnLoad1.UseVisualStyleBackColor = true;
            this.btnLoad1.Click += new System.EventHandler(this.btnLoad1_Click);
            // 
            // btnLoad2
            // 
            this.btnLoad2.Location = new System.Drawing.Point(10, 180);
            this.btnLoad2.Name = "btnLoad2";
            this.btnLoad2.Size = new System.Drawing.Size(85, 25);
            this.btnLoad2.TabIndex = 3;
            this.btnLoad2.Text = "Load2";
            this.btnLoad2.UseVisualStyleBackColor = true;
            this.btnLoad2.Click += new System.EventHandler(this.btnLoad2_Click);
            // 
            // btnSave2
            // 
            this.btnSave2.Location = new System.Drawing.Point(10, 150);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(85, 25);
            this.btnSave2.TabIndex = 2;
            this.btnSave2.Text = "Save2";
            this.btnSave2.UseVisualStyleBackColor = true;
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(10, 50);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 25);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // SignTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(931, 649);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnLoad2);
            this.Controls.Add(this.btnSave2);
            this.Controls.Add(this.btnLoad1);
            this.Controls.Add(this.btnSave1);
            this.Name = "SignTest";
            this.Text = "SignTest";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Sign_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Sign_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Sign_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Sign_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave1;
        private System.Windows.Forms.Button btnLoad1;
        private System.Windows.Forms.Button btnLoad2;
        private System.Windows.Forms.Button btnSave2;
        private System.Windows.Forms.Button btnClear;
    }
}