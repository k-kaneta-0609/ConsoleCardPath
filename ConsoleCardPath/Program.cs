using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCardPath
{
    class Program
    {
        class CardInfo { public int Y; public int X; public string Chr; }
        class Position { public int Y; public int X; }

        static void Main(string[] args)
        {
            // 1行目の入力値を格納
            string line1 = Console.ReadLine();
            int h = int.Parse(line1.Split(' ')[0]); //高さ
            int w = int.Parse(line1.Split(' ')[1]); //幅
            int n = int.Parse(line1.Split(' ')[2]); //チェックペア数

            // カードパネルを作成
            List<CardInfo> cardPanel = new();
            for (int y = 0; y <= h + 1; y++)
            {
                for (int x = 0; x <= w + 1; x++)
                {
                    cardPanel.Add(new CardInfo() { Y = y, X = x, Chr = "." });
                }
            }

            // カードパネルにアルファベット等を入力値から設定する
            for (int y = 1; y <= h; y++)
            {
                string line = Console.ReadLine();
                for (int x = 1; x <= w; x++)
                {
                    cardPanel.First(panel => panel.Y == y && panel.X == x).Chr = line.Split(' ')[x - 1];
                }
            }

            // 開始位置と終了位置(チェックペア)を入力行分リストに格納
            List<Position> startList = new();
            List<Position> endList = new();
            foreach (int _ in Enumerable.Range(1, n))
            {
                string[] positions = Console.ReadLine().Split(' ');
                startList.Add(new Position() { Y = int.Parse(positions[0]), X = int.Parse(positions[1]) });
                endList.Add(new Position() { Y = int.Parse(positions[2]), X = int.Parse(positions[3]) });
            }

            // 入力行(チェックペア)毎にルート探索
            List<CardInfo> path = new();
            for (int i = 0; i < n; i++)
            {
                // 開始位置と終了位置のアルファベットチェック
                bool ok = false;
                string startChar = cardPanel.First(panel => panel.X == startList[i].X && panel.Y == startList[i].Y).Chr;
                string endChar = cardPanel.First(panel => panel.X == endList[i].X && panel.Y == endList[i].Y).Chr;
                if (startChar == endChar && startChar != "." && endChar != ".")
                {
                    // 全てのX軸のルートを探索
                    for (int x = 0; x <= w + 1; x++)
                    {
                        path.Clear();
                        AddPathX(x, startList[i].X, startList[i].Y, cardPanel, path);
                        AddPathY(startList[i].Y, endList[i].Y, x, cardPanel, path);
                        AddPathX(x, endList[i].X, endList[i].Y, cardPanel, path);
                        ok = !path.Where(p => p.X != startList[i].X || p.Y != startList[i].Y).
                                   Where(p => p.X != endList[i].X || p.Y != endList[i].Y).
                                   Any(p => p.Chr != ".");
                        if (ok) break;
                    }
                    if (ok == false)
                    {
                        // 全てのY軸のルートを探索
                        for (int y = 0; y <= h + 1; y++)
                        {
                            path.Clear();
                            AddPathY(y, startList[i].Y, startList[i].X, cardPanel, path);
                            AddPathX(startList[i].X, endList[i].X, y, cardPanel, path);
                            AddPathY(y, endList[i].Y, endList[i].X, cardPanel, path);
                            ok = !path.Where(p => p.X != startList[i].X || p.Y != startList[i].Y).
                                       Where(p => p.X != endList[i].X || p.Y != endList[i].Y).
                                       Any(p => p.Chr != ".");
                            if (ok) break;
                        }
                    }
                }

                // 結果出力
                if (ok)
                {
                    Console.WriteLine("yes");
                }
                else
                {
                    Console.WriteLine("no");
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Y軸を固定し、X軸の開始位置と終了位置の線(ルート)を設定
        /// </summary>
        /// <param name="start">X軸の開始位置</param>
        /// <param name="end">X軸の終了位置</param>
        /// <param name="y">Y軸の位置</param>
        /// <param name="cardPanel">カードパネル</param>
        /// <param name="path">ルートを追加</param>
        static void AddPathX(int start, int end, int y, List<CardInfo> cardPanel, List<CardInfo> path)
        {
            int min;
            int max;
            if (start <= end)
            {
                min = start;
                max = end;
            }
            else
            {
                min = end;
                max = start;
            }

            for (int x = min; x <= max; x++)
            {
                if (path.Any(p => p.X == x && p.Y == y) == false)
                {
                    path.Add(new CardInfo() { X = x, Y = y, Chr = cardPanel.First(panel => panel.X == x && panel.Y == y).Chr });
                }
            }
        }

        /// <summary>
        /// X軸を固定し、Y軸の開始位置と終了位置の線(ルート)を設定
        /// </summary>
        /// <param name="start">Y軸の開始位置</param>
        /// <param name="end">Y軸の終了位置</param>
        /// <param name="x">X軸の位置</param>
        /// <param name="cardPanel">カードパネル</param>
        /// <param name="path">ルートを追加</param>
        static void AddPathY(int start, int end, int x, List<CardInfo> cardPanel, List<CardInfo> path)
        {
            int min;
            int max;
            if (start <= end)
            {
                min = start;
                max = end;
            }
            else
            {
                min = end;
                max = start;
            }

            for (int y = min; y <= max; y++)
            {
                if (path.Any(p => p.X == x && p.Y == y) == false)
                {
                    path.Add(new CardInfo() { X = x, Y = y, Chr = cardPanel.First(panel => panel.X == x && panel.Y == y).Chr });
                }
            }
        }
    }
}
