using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Singleton */
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    /*GameState*/
    public enum GameState { Idle, MiniGame }
    private GameState currentState = GameState.Idle;

    /*Actions*/
    public Action OnMinigameEnterd { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeGameState(GameState state)
    {
        if (currentState == state) return;
        
        currentState = state;

        if (currentState == GameState.Idle)
        {

        }
        else if(currentState == GameState.MiniGame)
        {
            // 플레이어 이동
            // 카메라 전환
            OnMinigameEnterd?.Invoke();
        }
    }
}