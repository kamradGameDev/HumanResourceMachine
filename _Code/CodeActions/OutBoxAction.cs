using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

internal class OutBoxAction : CodeTaskBase
{
    internal async override UniTask StartTaskCode(TypeMainCodeTask typeMainCodeTask)
    {
        if (LevelManager.instance.StateLevel == StateLevel.fail || LevelManager.instance.StateLevel == StateLevel.comply)
            return;

        Debug.Log("Out In Box Action");

        //���� ����� ������ ������ �� ��������� �������, �� ���� � ������� 7,2, ���� ����� ������ 2 �� ��� ������ �����
        //����� ��� ��������, ���� �� ���������� ��������� ������������, ������ �� ����� �����, ���� ���� ���� ������ �� ���������� ������
        ConveerDown conveerDown = new ConveerDown();
        ITargetMovable targetMovable = LevelEvents.GetTargetMovable(conveerDown);

        await LevelEvents.CallCodeAction(TypeCharacter.player, targetMovable);

        if(LevelEvents.GetPlayerCollectItem())
        {
            Debug.Log("�������� ������� �� �������");
            
            LevelManager.instance.complyTasks.Add(this);
            LevelManager.instance.endTasks.Add(this);

            LevelEvents.SetPlayerCollectItem(null);

            if (LevelManager.instance.complyTasks.Count < LevelManager.instance.activeTasks.Count && 
                LevelManager.instance.activeTasks[LevelManager.instance.complyTasks.Count].GetType().Name != "OutBoxAction")
            {
                Debug.Log("����� ��������� ������");
                await LevelManager.instance.StartNextTask(typeMainCodeTask);
            }
            else
            {
                LevelManager.instance.CheckComplyAllTasks(typeMainCodeTask);
                await LevelManager.instance.LevelRules.CheckRules();
            }
            return;
        }

        if (LevelManager.instance.StateLevel == StateLevel.comply || LevelManager.instance.StateLevel == StateLevel.fail)
            return;

        Debug.Log("������ ��������! �� �� ������ ������ �������� OUTBOX � ������� ������!.");
        

        LevelManager.instance.complyTasks.Add(this);

        if (!LevelEvents.GetPlayerCollectItem())
        {
            LevelManager.instance.endTasks.Add(this);
            MessageScreen.instance.NewMessageScreen("������ ��������! �� �� ������ ������ �������� OUTBOX � ������� ������!.", Color.red);
            LevelEvents.NewStateLevel(StateLevel.fail);
            return;
        }

        //����� ������

        if (LevelManager.instance.CheckComplyAllTasks(typeMainCodeTask))
            return;

        if (!LevelManager.instance.LevelRules.CheckRules().GetAwaiter().GetResult())
            return;
        Debug.Log("end task out box");

        await LevelManager.instance.StartNextTask(typeMainCodeTask);
        
    }
}
