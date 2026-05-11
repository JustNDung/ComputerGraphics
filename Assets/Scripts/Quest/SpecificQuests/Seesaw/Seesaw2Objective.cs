using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.Seesaw
{
    [CreateAssetMenu(menuName = "Quest/Seesaw/Objectives/Seesaw2 Knowledge")]
    public class Seesaw2Objective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "seesaw"
                   && e.actionId == "seesaw2"
                   && e.success;
        }
    }
}