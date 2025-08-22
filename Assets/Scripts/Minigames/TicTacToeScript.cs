using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicTacToeScript : MinigameScript
{
    public Image[] fields;
    
    public Sprite circle;
    public Sprite cross;
    
    public Image connectionLineHorizontal;
    public Image connectionLineDiagonal;
    
    private bool _isCircleTurn = true;  // true = Spieler Kreis, false = Gegner Kreuz
    private int[] board = new int[9];  // 0 = leer, 1 = Kreis (Spieler), 2 = Kreuz (KI)

    private void Start()
    {
        for (int i = 0; i < fields.Length; i++)
        {
            int index = i;
            Image field = fields[i];
            var button = field.GetComponent<Button>();

            // Feld unsichtbar machen
            field.color = new Color(1f, 1f, 1f, 0f);

            // auf Klick warten
            button.onClick.AddListener(() =>
            {
                HandleFieldClick(index);
            });
        }
    }

    private void HandleFieldClick(int index)
    {
        // Falls Feld schon belegt -> ignorieren
        if (board[index] != 0) return;

        if (!_isCircleTurn) return;

        Debug.Log("Player sets circle");
        SetSprite("circle", fields[index]);
        board[index] = 1;
        _isCircleTurn = false;

        if (!CheckWin(board, 1)) 
        {
            // Statt sofort EnemyMove:
            StartCoroutine(DelayedEnemyMove());
        }
    }
    
    // Methode die automatisch Sprite setzt und image auf sichtbar setzen
    private void SetSprite(string move, Image field)
    {
        if (move == "circle")
        {
            Debug.Log("set sprite circle");
            field.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            field.sprite = circle;
            
        }
        else if (move == "cross")
        {
            // yield return new WaitForSeconds(0.5f);
            Debug.Log("set sprite cross");
            field.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            field.sprite = cross;
        }

        // Button danach deaktivieren
        field.GetComponent<Button>().interactable = false;
    }
    
    
    private void EnemyMove()
    {
        Debug.Log("EnemyMove called");

        int bestMove = FindBestMove();
        if (bestMove != -1)
        {
            SetSprite("cross", fields[bestMove]);
            board[bestMove] = 2;
            _isCircleTurn = true;
        }
    }
    
    // --- Minimax & Board Logic ---
    
     /// <summary>
     /// bewertet das Board
     /// </summary>
     /// <param name="currentBoard">Kopie des aktuellen Boards</param>
    private int Evaluate(int[] currentBoard)
    {
        int[,] wins = new int[,]
        {
            {0,1,2}, {3,4,5}, {6,7,8}, // Reihen
            {0,3,6}, {1,4,7}, {2,5,8}, // Spalten
            {0,4,8}, {2,4,6}           // Diagonalen
        };

        for (int i = 0; i < wins.GetLength(0); i++)
        {
            int a = wins[i, 0], c = wins[i, 1], d = wins[i, 2];
            if (currentBoard[a] != 0 && currentBoard[a] == currentBoard[c] && currentBoard[c] == currentBoard[d])
            {
                if (currentBoard[a] == 2) return +10; // Gegner gewinnt
                if (currentBoard[a] == 1) return -10; // Spieler gewinnt
            }
        }
        return 0; // kein Gewinner
    }

    /// Überprüft ob noch Züge gemacht werden können; false = unentschieden, true = noch Züge möglich
    private bool IsMovesLeft(int[] currentBoard)
    {
        for (int i = 0; i < 9; i++)
            if (currentBoard[i] == 0) return true;
        return false;
    }

    /// <summary>
    /// simuliert durch rekursion alle möglichen Züge
    /// </summary>
    /// <param name="currentBoard">Kopie des aktuellen Boards</param>
    /// <param name="depth">Tiefe der Rekursion; schnellerer Sieg ist besser</param>
    /// <param name="isMax">true = Gegnerzug --> versucht Wert zu maximieren, false = Spielerzug
    /// --> versucht Wert zu minimieren</param>
    /// <returns>besten möglichen Zug</returns>
    private int Minimax(int[] currentBoard, int depth, bool isMax)
    {
        int score = Evaluate(currentBoard);

        if (score == 10) return score - depth; // Gegner gewinnt
        if (score == -10) return score + depth; // Spieler gewinnt
        if (!IsMovesLeft(currentBoard)) return 0; // Unentschieden

        if (isMax) // Gegner (Kreuz)
        {
            int best = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (currentBoard[i] == 0)
                {
                    currentBoard[i] = 2;
                    best = Math.Max(best, Minimax(currentBoard, depth + 1, !isMax));
                    currentBoard[i] = 0;
                }
            }
            return best;
        }
        else // Spieler (Kreis)
        {
            int best = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (currentBoard[i] == 0)
                {
                    currentBoard[i] = 1;
                    best = Math.Min(best, Minimax(currentBoard, depth + 1, !isMax));
                    currentBoard[i] = 0;
                }
            }
            return best;
        }
    }

    /// findet den besten Zug und gibt diesen zurück
    private int FindBestMove()
    {
        int bestVal = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 9; i++)
        {
            if (board[i] == 0)
            {
                board[i] = 2;
                int moveVal = Minimax(board, 0, false);
                board[i] = 0;

                if (moveVal > bestVal)
                {
                    bestMove = i;
                    bestVal = moveVal;
                }
            }
        }
        return bestMove;
    }

    // TODO: strich durchziehen, sodass es klar sichtbar ist, ggf noch panel für unentschieden und start again
    /// <summary>
    /// prüft ob bestimmter Spieler gewonnen hat
    /// </summary>
    /// <param name="b">Kopie des aktuellen Boards</param>
    /// <param name="player">Spieler = 1, Gegner = 2</param>
    /// <returns>true, wenn jemand gewonnen hat</returns>
    private bool CheckWin(int[] b, int player)
    {
        int[,] wins = new int[,]
        {
            {0,1,2}, {3,4,5}, {6,7,8}, 
            {0,3,6}, {1,4,7}, {2,5,8}, 
            {0,4,8}, {2,4,6}
        };

        for (int i = 0; i < wins.GetLength(0); i++)
        {
            if (b[wins[i,0]] == player &&
                b[wins[i,1]] == player &&
                b[wins[i,2]] == player)
            {
                return true;
            }
        }
        return false;
    }
    
    private IEnumerator DelayedEnemyMove()
    {
        yield return new WaitForSeconds(0.5f); // 1 Sekunde warten
        EnemyMove();
    }
}
