namespace ScrabbleSolver {
  partial class SolverInfo {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.btPlay = new System.Windows.Forms.Button();
      this.btCancel = new System.Windows.Forms.Button();
      this.lbInfo = new System.Windows.Forms.ListBox();
      this.SuspendLayout();
      // 
      // btPlay
      // 
      this.btPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btPlay.Location = new System.Drawing.Point(74, 558);
      this.btPlay.Name = "btPlay";
      this.btPlay.Size = new System.Drawing.Size(75, 29);
      this.btPlay.TabIndex = 1;
      this.btPlay.Text = "Play It";
      this.btPlay.UseVisualStyleBackColor = true;
      this.btPlay.Click += new System.EventHandler(this.btPlay_Click);
      // 
      // btCancel
      // 
      this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btCancel.Location = new System.Drawing.Point(234, 557);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(75, 29);
      this.btCancel.TabIndex = 2;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      // 
      // lbInfo
      // 
      this.lbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lbInfo.FormattingEnabled = true;
      this.lbInfo.Location = new System.Drawing.Point(1, 0);
      this.lbInfo.Name = "lbInfo";
      this.lbInfo.Size = new System.Drawing.Size(398, 550);
      this.lbInfo.TabIndex = 3;
      // 
      // SolverInfo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(399, 593);
      this.ControlBox = false;
      this.Controls.Add(this.lbInfo);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btPlay);
      this.Name = "SolverInfo";
      this.Text = "Solver";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SolverInfo_FormClosing);
      this.ResumeLayout(false);

    }

        #endregion
        private System.Windows.Forms.Button btPlay;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ListBox lbInfo;
    }
}