using Reward;

namespace Quest.SpecificQuests.Lever
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Lever Identify/Objectives/Identify Wheelbarrow")]
    public class IdentifyWheelbarrowObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "wheelbarrow"
                   && e.success;
        }
    }
}