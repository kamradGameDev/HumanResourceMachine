using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

internal class JumpAction : CodeTaskBase, IJumpAction
{
    internal override void InitializeTask(int index = 0)
    {
        base.InitializeTask(index);
    }

    internal async override UniTask StartTaskCode(TypeMainCodeTask typeMainCodeTask)
    {
        /*�������� ������ ����� ��������
         * ����� ��������� ������ ���������� ������
         * � ��������� ���� ���� �� ����� �������� ��� ������� ������
         * ��� �� ����� ������� ������� ��������
         * � ������� ��� ���� ������� �������
         * ��� ��� ���� �������� �������
         */

        await UniTask.Delay(100);

        if (LevelManager.instance.StateLevel == StateLevel.process)
        {
            Debug.Log("start loop jump: " + name);

            IJumpAction iJump = this;

            iJump.LoopJump(IndexTask, LevelManager.instance.activeTasks);
            //LevelEvents.NewStateLevel(StateLevel.jump);
        }

        //���������� �� ���� ������� ���� ������
    }
}

internal interface IJumpAction
{
    internal async void LoopJump(int thisCodeIndex, List<CodeTaskBase> activeTasks)
    {
        //LevelEvents.NewStateLevel(StateLevel.jump);

        int startLoopIndex = ++thisCodeIndex;

        Debug.Log($"start loop index: " + thisCodeIndex);

        LevelEvents.NewStateLevel(StateLevel.jump);

        while (LevelManager.instance.StateLevel == StateLevel.jump)
        {
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);

            for (var i = startLoopIndex; i < activeTasks.Count; i++)
                await activeTasks[i].StartTaskCode(TypeMainCodeTask.Jump);
        }
    }

    //������ ������� ��� ������
    //����� �������� ���������� ��������� ������
    //���� �� ������ ������ ������ ������, �������� � ���� ������ � ��� ���������
    //����� ����������� ���� ����� ����� ��� ������
    //��� ��������� �������
    //��� ������� ������ ���������� ������
    //�������� ������� �������� ��������� ������
}
