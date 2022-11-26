using System.Windows.Forms;

namespace ScrabbleSolver {
  class UnselectableButton : Button {
    public UnselectableButton() {
      SetStyle(ControlStyles.Selectable, false);
    }
  }
}
