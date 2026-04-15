namespace DefaultNamespace.SpecificRewards.WrenchExperiment
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Wrench/LongerDistance")]
    public class WrenchLongerDistanceReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "increase_distance"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 40,
                message = "Khoảng cách càng lớn → moment lực càng lớn!"
            };
        }
    }
}