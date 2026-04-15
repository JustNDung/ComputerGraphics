namespace DefaultNamespace.SpecificRewards.Lever
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Identify/Hammer")]
    public class IdentifyHammerReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "hammer"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 60,
                message = "Chuẩn! Búa quay quanh điểm tì, cánh tay đòn kéo dài đến tay cầm."
            };
        }
    }
}