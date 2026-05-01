using UnityEngine;

namespace Reward.SpecificRewards.RealWorldLever
{
    [CreateAssetMenu(menuName="Reward/Real Lever/Complete Quest")]
    public class RealLeverCompleteReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "Quest"
                   && e.actionId == "everyday_lever"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 30,
                message = "Everyday Lever Challenge Complete!"
            };
        }
    }
}