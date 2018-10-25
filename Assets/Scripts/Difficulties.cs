using UnityEngine;

public class Difficulties : MonoBehaviour
{
    public static Difficulty tutorial = new Difficulty
    {
        name = "Tutorial",
        boardCount = 50,
        width = 12,
        height = 11,
        percentageEmpty = 0.70f,
        diagonals = false
    };
    public static Difficulty easy = new Difficulty
    {
        name = "Easy",
        boardCount = 100,
        width = 12,
        height = 14,
        percentageEmpty = 0.775f,
        diagonals = false
    };
    public static Difficulty medium = new Difficulty
    {
        name = "Medium",
        boardCount = 110,
        width = 11,
        height = 13,
        percentageEmpty = 0.825f,
        diagonals = false
    };
    public static Difficulty hard = new Difficulty
    {
        name = "Hard",
        boardCount = 120,
        width = 11,
        height = 13,
        percentageEmpty = 0.875f,
        diagonals = false
    };
    public static Difficulty expert = new Difficulty
    {
        name = "Expert",
        boardCount = 130,
        width = 11,
        height = 14,
        percentageEmpty = 0.925f,
        diagonals = false
    };
    public static Difficulty easyDiag = new Difficulty
    {
        name = "Easy +",
        boardCount = 60,
        width = 12,
        height = 14,
        percentageEmpty = 0.75f,
        diagonals = true
    };
    public static Difficulty mediumDiag = new Difficulty
    {
        name = "Medium +",
        boardCount = 80,
        width = 11, 
        height = 14,
        percentageEmpty = 0.80f,
        diagonals = true
    };
    public static Difficulty hardDiag = new Difficulty
    {
        name = "Hard +",
        boardCount = 100,
        width = 11,
        height = 13,
        percentageEmpty = 0.85f,
        diagonals = true
    };
    public static Difficulty expertDiag = new Difficulty
    {
        name = "Expert +",
        boardCount = 120,
        width = 11,
        height = 13,
        percentageEmpty = 0.90f,
        diagonals = true
    };

    public class Difficulty
    {
        public string name;
        public int width;
        public int height;
        public int boardCount;
        public float percentageEmpty;
        public bool diagonals;
    }
}
