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
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonBuild = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.ImageFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picMaze)).BeginInit();
            this.SuspendLayout();
            // 
            // picMaze
            // 
            this.picMaze.BackColor = System.Drawing.Color.Black;
            this.picMaze.Location = new System.Drawing.Point(25, 60);
            this.picMaze.Name = "picMaze";
            this.picMaze.Size = new System.Drawing.Size(690, 570);
            this.picMaze.TabIndex = 1;
            this.picMaze.TabStop = false;
            this.picMaze.Paint += new System.Windows.Forms.PaintEventHandler(this.picMaze_Paint);
            // 
            // buttonClear
            // 
            this.buttonClear.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.buttonClear.Location = new System.Drawing.Point(110, 20);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 27);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonCancel_Click);
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
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.buttonSave.Location = new System.Drawing.Point(195, 20);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 27);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "SaveAs";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // ImageFileDialog
            // 
            this.ImageFileDialog.Filter = "JPG|*.jpg|PNG|*.png|BMP|*.bmp|GIF|*.gif";
            // 
            // MazeBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 661);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonBuild);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.picMaze);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MazeBuilder";
            this.Text = "MazeBuilder";
            ((System.ComponentModel.ISupportInitialize)(this.picMaze)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox picMaze;
        private Button buttonClear;
        private Button buttonBuild;
        private Button buttonSave;
        private SaveFileDialog ImageFileDialog;
    }
}