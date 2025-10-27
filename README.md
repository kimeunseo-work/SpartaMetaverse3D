# 🎥 카메라

* `Cinemachine`을 활용한 카메라 구현
* 처음에는 단순 Camera와 Pixel Perfect Camera 사용
* 보간을 이용하여 콜라이더와 줌인/줌아웃 처리
* 그러나 출력이 매끄럽지 않은 [현상](https://www.notion.so/298d9b031590802ba900e8eb3c417d2c?source=copy_link) 확인
* 기존 카메라 ➡️ 시네머신으로 변경
## **CameraAreaController** prefab
*  **Cam** : 시네머신 가상 카메라
	* `CinemachineConFiner2D`
	* `CinemachinePixelPerfect`
*  **Collision** : 카메라 이동 제한 콜라이더 (PolygonCollider2D)
*  **Trigger** : 카메라 전환 트리거 (EdgeCollider2D)

# 🎮 미니게임으로 전환

* 메타버스(**2D**) ↔️ TheStack(**3D**)
## Camera
* Rendering - **Renderer**
  *	각 환경에 맞는 렌더러로 전환
* Output - Viewport Rect
  *	1920 * 1080 해상도 ↔️ 9 : 16 비율 전환

# 트러블 슈팅

## 렌더링 영역 변경에 따라 렌더링되지 않는 영역에 직전 프레임의 잔상이 남는 현상

미니게임으로 진입하며 ViewportRect를 수정하자 렌더링되지 않는 부분이 검정색 처리되는 것이 아닌 직전 프레임의 잔상이 남는 현상. 보간으로 서서히 변경한 시도도 유효하지 않았음.

### 해결 방법
*	배경용 카메라를 생성, priority를 메인 카메라보다 후순위로 설정하여 해결.
*	이 외에 ViewportRect를 조정하지 않고, 검은색 UI를 양측에 배치하여 비율이 바뀐 듯한 트릭을 주는 방법을 고려.
