using Cinemachine;
using UnityEngine;

public class TheStackManager : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private TheStack stack;

    private void Awake()
    {
        cam = GetComponentInChildren<CinemachineVirtualCamera>();
        stack = GetComponentInChildren<TheStack>();
    }
    private void OnEnable()
    {
        GameManager.Instance.OnMinigameEnterd += EnterGame;
    }
    private void Start()
    {
        stack.transform.parent.gameObject.SetActive(false);
    }

    private void EnterGame()
    {
        CameraManager.Instance.ChangeCam(cam, false, new Rect(0.342f, 0f, 0.316f, 1.0f), true); 
        stack.transform.parent.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnMinigameEnterd -= EnterGame;
    }
}