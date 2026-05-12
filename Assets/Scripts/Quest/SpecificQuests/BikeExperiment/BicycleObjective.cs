using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.BikeExperiment
{
    [CreateAssetMenu(menuName = "Quest/Bicycle/Objectives/Bicycle Knowledge")]
    public class BicycleObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "bicycle"
                   && e.actionId == "bicycle_knowledge"
                   && e.success;
        }
    }
}