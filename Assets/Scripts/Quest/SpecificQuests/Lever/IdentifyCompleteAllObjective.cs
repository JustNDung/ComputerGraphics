using Reward;

namespace Quest.SpecificQuests.Lever
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Lever Identify/Objectives/Complete All")]
    public class IdentifyCompleteAllObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "complete_all";
        }
    }
}