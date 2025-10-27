public class InteractiveNPC : BaseNPC
{
    public override void EnterGame()
    {
        Exit();

        GameManager.Instance.ChangeGameState(GameManager.GameState.MiniGame);
    }
}