

namespace Reward.SpecificRewards.DoorHandleExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Handle/WrongPosition")]
    public class DoorHandleWrongPositionReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "near"
                   && !e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 10,
                message = "Gần trục quay → moment nhỏ → khó làm quay hơn."
            };
        }
    }
}