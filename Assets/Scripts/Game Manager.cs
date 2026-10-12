using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    enum Cell { Empty, X, O };
    Cell[] board = new Cell[9];
    public TMP_Text[] cellTexts = new TMP_Text[9];
    bool gameOver = false;
    int[][] winLines = new int[][]
    {
        new int[] { 0, 1, 2 },
        new int[] { 3, 4, 5 },
        new int[] { 6, 7, 8 },
        new int[] { 0, 3, 6 },
        new int[] { 1, 4, 7 },
        new int[] { 2, 5, 8 },
        new int[] { 0, 4, 8 },
        new int[] { 2, 4, 6 }
    };
    Cell currentPlayer = Cell.X;
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    bool is1Player = true;
    public GameObject mainMenuPanel;
    public TMP_Text playermodeText;
    public GameObject boardContainer;
    public bool isHumanX = true;

    public void TogglePlayerMode()
    {
        this.is1Player = !is1Player;
        playermodeText.text = is1Player ? "1 Player" : "2 Players";
    }

    public void StartGame()
    {
        isHumanX = Random.Range(0, 2) == 0;
        mainMenuPanel.SetActive(false);
        boardContainer.SetActive(true);
        RestartGame();
    }

    public void CellClicked(int number)
    {
        if (gameOver)
        {
            return;
        }
        if (board[number] != Cell.Empty)
        {
            return;
        }
        board[number] = currentPlayer;
        cellTexts[number].text = currentPlayer == Cell.X ? "X" : "O";
        if (currentPlayer == Cell.X)
        {
            currentPlayer = Cell.O;
        }
        else
        {
            currentPlayer = Cell.X;
        }
        CheckWin();
        if (is1Player && !gameOver && currentPlayer == (isHumanX ? Cell.O : Cell.X))
        {
            ComputerMove();
        }
    }

    public void ComputerMove()
    {
        int index = Random.Range(0, 9);
        Cell computerMark = isHumanX ? Cell.O : Cell.X;
        Cell humanMark = isHumanX ? Cell.X : Cell.O;
        int winMove = FindWinningMove(computerMark);
        int blockMove = FindWinningMove(humanMark);
        if (winMove != -1)
        {
            CellClicked(winMove);
            return;
        }

        if (blockMove != -1)
        {
            CellClicked(blockMove);
            return;
        }

        while (board[index] != Cell.Empty)
        {
            index = Random.Range(0, 9);
        }
        CellClicked(index);
    }

    public void CheckWin()
    {
        foreach (int[] lines in winLines)
        {
            if (board[lines[0]] != Cell.Empty &&
               board[lines[0]] == board[lines[1]] &&
               board[lines[1]] == board[lines[2]])
            {
                gameOver = true;
                gameOverPanel.SetActive(true);
                gameOverText.text = "Player " + board[lines[0]] + " wins";
                return;
            }
        }

        foreach (Cell cell in board)
        {
            if (cell == Cell.Empty)
            {
                return;
            }
        }
        gameOver = true;
        gameOverPanel.SetActive(true);
        gameOverText.text = "It's a draw!";
    }

    public void RestartGame()
    {
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = Cell.Empty;
            cellTexts[i].text = "";
        }
        gameOver = false;
        currentPlayer = Cell.X;
        gameOverPanel.SetActive(false);
        if (is1Player && !isHumanX)
        {
            ComputerMove();
        }
    }

    int FindWinningMove(Cell mark)
    {
        foreach (int[] lines in winLines)
        {
            if (board[lines[0]] == mark && board[lines[1]] == mark && board[lines[2]] == Cell.Empty)
            {
                return lines[2];
            }
            if (board[lines[0]] == mark && board[lines[2]] == mark && board[lines[1]] == Cell.Empty)
            {
                return lines[1];
            }
            if (board[lines[1]] == mark && board[lines[2]] == mark && board[lines[0]] == Cell.Empty)
            {
                return lines[0];
            }
        }
        return -1;
    }

    public void ReturnToMainMenu()
    {
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = Cell.Empty;
            cellTexts[i].text = "";
        }
        gameOver = false;
        currentPlayer = Cell.X;
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
