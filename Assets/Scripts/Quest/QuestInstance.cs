

using System.Collections.Generic;
using Reward;

namespace Quest
{
    public class QuestInstance
    {
        public QuestSO data;

        public Dictionary<ObjectiveSO, int> progress =
            new Dictionary<ObjectiveSO, int>();

        public bool IsCompleted { get; private set; }

        public QuestInstance(QuestSO quest)
        {
            data = quest;

            foreach (var obj in data.objectives)
                progress[obj] = 0;
        }

        public void ProcessEvent(RewardEvent e)
        {
            if (IsCompleted) return;

            foreach (var obj in data.objectives)
            {
                if (obj.Match(e))
                {
                    progress[obj]++;

                    if (progress[obj] > obj.targetAmount)
                        progress[obj] = obj.targetAmount;
                }
            }

            CheckComplete();
        }

        void CheckComplete()
        {
            foreach (var obj in data.objectives)
            {
                if (progress[obj] < obj.targetAmount)
                    return;
            }

            IsCompleted = true;
        }
    }
}