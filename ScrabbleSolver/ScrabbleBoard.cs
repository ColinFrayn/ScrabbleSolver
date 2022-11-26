using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Drawing.Drawing2D;

namespace ScrabbleSolver {
  public partial class ScrabbleBoard : Form {
    private enum Square { None, DblLtr, TplLtr, DblWrd, TplWrd, Centre };
    private readonly Dictionary<char, int> Values = new Dictionary<char, int>();
    private readonly char?[] Hand = new char?[7];
    private readonly Dictionary<char, int> Bag = new Dictionary<char, int>();
    private readonly char?[,] Board = new char?[15, 15];
    private readonly BackgroundWorker Solver = new BackgroundWorker();
    private readonly HashSet<string> hsWordlist = new HashSet<string>();
    private readonly SolverInfo sInfo;
    private readonly Dictionary<Point, char> Spaces = new Dictionary<Point, char>();
    private readonly Font tileFont = new Font("Microsoft Sans Serif", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
    private readonly Font tileSmallFont = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
    private readonly StringFormat sfCentre = new StringFormat();
    private readonly int TileW, TileH;
    private readonly List<Dictionary<char, List<string>>> dWords = new List<Dictionary<char, List<string>>>();
    private readonly List<string> lAllShortWords = new List<string>();
    private static readonly Square[,] Squares = new Square[15, 15];
    private static readonly PictureBox[] HandTiles = new PictureBox[7];
    private Candidate Suggestion;
    private int CurrentHand = -1;
    private const int BorderX = 25, BorderY = 25;
    private int sx = -1, sy = -1, BestScore = -1, xstep, ystep;

    // Colours
    private readonly Color BorderColour = Color.DarkGray;
    private readonly Color GridColour = Color.Black;
    private readonly Color GridHighlightColour = Color.White;
    private readonly Color TileEdgeColour = Color.FromArgb(255, 200, 160, 40);
    private readonly Color TileColour = Color.FromArgb(255, 255, 255, 120);
    private readonly Color TileEdgeColourFaded = Color.FromArgb(100, 200, 160, 40);
    private readonly Color TileColourFaded = Color.FromArgb(100, 255, 255, 120);

    public ScrabbleBoard() {
      sfCentre.Alignment = StringAlignment.Center;
      sfCentre.LineAlignment = StringAlignment.Center;
      InitializeComponent();
      HandTiles[0] = lbL1;
      HandTiles[1] = lbL2;
      HandTiles[2] = lbL3;
      HandTiles[3] = lbL4;
      HandTiles[4] = lbL5;
      HandTiles[5] = lbL6;
      HandTiles[6] = lbL7;
      ResetBag();
      ResetHand();
      ResetBoard();
      SetupSquares();
      SetupValues();
      sInfo = new SolverInfo(PlayBest, KillSolver);
      Solver.DoWork += new DoWorkEventHandler(Solver_DoWork);
      Solver.WorkerSupportsCancellation = true;
      LoadWords();
      TileW = pbBoard.Width / 18;
      TileH = pbBoard.Height / 18;
    }

    #region Initialisation
    private void SetupValues() {
      int[] vals = new int[26] { 1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10 };
      Values.Clear();
      for (int n = 0; n < 26; n++) {
        Values.Add((char)(n + (int)'A'), vals[n]);
      }
      Values.Add(' ', 0);
    }
    private void ResetBag() {
      int[] freq = new int[26] { 9, 2, 2, 4, 12, 2, 3, 2, 9, 1, 1, 4, 2, 6, 8, 2, 1, 6, 4, 6, 4, 2, 2, 1, 2, 1 };
      Bag.Clear();
      for (int n = 0; n < 26; n++) {
        Bag.Add((char)(n + (int)'A'), freq[n]);
      }
      Bag.Add(' ', 2);
    }
    private void ResetHand() {
      for (int n = 0; n < 7; n++) {
        Hand[n] = null;
        DrawHandTile(n);
      }
    }
    private void ResetBoard() {
      for (int y = 0; y < 15; y++) {
        for (int x = 0; x < 15; x++) {
          Board[x, y] = null;
        }
      }
      Suggestion = null;
      Spaces.Clear();
      pbBoard.Invalidate();
    }
    private void SetupSquares() {
      for (int y = 0; y < 15; y++) {
        for (int x = 0; x < 15; x++) {
          if ((x == 0 || x == 7 || x == 14) && (y == 0 || y == 7 || y == 14)) Squares[x, y] = Square.TplWrd;
          else if ((x == 1 || x == 5 || x == 9 || x == 13) && (y == 1 || y == 5 || y == 9 || y == 13)) Squares[x, y] = Square.TplLtr;
          else Squares[x, y] = Square.None;
        }
      }
      for (int i = 1; i <= 4; i++) {
        Squares[i, i] = Square.DblWrd;
        Squares[14 - i, i] = Square.DblWrd;
        Squares[i, 14 - i] = Square.DblWrd;
        Squares[14 - i, 14 - i] = Square.DblWrd;
      }
      Squares[3, 0] = Squares[11, 0] = Square.DblLtr;
      Squares[6, 2] = Squares[8, 2] = Square.DblLtr;
      Squares[0, 3] = Squares[7, 3] = Squares[14, 3] = Square.DblLtr;
      Squares[2, 6] = Squares[6, 6] = Squares[8, 6] = Squares[12, 6] = Square.DblLtr;
      Squares[3, 7] = Squares[11, 7] = Square.DblLtr;
      Squares[3, 14] = Squares[11, 14] = Square.DblLtr;
      Squares[6, 12] = Squares[8, 12] = Square.DblLtr;
      Squares[0, 11] = Squares[7, 11] = Squares[14, 11] = Square.DblLtr;
      Squares[2, 8] = Squares[6, 8] = Squares[8, 8] = Squares[12, 8] = Square.DblLtr;
      Squares[7, 7] = Square.Centre;
    }
    private void LoadWords() {
      string[] words = File.ReadAllLines("Text/Dictionary.txt");
      foreach (string str in words) {
        hsWordlist.Add(str.ToUpper());
        if (str.Length <= 7) lAllShortWords.Add(str.ToUpper());
      }
      for (int n=0; n<15; n++) {
        Dictionary<char, List<string>> dict = new Dictionary<char, List<string>>();
        foreach (string str in hsWordlist) {
          if (str.Length <= n) continue;
          char c = str[n];
          if (dict.ContainsKey(c)) dict[c].Add(str);
          else dict.Add(c, new List<string>() { str });
        }
        dWords.Add(dict);
      }
    }
    #endregion // Initialisation

    #region Handlers
    private void pbBoard_MouseMove(object sender, MouseEventArgs e) {
      int oldsx = sx, oldsy = sy;
      sx = (e.X - pbBoard.Left) - BorderX;
      if (sx < 0) sx = -1;
      else sx = (sx * 15) / (pbBoard.Width - (BorderX * 2));
      sy = (e.Y - pbBoard.Top) - BorderY;
      if (sy < 0) sy = -1;
      else sy = (sy * 15) / (pbBoard.Height - (BorderY * 2));

      // Cursor has moved?
      if (sx != oldsx || sy != oldsy) {
        using (Graphics g = pbBoard.CreateGraphics()) {
          // Delete old cursor
          Rectangle r1 = Rectangle.FromLTRB(BorderX + (oldsx * (pbBoard.Width - (BorderX * 2)) / 15),
                                            BorderY + (oldsy * (pbBoard.Height - (BorderY * 2)) / 15),
                                            BorderX + ((oldsx + 1) * (pbBoard.Width - (BorderX * 2)) / 15),
                                            BorderY + ((oldsy + 1) * (pbBoard.Height - (BorderY * 2)) / 15));
          if (oldsx >= 0 && oldsx <= 14 && oldsy >= 0 && oldsy <= 14) {
            g.DrawRectangle(new Pen(GridColour), r1);
            g.DrawRectangle(new Pen(CoordsToColour(oldsx,oldsy)), r1.Left + 1, r1.Top + 1, r1.Right - r1.Left - 2, r1.Bottom - r1.Top - 2);
          }

          // Draw new cursor
          Rectangle r2 = Rectangle.FromLTRB(BorderX + (sx * (pbBoard.Width - (BorderX * 2)) / 15),
                                            BorderY + (sy * (pbBoard.Height - (BorderY * 2)) / 15),
                                            BorderX + ((sx + 1) * (pbBoard.Width - (BorderX * 2)) / 15),
                                            BorderY + ((sy + 1) * (pbBoard.Height - (BorderY * 2)) / 15));
          if (sx >= 0 && sx <= 14 && sy >= 0 && sy <= 14) {
            g.DrawRectangle(new Pen(GridHighlightColour), r2);
            g.DrawRectangle(new Pen(GridHighlightColour), r2.Left + 1, r2.Top + 1, r2.Right - r2.Left - 2, r2.Bottom - r2.Top - 2);
          }
        }
      }
    }
    private void lb_MouseEnter(object sender, EventArgs e) {
      int x = ((PictureBox)sender).Left - lbL1.Left;
      CurrentHand = x / (lbL2.Left - lbL1.Left);
      DrawHandTile(CurrentHand);
    }
    private void lb_MouseLeave(object sender, EventArgs e) {
      int n = CurrentHand;
      CurrentHand = -1;
      DrawHandTile(n);
    }
    private void ScrabbleBoard_KeyPress(object sender, KeyPressEventArgs e) {
      if (Solver.IsBusy) return;
      if (sx < 0 || sx > 14 || sy < 0) return;
      if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ') return;
      char c = Char.ToUpper(e.KeyChar);

      // Check the player's letters
      if (sy > 14) {
        if (CurrentHand == -1) return;
        if (Bag[c] == 0) return;
        Bag[c]--;
        if (Hand[CurrentHand].HasValue) Bag[Hand[CurrentHand].Value]++;
        Hand[CurrentHand] = c;
        DrawHandTile(CurrentHand);
        return;
      }

      // Handle other key presses (on the main board)
      if (e.KeyChar == ' ') {
        if (Bag[' '] == 0) return;
        InsertSpace isp = new InsertSpace();
        isp.ShowDialog();
        if (!isp.Key.HasValue) return;
        Bag[' ']--;
        if (Board[sx, sy].HasValue) Bag[Board[sx, sy].Value]++;
        Board[sx, sy] = ' ';
        Spaces.Add(new Point(sx, sy), isp.Key.Value);
      }
      else {
        if (Bag[c] == 0) return;
        Bag[c]--;
        if (Board[sx, sy].HasValue) Bag[Board[sx, sy].Value]++;
        Board[sx, sy] = c;
      }
      pbBoard.Invalidate();
    }
    private void ScrabbleBoard_KeyDown(object sender, KeyEventArgs e) {
      if (Solver.IsBusy) return;
      if (sx < 0 || sx > 14 || sy < 0) return;
      if (e.KeyCode != Keys.Delete) return;

      if (sy > 14) {
        if (CurrentHand == -1) return;
        if (Hand[CurrentHand].HasValue) {
          Bag[Hand[CurrentHand].Value]++;
          Hand[CurrentHand] = null;
          DrawHandTile(CurrentHand);
        }
        return;
      }
      if (Board[sx, sy].HasValue) {
        Bag[Board[sx, sy].Value]++;
        Board[sx, sy] = null;
        pbBoard.Invalidate();
      }
    }
    private void btReset_Click(object sender, EventArgs e) {
      ResetBag();
      ResetHand();
      ResetBoard();
    }
    private void btSolve_Click(object sender, EventArgs e) {
      Suggestion = null;
      BestScore = -1;
      btSolve.Enabled = false;
      sInfo.Reset();
      sInfo.Show();
      sInfo.BringToFront();
      Solver.RunWorkerAsync();
    }
    private void btGetLetters_Click(object sender, EventArgs e) {
      Random rnd = new Random();
      for (int n=0; n<7; n++) {
        if (!Hand[n].HasValue) {
          Hand[n] = GetRandomLetterFromBag(rnd);
          DrawHandTile(n);
        }
      }
    }
    private char? GetRandomLetterFromBag(Random rnd) {
      int count = 0;
      foreach (int i in Bag.Values) {
        count += i;
      }
      if (count == 0) return null;
      int r = rnd.Next(count);
      foreach (char c in Bag.Keys) {
        r -= Bag[c];
        if (r < 0) {
          Bag[c]--;
          return c;
        }
      }
      return null;
    }
    #endregion // Handlers

    #region Solver
    private void PlayBest() {
      Solver.CancelAsync();
      if (Suggestion != null) {
        for (int n=0; n<Suggestion.Length; n++) {
          int xx = Suggestion.X, yy = Suggestion.Y;
          if (Suggestion.Horiz) xx += n;
          else yy += n;
          if (!Board[xx, yy].HasValue) {
            Board[xx, yy] = Suggestion.Tiles[n];
            RemoveLetter(Suggestion.Tiles[n]);
            if (Suggestion.Tiles[n] == ' ') Spaces.Add(new Point(xx, yy), Suggestion.Word[n]);
          }
        }
      }
    }
    private void KillSolver() {
      btSolve.Enabled = true;
      Solver.CancelAsync();
      sInfo.Close();
      Suggestion = null;
      pbBoard.Invalidate();
    }
    private bool RemoveLetter(char c) {
      for (int n = 0; n < 7; n++) {
        if (Hand[n].HasValue && Hand[n].Value == c) {
          Hand[n] = null;
          DrawHandTile(n);
          return true;
        }
      }
      throw new Exception("Could not find hand letter : " + c.ToString());
    }
    private void Solver_DoWork(object sender, DoWorkEventArgs e) {
      sInfo.AddLine("Dictionary size = " + hsWordlist.Count + " words");
      sInfo.AddLine("Starting solver ... ");
      Stopwatch sw = Stopwatch.StartNew();
      CalculateBestWord();
      sInfo.AddLine("Solver complete");
      if (BestScore == -1) sInfo.AddLine("No words found");
      sInfo.AddLine("Elapsed time = " + sw.ElapsedMilliseconds + "ms");
    }
    private void CalculateBestWord() {
      Dictionary<char, int> dLetters = new Dictionary<char, int>();
      bool[,] Playable = new bool[15, 15];
      for (int y = 0; y < 15; y++) {
        for (int x = 0; x < 15; x++) {
          if (Board[x, y].HasValue) Playable[x, y] = true;
          if (x > 0 && Board[x - 1, y].HasValue) Playable[x, y] = true;
          if (x < 14 && Board[x + 1, y].HasValue) Playable[x, y] = true;
          if (y > 0 && Board[x, y - 1].HasValue) Playable[x, y] = true;
          if (y < 14 && Board[x, y + 1].HasValue) Playable[x, y] = true;
        }
      }
      bool FirstWord = !(Board[7, 7].HasValue);
      for (int n = 0; n < 7; n++) {
        if (Hand[n].HasValue) {
          if (dLetters.ContainsKey(Hand[n].Value)) dLetters[Hand[n].Value]++;
          else dLetters.Add(Hand[n].Value, 1);
        }
      }
      // Loop through starting squares
      for (int y = 0; y < 15; y++) {
        for (int x = 0; x < 15; x++) {
          if (Solver.CancellationPending) return;
          if (x == 0 || !Board[x - 1, y].HasValue) {
            if (!FirstWord || (y == 7 && x > 0 && x <= 7)) CheckHorizontalWords(x, y, dLetters, FirstWord, Playable);
          }
          if (y == 0 || !Board[x, y - 1].HasValue) {
            if (!FirstWord || (x == 7 && y > 0 && y <= 7)) CheckVerticalWords(x, y, dLetters, FirstWord, Playable);
          }
        }
      }
    }
    private void CheckHorizontalWords(int x, int y, Dictionary<char, int> dLetters, bool FirstWord, bool[,] Playable) {
      // Get list of letters
      char?[] clist = new char?[15 - x];
      int countEmpty = 0;
      for (int xx = 0; xx < 15 - x; xx++) {
        if (!Board[x + xx, y].HasValue && countEmpty == 7) break; // Can't deal with more than 7 empty squares
        clist[xx] = Board[x + xx, y];
        if (Board[x + xx, y] == ' ') clist[xx] = Spaces[new Point(x + xx, y)];
        if (!clist[xx].HasValue) countEmpty++;
      }

      // Get all words
      List<Tuple<string,string>> lWords = GetAllFittingWords(clist, dLetters);

      // Check fit with surrounding letters in other rows
      foreach (Tuple<string,string> tp in lWords) {
        string word = tp.Item1;
        bool bOK = FirstWord;
        if (Solver.CancellationPending) return;
        if (FirstWord) {
          if (x + word.Length <= 7) continue;
        }
        else {
          // Make sure we're playing off an existing word, unless it's the first one
          bOK = FirstWord;
          for (int xx=x; xx < x+word.Length; xx++) {
            if (Playable[xx,y]) { bOK = true;break; }
          }
          if (!bOK) continue;
          // Check extra side-words
          for (int n = 0; n < word.Length; n++) {
            int xx = x + n;
            if (Board[xx, y].HasValue) continue; // Not a side-word because we didn't place this letter
            if ((y > 0 && Board[xx, y - 1].HasValue) || (y < 14 && Board[xx, y + 1].HasValue)) {
              int fy = y;
              while (fy > 0 && Board[xx, fy - 1].HasValue) fy--;
              int ty = y;
              while (ty < 14 && Board[xx, ty + 1].HasValue) ty++;
              StringBuilder sb = new StringBuilder();
              for (int yy = fy; yy <= ty; yy++) {
                if (Board[xx, yy].HasValue) sb.Append(LetterAt(xx, yy));
                else {
                  if (yy != y) throw new Exception("Missing board letter testing side words");
                  sb.Append(word[n]);
                }

              }
              if (!hsWordlist.Contains(sb.ToString().ToUpper())) { bOK = false; break; }
            }
          }
        }
        if (!bOK) continue;
        int score = ScoreWord(new Candidate(x, y, true, word, tp.Item2));
        if (score > BestScore) {
          BestScore = score;
          CandidateWord(x, y, tp.Item2, word, true);
          sInfo.AddLine("(" + x + "," + y + ") : " + word + "  [" + score + "]");
        }
      }
    }
    private void CheckVerticalWords(int x, int y, Dictionary<char, int> dLetters, bool FirstWord, bool[,] Playable) {
      // Get list of letters
      char?[] clist = new char?[15 - y];
      int countEmpty = 0;
      for (int yy = 0; yy < 15 - y; yy++) {
        if (!Board[x, y + yy].HasValue && countEmpty == 7) break; // Can't deal with more than 7 empty squares
        clist[yy] = Board[x, y + yy];
        if (Board[x, y + yy] == ' ') clist[yy] = Spaces[new Point(x, y + yy)];
        if (!clist[yy].HasValue) countEmpty++;
      }

      // Get all words
      List<Tuple<string, string>> lWords = GetAllFittingWords(clist, dLetters);

      // Check fit with surrounding letters in other rows
      foreach (Tuple<string, string> tp in lWords) {
        string word = tp.Item1;
        bool bOK = FirstWord;
        if (Solver.CancellationPending) return;
        if (FirstWord) {
          if (y + word.Length <= 7) continue;
        }
        else {
          bOK = FirstWord;
          for (int yy = y; yy < y + word.Length; yy++) {
            if (Playable[x,yy]) { bOK = true; break; }
          }
          if (!bOK) continue;
          // Check extra side-words
          for (int n = 0; n < word.Length; n++) {
            int yy = y + n;
            if (Board[x, yy].HasValue) continue; // Not a cross-word because we didn't place this letter
            if ((x > 0 && Board[x - 1, yy].HasValue) || (x < 14 && Board[x + 1, yy].HasValue)) {
              int fx = x;
              while (fx > 0 && Board[fx - 1, yy].HasValue) fx--;
              int tx = x;
              while (tx < 14 && Board[tx + 1, yy].HasValue) tx++;
              StringBuilder sb = new StringBuilder();
              for (int xx = fx; xx <= tx; xx++) {
                if (Board[xx, yy].HasValue) sb.Append(LetterAt(xx, yy));
                else {
                  if (xx != x) throw new Exception("Missing board letter testing side words");
                  sb.Append(word[n]);
                }
              }
              if (!hsWordlist.Contains(sb.ToString().ToUpper())) { bOK = false; break; }
            }
          }
        }
        if (!bOK) continue;
        int score = ScoreWord(new Candidate(x,y,false,word,tp.Item2));
        if (score > BestScore) {
          BestScore = score;
          CandidateWord(x, y, tp.Item2, word, false);
          sInfo.AddLine("(" + x + "," + y + ") : " + word + "  [" + score + "]");
        }
      }
    }
    private List<Tuple<string,string>> GetAllFittingWords(char?[] chars, Dictionary<char, int> dLetters) {
      Dictionary<char, int> lett = new Dictionary<char, int>(dLetters);
      List<Tuple<string, string>> lWords = new List<Tuple<string, string>>();
      List<char> Removed = new List<char>();

      // Get a subset of words to consider
      List<string> lSource = null;
      for (int i=0; i<chars.Length; i++) {
        if (chars[i].HasValue) {
          if (!dWords[i].ContainsKey(chars[i].Value)) return lWords;
          lSource = dWords[i][chars[i].Value];
          break;
        }
      }
      if (lSource == null) lSource = lAllShortWords; // No char to build off, so we have to make 7-letter words or shorter using our tiles

      foreach (string str in lSource) {
        Removed.Clear();
        bool bOK = true;
        if (str.Length > chars.Length) continue;
        if (str.Length < chars.Length && chars[str.Length].HasValue) continue; // Should consider this as a whole word
        StringBuilder sb = new StringBuilder();
        for (int n=0; n<str.Length; n++) {
          if (chars[n].HasValue) {
            if (chars[n].Value != str[n]) { bOK = false; break; }
            sb.Append(str[n]);
          }
          if (!chars[n].HasValue) {
            if (lett.ContainsKey(str[n]) && lett[str[n]] > 0) {
              lett[str[n]]--;
              Removed.Add(str[n]);
              sb.Append(str[n]);
            }
            else if (lett.ContainsKey(' ') && lett[' '] > 0) {
              lett[' ']--;
              Removed.Add(' ');
              sb.Append(' ');
            }
            else { bOK = false; break; }
          }
        }
        if (bOK && Removed.Count > 0) lWords.Add(new Tuple<string,string>(str,sb.ToString()));
        foreach (char c in Removed) lett[c]++;
      }

      return lWords;
    }
    private void CandidateWord(int x, int y, string str, string word, bool bHoriz) {
      Suggestion = new Candidate(x, y, bHoriz, word, str);
      sInfo.CanPlay();
      pbBoard.Invalidate();
    }
    private int ScoreWord(Candidate c) {
      int score = ScoreWordNoShuffle(c);
      // Check if we can do better by shuffling spaces and letters
      for (int n=0; n<c.Length; n++) {
        if (c.Tiles[n] == ' ') {
          for (int i = 0; i < c.Length; i++) {
            if (i != n && c.Word[i] == c.Word[n] && ((c.Horiz && !Board[c.X+i,c.Y].HasValue) || (!c.Horiz && !Board[c.X, c.Y + i].HasValue))) {
              Candidate c2 = c.CloneWithSwappedBlank(n, i);
              score = Math.Max(score, ScoreWordNoShuffle(c2));
            }
          }
        }
      }
      return score;
    }
    private int ScoreWordNoShuffle(Candidate c) {
      int score = 0, mult = 1, used = 0;

      for (int n = 0; n < c.Length; n++) {
        int xx = c.X, yy = c.Y;
        if (c.Horiz) xx += n;
        else yy += n;
        if (Board[xx, yy].HasValue) {
          if (LetterAt(xx, yy) != c.Word[n]) throw new Exception("Error - unexpected board char");
          score += Values[c.Word[n]];
        }
        else {
          score += Values[c.Tiles[n]];
          used++;
          if (Squares[xx, yy] == Square.DblWrd) mult *= 2;
          else if (Squares[xx, yy] == Square.TplWrd) mult *= 3;
          else if (Squares[xx, yy] == Square.DblLtr) score += Values[c.Tiles[n]];
          else if (Squares[xx, yy] == Square.TplLtr) score += Values[c.Tiles[n]] * 2;
          else if (Squares[xx, yy] == Square.Centre) mult *= 2;
        }
      }
      int total = score * mult;
      if (used == 7) total += 50;

      // Calculate extra side-words
      for (int n = 0; n < c.Length; n++) {
        if (c.Horiz) {
          int xx = c.X + n;
          if (Board[xx, c.Y].HasValue) continue; // Not a cross-word because we didn't place this letter
          if ((c.Y > 0 && Board[xx, c.Y - 1].HasValue) || (c.Y < 14 && Board[xx, c.Y + 1].HasValue)) {
            int fy = c.Y;
            while (fy > 0 && Board[xx, fy - 1].HasValue) fy--;
            int ty = c.Y;
            while (ty < 14 && Board[xx, ty + 1].HasValue) ty++;
            score = Values[c.Tiles[n]];
            mult = 1;
            if (Squares[xx, c.Y] == Square.DblWrd) mult = 2;
            else if (Squares[xx, c.Y] == Square.TplWrd) mult = 3;
            else if (Squares[xx, c.Y] == Square.DblLtr) score *= 2;
            else if (Squares[xx, c.Y] == Square.TplLtr) score *= 3;
            for (int yy = fy; yy <= ty; yy++) {
              if (yy!=c.Y) score += Values[Board[xx, yy].Value];
            }
            score *= mult;
            total += score;
          }
        }
        else {
          int yy = c.Y + n;
          if (Board[c.X, yy].HasValue) continue; // Not a cross-word because we didn't place this letter
          if ((c.X > 0 && Board[c.X - 1, yy].HasValue) || (c.X < 14 && Board[c.X + 1, yy].HasValue)) {
            int fx = c.X;
            while (fx > 0 && Board[fx - 1, yy].HasValue) fx--;
            int tx = c.X;
            while (tx < 14 && Board[tx + 1, yy].HasValue) tx++;
            score = Values[c.Tiles[n]];
            mult = 1;
            if (Squares[c.X, yy] == Square.DblWrd) mult = 2;
            else if (Squares[c.X, yy] == Square.TplWrd) mult = 3;
            else if (Squares[c.X, yy] == Square.DblLtr) score *= 2;
            else if (Squares[c.X, yy] == Square.TplLtr) score *= 3;
            for (int xx = fx; xx <= tx; xx++) {
              if (xx != c.X) score += Values[Board[xx, yy].Value];
            }
            score *= mult;
            total += score;
          }
        }
      }

      return total;
    }
    private char? LetterAt(int x, int y) {
      if (!Board[x, y].HasValue) return null;
      if (Board[x,y].Value == ' ') {
        if (Spaces.ContainsKey(new Point(x, y))) return Spaces[new Point(x, y)];
        throw new Exception("Missing space reference");
      }
      return Board[x, y];
    }
    #endregion // Solver

    #region Graphics
    private void pbBoard_Paint(object sender, PaintEventArgs e) {
      // Clear the background.
      e.Graphics.Clear(BorderColour);
      e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

      xstep = (pbBoard.Width - (BorderX * 2)) / 15 + 1;
      ystep = (pbBoard.Height - (BorderY * 2)) / 15 + 1;

      for (int y = 0; y < 15; y++) {
        for (int x = 0; x < 15; x++) {
          DrawBoardTile(x, y, e.Graphics);
        }
      }
      for (int y=0; y<=15; y++) {
        e.Graphics.DrawLine(new Pen(GridColour), BorderX, BorderY + (y * (pbBoard.Height - (BorderY * 2)) / 15), pbBoard.Width - BorderX, BorderY + (y * (pbBoard.Height - (BorderY * 2)) / 15));
      }
      for (int x=0; x<=15; x++) {
        e.Graphics.DrawLine(new Pen(GridColour), BorderX + (x * (pbBoard.Width - (BorderX * 2)) / 15), BorderY, BorderX + (x * (pbBoard.Width - (BorderX * 2)) / 15), pbBoard.Height - BorderY);
      }
      if (Suggestion != null) {
        for (int n = 0; n < Suggestion.Length; n++) {
          int xx = Suggestion.X, yy = Suggestion.Y;
          if (Suggestion.Horiz) xx += n;
          else yy += n;
          DrawTile(BorderX + ((xx * 2 + 1) * (pbBoard.Width - (BorderX * 2)) / 30), BorderY + ((yy * 2 + 1) * (pbBoard.Height - (BorderY * 2)) / 30), Suggestion.Word[n], e.Graphics, Suggestion.Tiles[n] == ' ', true);
        }
      }
    }
    private void DrawBoardTile(int x, int y, Graphics g) {
      Color col = CoordsToColour(x, y);
      g.FillRectangle(new SolidBrush(col), BorderX + (x * (pbBoard.Width - (BorderX * 2)) / 15), BorderY + (y * (pbBoard.Height - (BorderY * 2)) / 15), xstep, ystep);
      if (Board[x, y].HasValue) {
        if (Board[x, y].Value == ' ') {
          DrawTile(BorderX + ((x * 2 + 1) * (pbBoard.Width - (BorderX * 2)) / 30), BorderY + ((y * 2 + 1) * (pbBoard.Height - (BorderY * 2)) / 30), Spaces[new Point(x, y)], g, true);
        }
        else {
          DrawTile(BorderX + ((x * 2 + 1) * (pbBoard.Width - (BorderX * 2)) / 30), BorderY + ((y * 2 + 1) * (pbBoard.Height - (BorderY * 2)) / 30), Board[x, y].Value, g, false);
        }
      }
    }
    private void DrawTile(int x, int y, char val, Graphics g, bool bIsSpace, bool bFaded = false) {
      Rectangle rect = new Rectangle(x-TileW/2,y-TileH/2,TileW,TileH);
      GraphicsPath path = MakeRoundedRect(rect);
      g.FillPath(new SolidBrush(bFaded ? TileColourFaded : TileColour), path);
      g.DrawPath(new Pen(bFaded ? TileEdgeColourFaded : TileEdgeColour), path);
      if (bIsSpace) {
        g.DrawString(val.ToString().ToUpper(), tileFont, Brushes.LightGray, x, y, sfCentre);
      }
      else {
        g.DrawString(val.ToString().ToUpper(), tileFont, Brushes.Black, x, y, sfCentre);
        g.DrawString(Values[val].ToString(), tileSmallFont, Brushes.Black, x + (TileW / 3), y + (TileH / 3), sfCentre);
      }
    }
    private void DrawHandTile(int n) {
      if (n == CurrentHand) HandTiles[n].CreateGraphics().Clear(Color.LightGreen);
      else HandTiles[n].CreateGraphics().Clear(Color.DarkGray);
      if (Hand[n].HasValue) {
        DrawTile(HandTiles[n].Width / 2 - 1, HandTiles[n].Height / 2 - 1, Hand[n].Value, HandTiles[n].CreateGraphics(), Hand[n].Value == ' ');
      }
    }
    private static Color CoordsToColour(int x, int y) {
      switch (Squares[x, y]) {
        case Square.Centre: return Color.FromArgb(255, 255, 150, 150);
        case Square.DblWrd: return Color.FromArgb(255, 255, 180, 180);
        case Square.DblLtr: return Color.FromArgb(255, 165, 210, 255);
        case Square.TplLtr: return Color.FromArgb(255, 75, 160, 230);
        case Square.TplWrd: return Color.FromArgb(255, 255, 90, 50);
        case Square.None: return Color.LightGray;
      }
      return Color.LightGray;
    }
    private GraphicsPath MakeRoundedRect(RectangleF rect) {
      float xradius = rect.Width / 10.0f;
      float yradius = rect.Height / 10.0f;
      PointF point1, point2;
      GraphicsPath path = new GraphicsPath();

      // Upper left corner.
      RectangleF corner1 = new RectangleF(rect.X, rect.Y, 2 * xradius, 2 * yradius);
      path.AddArc(corner1, 180, 90);
      point1 = new PointF(rect.X + xradius, rect.Y);

      // Top side.
      point2 = new PointF(rect.Right - xradius, rect.Y);
      path.AddLine(point1, point2);

      // Upper right corner.
      RectangleF corner2 = new RectangleF(rect.Right - 2 * xradius, rect.Y, 2 * xradius, 2 * yradius);
      path.AddArc(corner2, 270, 90);
      point1 = new PointF(rect.Right, rect.Y + yradius);

      // Right side.
      point2 = new PointF(rect.Right, rect.Bottom - yradius);
      path.AddLine(point1, point2);

      // Lower right corner.
      RectangleF corner3 = new RectangleF(rect.Right - 2 * xradius, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
      path.AddArc(corner3, 0, 90);
      point1 = new PointF(rect.Right - xradius, rect.Bottom);

      // Bottom side.
      point2 = new PointF(rect.X + xradius, rect.Bottom);
      path.AddLine(point1, point2);

      // Lower left corner.
      RectangleF corner4 = new RectangleF(rect.X, rect.Bottom - 2 * yradius, 2 * xradius, 2 * yradius);
      path.AddArc(corner4, 90, 90);
      point1 = new PointF(rect.X, rect.Bottom - yradius);

      // Left side.
      point2 = new PointF(rect.X, rect.Y + yradius);
      path.AddLine(point1, point2);

      // Join with the start point.
      path.CloseFigure();

      return path;
    }
    #endregion
  }
}
