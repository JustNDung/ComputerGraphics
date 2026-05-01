using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.RealWorldLever
{
    [CreateAssetMenu(menuName="Quest/Real World Lever/Objectives/Complete")]
    public class RealLeverCompleteObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "real_lever"
                   && e.actionId == "complete";
        }
    }
}