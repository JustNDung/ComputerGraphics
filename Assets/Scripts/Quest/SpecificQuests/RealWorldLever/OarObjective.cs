using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.RealWorldLever
{
    [CreateAssetMenu(menuName="Quest/Real World Lever/Objectives/Oar")]
    public class OarObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "real_lever"
                   && e.actionId == "oar_class_1"
                   && e.success;
        }
    }
}