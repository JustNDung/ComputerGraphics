using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.BodyLever
{
    [CreateAssetMenu(menuName = "Quest/BodyLever/Objectives/Head Lever Knowledge")]
    public class HeadLeverObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            // Kiểm tra xem ID của hành động người dùng thực hiện 
            // có trùng với ID đáp án đúng đã thiết lập không.
            return e.experimentId == "body_lever" && e.actionId == "head_lever" && e.success;
        }
    }
}