using Reward;

namespace Quest.SpecificQuests.DoorExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Door/Objectives/Open Door Successfully")]
    public class DoorCompleteObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "door"
                   && e.success;
        }
    }
}