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
        // �ߺ� �Ҵ� ���� ���� �Ҵ�
        currentState = state;


        if (currentState == GameState.Idle)
        {
            OnMinigameExited?.Invoke();
        }
        else if(currentState == GameState.MiniGame)
        {
            // �÷��̾� - �÷��̾� ��ġ ����
            // �� ���� - ī�޶� ����, ������Ʈ Ȱ��ȭ
            OnMinigameEnterd?.Invoke();
        }
    }
}