using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    private int questIndex = 0;
    public int QuestIndex
    {
        get { return questIndex; }
    }
    private string questName = "";
    private int questType = 0;
    private int questLimitMin = 0;
    private int questLimitMax = 0;

    private int questStart = 0;
    private int questStartNpc = 0;
    private int questEnd = 0;
    private int questEndNpc = 0;
    private int questDialogueStart = 0;

    private int questDialogueEnd = 0;
    private int questEndTerms = 0;
    private int questLink = 0;
    private string unlockLev1 = "";
    private string unlockLev2 = "";

    private int fixReward1 = 0;
    private int fixReward2 = 0;
    private int fixReward3 = 0;
    private int rewardGoldBasic = 0;
    private int rewardGoldLev = 0;

    public void InitQuestData(Dictionary<string, object> questData)
    {
        questIndex = int.Parse((questData["Quest_Index"].ToString() != "Null") ? questData["Quest_Index"].ToString() : "-1");
        questName = questData["Quest_Name"].ToString();
        questType = int.Parse((questData["Quest_Type"].ToString() != "Null") ? questData["Quest_Type"].ToString() : "-1");
        questLimitMin = int.Parse((questData["Quest_Limit_Min"].ToString() != "Null") ? questData["Quest_Limit_Min"].ToString() : "-1");
        questLimitMax = int.Parse((questData["Quest_Limit_Max"].ToString() != "Null") ? questData["Quest_Limit_Max"].ToString() : "-1");

        questStart = int.Parse((questData["Quest_Start"].ToString() != "Null") ? questData["Quest_Start"].ToString() : "-1");
        questStartNpc = int.Parse((questData["Quest_Start_NPC"].ToString() != "Null") ? questData["Quest_Start_NPC"].ToString() : "-1");
        questEnd = int.Parse((questData["Quest_End"].ToString() != "Null") ? questData["Quest_End"].ToString() : "-1");
        questEndNpc = int.Parse((questData["Quest_End_NPC"].ToString() != "Null") ? questData["Quest_End_NPC"].ToString() : "-1");
        questDialogueStart = int.Parse((questData["Quest_Dialogue_Start"].ToString() != "Null") ? questData["Quest_Dialogue_Start"].ToString() : "-1");

        questDialogueEnd = int.Parse((questData["Quest_Dialogue_End"].ToString() != "Null") ? questData["Quest_Dialogue_End"].ToString() : "-1");
        questEndTerms = int.Parse((questData["Quest_End_Terms"].ToString() != "Null") ? questData["Quest_End_Terms"].ToString() : "-1");
        questLink = int.Parse((questData["Quest_Link"].ToString() != "Null") ? questData["Quest_Link"].ToString() : "-1");
        unlockLev1 = questData["Unlock_Lev_1"].ToString();
        unlockLev2 = questData["Unlock_Lev_2"].ToString();

        fixReward1 = int.Parse((questData["Fix_Reward_1"].ToString() != "Null") ? questData["Fix_Reward_1"].ToString() : "-1");
        fixReward2 = int.Parse((questData["Fix_Reward_2"].ToString() != "Null") ? questData["Fix_Reward_2"].ToString() : "-1");
        fixReward3 = int.Parse((questData["Fix_Reward_3"].ToString() != "Null") ? questData["Fix_Reward_3"].ToString() : "-1");
        rewardGoldBasic = int.Parse((questData["Reward_Gold_Basic"].ToString() != "Null") ? questData["Reward_Gold_Basic"].ToString() : "-1");
        rewardGoldLev = int.Parse((questData["Reward_Gold_Lev"].ToString() != "Null") ? questData["Reward_Gold_Lev"].ToString() : "-1");
}
    //디버깅용
    public void PrintQuestData()
    {
        Debug.Log(questIndex + " " + questName + " " + questLimitMin + " "
            + questLimitMax + " " + questStart + " " + questStartNpc + " "
            + questEnd + " " + questEndNpc + " " + questDialogueStart + " "
            + questDialogueEnd + " " + questEndTerms + " " + questLink + " "
            + unlockLev1 + " " + unlockLev2 + " " + fixReward1 + " "
            + fixReward2 + " " + fixReward3 + " " + rewardGoldBasic + " " + rewardGoldLev);
    }
}

public class QuestDataBundle : MonoBehaviour
{
    Dictionary<int, QuestData> questDatas = new Dictionary<int, QuestData>();

    private void Start()
    {
        InitQuestDatas();
    }

    //csv파일을 모두 읽어 questDatas변수에 할당 합니다.
    public void InitQuestDatas()
    {
        questDatas.Clear();

        TextAsset questDataCSV = Resources.Load<TextAsset>("DataTable_Quest");
        if (questDataCSV != null)
        {
            List<Dictionary<string, object>> questDataList = CSVAssetReader.Read(questDataCSV);
            for (int i = 0; i < questDataList.Count; i++)
            {
                QuestData newQuestData = new QuestData();
                newQuestData.InitQuestData(questDataList[i]);
                questDatas.Add(newQuestData.QuestIndex, newQuestData);
            }
        }
    }
}
