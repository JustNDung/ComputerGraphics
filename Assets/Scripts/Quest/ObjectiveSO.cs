
using Reward;
using UnityEngine;

namespace Quest
{
    public abstract class ObjectiveSO : ScriptableObject
    {
        public string description;
        public int targetAmount = 1;

        public abstract bool Match(RewardEvent e);
    }
}