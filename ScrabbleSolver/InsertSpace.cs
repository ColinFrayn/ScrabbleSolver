using System;
using System.Windows.Forms;

namespace ScrabbleSolver {
  public partial class InsertSpace : Form {
    public char? Key { get; private set; }

    public InsertSpace() {
      InitializeComponent();
      Key = null;
    }

    private void InsertSpace_KeyPress(object sender, KeyPressEventArgs e) {
      if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ') return;
      Key = Char.ToUpper(e.KeyChar);
      this.Close();
    }

    private void InsertSpace_KeyDown(object sender, KeyEventArgs e) {
      if (e.KeyCode == Keys.Escape) this.Close();
    }
  }
}
