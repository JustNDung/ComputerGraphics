

namespace Reward.SpecificRewards.DoorHandleExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Handle/Complete")]
    public class DoorHandleCompleteReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "complete"  
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 70,
                message = "Hoàn thành! Khoảng cách đến trục quay quyết định hiệu quả của lực."
            };
        }
    }
}