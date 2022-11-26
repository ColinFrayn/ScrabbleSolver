using System;
using System.Linq;

namespace ScrabbleSolver {
  class Candidate {
    public int X, Y;
    public bool Horiz;
    public string Word;
    public char[] Tiles;

    public int Length {  get { return Word.Length; } }

    public Candidate(int x, int y, bool bHoriz, string w, string t) {
      X = x;
      Y = y;
      Horiz = bHoriz;
      Word = w;
      Tiles = t.ToCharArray();
      if (Word.Length != Tiles.Length) throw new  ArgumentException("Word and TileString not equal length");
      if (Word.Contains(' ')) throw new ArgumentException("Word contains spaces. Did you put word and tile string the wrong way round?");
    }
    public Candidate(int x, int y, bool bHoriz, string w, char[] ts) {
      X = x;
      Y = y;
      Horiz = bHoriz;
      Word = w;
      Tiles = ts;
      if (Word.Length != Tiles.Length) throw new ArgumentException("Word and TileString not equal length");
    }

    public Candidate CloneWithSwappedBlank(int a, int b) {
      if (a >= Tiles.Length || b >= Tiles.Length) throw new ArgumentException("CloneCandidateWithSwappedBlank - illegal index");
      char[] c2 = new char[Tiles.Length];
      Array.Copy(Tiles, c2, Tiles.Length);
      c2[a] = Tiles[b];
      c2[b] = Tiles[a];
      return new Candidate(X, Y, Horiz, Word, c2);
    }
  }
}
