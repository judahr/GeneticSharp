using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Checkers
{
    [TestFixture]
    [Category("Extensions")]
    public class CheckersSquareTest
    {
        [Test]
        public void Constructor_ColumnAndRowIndex_FreeOrNotPlayable()
        {
            var target = new CheckersSquare(0, 0);
            ClassicAssert.AreEqual(CheckersSquareState.NotPlayable, target.State);

            target = new CheckersSquare(7, 7);
            ClassicAssert.AreEqual(CheckersSquareState.NotPlayable, target.State);

            target = new CheckersSquare(1, 0);
            ClassicAssert.AreEqual(CheckersSquareState.Free, target.State);

            target = new CheckersSquare(2, 0);
            ClassicAssert.AreEqual(CheckersSquareState.NotPlayable, target.State);

            target = new CheckersSquare(3, 0);
            ClassicAssert.AreEqual(CheckersSquareState.Free, target.State);

            target = new CheckersSquare(4, 0);
            ClassicAssert.AreEqual(CheckersSquareState.NotPlayable, target.State);
        }

        [Test]
        public void PutPiece_PlayerSquare_False()
        {
            var square = new CheckersSquare(3, 2);
            square.PutPiece(new CheckersPiece(CheckersPlayer.PlayerOne));

            ClassicAssert.IsFalse(square.PutPiece(new CheckersPiece(CheckersPlayer.PlayerOne)));
        }

        [Test]
        public void PutPiece_NoPlayableSquare_Exception()
        {
            var square = new CheckersSquare(0, 0);

            Assert.Catch<ArgumentException>(() =>
            {
                square.PutPiece(new CheckersPiece(CheckersPlayer.PlayerOne));
            }, "Attempt to put a piece in a not playable square.");
        }

        [Test]
        public void RemovePiece_CurrentSquareNull_False()
        {
            var square = new CheckersSquare(3, 2);

            ClassicAssert.IsFalse(square.RemovePiece());
        }

        [Test]
        public void Equals_NotPiece_False()
        {
            var square = new CheckersSquare(3, 2);

            ClassicAssert.IsFalse(square.Equals("square"));
        }

        [Test]
        public void Equals_OtherDiffSquare_False()
        {
            var square = new CheckersSquare(3, 2);
            var other = new CheckersSquare(3, 3);

            ClassicAssert.IsFalse(square.Equals(other));
        }

        [Test]
        public void Equals_OtherEqualSquare_True()
        {
            var square = new CheckersSquare(3, 3);
            var other = new CheckersSquare(3, 3);

            ClassicAssert.IsTrue(square.Equals(other));
        }

        [Test]
        public void GetHashCode_DiffSquares_DiffHashes()
        {
            var square = new CheckersSquare(3, 3);
            var other = new CheckersSquare(3, 2);

            ClassicAssert.AreNotEqual(square.GetHashCode(), other.GetHashCode());
        }
    }
}