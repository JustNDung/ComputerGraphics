

namespace Reward.SpecificRewards.WrenchExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Wrench/IncreaseForce")]
    public class WrenchIncreaseForceReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "increase_force"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 40,
                message = "Lực càng lớn → moment lực càng lớn!"
            };
        }
    }
}