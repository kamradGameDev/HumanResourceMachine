using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuLevels : MenuBase
{
    [SerializeField] private ClearTaskCodes clearTaskCodes;

    [SerializeField] private Button[] levelButtons;
    [SerializeField] private GameObject[] levels;

    private void OnEnable()
    {
        LevelEvents.NewStateLevelEvent += CheckNewStateLevel;
        LoadProgressLevels();
    }

    private void OnDisable() =>
        LevelEvents.NewStateLevelEvent -= CheckNewStateLevel;

    private void CheckNewStateLevel(StateLevel newStateLevel)
    {
        if (newStateLevel == StateLevel.comply)
            SavedProgressLevel();

        //сразу после выигрыша уровня перекидываем в это меню но можно и в другое сделать
        if (newStateLevel == StateLevel.menuLevels || newStateLevel == StateLevel.comply)
        {
            //также загружаем сохранёнку и проверяем пройденные уровни
            LoadProgressLevels();
            Open();
        }
        else if (newStateLevel == StateLevel.loading)
            Close();
    }

    public void RestartLevel()
    {
        LevelEvents.NewStateLevel(StateLevel.restart);
        LevelEvents.NewStateLevel(StateLevel.loading);
        MessageScreen.instance.NewMessageScreen("", Color.white);
        LevelManager.instance.NumberTaskCode = 0;
    }

    public void CloseLevel()
    {
        MessageScreen.instance.NewMessageScreen("", Color.white);

        clearTaskCodes.Clear();

        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            levels[LevelManager.instance.IndexLevel].transform.DOScale(Vector3.zero, 0.5f);
        });
        levels[LevelManager.instance.IndexLevel].SetActive(false);
        Open();
    }

    private void SavedProgressLevel()
    {
        PlayerPrefs.SetInt("LevelProgress_" + LevelManager.instance.IndexLevel, 1);
    }

    public void StartLevel(int index)
    {
        var sequence = DOTween.Sequence();

        levels[index].gameObject.SetActive(true);

        sequence.AppendCallback(() =>
        {
            Close();
            levels[index].transform.DOScale(Vector3.one, 0.5f);
        });

        sequence.Play();

        LevelEvents.NewStateLevel(StateLevel.preLoading);
        LevelManager.instance.InitLevel();
    }

    private void LoadProgressLevels()
    {
        List<int> statusProgresLeves = new List<int>();

        for (var i = 0; i < levelButtons.Length; i++)
        {
            int status = PlayerPrefs.GetInt("LevelProgress_" + i);

            statusProgresLeves.Add(status);
        }

        for (var i = 0; i < levelButtons.Length - 1; i++)
            if (statusProgresLeves[i] == 1)
                levelButtons[i + 1].interactable = true;
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }
}
