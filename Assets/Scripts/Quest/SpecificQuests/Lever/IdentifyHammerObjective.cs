using Reward;

namespace Quest.SpecificQuests.Lever
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Lever Identify/Objectives/Identify Hammer")]
    public class IdentifyHammerObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "hammer"
                   && e.success;
        }
    }
}