namespace ScrabbleSolver {
  partial class ScrabbleBoard {
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
      this.pbBoard = new System.Windows.Forms.PictureBox();
      this.btSolve = new ScrabbleSolver.UnselectableButton();
      this.btReset = new ScrabbleSolver.UnselectableButton();
      this.lbL3 = new System.Windows.Forms.PictureBox();
      this.lbL5 = new System.Windows.Forms.PictureBox();
      this.lbL4 = new System.Windows.Forms.PictureBox();
      this.lbL7 = new System.Windows.Forms.PictureBox();
      this.lbL6 = new System.Windows.Forms.PictureBox();
      this.lbL2 = new System.Windows.Forms.PictureBox();
      this.lbL1 = new System.Windows.Forms.PictureBox();
      this.btGetLetters = new ScrabbleSolver.UnselectableButton();
      ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL5)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL4)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL7)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL6)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL1)).BeginInit();
      this.SuspendLayout();
      // 
      // pbBoard
      // 
      this.pbBoard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pbBoard.Location = new System.Drawing.Point(-1, 2);
      this.pbBoard.Name = "pbBoard";
      this.pbBoard.Size = new System.Drawing.Size(764, 764);
      this.pbBoard.TabIndex = 0;
      this.pbBoard.TabStop = false;
      this.pbBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pbBoard_Paint);
      this.pbBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbBoard_MouseMove);
      // 
      // btSolve
      // 
      this.btSolve.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btSolve.Location = new System.Drawing.Point(505, 791);
      this.btSolve.Name = "btSolve";
      this.btSolve.Size = new System.Drawing.Size(113, 38);
      this.btSolve.TabIndex = 8;
      this.btSolve.TabStop = false;
      this.btSolve.Text = "Solve";
      this.btSolve.UseVisualStyleBackColor = true;
      this.btSolve.Click += new System.EventHandler(this.btSolve_Click);
      // 
      // btReset
      // 
      this.btReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btReset.Location = new System.Drawing.Point(636, 791);
      this.btReset.Name = "btReset";
      this.btReset.Size = new System.Drawing.Size(113, 38);
      this.btReset.TabIndex = 9;
      this.btReset.TabStop = false;
      this.btReset.Text = "Reset";
      this.btReset.UseVisualStyleBackColor = true;
      this.btReset.Click += new System.EventHandler(this.btReset_Click);
      // 
      // lbL3
      // 
      this.lbL3.BackColor = System.Drawing.Color.DarkGray;
      this.lbL3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lbL3.Location = new System.Drawing.Point(120, 788);
      this.lbL3.Margin = new System.Windows.Forms.Padding(0);
      this.lbL3.Name = "lbL3";
      this.lbL3.Size = new System.Drawing.Size(45, 45);
      this.lbL3.TabIndex = 11;
      this.lbL3.TabStop = false;
      this.lbL3.MouseEnter += new System.EventHandler(this.lb_MouseEnter);
      this.lbL3.MouseLeave += new System.EventHandler(this.lb_MouseLeave);
      // 
      // lbL5
      // 
      this.lbL5.BackColor = System.Drawing.Color.DarkGray;
      this.lbL5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lbL5.Location = new System.Drawing.Point(216, 788);
      this.lbL5.Margin = new System.Windows.Forms.Padding(0);
      this.lbL5.Name = "lbL5";
      this.lbL5.Size = new System.Drawing.Size(45, 45);
      this.lbL5.TabIndex = 13;
      this.lbL5.TabStop = false;
      this.lbL5.MouseEnter += new System.EventHandler(this.lb_MouseEnter);
      this.lbL5.MouseLeave += new System.EventHandler(this.lb_MouseLeave);
      // 
      // lbL4
      // 
      this.lbL4.BackColor = System.Drawing.Color.DarkGray;
      this.lbL4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lbL4.Location = new System.Drawing.Point(168, 788);
      this.lbL4.Margin = new System.Windows.Forms.Padding(0);
      this.lbL4.Name = "lbL4";
      this.lbL4.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.lbL4.Size = new System.Drawing.Size(45, 45);
      this.lbL4.TabIndex = 12;
      this.lbL4.TabStop = false;
      this.lbL4.MouseEnter += new System.EventHandler(this.lb_MouseEnter);
      this.lbL4.MouseLeave += new System.EventHandler(this.lb_MouseLeave);
      // 
      // lbL7
      // 
      this.lbL7.BackColor = System.Drawing.Color.DarkGray;
      this.lbL7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lbL7.Location = new System.Drawing.Point(312, 788);
      this.lbL7.Margin = new System.Windows.Forms.Padding(0);
      this.lbL7.Name = "lbL7";
      this.lbL7.Size = new System.Drawing.Size(45, 45);
      this.lbL7.TabIndex = 15;
      this.lbL7.TabStop = false;
      this.lbL7.MouseEnter += new System.EventHandler(this.lb_MouseEnter);
      this.lbL7.MouseLeave += new System.EventHandler(this.lb_MouseLeave);
      // 
      // lbL6
      // 
      this.lbL6.BackColor = System.Drawing.Color.DarkGray;
      this.lbL6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lbL6.Location = new System.Drawing.Point(264, 788);
      this.lbL6.Margin = new System.Windows.Forms.Padding(0);
      this.lbL6.Name = "lbL6";
      this.lbL6.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.lbL6.Size = new System.Drawing.Size(45, 45);
      this.lbL6.TabIndex = 14;
      this.lbL6.TabStop = false;
      this.lbL6.MouseEnter += new System.EventHandler(this.lb_MouseEnter);
      this.lbL6.MouseLeave += new System.EventHandler(this.lb_MouseLeave);
      // 
      // lbL2
      // 
      this.lbL2.BackColor = System.Drawing.Color.DarkGray;
      this.lbL2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lbL2.Location = new System.Drawing.Point(72, 788);
      this.lbL2.Margin = new System.Windows.Forms.Padding(0);
      this.lbL2.Name = "lbL2";
      this.lbL2.Size = new System.Drawing.Size(45, 45);
      this.lbL2.TabIndex = 10;
      this.lbL2.TabStop = false;
      this.lbL2.MouseEnter += new System.EventHandler(this.lb_MouseEnter);
      this.lbL2.MouseLeave += new System.EventHandler(this.lb_MouseLeave);
      // 
      // lbL1
      // 
      this.lbL1.BackColor = System.Drawing.Color.DarkGray;
      this.lbL1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lbL1.Location = new System.Drawing.Point(24, 788);
      this.lbL1.Margin = new System.Windows.Forms.Padding(0);
      this.lbL1.Name = "lbL1";
      this.lbL1.Size = new System.Drawing.Size(45, 45);
      this.lbL1.TabIndex = 16;
      this.lbL1.TabStop = false;
      this.lbL1.MouseEnter += new System.EventHandler(this.lb_MouseEnter);
      this.lbL1.MouseLeave += new System.EventHandler(this.lb_MouseLeave);
      // 
      // btGetLetters
      // 
      this.btGetLetters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btGetLetters.Location = new System.Drawing.Point(373, 791);
      this.btGetLetters.Name = "btGetLetters";
      this.btGetLetters.Size = new System.Drawing.Size(113, 38);
      this.btGetLetters.TabIndex = 17;
      this.btGetLetters.TabStop = false;
      this.btGetLetters.Text = "Get Letters";
      this.btGetLetters.UseVisualStyleBackColor = true;
      this.btGetLetters.Click += new System.EventHandler(this.btGetLetters_Click);
      // 
      // ScrabbleBoard
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(761, 851);
      this.Controls.Add(this.btGetLetters);
      this.Controls.Add(this.lbL1);
      this.Controls.Add(this.lbL7);
      this.Controls.Add(this.lbL6);
      this.Controls.Add(this.lbL5);
      this.Controls.Add(this.lbL4);
      this.Controls.Add(this.lbL3);
      this.Controls.Add(this.lbL2);
      this.Controls.Add(this.btReset);
      this.Controls.Add(this.btSolve);
      this.Controls.Add(this.pbBoard);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.Name = "ScrabbleBoard";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "Scrabble Solver";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScrabbleBoard_KeyDown);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScrabbleBoard_KeyPress);
      ((System.ComponentModel.ISupportInitialize)(this.pbBoard)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL5)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL4)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL7)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL6)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.lbL1)).EndInit();
      this.ResumeLayout(false);

    }

        #endregion

      private System.Windows.Forms.PictureBox pbBoard;
      private UnselectableButton btSolve;
      private UnselectableButton btReset;
      private System.Windows.Forms.PictureBox lbL1;
      private System.Windows.Forms.PictureBox lbL2;
      private System.Windows.Forms.PictureBox lbL3;
      private System.Windows.Forms.PictureBox lbL4;
      private System.Windows.Forms.PictureBox lbL5;
      private System.Windows.Forms.PictureBox lbL6;
      private System.Windows.Forms.PictureBox lbL7;
      private UnselectableButton btGetLetters;
    }
}

