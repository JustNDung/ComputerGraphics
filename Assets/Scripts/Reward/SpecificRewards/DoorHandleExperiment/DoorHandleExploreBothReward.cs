

namespace Reward.SpecificRewards.DoorHandleExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Handle/ExploreBoth")]
    public class DoorHandleExploreBothReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "explore_both";
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 30,
                message = "Bạn đã thử cả hai vị trí. So sánh giúp hiểu rõ hơn!"
            };
        }
    }
}