

namespace Reward.SpecificRewards.DoorHandleExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Rules/Handle/CorrectPosition")]
    public class DoorHandleCorrectPositionReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "far"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 50,
                message = "Chính xác! Tác dụng lực xa trục quay giúp tạo moment lớn hơn."
            };
        }
    }
}