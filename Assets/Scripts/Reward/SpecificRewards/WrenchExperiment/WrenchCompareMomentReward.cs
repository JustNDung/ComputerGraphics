namespace DefaultNamespace.SpecificRewards.WrenchExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Wrench/CompareMoment")]
    public class WrenchCompareMomentReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "compare_correct";
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 60,
                message = "Bạn đã so sánh đúng! Moment phụ thuộc vào cả lực và khoảng cách."
            };
        }
    }
}