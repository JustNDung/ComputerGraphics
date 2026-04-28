using UnityEngine;

namespace Reward.SpecificRewards.RealWorldLever
{
    [CreateAssetMenu(menuName="Reward/Rowing/Correct Answer")]
    public class RowingCorrectReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "real_lever"
                   && e.actionId == "oar_class_1"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 15,
                message = "Correct! Oar is a first-class lever."
            };
        }
    }
}