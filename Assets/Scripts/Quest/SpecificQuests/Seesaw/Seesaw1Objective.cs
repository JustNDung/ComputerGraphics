using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.Seesaw
{
    [CreateAssetMenu(menuName = "Quest/Seesaw/Objectives/Seesaw1 Knowledge")]
    public class Seesaw1Objective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "seesaw"
                   && e.actionId == "seesaw1"
                   && e.success;
        }
    }
}