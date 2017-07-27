using System.Windows.Forms;
namespace MyTest
{
    partial class EUDCtoImage
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
            this.txtInput = new System.Windows.Forms.TextBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.panelOutput = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(25, 60);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(215, 22);
            this.txtInput.TabIndex = 1;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(25, 20);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(105, 25);
            this.buttonImport.TabIndex = 0;
            this.buttonImport.Text = "Export EUDC";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // panelOutput
            // 
            this.panelOutput.BackColor = System.Drawing.Color.Ivory;
            this.panelOutput.Location = new System.Drawing.Point(25, 100);
            this.panelOutput.Name = "panelOutput";
            this.panelOutput.Size = new System.Drawing.Size(680, 445);
            this.panelOutput.TabIndex = 2;
            this.panelOutput.Paint += new System.Windows.Forms.PaintEventHandler(this.panelOutput_Paint);
            // 
            // EUDCtoImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 575);
            this.Controls.Add(this.panelOutput);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.buttonImport);
            this.Name = "EUDCtoImage";
            this.Text = "EUDCtoImage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button buttonImport;
        private TextBox txtInput;
        private Panel panelOutput;
    }
}