using UnityEngine;

public class Difficulties : MonoBehaviour
{
    public static Difficulty tutorial = new Difficulty
    {
        name = "Tutorial",
        boardCount = 50,
        maxBoardSize = 14,
        percentageEmpty = 0.75f,
        diagonals = false
    };
    public static Difficulty easy = new Difficulty
    {
        name = "Easy",
        boardCount = 100,
        maxBoardSize = 14,
        percentageEmpty = 0.80f,
        diagonals = false
    };
    public static Difficulty medium = new Difficulty
    {
        name = "Medium",
        boardCount = 110,
        maxBoardSize = 14,
        percentageEmpty = 0.85f,
        diagonals = false
    };
    public static Difficulty hard = new Difficulty
    {
        name = "Hard",
        boardCount = 120,
        maxBoardSize = 13,
        percentageEmpty = 0.90f,
        diagonals = false
    };
    public static Difficulty expert = new Difficulty
    {
        name = "Expert",
        boardCount = 130,
        maxBoardSize = 13,
        percentageEmpty = 0.95f,
        diagonals = false
    };
    public static Difficulty easyDiag = new Difficulty
    {
        name = "Easy +",
        boardCount = 60,
        maxBoardSize = 13,
        percentageEmpty = 0.75f,
        diagonals = true
    };
    public static Difficulty mediumDiag = new Difficulty
    {
        name = "Medium +",
        boardCount = 80,
        maxBoardSize = 13,
        percentageEmpty = 0.80f,
        diagonals = true
    };
    public static Difficulty hardDiag = new Difficulty
    {
        name = "Hard +",
        boardCount = 100,
        maxBoardSize = 12,
        percentageEmpty = 0.85f,
        diagonals = true
    };
    public static Difficulty expertDiag = new Difficulty
    {
        name = "Expert +",
        boardCount = 120,
        maxBoardSize = 12,
        percentageEmpty = 0.90f,
        diagonals = true
    };

    public class Difficulty
    {
        public string name;
        public int maxBoardSize;
        public int boardCount;
        public float percentageEmpty;
        public bool diagonals;
    }
}
