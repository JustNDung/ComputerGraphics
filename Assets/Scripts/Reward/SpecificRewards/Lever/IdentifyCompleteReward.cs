namespace DefaultNamespace.SpecificRewards.Lever
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Identify/CompleteAll")]
    public class IdentifyCompleteAllReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "lever_identify"
                   && e.actionId == "complete_all";
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 100,
                message = "Xuất sắc! Bạn đã xác định đúng điểm tựa và cánh tay đòn trong cả 3 trường hợp."
            };
        }
    }
}