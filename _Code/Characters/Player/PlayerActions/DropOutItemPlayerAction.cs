using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Data/CharacterActions/PlayerActions/DropOutItemPlayerAction", fileName = "NewDropOutItemPlayerAction", order = 59)]

class DropOutItemPlayerAction : PlayerActionBase
{
    private ItemBase dropOutItem;
    protected override void End()
    {
        //LevelManager.instance.
    }

    internal override void Init<T>(ref T t)
    {
        dropOutItem = t as ItemBase;
    }

    internal override void Process()
    {
        /*���� ��� ����� ��������� �������, ����� ����� ����� ������ �������� �������� �/��� ������, ����� ����
        ������ � ��� ����� ������� ����� ���� � ������ ������ � ���������� �� ������������� ������ ���������
        �� ���������� ������*/
        Destroy(dropOutItem.gameObject);
        End();
    }
}
