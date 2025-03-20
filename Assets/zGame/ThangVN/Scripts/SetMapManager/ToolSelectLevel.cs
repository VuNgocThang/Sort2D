using ntDev;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolSelectLevel : MonoBehaviour
{
    public EasyButton btnOke, btnPlusCoin, btnPlusPigment, btnPlusBooster, btnExitTool, btnAddHeart, btnUI;
    public TMP_InputField inputField;
    public GameObject nUIGame;

    private void Awake()
    {
        btnPlusBooster.OnClick(() =>
        {
            SaveGame.Hammer += 5;
            SaveGame.Swap += 5;
            SaveGame.Refresh += 5;
        });

        btnPlusCoin.OnClick(() => GameManager.AddGold(5000));
        btnPlusPigment.OnClick(() => GameManager.AddPigment(5000));

        btnExitTool.OnClick(() => gameObject.SetActive(false));

        btnAddHeart.OnClick(() => SaveGame.Heart = 5);

        btnUI.OnClick(() =>
        {
            HideOrUnHideUI();
        });
    }

    private void Start()
    {
        btnOke.OnClick(MoveToLevel);
    }

    void MoveToLevel()
    {
        Debug.Log(inputField.text);
        SaveGame.IsShowHammer = true;
        SaveGame.IsShowRefresh = true;
        SaveGame.IsShowSwap = true;
        ManagerEvent.ClearEvent();
        SaveGame.Level = int.Parse(inputField.text) - 1;
        SaveGame.IsDoneTutorial = true;
        SaveGame.IsDoneTutPoint = true;
        LogicGame.Instance.DeleteSaveDataGame();

        SceneManager.LoadScene("SceneGame");
    }

    void HideOrUnHideUI()
    {
        SaveGame.HideUI = !SaveGame.HideUI;

        if (HomeUI.Instance != null)
        {
            HomeUI.Instance.gameObject.SetActive(SaveGame.HideUI);
        }

        if (nUIGame != null)
        {
            nUIGame.SetActive(SaveGame.HideUI);
        }
    }
}