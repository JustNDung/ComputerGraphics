using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.BodyLever
{
    [CreateAssetMenu(menuName = "Quest/BodyLever/Objectives/Arm Lever Knowledge")]
    public class ArmLeverObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "body_lever"
                   && e.actionId == "arm_lever"
                   && e.success;
        }
    }
}