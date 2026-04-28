using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.RealWorldLever
{
    [CreateAssetMenu(menuName="Quest/Real World Lever/Objectives/Fishing Rod")]
    public class FishingRodObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "real_lever"
                   && e.actionId == "fishing_class_3"
                   && e.success;
        }
    }
}