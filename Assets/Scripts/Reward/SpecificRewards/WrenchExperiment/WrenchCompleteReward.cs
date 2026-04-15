namespace DefaultNamespace.SpecificRewards.WrenchExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Wrench/Complete")]
    public class WrenchCompleteReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "complete"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 80,
                message = "Hoàn thành! Cờ lê dài giúp vặn dễ hơn."
            };
        }
    }
}