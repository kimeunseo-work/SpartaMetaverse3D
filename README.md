# 🎥 카메라

* `Cinemachine`을 활용한 카메라 구현
* 처음에는 단순 Camera와 Pixel Perfect Camera 사용
* 보간을 이용하여 콜라이더와 줌인/줌아웃 처리
* 그러나 출력이 매끄럽지 않은 [현상] 확인(https://www.notion.so/298d9b031590802ba900e8eb3c417d2c?source=copy_link)
* 기존 카메라 => 시네머신으로 변경
## **CameraAreaController** prefab
*  **Cam** : 시네머신 가상 카메라
	* `CinemachineConFiner2D`
	* `CinemachinePixelPerfect`
*  **Collision** : 카메라 이동 제한 콜라이더 (PolygonCollider2D)
*  **Trigger** : 카메라 전환 트리거 (EdgeCollider2D)
