using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System;
using System.Runtime.CompilerServices;

namespace Minesweeper.Models
{
    public class MinesweeperModel
    {
        public int Difficulty { get; set; }
        public int[,] IdButtonState { get; set; } = null;
        public (int, int) BoardSize { get; set; }
        public int BombsTotal { get; set; }

        public MinesweeperModel(int Difficulty)
        {
            this.Difficulty = Difficulty;

            switch (Difficulty)
            {
                case 1:
                    BoardSize = (9, 9);
                    BombsTotal = 10;
                    break;
                case 2:
                    BoardSize = (16, 16);
                    BombsTotal = 40;
                    break;
                case 3:
                    BoardSize = (16, 30);
                    BombsTotal = 99;
                    break;
                default:
                    BoardSize = (0, 0);
                    BombsTotal = 0;
                    break;
            }

            //InitializeNewBoard();
            //PlantBombs(InitializeNewBoard());
            CalculateNearbyFields(PlantBombs(InitializeNewBoard()));
            //GenerateTestBoard();

        }

        public static int[,] HiddenFields(int[,] board)
        {
            int[,] arr2d = new int[board.GetLength(0), board.GetLength(1)];

            for (int i = 0; i < arr2d.GetLength(0); i++)
            {
                for (int j = 0; j < arr2d.GetLength(1); j++)
                {
                    arr2d[i, j] = 10;
                }
            }

            return arr2d;
        }

        //state 0 : bomb
        //state 1-8 : bomb counter in radius of 1
        //state 9 : none

        public int[,] InitializeNewBoard()
        {
            int[,] arr2d = new int[BoardSize.Item1,BoardSize.Item2];

            for (int i = 0; i < arr2d.GetLength(0); i++)
            {
                for (int j = 0; j < arr2d.GetLength(1); j++)
                {
                    arr2d[i, j] = 9;
                }
            }

            //IdButtonState = arr2d;

            return arr2d;
        }

        public int[,] PlantBombs(int[,] clearBoard)
        {
            //int[,] boardWithBombs = new int[BoardSize.Item1, BoardSize.Item2];

            Random rnd = new Random();

            List<int> placeBombFor = new List<int>();

            for (int i = 0; i < BombsTotal; i++)
            {
                var pickRandom = rnd.Next(0, BoardSize.Item1 * BoardSize.Item2);

                while (placeBombFor.Any(x => x == pickRandom))
                {
                    pickRandom = default;
                    pickRandom = rnd.Next(0, BoardSize.Item1 * BoardSize.Item2);
                }

                placeBombFor.Add(pickRandom);

            }

            for (int i = 0; i < clearBoard.GetLength(0); i++)
            {
                for (int j = 0; j < clearBoard.GetLength(1); j++)
                {
                    bool valueFound;

                    try
                    {
                        valueFound = placeBombFor.Any(x => x == i * clearBoard.GetLength(0) + j);
                    }
                    catch
                    {
                        valueFound = placeBombFor.Any(x => x == j);
                    }

                    if (valueFound)
                    {
                        clearBoard[i, j] = 0;
                    }

                }
            }

            //IdButtonState = clearBoard;

            return clearBoard;

        }

        public void CalculateNearbyFields(int[,] boardWithBombs)
        {

            int sum = default;

            for (int i = 0; i < boardWithBombs.GetLength(0); i++)
            {

                for (int j = 0; j < boardWithBombs.GetLength(1); j++)
                {
                    if (boardWithBombs[i, j] != 0)
                    {
                        List<int?> nearbyFields = new List<int?>(8);

                        int? upperLeftCorner = i > 0 && j > 0 ? boardWithBombs[i - 1, j - 1] : null;
                        int? upper = i > 0 ? boardWithBombs[i - 1, j] : null;
                        int? upperRightCorner = i > 0 && j < boardWithBombs.GetLength(1) - 1 ? boardWithBombs[i - 1, j + 1] : null;

                        int? left = j > 0 ? boardWithBombs[i, j - 1] : null;
                        int? right = j < boardWithBombs.GetLength(1) - 1 ? boardWithBombs[i, j + 1] : null;

                        int? bottomLeftCorner = i < boardWithBombs.GetLength(0) - 1 && j > 0 ? boardWithBombs[i + 1, j - 1] : null;
                        int? bottom = i < boardWithBombs.GetLength(0) - 1 ? boardWithBombs[i + 1, j] : null;
                        int? bottomRightCorner = i < boardWithBombs.GetLength(0) - 1 && j < boardWithBombs.GetLength(1) - 1 ? boardWithBombs[i + 1, j + 1] : null;

                        nearbyFields.Add(upperLeftCorner);
                        nearbyFields.Add(upper);
                        nearbyFields.Add(upperRightCorner);
                        nearbyFields.Add(left);
                        nearbyFields.Add(right);
                        nearbyFields.Add(bottomLeftCorner);
                        nearbyFields.Add(bottom);
                        nearbyFields.Add(bottomRightCorner);

                        IEnumerable<int> nonNullInts = nearbyFields
                                        .Where(i => i.HasValue)
                                        .Select(i => i.Value);

                        foreach (var item in nonNullInts)
                        {
                            if (item == 0)
                                sum += 1;
                        }

                        if (sum == 0)
                            boardWithBombs[i, j] = 9;
                        else
                            boardWithBombs[i, j] = sum;

                        sum = default;
                    }
                        

                    
                }
            }


            IdButtonState = boardWithBombs;
        }

        public void GenerateTestBoard()
        {
            int[,] board = new int[9, 9];

            Difficulty = 1;
            BoardSize = (9, 9);
            BombsTotal = 10;

            //row 0
            board[0, 0] = 1;
            board[0, 1] = 0;
            board[0, 2] = 0;
            board[0, 3] = 0;
            board[0, 4] = 0;
            board[0, 5] = 0;
            board[0, 6] = 2;
            board[0, 7] = 0;
            board[0, 8] = 1;

            //row 1
            board[1, 0] = 1;
            board[1, 1] = 3;
            board[1, 2] = 0;
            board[1, 3] = 5;
            board[1, 4] = 4;
            board[1, 5] = 3;
            board[1, 6] = 2;
            board[1, 7] = 1;
            board[1, 8] = 1;

            //row 2
            board[2, 0] = 9;
            board[2, 1] = 1;
            board[2, 2] = 1;
            board[2, 3] = 2;
            board[2, 4] = 0;
            board[2, 5] = 1;
            board[2, 6] = 9;
            board[2, 7] = 9;
            board[2, 8] = 9;

            //row 3
            board[3, 0] = 9;
            board[3, 1] = 9;
            board[3, 2] = 9;
            board[3, 3] = 1;
            board[3, 4] = 1;
            board[3, 5] = 1;
            board[3, 6] = 9;
            board[3, 7] = 9;
            board[3, 8] = 9;

            //row 4
            board[4, 0] = 9;
            board[4, 1] = 9;
            board[4, 2] = 9;
            board[4, 3] = 9;
            board[4, 4] = 9;
            board[4, 5] = 9;
            board[4, 6] = 9;
            board[4, 7] = 9;
            board[4, 8] = 9;

            //row 5
            board[5, 0] = 9;
            board[5, 1] = 9;
            board[5, 2] = 9;
            board[5, 3] = 9;
            board[5, 4] = 9;
            board[5, 5] = 9;
            board[5, 6] = 1;
            board[5, 7] = 1;
            board[5, 8] = 1;

            //row 6
            board[6, 0] = 9;
            board[6, 1] = 9;
            board[6, 2] = 9;
            board[6, 3] = 9;
            board[6, 4] = 9;
            board[6, 5] = 9;
            board[6, 6] = 1;
            board[6, 7] = 0;
            board[6, 8] = 1;

            //row 7
            board[7, 0] = 9;
            board[7, 1] = 9;
            board[7, 2] = 9;
            board[7, 3] = 9;
            board[7, 4] = 1;
            board[7, 5] = 1;
            board[7, 6] = 2;
            board[7, 7] = 1;
            board[7, 8] = 1;

            //row 8
            board[8, 0] = 9;
            board[8, 1] = 9;
            board[8, 2] = 9;
            board[8, 3] = 9;
            board[8, 4] = 1;
            board[8, 5] = 0;
            board[8, 6] = 1;
            board[8, 7] = 9;
            board[8, 8] = 9;

            IdButtonState = board;
        }

    }

}



