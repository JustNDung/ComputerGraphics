namespace DefaultNamespace.SpecificRewards
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Rules/Door/Complete")]
    public class DoorCompleteReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "door" && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 50,
                message = "Bạn đã mở cửa thành công!"
            };
        }
    }
}