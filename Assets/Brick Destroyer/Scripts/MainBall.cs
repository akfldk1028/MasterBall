using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrickDestroyer
{
	[RequireComponent(typeof(LineRenderer))] // LineRenderer 컴포넌트가 필요함을 명시
	[RequireComponent(typeof(Rigidbody2D))] // Rigidbody2D가 필요함을 명시
	[RequireComponent(typeof(Collider2D))]  // Collider2D가 필요함을 명시
	public class MainBall : MonoBehaviour
	{
		// private const float BOTTOM_Y_POSITION = 3f; // 더 이상 사용하지 않음

		public Rigidbody2D rb;
		private TextMesh numberOfBalls;
		private bool canShoot = true;
		private GameObject lines;
		private bool multipleBalls = false;
		private int ballsShooted = 0;
		private float timer = 0;
		private bool waveEnd = false;
		private float ballStuckTimer = 0;
		private float ballStuckYPos = 0;
		private Text score;
		private Text bestScore;
		private float speedUpTimer = 0;
		private GameObject speedUpButton;
		public GameObject ballNumberText;
		public Material[] ballMaterials;
		GameObject[] ballz;
		public GameObject ballModel;

		// --- LineRenderer 관련 변수 추가 ---
		private LineRenderer lineRenderer;
		[SerializeField] private LayerMask predictionLayerMask; // Raycast가 충돌할 레이어 마스크 (Inspector에서 설정)
		[SerializeField] private float maxRayDistance = 15f; // 예측선의 최대 길이
		[SerializeField] private float reflectionLineLength = 2f; // 반사선의 길이
		// ---------------------------------

		[Header("Plank Interaction")] // Inspector에서 보기 좋게 그룹화
		[SerializeField] private float maxBounceAngle = 75f; // 플랭크에서 튕길 때 최대 각도 (좌우 끝)
		public Plank plank; // 플랭크 참조 (Inspector에서 할당 필요)

		[Header("Auto Launch Settings")]
		[SerializeField] private float plankMoveThreshold = 0.1f; // 플랭크 이동 감지 임계값
		[SerializeField] private float launchForce = 1250f; // 발사 힘 (기존 값 사용)
		[SerializeField] private Vector2 launchDirection = Vector2.up; // 자동 발사 방향 (위쪽)

		private float initialPlankX; // 플랭크 초기 X 좌표 저장용
		private float previousPlankX; // <<<--- 추가: 이전 프레임 플랭크 X 좌표 저장용
		private bool isReadyForPlankLaunch = false;
        private Collider2D ballCollider; // 공 콜라이더 캐싱용
        private Collider2D plankCollider; // 플랭크 콜라이더 캐싱용
        private const float SPAWN_OFFSET_Y = 0.05f; // 플랭크 위 스폰 여유 공간

		void Awake() // Start 대신 Awake 사용 권장 (컴포넌트 초기화)
		{
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.enabled = false; // 초기에는 비활성화
			rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 참조 확인
			ballCollider = GetComponent<Collider2D>(); // 공 콜라이더 가져오기
			if (plank == null)
			{
				Debug.LogError("Plank 참조가 MainBall에 할당되지 않았습니다!", this);
			}
            else
            {
                plankCollider = plank.GetComponent<Collider2D>(); // 플랭크 콜라이더 가져오기
                if (plankCollider == null)
                {
                     Debug.LogError("Plank에 Collider2D가 없습니다!", plank.gameObject);
                }
            }
            if (ballCollider == null)
            {
                 Debug.LogError("MainBall에 Collider2D가 없습니다!", this.gameObject);
            }
		}

		void Start()
		{
			Time.timeScale = 1;
			numberOfBalls = ballNumberText.GetComponent<TextMesh>();
			// score = GameObject.Find("scoreText").GetComponent<Text>();
			// bestScore = GameObject.Find("bestScoreText").GetComponent<Text>();
			// if (PlayerPrefs.GetInt("currentLanguage") == 0)
			// {
			// 	bestScore.text = "BEST\n" + PlayerPrefs.GetInt("bestScore").ToString();
			// }
			// else if (PlayerPrefs.GetInt("currentLanguage") == 1)
			// {
			// 	bestScore.text = "MIGLIORE\n" + PlayerPrefs.GetInt("bestScore").ToString();
			// }
			// else if (PlayerPrefs.GetInt("currentLanguage") == 2)
			// {
			// 	bestScore.text = "REKORD\n" + PlayerPrefs.GetInt("bestScore").ToString();
			// }
			GetComponent<ObjectPlacement>().PlaceNewObjectsOnTheScene();
			// lines = transform.Find("lines").gameObject;
			GameObject.Find("numberOfStarsText").GetComponent<Text>().text = PlayerPrefs.GetInt("numberOfStars").ToString();
			// lines.SetActive(false);
			speedUpButton = GameObject.Find("speedUpButton");
			speedUpButton.SetActive(false);
			BallColorAndSprite();

			// --- 발사 준비 상태 설정 및 위치 초기화 (플랭크 기준) ---
			rb.linearVelocity = Vector2.zero;
			rb.angularVelocity = 0f;

			canShoot = true;
			isReadyForPlankLaunch = true;
			multipleBalls = false; // 첫 발사는 단일 공

			// 공 위치 계산 및 설정
			SetBallPositionAbovePlank();
            previousPlankX = initialPlankX; // <<<--- 추가: previousPlankX 초기화

			if (ballNumberText != null)
			{
				numberOfBalls.color = Color.white;
				numberOfBalls.text = Vars.numberOfBalls + "x";
				UpdateBallNumberTextPosition(); // 텍스트 위치 업데이트 함수 호출
			}
			// --- 발사 준비 상태 설정 끝 ---
		}

		void OnEnable()
		{
			ballNumberText.SetActive(true);
			BallColorAndSprite();
		}

		void OnDisable()
		{
			if (ballNumberText != null)
			{
				ballNumberText.SetActive(false);
			}
			// 게임 종료 또는 비활성화 시 라인 숨기기
			if (lineRenderer != null)
			{
				lineRenderer.enabled = false;
			}
		}

		void Update()
		{
			// --- 플랭크 이동 감지 및 자동 발사 로직 ---
			if (canShoot && plank != null && Vars.canContinue) // isReadyForPlankLaunch 조건을 잠시 빼고 다른 조건 먼저 확인
            {
                 // <<<--- 추가된 로그 1: 발사 조건 확인 시점 및 기본 상태
                 Debug.Log($"[Plank Launch Check] Time: {Time.time:F3}, Ready={isReadyForPlankLaunch}, CanShoot={canShoot}, CanCont={Vars.canContinue}");

                 if (isReadyForPlankLaunch) // 실제 발사 준비가 되었을 때만 이동량 체크
                 {
                    float currentPlankX = plank.transform.position.x;
                    float deltaX = Mathf.Abs(currentPlankX - previousPlankX);

                    // <<<--- 추가된 로그 2: X 좌표 변화량 상세 확인
                     Debug.Log($"[Plank Launch Check] CurrentX: {currentPlankX:F3}, PrevX: {previousPlankX:F3}, DeltaX: {deltaX:F3}, Threshold: {plankMoveThreshold}");

                    if (deltaX > plankMoveThreshold)
                    {
                         Debug.Log($"[Plank Launch Check] Threshold EXCEEDED! Launching ball."); // 발사 직전 로그
                        LaunchBall(launchDirection.normalized);
                        isReadyForPlankLaunch = false;
                    }
                    // else
                    // {
                         // <<<--- 추가된 로그 3: 스레숄드 미달 시 (필요시 주석 해제)
                         Debug.Log($"[Plank Launch Check] Threshold NOT met.");
                    // }
                    
                    previousPlankX = currentPlankX; // 이전 프레임 X 좌표 업데이트
                 }
                 // else
                 // {
                      // <<<--- 추가된 로그 4: 아직 발사 준비 안됨 (필요시 주석 해제)
                      Debug.Log($"[Plank Launch Check] Not ready for plank launch yet (isReadyForPlankLaunch is false).");
                 // }
            }
			// --- 자동 발사 로직 끝 ---

			speedUpTimer += Time.deltaTime;
			if (speedUpTimer >= 5 + (Vars.numberOfBalls / 10))
			{
				speedUpTimer = 0;
				if (Time.timeScale == 1 && !canShoot)
				{
					speedUpButton.SetActive(true);
				}
			}

			if (multipleBalls == true) // 이 로직은 첫 발사 이후에만 작동해야 함
			{
				timer += Time.deltaTime;
				if (timer >= 0.1f)
				{
					timer = 0;
					if (Vars.numberOfBalls > ballsShooted + 1)
					{
						ballz = GameObject.FindGameObjectsWithTag("ball");
						if (ballsShooted < ballz.Length && ballz[ballsShooted] != null)
						{
							// 추가 공 발사 시에도 동일한 초기 발사 방향 또는 저장된 방향 사용?
							// 여기서는 일단 저장된 초기 발사 방향(launchDirection) 사용
							Rigidbody2D ballRb = ballz[ballsShooted].GetComponent<Rigidbody2D>();
							if (ballRb != null)
							{
								// 이미 움직이는 공의 속도/방향을 방해하지 않도록 주의
								// 여기서는 그냥 AddForce를 하지만, 초기 발사처럼 속도 설정이 나을 수도 있음
								ballRb.linearVelocity = Vector2.zero; // 기존 속도 제거 후
								ballRb.AddForce(launchDirection.normalized * launchForce); // 동일 방향/힘으로 발사
							}
							ballsShooted++;
						}
						else { /* ... */ }
					}
					else
					{
						multipleBalls = false;
						ballsShooted = 0;
					}
				}
			}

			// --- 웨이브 리셋 로직 ---
			if (Vars.startMovingTowardsMainBall)
			{
				float step = 35 * Time.deltaTime;
				transform.position = Vector2.MoveTowards(transform.position, new Vector2(Vars.firstBallHitXPos, GetPlankRelativeSpawnY()), step);
				if (Vector3.Distance(transform.position, new Vector2(Vars.firstBallHitXPos, GetPlankRelativeSpawnY())) == 0 && waveEnd == false)
				{
					waveEnd = true;
					Vars.ballsReachedDistance++;
					if (Vars.ballsReachedDistance == Vars.numberOfBalls)
					{
						Vars.ballsReachedDistance = 0;
						Vars.canContinue = true;
						Vars.startMovingTowardsMainBall = false;
						Vars.firstBallHitBottomCollider = false;
						Vars.newWaveOfBricks = true;
					}
				}
			}
			else
			{
				waveEnd = false;
				if (Vars.newWaveOfBricks)
				{
					// ... (기존 공 생성, 오브젝트 이동 등) ...

					// 공 위치 먼저 설정
					SetBallPositionAbovePlank();

					// 그 다음에 오브젝트 배치
					GetComponent<ObjectPlacement>().PlaceNewObjectsOnTheScene(); // 순서 변경

					// 발사 가능 상태 및 플랭크 이동 감지 초기화
					canShoot = true;
					isReadyForPlankLaunch = true;
					// ... (나머지 리셋 로직) ...
					Vars.newWaveOfBricks = false;
				}
			}
			// --- 웨이브 리셋 로직 끝 ---

			// Prevent ball from getting stuck horizontally
			if (!canShoot && rb.linearVelocity.magnitude > 0.1f) // 공이 움직이고 있을 때만 체크
			{
				if (Mathf.Abs(rb.linearVelocity.y) < 0.1f) // Y축 속도가 매우 낮으면
				{
					ballStuckTimer += Time.deltaTime;
					if (ballStuckTimer > 2f) // 2초 이상 Y축 속도가 낮으면
					{
						 Debug.Log("Ball stuck horizontally, adding vertical force.");
						 rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse); // 약간의 수직 힘 가하기
						 ballStuckTimer = 0; // 타이머 리셋
					}
				}
				else
				{
					ballStuckTimer = 0; // Y축 속도가 정상이면 타이머 리셋
				}

				// Prevent ball getting stuck oscillating vertically very close to the same spot
				if(Mathf.Abs(transform.position.y - ballStuckYPos) < 0.05f)
				{
					ballStuckTimer += Time.deltaTime;
					 if (ballStuckTimer > 3f)
					 {
						 Debug.Log("Ball stuck vertically, adding horizontal force.");
						 float randomX = Random.Range(-1f, 1f);
						 rb.AddForce(new Vector2(randomX, 0).normalized * 2f, ForceMode2D.Impulse);
						 ballStuckTimer = 0;
					 }
				} else {
					ballStuckYPos = transform.position.y;
					ballStuckTimer = 0; // Reset timer if Y position changes significantly
				}

			} else {
				 ballStuckTimer = 0; // Reset timer if not moving or can shoot
			}
		}

		// --- 예측선 그리기 메서드 ---
		void DrawPredictionLine(Vector2 direction)
		{
			lineRenderer.positionCount = 2; // 최소 2개의 점 (시작, 끝 또는 첫 충돌점)
			lineRenderer.SetPosition(0, transform.position);

			RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxRayDistance, predictionLayerMask);

			if (hit.collider != null)
			{
				lineRenderer.SetPosition(1, hit.point);

				// 반사 계산
				Vector2 reflectDir = Vector2.Reflect(direction, hit.normal);
				RaycastHit2D reflectHit = Physics2D.Raycast(hit.point + (reflectDir * 0.01f), reflectDir, reflectionLineLength, predictionLayerMask); // 약간 앞에서 시작하여 자기 자신 충돌 방지

				 // Debug Draw Rays (Scene view에서 확인 가능)
				Debug.DrawRay(transform.position, direction * hit.distance, Color.blue); // 초기 레이

				if (reflectHit.collider != null)
				{
					lineRenderer.positionCount = 3;
					lineRenderer.SetPosition(2, reflectHit.point);
					Debug.DrawRay(hit.point, reflectDir * reflectHit.distance, Color.yellow); // 반사 레이 (충돌까지)
				}
				else
				{
					lineRenderer.positionCount = 3;
					lineRenderer.SetPosition(2, (Vector2)hit.point + reflectDir * reflectionLineLength);
					 Debug.DrawRay(hit.point, reflectDir * reflectionLineLength, Color.red); // 반사 레이 (최대 길이까지)
				}
			}
			else
			{
				lineRenderer.SetPosition(1, (Vector2)transform.position + direction * maxRayDistance);
				 Debug.DrawRay(transform.position, direction * maxRayDistance, Color.green); // 초기 레이 (충돌 없음)
			}
		}
		// --------------------------

		void BallColorAndSprite()
		{
			ballModel.GetComponent<MeshRenderer>().material = ballMaterials[0];
			if (PlayerPrefs.GetString("selectedBall").Equals("white"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("green"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(0, 255, 44, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("blue"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(0, 128, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("pink"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(251, 0, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("red"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("yellow"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 0, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("brown"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(136, 84, 11, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("silver"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(192, 192, 192, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("aqua"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(0, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("purple"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(128, 0, 128, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("olive"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(128, 128, 0, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("violet"))
			{
				ballModel.GetComponent<Renderer>().material.color = new Color32(138, 43, 226, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("brick"))
			{
				ballModel.GetComponent<MeshRenderer>().material = ballMaterials[1];
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("tiles"))
			{
				ballModel.GetComponent<MeshRenderer>().material = ballMaterials[2];
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}
			else if (PlayerPrefs.GetString("selectedBall").Equals("metal"))
			{
				ballModel.GetComponent<MeshRenderer>().material = ballMaterials[3];
				ballModel.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
			}

			// Ensure LineRenderer material matches ball if needed, or set a default one
			if (ballMaterials.Length > PlayerPrefs.GetInt("currentBallMaterial"))
			{
				// Optional: Set LineRenderer material based on ball material
				// lineRenderer.material = ballMaterials[PlayerPrefs.GetInt("currentBallMaterial)];
				// Make sure the material is suitable for LineRenderer (e.g., Unlit/Color)
			}
			// Set default LineRenderer properties if needed (width, color) in Inspector or here
			// lineRenderer.startWidth = 0.1f;
			// lineRenderer.endWidth = 0.1f;
			// lineRenderer.startColor = Color.white;
			// lineRenderer.endColor = Color.white;
		}

		public void SpeedUp()
		{
			Time.timeScale = 3;
			speedUpButton.SetActive(false);
		}

		// --- 충돌 처리 메서드 ---
		void OnCollisionEnter2D(Collision2D collision) // 물리 충돌 감지
		{
			// --- 플랭크 충돌 처리 추가 ---
			if (collision.gameObject.CompareTag("Plank")) // Plank 태그를 가진 오브젝트와 충돌 시
			{
				// 충돌 지점과 플랭크 정보 가져오기
				Vector2 hitPoint = collision.contacts[0].point; // 첫 번째 충돌 지점
				Transform plankTransform = collision.transform;  // 충돌한 플랭크의 Transform
				Collider2D plankCollider = collision.collider;   // 충돌한 플랭크의 Collider

				// 플랭크 중심 대비 충돌 지점의 X축 오프셋 계산
				float xOffset = hitPoint.x - plankTransform.position.x;

				// 플랭크 콜라이더 폭의 절반으로 오프셋 정규화 (-1 ~ 1 범위)
				float normalizedOffset = xOffset / (plankCollider.bounds.size.x / 2f);
				// 오프셋 값이 -1과 1 사이를 벗어나지 않도록 제한 (안정성)
				normalizedOffset = Mathf.Clamp(normalizedOffset, -1f, 1f);

				// 정규화된 오프셋을 사용하여 튕겨나갈 각도 계산 (0도는 위쪽)
				float bounceAngle = normalizedOffset * maxBounceAngle; // 중앙은 0도, 끝은 maxBounceAngle

				// 각도를 방향 벡터로 변환 (수학적 계산: Sin이 X, Cos가 Y)
				// 각도를 라디안으로 변환해야 함
				float bounceAngleRad = bounceAngle * Mathf.Deg2Rad;
				Vector2 bounceDirection = new Vector2(Mathf.Sin(bounceAngleRad), Mathf.Cos(bounceAngleRad)).normalized;

				// 현재 속력 유지 또는 최소/최대 속력 설정 (선택적)
				float currentSpeed = rb.linearVelocity.magnitude;
				 // float minSpeed = 10f; // 최소 속력 설정 예시
				 // float targetSpeed = Mathf.Max(currentSpeed, minSpeed); // 현재 속력이 너무 낮으면 최소 속력 사용
				 float targetSpeed = currentSpeed; // 일단 현재 속력 유지

				  if (targetSpeed < 5f) targetSpeed = 10f; // 속력이 너무 느리면 기본 속도로 설정 (Stuck 방지)


				// 계산된 방향과 속력으로 공의 속도 설정
				rb.linearVelocity = bounceDirection * targetSpeed;

				 // Debug.Log($"Hit Plank: Offset={normalizedOffset:F2}, Angle={bounceAngle:F1}, Dir={bounceDirection}");

				 // 플랭크에 맞으면 다시 쏠 수 있게 할지 여부 (게임 규칙에 따라)
				 // canShoot = true; // 만약 플랭크에 맞으면 바로 다음 샷 가능하게 하려면 주석 해제
			}
			// --------------------------
		}
		// --------------------------

		// 기존 OnTriggerEnter2D는 그대로 유지 (BottomBoundary, star, newBall 처리용)
		void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("BottomBoundary")) // 바닥 경계 오브젝트의 태그를 "BottomBoundary"로 설정해야 합니다.
            {
                // 물리적 움직임 즉시 정지
				Debug.Log("BottomBoundary triggered");
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f; // 회전 속도도 0으로

                // 이 웨이브에서 첫 번째로 바닥에 닿은 공인지 확인
                if (!Vars.firstBallHitBottomCollider)
                {
                    Vars.firstBallHitXPos = transform.position.x; // 첫 공의 X 위치 저장
                    Vars.firstBallHitBottomCollider = true;       // 바닥 충돌 플래그 설정
                }

                // 공 수집 로직 시작 (Update 문에서 처리됨)
                Vars.startMovingTowardsMainBall = true;

                // 공이 바닥에 닿으면 더 이상 발사 가능 상태가 아님 (필요시)
                // canShoot = false;

                // 바닥에 닿은 후에는 이 공의 충돌 처리를 더 이상 하지 않도록 비활성화 (선택적)
                // GetComponent<Collider2D>().enabled = false;

                return; // 바닥 충돌 처리 후 다른 충돌 검사 중지
            }

			if (collision.gameObject.CompareTag("star"))
			{
				PlayerPrefs.SetInt("numberOfStars", PlayerPrefs.GetInt("numberOfStars") + 1);
				GameObject.Find("numberOfStarsText").GetComponent<Text>().text = PlayerPrefs.GetInt("numberOfStars").ToString();
				Destroy(collision.gameObject);
				if(PlayerPrefs.GetInt("numberOfStars") >= 1000)
				{
					if(PlayerPrefs.GetInt("collect1000stars") != 1)
					{
						PlayerPrefs.SetInt("collect1000stars", 1);
						GameObject.Find("Canvas").GetComponent<AchievementUnlocked>().enabled = true;
						GameObject.Find("Canvas").GetComponent<AchievementUnlocked>().NameOfTheAchievement("collect 1000 stars");
					}
				}
			}

			 if (collision.gameObject.CompareTag("newBall"))
			{
				Vars.newBalls++;
				Destroy(collision.gameObject);
				if (Vars.newBalls >= 50)
				{
					 if (PlayerPrefs.GetInt("get50balls") != 1)
					 {
						 PlayerPrefs.SetInt("get50balls", 1);
						 GameObject.Find("Canvas").GetComponent<AchievementUnlocked>().enabled = true;
						 GameObject.Find("Canvas").GetComponent<AchievementUnlocked>().NameOfTheAchievement("get 50 balls");
					 }
				}
			}
		}

		// --- 발사 함수 ---
		void LaunchBall(Vector2 direction)
		{
			 if (!canShoot) return; // 이미 발사 중이면 중복 실행 방지

			 Debug.Log($"Launching ball in direction: {direction}");

			 // 기존 AddForce 방식 사용
			 rb.AddForce(direction * launchForce);

			 // 발사 후 상태 변경
			 canShoot = false;
			 numberOfBalls.color = new Color(1, 1, 1, 0); // 공 개수 텍스트 숨기기

			 // 여러 개 공 발사 준비 (첫 발사 이후 시작되도록)
			 if (Vars.numberOfBalls > 1)
			 {
				 multipleBalls = true;
				 ballsShooted = 0; // 카운터 초기화
				 timer = 0f; // 타이머 초기화
			 }
			 else
			 {
				 multipleBalls = false;
			 }

			 // 발사 후 속도 체크 타이머 시작
			 speedUpTimer = 0;
			 speedUpButton.SetActive(false); // 혹시 활성화 되어있었다면 숨기기
		}
		// ---------------

        // 공 위치를 플랭크 바로 위로 설정하는 함수
        void SetBallPositionAbovePlank()
        {
            if (plank != null && ballCollider != null)
            {
                // <<<--- 추가된 로그 1: 함수 호출 시점 및 플랭크 위치 확인
                Debug.Log($"[SetBallPosition] Called at time {Time.time:F3}. Plank position: {plank.transform.position}");

                initialPlankX = plank.transform.position.x;
                previousPlankX = initialPlankX; // <<<--- 추가: 여기서도 previous 초기화

                // --- Y 좌표 계산 방식 변경 (localScale 사용) ---
                // (플랭크 피벗이 중앙이고 기본 높이가 1이라고 가정)
                float plankHalfHeight = (plank.transform.localScale.y * 0.5f);
                float plankTopY = plank.transform.position.y + plankHalfHeight;

                // (공 피벗이 중앙이고 기본 지름이 1이라고 가정)
                // ballCollider.bounds.size.y 대신 transform.localScale.y 사용
                float ballRadiusY = (transform.localScale.y * 0.5f);

                // SPAWN_OFFSET_Y는 작은 값 (예: 0.01f) 유지 권장
                float spawnY = plankTopY + ballRadiusY + SPAWN_OFFSET_Y;
                // -------------------------------------------

                Vector2 targetSpawnPosXY = new Vector2(initialPlankX, spawnY);

                // <<<--- 추가된 로그 2: 계산된 최종 스폰 좌표 확인
                Debug.Log($"[SetBallPosition] Calculated Spawn Position (using scale): {targetSpawnPosXY}");

                // --- Z 좌표 유지 또는 설정 ---
                // 방법 1: 현재 공의 Z 좌표 유지
                // transform.position = new Vector3(targetSpawnPosXY.x, targetSpawnPosXY.y, transform.position.z);

                // 방법 2: 플랭크와 동일한 Z 좌표 사용 (권장)
                 float targetZ = plank.transform.position.z; // 플랭크의 Z 사용
                 transform.position = new Vector3(targetSpawnPosXY.x, targetSpawnPosXY.y, targetZ);

                // 방법 3: 고정 Z 좌표 사용 (예: 0) - 현재 Vector2와 동일하지만 명시적
                // transform.position = new Vector3(targetSpawnPosXY.x, targetSpawnPosXY.y, 0f);
                // --------------------------

                // <<<--- 추가된 로그 3: 실제 설정된 위치 확인
                 Debug.Log($"[SetBallPosition] Ball position actually set to: {transform.position}");
            }
            else
            {
                // ... (기존 에러 처리 로그) ...
                 if(plank == null) Debug.LogWarning("SetBallPositionAbovePlank: Plank is null");
                 // if(plankCollider == null) Debug.LogWarning("SetBallPositionAbovePlank: Plank Collider is null"); // 더 이상 직접 사용 안 함
                 if(ballCollider == null) Debug.LogWarning("SetBallPositionAbovePlank: Ball Collider is null");
            }
        }

        // 공 개수 텍스트 위치 업데이트 함수
        void UpdateBallNumberTextPosition()
        {
             if (ballNumberText != null)
             {
                 // 현재 공 위치 기준으로 텍스트 위치 설정
                 ballNumberText.GetComponentInParent<Transform>().transform.position = new Vector3(transform.position.x, transform.position.y - (ballCollider.bounds.size.y / 2f) - 0.6f, -1f);
             }
        }

        // 플랭크 기준 스폰 Y 좌표 계산 함수 (수집 로직 등에서 사용 가능)
        float GetPlankRelativeSpawnY()
        {
             if (plank != null && ballCollider != null)
            {
                 // (플랭크 피벗이 중앙이고 기본 높이가 1이라고 가정)
                float plankHalfHeight = (plank.transform.localScale.y * 0.5f);
                float plankTopY = plank.transform.position.y + plankHalfHeight;

                // (공 피벗이 중앙이고 기본 지름이 1이라고 가정)
                float ballRadiusY = (transform.localScale.y * 0.5f);

                return plankTopY + ballRadiusY + SPAWN_OFFSET_Y;
            }
            Debug.LogWarning("GetPlankRelativeSpawnY: Cannot calculate position, returning current Y.");
            return transform.position.y;
        }
	}
}