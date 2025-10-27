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
        GameManager.Instance.OnMinigameExited += ExitGame;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnMinigameEnterd -= EnterGame;
        GameManager.Instance.OnMinigameExited -= ExitGame;
    }

    private void Start()
    {
        stack.transform.parent.gameObject.SetActive(false);
    }

    private void EnterGame()
    {
        CameraManager.Instance.ChangeCam(false, cam, false, true, new Rect(0.342f, 0f, 0.316f, 1.0f)); 
        stack.transform.parent.gameObject.SetActive(true);
    }

    private void ExitGame()
    {
        CameraManager.Instance.ChangeCam();
        stack.transform.parent.gameObject.SetActive(false);
    }
}