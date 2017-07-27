using System.Windows.Forms;
namespace MyTest
{
    partial class MazeBuilder
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
            this.picMaze = new System.Windows.Forms.PictureBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonBuild = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picMaze)).BeginInit();
            this.SuspendLayout();
            // 
            // picMaze
            // 
            this.picMaze.BackColor = System.Drawing.Color.Black;
            this.picMaze.Location = new System.Drawing.Point(25, 60);
            this.picMaze.Name = "picMaze";
            this.picMaze.Size = new System.Drawing.Size(695, 575);
            this.picMaze.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picMaze.TabIndex = 1;
            this.picMaze.TabStop = false;
            this.picMaze.Paint += new System.Windows.Forms.PaintEventHandler(this.picMaze_Paint);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.buttonCancel.Location = new System.Drawing.Point(110, 20);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 27);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonBuild
            // 
            this.buttonBuild.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.buttonBuild.Location = new System.Drawing.Point(25, 20);
            this.buttonBuild.Name = "buttonBuild";
            this.buttonBuild.Size = new System.Drawing.Size(75, 27);
            this.buttonBuild.TabIndex = 3;
            this.buttonBuild.Text = "Start";
            this.buttonBuild.UseVisualStyleBackColor = true;
            this.buttonBuild.Click += new System.EventHandler(this.buttonBuild_Click);
            // 
            // MazeBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 661);
            this.Controls.Add(this.buttonBuild);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.picMaze);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MazeBuilder";
            this.Text = "MazeBuilder";
            ((System.ComponentModel.ISupportInitialize)(this.picMaze)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox picMaze;
        private Button buttonCancel;
        private Button buttonBuild;
    }
}