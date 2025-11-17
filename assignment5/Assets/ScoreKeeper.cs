using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreKeeper : MonoBehaviour
{
    //look into singleton pattern from dodgeball assignment, should prob use here
    public static ScoreKeeper Singleton;
    private int p1Score = 0;
    private int p2Score = 0;
    private TMP_Text board;

    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Singleton = this;
        board = GetComponent<TMP_Text>();
        timer = 120f;
        Singleton.UpdateBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            timer = 120f;
            p1Score = 0;
            p2Score = 0;
            Singleton.UpdateBoard();
            GameObject.Find("Ball").GetComponent<Ball>().Reset();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().Reset();
            }
        }
        Singleton.UpdateTimer();
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        Singleton.UpdateBoard();
        if (timer <= 0f)
        {
            Singleton.EndGame();
        }
    }

    public void Score(int player)
    {
        if (timer > 0f)
        {
            if (player == 1)
            {
                p1Score++;
            }
            else if (player == 2)
            {
                p2Score++;
            }

            Singleton.UpdateBoard();
        }
    }
    
    private void UpdateBoard()
    {
        string leading_zero = (timer % 60 < 10 ? "0" : "");
        board.text = $"{(int)Mathf.Floor(timer / 60f)}:{leading_zero}{(int)(timer % 60f)}\n{p1Score} - {p2Score}";
    }
    
    private void EndGame()
    {
        string text = "";
        if (p1Score == p2Score)
        {
            text = "DRAW";
        } else if (p1Score > p2Score)
        {
            text = "BLUE WINS";
        } else if (p1Score < p2Score)
        {
            text = "RED WINS";
        }
        board.text = $"{text}\n{p1Score} - {p2Score}";
    }
}
