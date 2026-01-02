using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GeneticSharp.Extensions.UnitTests.Checkers
{
    [TestFixture()]
    [Category("Extensions")]
    public class CheckersBoardTest
    {
        [Test()]
        public void Constructos_InvalidSize_Exception()
        {
            Assert.Catch<ArgumentException>(() =>
         {
             new CheckersBoard(7);
         }, "The minimum valid size is 8.");

            Assert.Catch<ArgumentException>(() =>
            {
                new CheckersBoard(-8);
            }, "The minimum valid size is 8.");
        }

        [Test()]
        public void Contructor_ValidSize_PlayerOnePiecedPlaced()
        {
            var target = new CheckersBoard(8);
            ClassicAssert.AreEqual(8, target.Size);

            // First row.
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(1, 0).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(3, 0).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(5, 0).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(7, 0).State);

            // second row
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(0, 1).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(2, 1).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(4, 1).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(6, 1).State);

            // third row
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(1, 2).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(3, 2).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(5, 2).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerOne, target.GetSquare(7, 2).State);
        }

        [Test()]
        public void Contructor_ValidSize_PlayerTwoPiecedPlaced()
        {
            var target = new CheckersBoard(8);
            ClassicAssert.AreEqual(8, target.Size);

            // first row.
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(0, 7).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(2, 7).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(4, 7).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(6, 7).State);

            // second row            
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(1, 6).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(3, 6).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(5, 6).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(7, 6).State);

            // third row
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(0, 5).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(2, 5).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(4, 5).State);
            ClassicAssert.AreEqual(CheckersSquareState.OccupiedByPlayerTwo, target.GetSquare(6, 5).State);
        }

        [Test()]
        public void Contructor_ValidSize_FreeAndNotPlayableSquaresOk()
        {
            var target = new CheckersBoard(8);
            ClassicAssert.AreEqual(8, target.Size);

            for (int c = 0; c < 8; c++)
            {
                for (int r = 0; r < 8; r++)
                {
                    var notPlayable = CheckersSquare.IsNotPlayableSquare(c, r);
                    var actual = target.GetSquare(c, r).State;

                    if (notPlayable)
                    {
                        ClassicAssert.AreEqual(CheckersSquareState.NotPlayable, actual);
                    }
                    else
                    {
                        ClassicAssert.AreNotEqual(CheckersSquareState.NotPlayable, actual);
                    }
                }
            }
        }

        [Test]
        public void IsNotPlayableSquare_DiffSquares_DiffResults()
        {
            ClassicAssert.IsTrue(CheckersSquare.IsNotPlayableSquare(0, 0));
            ClassicAssert.IsFalse(CheckersSquare.IsNotPlayableSquare(0, 1));
            ClassicAssert.IsTrue(CheckersSquare.IsNotPlayableSquare(0, 2));

            ClassicAssert.IsFalse(CheckersSquare.IsNotPlayableSquare(1, 0));
            ClassicAssert.IsTrue(CheckersSquare.IsNotPlayableSquare(1, 1));
            ClassicAssert.IsFalse(CheckersSquare.IsNotPlayableSquare(1, 2));
        }

        [Test()]
        public void GetSize_InvalidIndexes_Exception()
        {
            var target = new CheckersBoard(10);

            var actual = Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.GetSquare(-1, 0);
            });
            ClassicAssert.AreEqual("columnIndex", actual.ParamName);

            actual = Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.GetSquare(10, 0);
            });
            ClassicAssert.AreEqual("columnIndex", actual.ParamName);

            actual = Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.GetSquare(0, -1);
            });
            ClassicAssert.AreEqual("rowIndex", actual.ParamName);

            actual = Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                target.GetSquare(0, 10);
            });
            ClassicAssert.AreEqual("rowIndex", actual.ParamName);
        }

        [Test()]
        public void MovePiece_NullMove_Exception()
        {
            var target = new CheckersBoard(10);

            var actual = Assert.Catch<ArgumentNullException>(() =>
            {
                target.MovePiece(null);
            });
            ClassicAssert.AreEqual("move", actual.ParamName);
        }

        [Test()]
        public void MovePiece_InvalidMove_False()
        {
            var target = new CheckersBoard(8);

            // Horizontal move.
            var move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(1, 0) }, new CheckersSquare(3, 0));
            ClassicAssert.IsFalse(target.MovePiece(move));

            // Vertical move.
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(1, 0) }, new CheckersSquare(1, 2));
            ClassicAssert.IsFalse(target.MovePiece(move));

            // Back move.
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(2, 3) }, new CheckersSquare(1, 2));
            ClassicAssert.IsFalse(target.MovePiece(move));

            // Move to occupied square to right side.
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(1, 2) }, new CheckersSquare(2, 3));
            ClassicAssert.IsTrue(target.MovePiece(move));
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(2, 3) }, new CheckersSquare(3, 4));
            ClassicAssert.IsTrue(target.MovePiece(move));
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(3, 4) }, new CheckersSquare(4, 5)); // Occupied.
            ClassicAssert.IsFalse(target.MovePiece(move));

            // Move to occupied square to left side.
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(7, 2) }, new CheckersSquare(6, 3));
            ClassicAssert.IsTrue(target.MovePiece(move));
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(6, 3) }, new CheckersSquare(5, 4));
            ClassicAssert.IsTrue(target.MovePiece(move));
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(5, 4) }, new CheckersSquare(6, 5)); // Occupied.
            ClassicAssert.IsFalse(target.MovePiece(move));

            // Move more than 1 square not capturing.
            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(1, 2) }, new CheckersSquare(3, 4));
            ClassicAssert.IsFalse(target.MovePiece(move));
        }

        [Test()]
        public void MovePiece_ValidMove_True()
        {
            var target = new CheckersBoard(8);

            // Move to occupied square to right side.
            var move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(3, 2) }, new CheckersSquare(4, 3));
            ClassicAssert.IsTrue(target.MovePiece(move));

            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerTwo) { CurrentSquare = new CheckersSquare(6, 5) }, new CheckersSquare(5, 4));
            ClassicAssert.IsTrue(target.MovePiece(move));

            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerOne) { CurrentSquare = new CheckersSquare(4, 3) }, new CheckersSquare(6, 5));
            ClassicAssert.IsTrue(target.MovePiece(move));

            move = new CheckersMove(new CheckersPiece(CheckersPlayer.PlayerTwo) { CurrentSquare = new CheckersSquare(5, 6) }, new CheckersSquare(7, 4));
            ClassicAssert.IsTrue(target.MovePiece(move));
        }


        [Test()]
        public void CountCatchableByPiece_Null_Exception()
        {
            var target = new CheckersBoard(8);

            var actual = Assert.Catch<ArgumentNullException>(() =>
             {
                 target.CountCatchableByPiece(null);
             });

            ClassicAssert.AreEqual("piece", actual.ParamName);
        }

        [Test()]
        public void CountCatchableByPiece_ThereIsNoEnemyPieceAround_Zero()
        {
            var target = new CheckersBoard(8);

            foreach (var p in target.PlayerOnePieces)
            {
                ClassicAssert.AreEqual(0, target.CountCatchableByPiece(p));
            }

            foreach (var p in target.PlayerTwoPieces)
            {
                ClassicAssert.AreEqual(0, target.CountCatchableByPiece(p));
            }
        }

        [Test()]
        public void CountCatchableByPiece_ThereIsEnemyPieceAroundButCannotBeCaptured_Zero()
        {
            var target = new CheckersBoard(8);
            var piece = target.GetSquare(1, 2).CurrentPiece;
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(piece, target.GetSquare(2, 3))));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));

            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(piece, target.GetSquare(3, 4))));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));

            ClassicAssert.IsFalse(target.MovePiece(new CheckersMove(piece, target.GetSquare(4, 5))));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));
        }

        [Test()]
        public void CountCatchableByPiece_ThereIsEnemyPieceAround_CatchableCount()
        {
            var target = new CheckersBoard(8);
            var piece = target.GetSquare(1, 2).CurrentPiece;
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(piece, target.GetSquare(2, 3))));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));

            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(piece, target.GetSquare(3, 4))));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));

            var enemyPiece = target.GetSquare(4, 5).CurrentPiece;
            ClassicAssert.AreEqual(1, target.CountCatchableByPiece(enemyPiece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));

            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(enemyPiece, target.GetSquare(2, 3))));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece));

            var otherPiece = target.GetSquare(2, 1).CurrentPiece;
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(otherPiece, target.GetSquare(1, 2))));
            ClassicAssert.AreEqual(1, target.CountCatchableByPiece(otherPiece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece));
        }

        [Test()]
        public void CountCatchableByPiece_ThereIsTwoEnemyPieceAround_CatchableCountTwo()
        {
            var target = new CheckersBoard(8);
            var piece = target.GetSquare(3, 2).CurrentPiece;
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));

            var enemyPiece1 = target.GetSquare(6, 5).CurrentPiece;
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece1));
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(enemyPiece1, target.GetSquare(5, 4))));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(piece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece1));
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(enemyPiece1, target.GetSquare(4, 3))));
            ClassicAssert.AreEqual(1, target.CountCatchableByPiece(piece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece1));

            var enemyPiece2 = target.GetSquare(4, 5).CurrentPiece;
            ClassicAssert.AreEqual(1, target.CountCatchableByPiece(piece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece2));
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(enemyPiece2, target.GetSquare(3, 4))));
            ClassicAssert.AreEqual(1, target.CountCatchableByPiece(piece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece2));
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(enemyPiece2, target.GetSquare(2, 3))));
            ClassicAssert.AreEqual(2, target.CountCatchableByPiece(piece));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece2));
            ClassicAssert.AreEqual(0, target.CountCatchableByPiece(enemyPiece1));
        }

        [Test()]
        public void CountPieceChancesToBeCaptured_Null_Exception()
        {
            var target = new CheckersBoard(8);

            var actual = Assert.Catch<ArgumentNullException>(() =>
            {
                target.CountPieceChancesToBeCaptured(null);
            });

            ClassicAssert.AreEqual("piece", actual.ParamName);
        }

        [Test()]
        public void CountPieceChancesToBeCaptured_ThereIsNoEnemyPieceAround_Zero()
        {
            var target = new CheckersBoard(8);

            foreach (var p in target.PlayerOnePieces)
            {
                ClassicAssert.AreEqual(0, target.CountPieceChancesToBeCaptured(p));
            }

            foreach (var p in target.PlayerTwoPieces)
            {
                ClassicAssert.AreEqual(0, target.CountPieceChancesToBeCaptured(p));
            }
        }

        [Test()]
        public void CountPieceChancesToBeCaptured_CanAndCannotCapture_CapturedCount()
        {
            var target = new CheckersBoard(8);
            var piece = target.GetSquare(1, 2).CurrentPiece;
            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(piece, target.GetSquare(2, 3))));
            ClassicAssert.AreEqual(0, target.CountPieceChancesToBeCaptured(piece));

            ClassicAssert.IsTrue(target.MovePiece(new CheckersMove(piece, target.GetSquare(3, 4))));
            ClassicAssert.AreEqual(2, target.CountPieceChancesToBeCaptured(piece));

            var enemyPiece = target.GetSquare(4, 5).CurrentPiece;
            ClassicAssert.AreEqual(0, target.CountPieceChancesToBeCaptured(enemyPiece));

            ClassicAssert.IsFalse(target.MovePiece(new CheckersMove(piece, target.GetSquare(4, 5))));
            ClassicAssert.AreEqual(2, target.CountPieceChancesToBeCaptured(piece));
        }
    }
}

