using UnityEngine;
using TMPro;

public class DroppedItem : MonoBehaviour
{
    [Header("Core")]
    public string itemName = "포션";          // Inspector에서 지정 가능
    public GameObject nameUIPrefab;          // Inspector에서 프리팹 연결
    public float showDistance = 3f;          // UI 표시 범위
    public float pickupMessageTime = 2f;     // 습득 메세지 지속시간

    private GameObject nameUIObj;            // 인스턴스화된 UI 오브젝트
    private TextMeshProUGUI nameText;        // UI 텍스트
    private Transform player;                // 플레이어 Transform

    private bool canPickup = false;          // 플레이어가 범위 내에 있는지
    private bool picked = false;             // 습득했는지


    void Start()
    {
        // Canvas 찾기
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            //Debug.LogError("Canvas가 씬에 없습니다!");
            enabled = false; return;
        }

        // 이름 UI  생성(처음은 비활성화)
        nameUIObj = Instantiate(nameUIPrefab, canvas.transform);
        nameText = nameUIObj.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText != null)
        {
            nameText.text = itemName;
        }
        nameUIObj.SetActive(false);

        // Player 찾기
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            //Debug.LogError("Player 태그가 붙은 오브젝트가 없습니다!");
            enabled = false; return;
        }
        player = playerObj.transform;
    }

    void Update()
    {
        // Null 체크 (방어 코드)
        if (player == null || nameUIObj == null) return;

        // 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);
        //Debug.Log($"[DroppedItem] distance={distance}, showDistance={showDistance}");

        // 거리 이내면 이름 UI 표시, 아니면 숨김
        if (distance <= showDistance)
        {
            nameUIObj.SetActive(true);

            // 아이템 위 살짝 띄워서 표시
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.0f);
            nameUIObj.transform.position = screenPos;
            canPickup = true;

            // 상호작용으로 습득
            if (Input.GetKeyDown(KeyCode.F))
            {
                Pickup();
            }
        }
        else
        {
            nameUIObj.SetActive(false);
            canPickup = false;
        }
    }
    void Pickup()
    {
        if (picked) return; // 중복 방지

        picked = true;

        UIController.Instance.ShowCollectNotification($"{itemName}을(를) 획득했습니다!", pickupMessageTime);

        Destroy(gameObject);
        Destroy(nameUIObj);
    }
}
