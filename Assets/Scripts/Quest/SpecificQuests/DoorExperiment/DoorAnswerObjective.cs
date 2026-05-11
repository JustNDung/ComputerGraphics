using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.DoorExperiment
{
    [CreateAssetMenu(menuName = "Quest/Door/Objectives/DoorAnswer")]
    public class DoorAnswerObjective : ObjectiveSO
    {
        [Header("Cài đặt đáp án")]
        public string requiredActionId; // Ví dụ: "answer_c"
        public override bool Match(RewardEvent e)
        {
            // Kiểm tra xem ID của hành động người dùng thực hiện 
            // có trùng với ID đáp án đúng đã thiết lập không.
            return e.experimentId == "door" && e.actionId == requiredActionId && e.success;
        }
    }
}