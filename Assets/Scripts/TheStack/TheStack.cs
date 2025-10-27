using UnityEngine;

public class TheStack : MonoBehaviour
{
    [SerializeField] private GameObject originBlock;

    // ����
    private const float boundSize = 3.5f;
    private const float stackSpeed = 5.0f;
    private const float errorMargin = 0.5f;
    private Vector3 stackBounds = new(boundSize, 1, boundSize);
    private Vector3 targetPos;

    // ���
    private Transform lastBlock; // ���� ���
    private Vector3 prevBlockPosition; // ���� ����� ��ġ

    // ����
    private bool isMovingX = true;
    private const float blockMovingSpeed = 3.5f;
    public const float movingBoundsSize = 3.0f;
    private float blockTransition = 0f;

    // �÷�
    private Color prevColor;
    private Color nextColor;

    // ����
    private int stackCount = 0;
    public int Score { get { return stackCount; } }

    int comboCount = 0;
    public int Combo { get { return comboCount; } }

    private int maxCombo = 0;
    public int MaxCombo { get => maxCombo; }

    // �ְ� ����
    int bestScore = 0;
    public int BestScore { get => bestScore; }

    int bestCombo = 0;
    public int BestCombo { get => bestCombo; }

    // ���� ����
    private bool isGameOver = true;

    private void Start()
    {
        if (originBlock == null)
        {
            Debug.Log("OriginBlock is NULL");
            return;
        }

        bestScore = DataManager.Instance.BestScore;
        bestCombo = DataManager.Instance.BestCombo;

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        SpawnBlock();
        SpawnBlock();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (Input.GetMouseButtonDown(0))
        {

            if (PlaceBlock())
            {
                SpawnBlock();
            }
            else
            {
                Debug.Log("GameOver");
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
                UIManager.Instance.SetScoreUI();
            }
        }
        MoveBlock();
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, stackSpeed * Time.deltaTime);
    }

    public void ReStart()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        isGameOver = false;

        lastBlock = null;
        targetPos = Vector3.zero;
        stackBounds = new(boundSize, 1, boundSize);

        stackCount = -1;
        isMovingX = true;
        blockTransition = 0f;

        comboCount = 0;
        maxCombo = 0;

        prevBlockPosition = Vector3.down;

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        SpawnBlock();
        SpawnBlock();
    }

    private void GameOverEffect()
    {
        int childCount = this.transform.childCount;

        for (int i = 1; i < 20; i++)
        {
            if (childCount < i)
                break;

            GameObject go = this.transform.GetChild(childCount - i).gameObject;

            if (go.name.Equals("Rubble"))
                continue;

            Rigidbody rigid = go.AddComponent<Rigidbody>();

            rigid.AddForce((
                Vector3.up * Random.Range(0, 10f)
                + Vector3.right * (Random.Range(0, 10f) - 5f))
                * 100f
            );
        }
    }
    private void UpdateScore()
    {
        if (bestScore < stackCount)
        {
            Debug.Log("�ְ� ���� ����");
            bestScore = stackCount;
            bestCombo = maxCombo;

            DataManager.Instance.BestScore = bestScore;
            DataManager.Instance.BestCombo = bestCombo;
        }
    }
    private void ComboCheck()
    {
        comboCount++;

        if (comboCount > maxCombo)
            maxCombo = comboCount;

        if ((comboCount % 5) == 0)
        {
            Debug.Log("5Combo Success!");
            stackBounds += new Vector3(0.5f, 0, 0.5f);
            stackBounds.x =
                (stackBounds.x > boundSize) ? boundSize : stackBounds.x;
            stackBounds.z =
                (stackBounds.z > boundSize) ? boundSize : stackBounds.z;
        }
    }

    private bool SpawnBlock()
    {
        // ���� ��� ��ġ ����
        if (lastBlock != null)
            prevBlockPosition = lastBlock.localPosition;

        // ��� ����
        GameObject newBlock;
        Transform newTrans;

        newBlock = Instantiate(originBlock);

        if (newBlock == null)
        {
            Debug.Log("NewBlock Instantiate Failed!");
            return false;
        }
        ColorChange(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = transform;

        newTrans.localPosition = prevBlockPosition + Vector3.up;
        newTrans.localRotation = Quaternion.identity;
        newTrans.localScale = stackBounds;

        stackCount++;

        targetPos = Vector3.down * stackCount;
        blockTransition = 0f;
        isMovingX = !isMovingX;

        lastBlock = newTrans;

        UIManager.Instance.UpdateScore();
        return true;
    }
    private bool PlaceBlock()
    {
        Vector3 lastPosition = lastBlock.localPosition;

        if (isMovingX)
        {
            // ����
            float deltaX = prevBlockPosition.x - lastPosition.x;
            bool isNegativeNum = (deltaX < 0) ? true : false;

            deltaX = Mathf.Abs(deltaX);
            if (deltaX > errorMargin)
            {
                // �޺�
                comboCount = 0;

                stackBounds.x -= deltaX;
                if (stackBounds.x <= 0)
                {
                    return false;
                }

                // ���� ��
                lastBlock.localScale = stackBounds;

                float middle = (prevBlockPosition.x + lastPosition.x) / 2;

                var tempPos = lastBlock.localPosition;
                tempPos.x = middle;
                lastBlock.localPosition = tempPos;

                float rubbleHalfScale = deltaX / 2f;
                CreateRubble(
                    new Vector3(isNegativeNum
                            ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                            : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                        , lastPosition.y
                        , lastPosition.z),
                    new Vector3(deltaX, 1, stackBounds.z)
                );
            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        else
        {
            float deltaZ = prevBlockPosition.z - lastPosition.z;
            bool isNegativeNum = (deltaZ < 0) ? true : false;

            deltaZ = Mathf.Abs(deltaZ);
            if (deltaZ > errorMargin)
            {
                comboCount = 0;

                stackBounds.z -= deltaZ;
                if (stackBounds.z <= 0)
                {
                    return false;
                }

                // ���� ��
                lastBlock.localScale = stackBounds;

                float middle = (prevBlockPosition.z + lastPosition.z) / 2;

                var tempPos = lastBlock.localPosition;
                tempPos.z = middle;
                lastBlock.localPosition = tempPos;

                float rubbleHalfScale = deltaZ / 2f;

                CreateRubble(
                    new Vector3(
                        lastPosition.x
                        , lastPosition.y
                        , isNegativeNum
                            ? lastPosition.z + stackBounds.y / 2 + rubbleHalfScale
                            : lastPosition.z - stackBounds.y / 2 - rubbleHalfScale),
                    new Vector3(stackBounds.x, 1, deltaZ)
                );
            }
            else
            {
                ComboCheck();
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }

        return true;
    }
    private void MoveBlock()
    {
        blockTransition += blockMovingSpeed * Time.deltaTime;
        float movePosition = Mathf.PingPong(blockTransition, boundSize) - boundSize / 2;

        if (isMovingX)
        {
            lastBlock.localPosition = new Vector3(
                                                movePosition * movingBoundsSize,
                                                lastBlock.localPosition.y,
                                                lastBlock.localPosition.z
                                                );
        }
        else
        {
            lastBlock.localPosition = new Vector3(
                                                lastBlock.localPosition.x,
                                                lastBlock.localPosition.y,
                                                -movePosition * movingBoundsSize
                                                );
        }
    }
    private void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = transform;

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }
    private void ColorChange(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

        Renderer rn = go.GetComponent<Renderer>();

        if (rn == null)
        {
            Debug.Log("Renderer is NULL!");
            return;
        }

        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if (applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }
    private Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }
}