using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TicTacToeScript : MinigameScript
{
    public Image[] fields;
    
    public Sprite circle;
    public Sprite cross;
    
    public Image connectionLineHorizontal;
    public Image connectionLineDiagonal;
    
    private bool _isCircleTurn = true;  // true = Spieler Kreis, false = Gegner Kreuz
    private int[] _board = new int[9];  // 0 = leer, 1 = Kreis (Spieler), 2 = Kreuz (KI)
    public GameObject tryAgainPanel;
    public Button tryAgainButton;
    public TextMeshProUGUI resultText;
    
    public static bool MinigamePlayed = false;

    private void Start()
    {
        ResetBoard();
        tryAgainPanel.SetActive(false);
        
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
        if (_board[index] != 0) return;

        if (!_isCircleTurn) return;

        Debug.Log("Player sets circle");
        SetSprite("circle", fields[index]);
        _board[index] = 1;
        _isCircleTurn = false;
        
        if (CheckWin(_board, 1))
        {
            EndGame("You Win!");
            MinigamePlayed = true;
            return;
        }

        if (!IsMovesLeft(_board))
        {
            EndGame("Draw");
            return;
        }


        if (!CheckWin(_board, 1)) 
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
            _board[bestMove] = 2;
            _isCircleTurn = true;
        }
        
        if (CheckWin(_board, 2))
        {
            EndGame("Enemy Wins");
            return;
        }

        if (!IsMovesLeft(_board))
        {
            EndGame("Draw");
            return;
        }
    }
    
    public void ResetBoard()
    {
        // Logik-Board zurücksetzen
        for (int i = 0; i < _board.Length; i++)
        {
            _board[i] = 0;
        }

        // Felder im UI zurücksetzen
        foreach (var field in fields)
        {
            field.sprite = null; // Bild löschen
            field.color = new Color(1f, 1f, 1f, 0f); // unsichtbar machen
            field.GetComponent<Button>().interactable = true; // Button wieder aktivieren
        }

        // Spieler darf wieder anfangen
        _isCircleTurn = true;
        tryAgainPanel.SetActive(false);
    }
    
    private void EndGame(string result)
    {
        Debug.Log("Game Over: " + result);

        // Buttons deaktivieren → keine weiteren Klicks möglich
        foreach (var field in fields)
        {
            field.GetComponent<Button>().interactable = false;
        }

        // Panel einblenden
        tryAgainPanel.SetActive(true);

        // Optional: Ergebnis im Panel anzeigen (falls du z. B. einen Text hast)
        resultText.text = result;
    }

    // --- Minimax & Board Logic ---
    
     /// <summary>
     /// bewertet das Board
     /// </summary>
     /// <param name="board">Kopie des aktuellen Boards</param>
    private int Evaluate(int[] board)
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
            if (board[a] != 0 && board[a] == board[c] && board[c] == board[d])
            {
                if (board[a] == 2) return +10; // Gegner gewinnt
                if (board[a] == 1) return -10; // Spieler gewinnt
            }
        }
        return 0; // kein Gewinner
    }

    /// Überprüft ob noch Züge gemacht werden können; false = unentschieden, true = noch Züge möglich
    private bool IsMovesLeft(int[] board)
    {
        for (int i = 0; i < 9; i++)
            if (board[i] == 0) return true;
        return false;
    }

    /// <summary>
    /// simuliert durch rekursion alle möglichen Züge
    /// </summary>
    /// <param name="board">Kopie des aktuellen Boards</param>
    /// <param name="depth">Tiefe der Rekursion; schnellerer Sieg ist besser</param>
    /// <param name="isMax">true = Gegnerzug --> versucht Wert zu maximieren, false = Spielerzug
    /// --> versucht Wert zu minimieren</param>
    /// <returns>besten möglichen Zug</returns>
    private int Minimax(int[] board, int depth, bool isMax)
    {
        int score = Evaluate(board);

        if (score == 10) return score - depth; // Gegner gewinnt
        if (score == -10) return score + depth; // Spieler gewinnt
        if (!IsMovesLeft(board)) return 0; // Unentschieden

        if (isMax) // Gegner (Kreuz)
        {
            int best = int.MinValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = 2;
                    best = Math.Max(best, Minimax(board, depth + 1, !isMax));
                    board[i] = 0;
                }
            }
            return best;
        }
        else // Spieler (Kreis)
        {
            int best = int.MaxValue;
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = 1;
                    best = Math.Min(best, Minimax(board, depth + 1, !isMax));
                    board[i] = 0;
                }
            }
            return best;
        }
    }

    /// findet den besten Zug und gibt diesen zurück
    private int FindBestMove()
    {
        // Fehlerquote: 20% Chance, dass die KI absichtlich "schlecht" spielt
        float mistakeChance = 0.2f; // 0.0 = perfekt, 1.0 = nur zufällig
    
        // Würfeln ob ein Fehler gemacht werden soll
        if (UnityEngine.Random.value < mistakeChance)
        {
            // Wählt einen zufälligen freien Platz
            var possibleMoves = new System.Collections.Generic.List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (_board[i] == 0) possibleMoves.Add(i);
            }

            if (possibleMoves.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, possibleMoves.Count);
                return possibleMoves[randomIndex];
            }
        }

        // Normalerweise: Minimax → perfekter Zug
        int bestVal = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 9; i++)
        {
            if (_board[i] == 0)
            {
                _board[i] = 2;
                int moveVal = Minimax(_board, 0, false);
                _board[i] = 0;

                if (moveVal > bestVal)
                {
                    bestMove = i;
                    bestVal = moveVal;
                }
            }
        }
        return bestMove;
    }

    // TODO: strich durchziehen, sodass es klar sichtbar ist wer gewonnen hat
    /// <summary>
    /// prüft ob bestimmter Spieler gewonnen hat
    /// </summary>
    /// <param name="board">Kopie des aktuellen Boards</param>
    /// <param name="player">Spieler = 1, Gegner = 2</param>
    /// <returns>true, wenn jemand gewonnen hat</returns>
    private bool CheckWin(int[] board, int player)
    {
        int[,] wins = new int[,]
        {
            {0,1,2}, {3,4,5}, {6,7,8}, 
            {0,3,6}, {1,4,7}, {2,5,8}, 
            {0,4,8}, {2,4,6}
        };

        for (int i = 0; i < wins.GetLength(0); i++)
        {
            if (board[wins[i,0]] == player &&
                board[wins[i,1]] == player &&
                board[wins[i,2]] == player)
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
