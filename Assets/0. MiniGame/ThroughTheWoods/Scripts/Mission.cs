using UnityEngine;

namespace ThroughTheWoods
{
    [System.Serializable]
    public class Mission
    {
        public string missionName; // Tên nhiệm vụ
        public MonsterType monsterType;
        public int targetKillCount; // Số lượng quái vật cần tiêu diệt
        public int currentKillCount; // Số lượng quái vật đã tiêu diệt
        public int rewardExp; // EXP thưởng
        public int rewardGold; // Vàng thưởng
        public bool isMissionCompleted; // Trạng thái nhiệm vụ

        public Mission(string name, int target, int exp, int gold)
        {
            missionName = name;
            targetKillCount = target;
            currentKillCount = 0;
            rewardExp = exp;
            rewardGold = gold;
            isMissionCompleted = false;
        }

        public void OnMonsterKilled(MonsterType monsterDeadType)
        {
            if (isMissionCompleted || monsterDeadType != monsterType) return;

            currentKillCount++;
            if (currentKillCount >= targetKillCount)
            {
                CompleteMission();
            }
        }

        private void CompleteMission()
        {
            isMissionCompleted = true;
            Debug.Log($"Mission '{missionName}' completed! Reward: {rewardExp} EXP, {rewardGold} Gold");
        }
    }
}
