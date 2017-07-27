using System.Windows.Forms;
namespace MyTest
{
    partial class MazeSearch
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelFrom = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.buttonImport = new System.Windows.Forms.Button();
            this.ImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picMaze)).BeginInit();
            this.SuspendLayout();
            // 
            // picMaze
            // 
            this.picMaze.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMaze.Location = new System.Drawing.Point(25, 60);
            this.picMaze.Name = "picMaze";
            this.picMaze.Size = new System.Drawing.Size(695, 575);
            this.picMaze.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picMaze.TabIndex = 1;
            this.picMaze.TabStop = false;
            this.picMaze.Paint += new System.Windows.Forms.PaintEventHandler(this.picMaze_Paint);
            this.picMaze.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMaze_MouseDown);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.buttonCancel.Location = new System.Drawing.Point(180, 20);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.oBuCancel_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.buttonStart.Location = new System.Drawing.Point(640, 19);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 27);
            this.buttonStart.TabIndex = 3;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelFrom
            // 
            this.labelFrom.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.labelFrom.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.labelFrom.Location = new System.Drawing.Point(265, 25);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(110, 15);
            this.labelFrom.TabIndex = 4;
            this.labelFrom.Text = "From(1111,1111)";
            // 
            // labelTo
            // 
            this.labelTo.Font = new System.Drawing.Font("微軟正黑體", 9F);
            this.labelTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labelTo.Location = new System.Drawing.Point(380, 25);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(110, 16);
            this.labelTo.TabIndex = 5;
            this.labelTo.Text = "From(1111,1111)";
            // 
            // buttonImport
            // 
            this.buttonImport.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.buttonImport.Location = new System.Drawing.Point(25, 20);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(145, 30);
            this.buttonImport.TabIndex = 6;
            this.buttonImport.Text = "ImportImage";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // ImageFileDialog
            // 
            this.ImageFileDialog.FileName = "openFileDialog1";
            this.ImageFileDialog.Filter = "Image|*.jpg;*.png;*.bmp;*.gif";
            // 
            // MazeSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 653);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.labelFrom);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.picMaze);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MazeSearch";
            this.Text = "MazeSearch";
            ((System.ComponentModel.ISupportInitialize)(this.picMaze)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox picMaze;
        private Button buttonCancel;
        private Button buttonStart;
        private Label labelFrom;
        private Label labelTo;
        private Button buttonImport;
        private OpenFileDialog ImageFileDialog;
    }
}