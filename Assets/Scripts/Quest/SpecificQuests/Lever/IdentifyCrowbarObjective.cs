using Reward;

namespace Quest.SpecificQuests.Lever
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Lever Identify/Objectives/Identify Crowbar")]
    public class IdentifyCrowbarObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "crowbar"
                   && e.success;
        }
    }
}