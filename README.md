# 📄 SpartaMetaverse3D
>	본 문서는 Camera 전환 및 미니게임 전환 구조를 중점으로 작성한 문서입니다.  
>	튜터 리뷰 후 에셋 라이선스 문제로 레포지토리를 비공개로 전환할 예정입니다.

<br>

## 🎥 카메라 : Cinemachine을 활용한 카메라 구현
### Cinemachine을 사용하게 된 이유

1️⃣	단순 **Camera**와 **Pixel Perfect Camera** 컴포넌트로 프로젝트 진행  
2️⃣	카메라 영역을 제한하는 콜라이더 2개 이상 + 카메라 ZoomIn/ZoomOut 연출을 고려  
3️⃣	자연스러운 화면 연출을 위해 `lerp()`로 콜라이더와 카메라 크기를 보간 처리

* 그러나 카메라 ZoomIn/ZoomOut 과정에서 출력이 매끄럽지 않은 [현상](https://www.notion.so/298d9b031590802ba900e8eb3c417d2c?source=copy_link) 확인
* 기존 **Pixel Perfect Camera**으로는 해결할 수 없음을 확인

➡️ `Cinemachine`을 사용하기로 결정

>	시네머신 사용 이전 코드는 _Legacy/FollowCamera.cs에서 확인할 수 있습니다.

<br>

### 🧊 Prefab
#### CameraAreaController

```bash
CameraAreaController
├─	Cam : 시네머신 가상 카메라 (CinemachineConFiner2D, CinemachinePixelPerfect)
├─	Collision : 카메라 이동 제한 콜라이더 (PolygonCollider2D)
└─	Trigger : 카메라 전환 트리거 (EdgeCollider2D)
```

##### 프리펩으로 구성한 이유
<img width="484" height="130" alt="image" src="https://github.com/user-attachments/assets/44383dce-b9d5-4e94-a28a-12de4e23b55e" />  

카메라의 영역을 제한하는 익스텐션 컴포넌트에서 콜라이더를 인스펙터에서 참조하는 것을 확인.

카메라를 여러대 사용할 경우 콜라이더를 맵에 따로 그리고 참조시키는 것 보다  
이미 참조하고 있는 상태로 프리펩화 후 사용할 때 콜라이더를 수정하는 것이 좋다고 생각.  

하지만 콜라이더는 맵에 해당하는 부분이 아닐까 생각되어 카메라와 함께 프리펩화하는 것이 맞는가에 대한 명확한 결론이 나지 않음.

>	💡 **카메라 이동 제한 콜라이더와 전환 콜라이더의 소속(카메라 영역 vs 맵 영역)에 대한 구조적 판단이 필요합니다...**

<br>

### 📜 Scripts

#### CameraManager
>	전역에서 카메라 전환을 담당

<details>
	<summary>기본적인 카메라 전환 메서드</summary>

```cs
public void ChangeCam(CinemachineVirtualCamera cam)
{
    if (currentCam == cam) return;
    currentCam.Priority = noLive; // noLive = 0
    currentCam = cam;
    currentCam.Priority = onLive; // onLive = 10
}
```
	
</details>

<details>
	<summary>미니 게임 전용 전환 메서드(오버로드)</summary>

```cs

/*Camera*/
private CinemachineVirtualCamera currentCam;

/*Origin*/
private CinemachineVirtualCamera originCam;
private Rect originRect = new(0, 0, 1, 1);

public void ChangeCam(bool isExit = true, CinemachineVirtualCamera cam = null,
						bool orthographic = true, bool is3D = false, Rect rect = default)
{
	// 메서드 매개 변수란을 비운채 호출하면
	// 미니게임에서 나간다는 뜻
    if (isExit)
    {
        currentCam.Priority = noLive;
        currentCam = originCam;
        originCam = null;
        currentCam.Priority = onLive;

        Camera.main.orthographic = orthographic;
        Camera.main.rect = originRect;
        camData.SetRenderer(0);

        return;
    }

	// 미니게임에 진입
    if (currentCam == cam) return;
    currentCam.Priority = noLive; // noLive = 0
    originCam = currentCam;
    currentCam = cam;
    currentCam.Priority = onLive; // onLive = 10

    // camera - projection
    Camera.main.orthographic = orthographic;

    // camera - rendering - renderer
    if (is3D) camData.SetRenderer(1);
    else camData.SetRenderer(0);

    // camera - output - viewportRect
    if (rect != default) Camera.main.rect = rect;
}
```

</details>

#### CameraAreaTrigger

>	Prefab 중 Trigger(EdgeCollider2D)에 컴포넌트 추가.  
>	플레이어를 감지하여 카메라 전환 메서드를 호출.

<details>
	<summary>CameraAreaTrigger</summary>

```cs
public class CameraAreaTrigger : MonoBehaviour
{
    private CinemachineVirtualCamera cam; // 스크립트에서 카메라를 참조
    private void Awake()
    {
		cam.GetComponentInChildren<CinemachineVirtualCamera>();
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			CameraManager.Instance.ChangeCam(cam); // 충돌 발생시 카메라를 매개변수로 넘김
		}
	}
}
```
	
</details>

<br>

## 🎮 미니게임으로 전환

>	메타버스(**2D**) ↔️ TheStack(**3D**)

*	**씬 로드가 아닌 카메라 전환으로 시도**
*	매번 SceneManager로만 전환하다가 작은 프로젝트에서는 한 씬에서 카메라 전환으로 처리하기도 한다 해서 한 번 시도해보고자 함

### Camera

* Rendering - **Renderer**
  *	각 환경에 맞는 렌더러로 전환
* Output - **Viewport Rect**
  *	1920 * 1080 해상도 ↔️ 9 : 16 비율 전환

### Player

* 미니게임 진입시 플레이어 캐릭터는 카메라에 잡히지 않는 곳으로 이동
* 간단히 `PlayerController.enabled`을 조작하여 미니 게임시 플레이어 도트가 움직이는 것을 방지
* 미니게임 종료시 위치, 카메라, 컨트롤러를 원복

<br>

## 👤 Player

플레이어는 F를 누를 뿐인데 어떤 NPC와 상호작용 하는지 알게 하는 방법을 고민  
충돌한 NPC를 저장하고 그 NPC의 Interact()를 호출하도록 코드를 작성  

상태패턴 + 인터페이스까지는 아직 아닌 것 같아 클래스로만 작성

```cs
private BaseNPC currentTarget;

private void Update()
{
    if (currentTarget != null && Input.GetKeyDown(KeyCode.F))
    {
        currentTarget.Interact();
    }
}

/*Trigger*/
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("InteractiveNPC"))
    {
        currentTarget = collision.GetComponent<BaseNPC>();
        currentTarget.Enter();
    }
}

private void OnTriggerExit2D(Collider2D collision)
{
    if (currentTarget != null
        && currentTarget == collision.GetComponent<BaseNPC>())
    {
        currentTarget.Exit();
        currentTarget = null;
    }
}
```

<br>

## ⚙️ GameManager

>	게임 상태에 따라 이벤트를 호출하여 메타버스(**2D**) ↔️ TheStack(**3D**) 전환

이전 텍스트 알피지 프로젝트에서는 프로퍼티 내부에 메서드를 넣어 상태를 감시하게 했다면,  
이번에는 프로퍼티 내부 책임을 최소화하고 상태를 바꾸는 메서드가 상태 감시 + 이벤트 호출까지 하게 만듦.

```cs
public void ChangeGameState(GameState state)
{
    if (currentState == state) return;
    // 중복 할당 방지 위한 할당
    currentState = state;

    if (currentState == GameState.Idle)
    {
		// 플레이어 - 플레이어 위치 원복
        // 더 스택 - 카메라 원복, 더 스택 오브젝트 비활성화
		// NPC - 결과창 활성화
        OnMinigameExited?.Invoke();
    }
    else if(currentState == GameState.MiniGame)
    {
        // 플레이어 - 플레이어 위치 조정
        // 더 스택 - 카메라 조정, 더 스택 오브젝트 활성화
        OnMinigameEnterd?.Invoke();
    }
}
```

<br>

## 💽 DataManager

* TheStack에서 저장/로드하던 데이터를 DataManager로 이관 & 리팩토링
* 저장하는 데이터가 얼마 없고 프로젝트 목적을 생각해 PlayerPrefs를 통해 아래처럼 구현
  
```cs
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
```

<br>

## 💣 트러블 슈팅

### 렌더링 영역 변경에 따라 렌더링되지 않는 영역에 직전 프레임의 잔상이 남는 현상

1️⃣	런타임 중 ViewportRect 를 조정  
2️⃣	렌더링되지 않는 부분에 직전 프레임의 잔상이 남는 현상  
3️⃣	예상했던 결과는 렌더링 되지 않는 부분이 검정색으로 처리되는 것   
4️⃣	보간으로 서서히 변경해보았으나 역시 잔상이 남았음 

#### 해결

*	배경용 카메라를 생성, priority를 메인 카메라보다 후순위로 설정하여 해결

#### 대안

*	ViewportRect를 조정하지 않고, 검은색 UI를 양측에 배치하여 비율이 바뀐 듯한 트릭을 주는 방법을 고려하였음

<br>

>	동영상도 한 번 경험 삼아 넣어 보았습니다. 외부 사이트에 올리고, 다운 링크를 비디오 플레이어의 url에 넣는 방식입니다.  
>	그런데 필요할 때 활성화하니 로드가 느리고, 미리 활성화하니 이미 재생 중이더라고요.
>	더 파고들기엔 중요하지 않은 것에 너무 집중하는 것 같아 마무리하였습니다.

<br>

>	**💌 리뷰 후 private 전환 위해 연락 한 번 주시면 감사하겠습니다!**
