namespace CommonLibrary.Forms
{
    partial class NullableDateTimeSelector
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
            this.DateTimePickerMain = new System.Windows.Forms.DateTimePicker();
            this.TextBoxMain = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DateTimePickerMain
            // 
            this.DateTimePickerMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DateTimePickerMain.Location = new System.Drawing.Point(0, 0);
            this.DateTimePickerMain.Name = "DateTimePickerMain";
            this.DateTimePickerMain.Size = new System.Drawing.Size(200, 20);
            this.DateTimePickerMain.TabIndex = 0;
            this.DateTimePickerMain.CloseUp += new System.EventHandler(this.DateTimePickerMain_CloseUp);
            // 
            // TextBoxMain
            // 
            this.TextBoxMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxMain.Location = new System.Drawing.Point(0, 0);
            this.TextBoxMain.Name = "TextBoxMain";
            this.TextBoxMain.Size = new System.Drawing.Size(167, 20);
            this.TextBoxMain.TabIndex = 1;
            this.TextBoxMain.Leave += new System.EventHandler(this.TextBoxMain_Leave);
            // 
            // NullableDateTimeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextBoxMain);
            this.Controls.Add(this.DateTimePickerMain);
            this.Name = "NullableDateTimeSelector";
            this.Size = new System.Drawing.Size(200, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DateTimePickerMain;
        private System.Windows.Forms.TextBox TextBoxMain;
    }
}
