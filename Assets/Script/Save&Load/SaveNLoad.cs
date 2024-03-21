using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNLoad : MonoBehaviour
{
    public static void Save()
    {
        // 매개변수 구성
        // upgradeLevelList
        List<int> upgradeLevelList = new List<int>();
        int totalUpgradeContentCount = EventDBManager.instance.GetUpgradeDicCount();
        for (int i = 0; i < totalUpgradeContentCount; i++)
        {
            upgradeLevelList.Add(EventDBManager.instance.GetUpgradeItem(i).level);
        }

        // NPCEncounterCountList
        List<int> NPCEncounterCountList = new List<int>();
        int totalNPCCount = GameManager_JS.Instance.dialogueChecks.Length;
        for (int i = 0; i < totalNPCCount; i++)
        {
            NPCEncounterCountList.Add(GameManager_JS.Instance.dialogueChecks[i].Count);
        }

        // gem
        int gem = GameManager_JS.Instance.Gem;

        // dungeonEntryCount
        int dungeonEntryCount = GameManager_JS.Instance.GetTryCount();

        // bossKilledCount
        int bossKilledCount = 0;

        // SaveData 클래스 생성
        SaveData saveData = new SaveData(upgradeLevelList, NPCEncounterCountList, gem, dungeonEntryCount, bossKilledCount);
    }

}
