using UnityEngine;

namespace Reward.SpecificRewards.RealWorldLever
{
    [CreateAssetMenu(menuName="Reward/Fishing Rod/Correct Answer")]
    public class FishingRodCorrectReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "real_lever"
                   && e.actionId == "fishing_class_3"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 15,
                message = "Correct! Fishing rod is a third-class lever."
            };
        }
    }
}