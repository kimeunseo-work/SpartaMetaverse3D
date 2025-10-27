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
    public Action OnMinigameExited { get; set; }

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
        // 중복 할당 방지 위한 할당
        currentState = state;


        if (currentState == GameState.Idle)
        {
            OnMinigameExited?.Invoke();
        }
        else if(currentState == GameState.MiniGame)
        {
            // 플레이어 - 플레이어 위치 조정
            // 더 스택 - 카메라 조정, 오브젝트 활성화
            OnMinigameEnterd?.Invoke();
        }
    }
}