namespace DefaultNamespace.SpecificRewards.WrenchExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Wrench/ExploreBoth")]
    public class WrenchExploreBothReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "explore_both";
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 50,
                message = "Bạn đã thử cả tăng lực và tăng khoảng cách. Rất tốt!"
            };
        }
    }
}