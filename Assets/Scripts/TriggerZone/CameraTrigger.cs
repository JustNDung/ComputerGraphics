using System.Collections;
using MessageDispatcher.AllMessages;
using Quest;
using Reward;
using TriggerZone;
using UI;
using UnityEngine;
using Unity.Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineCamera experimentCam;
    public CinemachineCamera exploreCam;

    [Header("Cinematic Settings")]
    public float zoomDelay = 0.25f; // delay trước khi zoom
    public MonoBehaviour playerController; // script điều khiển player

    private bool _isPlayerInside = false;
    private bool _isFocused = false;
    private bool _isTransitioning = false;
    private CinemachineBrain brain;
    
    public TriggerZone.ZoneType zoneType;

    private void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    void Update()
    {
        if (_isPlayerInside && !_isTransitioning && Input.GetKeyDown(KeyCode.F))
        {
            if (!_isFocused)
            {
                StartCoroutine(FocusWithDelay());
            }
            else
            {
                ExitCamera();
            }
        }
    }

    IEnumerator FocusWithDelay()
    {
        _isTransitioning = true;

        // 👉 delay nhẹ để tạo cinematic feel
        yield return new WaitForSeconds(zoomDelay);

        // 👉 chuyển camera
        experimentCam.Priority = 20;
        exploreCam.Priority = 10;

        // 👉 khóa player (tuỳ chọn)
        if (playerController != null)
            playerController.enabled = false;
        
        // 👉 lấy thời gian blend thật
        float blendTime = brain.DefaultBlend.Time;
        yield return new WaitForSeconds(blendTime);
        
        _isFocused = true;
        _isTransitioning = false;
        playerController.gameObject.SetActive(false);
        MessageDispatcher.MessageDispatcher.Publish(new UIDisplayMessage(zoneType));

        Debug.Log("Zoom vào thí nghiệm 🎬");
    }

    void ExitCamera()
    {
        MessageDispatcher.MessageDispatcher.Publish(new UIHideMessage(zoneType));
        _isTransitioning = true;

        // 👉 trả camera về explore
        experimentCam.Priority = 10;
        exploreCam.Priority = 20;

        // 👉 mở lại player
        if (playerController != null)
            playerController.enabled = true;

        _isFocused = false;

        // delay nhỏ để tránh spam F
        StartCoroutine(ResetTransition());
        
        Debug.Log("Thoát camera 🎮");
    }

    IEnumerator ResetTransition()
    {
        yield return new WaitForSeconds(0.2f);
        _isTransitioning = false;
        playerController.gameObject.SetActive(true);
        CheckAllQuest();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;

            // TODO: Hiện UI [F] Interact
            Debug.Log("Nhấn F để tương tác");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;

            // Nếu đang focus mà đi ra thì auto thoát
            if (_isFocused)
                ExitCamera();

            // TODO: Ẩn UI
        }
    }

    private void CheckAllQuest()
    {
        switch (zoneType)
        {
            case ZoneType.DoorExperiment:
                DoorQuestCheck();
                break;
            case ZoneType.Seesaw1Experiment:
                Seesaw1QuestCheck();
                break;
            case ZoneType.Seesaw2Experiment:
                Seesaw2QuestCheck();
                break;
            case ZoneType.Seesaw3Experiment:
                Seesaw3QuestCheck();
                break;
            case ZoneType.HammerExperiment:
                HammerQuestCheck();
                break;
            case ZoneType.CrowbarExperiment:
                CrowbarQuestCheck();
                break;
            case ZoneType.WheelExperiment:
                WheelbarrowQuestCheck();
                break;
            case ZoneType.ArmExperiment:
                ArmQuestCheck();
                break;
            case ZoneType.HeadExperiment:
                HeadQuestCheck();
                break;
            case ZoneType.BicycleExperiment:
                BicycleQuestCheck();
                break;
            
        }
    }

    private void DoorQuestCheck()
    {
        RewardEvent doorReward = new RewardEvent
        {
            experimentId = "door",
            actionId = "door_right_answer",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(doorReward);
        }
    }
    
    private void Seesaw1QuestCheck()
    {
        RewardEvent seesaw1 = new RewardEvent
        {
            experimentId = "seesaw",
            actionId = "seesaw1",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(seesaw1);
        }
    }
    
    private void Seesaw2QuestCheck()
    {
        RewardEvent seesaw2 = new RewardEvent
        {
            experimentId = "seesaw",
            actionId = "seesaw2",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(seesaw2);
        }
    }
    
    private void Seesaw3QuestCheck()
    {
        RewardEvent seesaw3 = new RewardEvent
        {
            experimentId = "seesaw",
            actionId = "seesaw3",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(seesaw3);
        }
    }
    
    private void HammerQuestCheck()
    {
        RewardEvent hammer = new RewardEvent
        {
            experimentId = "lever_identify",
            actionId = "hammer",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(hammer);
        }
    }
    
    private void CrowbarQuestCheck()
    {
        RewardEvent crowbar = new RewardEvent
        {
            experimentId = "lever_identify",
            actionId = "crowbar",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(crowbar);
        }
    }
    
    private void WheelbarrowQuestCheck()
    {
        RewardEvent wheelbarrow = new RewardEvent
        {
            experimentId = "lever_identify",
            actionId = "wheelbarrow",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(wheelbarrow);
        }
    }
    
    private void ArmQuestCheck()
    {
        RewardEvent arm = new RewardEvent
        {
            experimentId = "body_lever",
            actionId = "arm_lever",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(arm);
        }
    }
    
    private void HeadQuestCheck()
    {
        RewardEvent head = new RewardEvent
        {
            experimentId = "body_lever",
            actionId = "head_lever",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(head);
        }
    }
    
    private void BicycleQuestCheck()
    {
        RewardEvent bicycle = new RewardEvent
        {
            experimentId = "bicycle",
            actionId = "bicycle_knowledge",
            success = true
        };
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.ProcessEvent(bicycle);
        }
    }
}