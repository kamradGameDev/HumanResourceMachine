using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

internal class JumpActionLocation : CodeTaskBase, IJumpAction
{
    [SerializeField] private JumpAction mainJumpAction;

    internal override void InitializeTask(int index = 0)
    {
        //������ ����� ������ ������� ������ ������
        int indexMainJump = mainJumpAction.IndexTask;
        //����� �������� ��� ������ �� ��������� �� 1 ������ ����
        indexTask = indexMainJump + 1;
        transform.parent = mainJumpAction.transform.parent;
        gameObject.SetActive(true);

        LevelManager.instance.activeTasks.Add(this);
        
        base.InitializeTask(indexTask);

        UIManager.instance.SwapActiveCodeActionButtons();

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

        await UniTask.Delay(300);

        if (LevelManager.instance.StateLevel == StateLevel.start)
        {
            Debug.Log("start loop jump: " + name);

            IJumpAction iJump = this;

            iJump.LoopJump(IndexTask, LevelManager.instance.activeTasks);
            //LevelEvents.NewStateLevel(StateLevel.jump);
        }

    }
}
