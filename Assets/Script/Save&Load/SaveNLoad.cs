using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveNLoad : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene" && GameManager_JS.Instance.GetLoad())
        {
            GameManager_JS.Instance.LoadEnd();
            Debug.Log("자동 로드 진행");
            Load();
        }
    }


    // 기본 저장 수행. 타이틀에서 첫 게임 시작 시 기본 세이브 데이터를 생성.
    public static void DefaultSave()
    {
        // 매개변수 구성. 모든 데이터 값은 0.
        // upgradeLevelList
        List<int> upgradeLevelList = new List<int>();
        int totalUpgradeContentCount = 6;
        for (int i = 0; i < totalUpgradeContentCount; i++)
        {
            upgradeLevelList.Add(0);
        }
        // NPCEncounterCountList
        List<int> NPCEncounterCountList = new List<int>();
        int totalNPCCount = 2;
        for (int i = 0; i < totalNPCCount; i++)
        {
            NPCEncounterCountList.Add(0);
        }
        // gem
        int gem = 0;
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

    // 일반 저장 수행. 플레이어가 게임 실행 중 마을에서 저장 시 해당 시점까지의 세이브 데이터를 생성.
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
            NPCEncounterCountList.Add(GameManager_JS.Instance.dialogueChecks[i].EncounterCount);
        }
        // gem
        int gem = GameManager_JS.Instance.Gem;
        // dungeonEntryCount
        int dungeonEntryCount = GameManager_JS.Instance.GetTryCount();
        // bossKilledCount
        int bossKilledCount = GameManager_JS.Instance.BossKillCount;

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
        string jsonData = JsonUtility.ToJson(_saveData, true);
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

        Debug.Log("세이브 파일 생성 완료.");
        Debug.Log("경로 : " + GetPath());
    }

    // 저장 경로 반환
    static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "save.json");
    }

    // 불러오기 수행
    public static void Load()
    {
        if (!File.Exists(GetPath()))
        {
            Debug.Log("세이브 파일 없음.");
            return;
        }

        string jsonData = LoadSaveFile();
        SaveData saveData = JSONTOSaveData(jsonData);

        // 데이터 적용
        saveData.ApplyLoadData();

        Debug.Log("불러오기 수행 완료");
    }

    static string LoadSaveFile()
    {
        using (FileStream saveFile = new FileStream(GetPath(), FileMode.Open, FileAccess.Read))
        {
            byte[] bytes = new byte[(int)saveFile.Length];

            saveFile.Read(bytes, 0, (int)saveFile.Length);

            string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
            return jsonData;
        }
    }

    static SaveData JSONTOSaveData(string _saveData)
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(_saveData);
        return saveData;
    }

}
