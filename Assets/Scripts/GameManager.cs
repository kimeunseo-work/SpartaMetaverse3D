using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Singleton */
    public static GameManager Instance { get; private set; }

    /* Init */
    [SerializeField] public Collider2D InitCollider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}