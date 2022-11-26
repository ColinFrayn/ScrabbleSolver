using System;
using System.Windows.Forms;

namespace ScrabbleSolver {
  public partial class SolverInfo : Form {
    private readonly Action playBest, killSolver;
    private delegate void SafeCallDelegate(string str);

    public SolverInfo(Action play, Action kill) {
      playBest = play;
      killSolver = kill;

      InitializeComponent();

      Reset();
    }

    public void Reset() {
      btPlay.Enabled = false;
      lbInfo.Items.Clear();
    }

    private void btPlay_Click(object sender, EventArgs e) {
      playBest();
      killSolver();
      this.Hide();
    }

    private void btCancel_Click(object sender, EventArgs e) {
      killSolver();
      this.Hide();
    }

    private void SolverInfo_FormClosing(object sender, FormClosingEventArgs e) {
      this.Hide();
      e.Cancel = true; // this cancels the close event.
    }

    public void AddLine(string str) {
      if (lbInfo.InvokeRequired) {
        var d = new SafeCallDelegate(AddLine);
        lbInfo.Invoke(d, new object[] { str });
      }
      else {
        lbInfo.Items.Add(str);
      }
    }

    public void CanPlay() {
      if (btPlay.InvokeRequired) {
        var a = new Action(CanPlay);
        btPlay.Invoke(a);
      }
      else {
        btPlay.Enabled = true;
      }
    }
  }
}
