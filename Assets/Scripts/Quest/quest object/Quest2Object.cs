using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest2Object : QuestObject
{
    // ����
    #region
    public Quest2Condition questCondition;

    public TextMeshProUGUI questDiscription_line1;
    public TextMeshProUGUI questDiscription_line2;
    public TextMeshProUGUI questDiscription_line3;
    #endregion
    private void Start()
    {
        SetNeedData();
    }

    public override void HandleCondition()
    {
        switch(gameObject.GetComponent<ItemObject>().itemData.itemID)
        {
            case 1:
                questCondition.getItemCount++;

                questManager.AddStrikethroughOnTMP(questDiscription_line1);
                questManager.ConvertColorRedOnTmp(questDiscription_line1);
                break;
            case 2:
                questCondition.getItemCount++;

                questManager.AddStrikethroughOnTMP(questDiscription_line2);
                questManager.ConvertColorRedOnTmp(questDiscription_line2);
                break;
            case 3:
                questCondition.getItemCount++;

                questManager.AddStrikethroughOnTMP(questDiscription_line3);
                questManager.ConvertColorRedOnTmp(questDiscription_line3);
                break;
        }

        CheckClear();
    }

    public override void CheckClear()
    {
        if (questCondition.getItemCount == 3)
            ActionOnClear();
        else
            Debug.Log("����Ʈ ���� �� --- quest2object");
    }

    private void OnDisable()        // ����Ʈ �������� �Ծ����� �� ���� �޼� ó��
    {
        HandleCondition();
    }
}
