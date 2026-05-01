

namespace Reward.SpecificRewards.Lever
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Identify/Crowbar")]
    public class IdentifyCrowbarReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "crowbar"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 60,
                message = "Đúng! Điểm tựa là điểm tì, cánh tay đòn kéo dài đến tay người."
            };
        }
    }
}