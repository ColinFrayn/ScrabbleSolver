using NUnit.Framework;
using ScrabbleSolver;
using System;

namespace ScrabbleTests {
  public class CandidateTests {
    [SetUp]
    public void Setup() {
    }

    [Test]
    public void Test_CandidateCreation() {
      Candidate c = new Candidate(0, 1, true, "Word", new char[4] { 'W', 'O', 'R', 'D' });
      Assert.IsInstanceOf<Candidate>(c);
      Candidate c2 = new Candidate(0, 1, true, "Word", "Word");
      Assert.IsInstanceOf<Candidate>(c2);
    }

    [Test]
    public void Test_CharStringWrongLengthShouldThrow() {
      Assert.Throws<ArgumentException>(() => new Candidate(0, 1, true, "Words", new char[4] { 'W', 'O', 'R', 'D' }));
      Assert.Throws<ArgumentException>(() => new Candidate(0, 1, true, "Test", "Test2"));
    }

    [Test]
    public void Test_InputStringIncludingBlankShouldThrow() { 
      Assert.Throws<ArgumentException>(() => new Candidate(0, 1, true, "A Test", "Another Test"));
    }
  }
}