using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.BomExperiment
{
    [CreateAssetMenu(menuName = "Quest/Bom/Objectives/Bom Knowledge")]
    public class BomObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "bom"
                   && e.actionId == "bom_knowledge"
                   && e.success;
        }
    }
}