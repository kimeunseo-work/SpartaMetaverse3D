using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string KEY_SCORE = "Score";
    private const string KEY_COMBO = "Combo";

    public int BestScore
    {
        get => PlayerPrefs.GetInt(KEY_SCORE, 0);
        set => PlayerPrefs.SetInt(KEY_SCORE, value);
    }
    public int BestCombo
    {
        get => PlayerPrefs.GetInt(KEY_COMBO, 0);
        set => PlayerPrefs.SetInt(KEY_COMBO, value);
    }

    public static DataManager Instance { get; private set; }
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
}