using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 저장 데이터 클래스. 인스턴스화 시 저장 데이터 변수를 매개변수로 받아 멤버 변수에 대입
[System.Serializable]
public class SaveData
{
    // 저장 데이터 변수
    // 영구 업그레이드 레벨
    [SerializeField]
    private List<int> upgradeLevelList;
    // 각 NPC별 회차당 첫 조우 횟수
    [SerializeField]
    private List<int> NPCEncounterCountList;
    // 재화 잼 보유량
    [SerializeField]
    private int gem;
    // 던전 진입 시도 횟수
    [SerializeField]
    private int dungeonEntryCount;
    // 보스 처치 횟수
    [SerializeField]
    private int bossKilledCount;

    //클래스 생성자
    public SaveData(List<int> _upgradeLevelList, List<int> _NPCEncounterCount, int _gem, int _dungeonEntryCount, int _bossKilledCount)
    {
        upgradeLevelList = new List<int>(_upgradeLevelList);

        NPCEncounterCountList = new List<int>(_NPCEncounterCount);

        gem = _gem;

        dungeonEntryCount = _dungeonEntryCount;

        bossKilledCount = _bossKilledCount;
    }

    // 불러오기 데이터 적용
    public void ApplyLoadData()
    {
        // upgradeLevelList
        EventDBManager.instance.SetLoadedUpgradeLevel(upgradeLevelList);

        // NPCEncounterCountList, gem, dungeonEntryCount, bossKilledCount
        GameManager_JS.Instance.ApplyLoadedData(NPCEncounterCountList, gem, dungeonEntryCount, bossKilledCount);
    }
}
