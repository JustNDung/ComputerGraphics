namespace DefaultNamespace.SpecificRewards.Lever
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Rules/Identify/Wheelbarrow")]
    public class IdentifyWheelbarrowReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "wheelbarrow"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 60,
                message = "Chính xác! Điểm tựa là bánh xe, cánh tay đòn là từ bánh đến tay cầm."
            };
        }
    }
}