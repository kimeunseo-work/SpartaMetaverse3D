using UnityEngine;

public class InteractiveNPC : BaseNPC
{
    private GameObject EnterUI;
    private GameObject ExitUI;

    protected override void Awake()
    {
        base.Awake();

        EnterUI = ui.transform.Find("StartPanel").gameObject;
        ExitUI = ui.transform.Find("EndPanel").gameObject;
    }
    private void OnEnable()
    {
        GameManager.Instance.OnMinigameExited += ExitGame;
    }

    protected override void Start()
    {
        base.Start();
        EnterUI.SetActive(false);
        ExitUI.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();
        EnterUI.SetActive(true);
    }

    protected override void EnterGame()
    {
        base.EnterGame();

        EnterUI.SetActive(false);
        GameManager.Instance.ChangeGameState(GameManager.GameState.MiniGame);
    }

    protected override void ExitGame()
    {
        base.ExitGame();

        ExitUI.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnMinigameExited -= ExitGame;
    }
}