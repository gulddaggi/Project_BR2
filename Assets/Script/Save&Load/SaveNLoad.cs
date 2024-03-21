using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        // JSON 변환
        string jsonData = SaveDataToJSON(saveData);

        // 세이브 파일 생성
        CreateSaveFile(jsonData);
    }

    // SaveData 클래스를 JSON으로 변환
    static string SaveDataToJSON(SaveData _saveData)
    {
        string jsonData = JsonUtility.ToJson(_saveData);
        return jsonData;
    }

    // JSON 데이터로 세이브 파일 생성
    static void CreateSaveFile(string _jsonData)
    {
        using (FileStream saveFile = new FileStream(GetPath(), FileMode.Create, FileAccess.Write))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(_jsonData);

            saveFile.Write(bytes, 0, bytes.Length);
        }
    }

    static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "save.abcd");
    }


}
