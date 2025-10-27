using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Singleton */
    public static GameManager Instance { get; private set; }

    /*GameState*/
    public enum GameState { Idle, MiniGame }
    private GameState currentState = GameState.Idle;

    /*Actions*/
    public Action OnMinigameEnterd { get; set; }
    public Action OnMinigameExited { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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