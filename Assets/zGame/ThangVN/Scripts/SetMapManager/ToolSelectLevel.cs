using ntDev;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolSelectLevel : MonoBehaviour
{
    public EasyButton btnOke, btnPlusCoin, btnPlusPigment, btnPlusBooster, btnExitTool, btnAddHeart;
    public TMP_InputField inputField;

    private void Awake()
    {
        btnPlusBooster.OnClick(() =>
        {
            SaveGame.Hammer += 5;
            SaveGame.Swap += 5;
            SaveGame.Refresh += 5;
        });

        btnPlusCoin.OnClick(() => GameManager.AddGold(500));
        btnPlusPigment.OnClick(() => GameManager.AddPigment(500));

        btnExitTool.OnClick(() => gameObject.SetActive(false));

        btnAddHeart.OnClick(() => SaveGame.Heart = 5);
    }

    private void Start()
    {
        btnOke.OnClick(MoveToLevel);
    }

    void MoveToLevel()
    {
        Debug.Log(inputField.text);
        ManagerEvent.ClearEvent();
        SaveGame.Level = int.Parse(inputField.text) - 1;
        SaveGame.IsDoneTutorial = true;
        LogicGame.Instance.saveGameNormal = null;
        PlayerPrefs.DeleteKey(GameConfig.GAMESAVENORMAL);

        SceneManager.LoadScene("SceneGame");
    }
}