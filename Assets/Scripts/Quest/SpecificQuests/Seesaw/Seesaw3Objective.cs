using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.Seesaw
{
    [CreateAssetMenu(menuName = "Quest/Seesaw/Objectives/Seesaw3 Knowledge")]
    public class Seesaw3Objective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "seesaw"
                   && e.actionId == "seesaw3"
                   && e.success;
        }
    }
}