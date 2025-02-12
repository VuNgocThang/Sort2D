using ntDev;
using System.Collections;
using TMPro;
using UnityEngine;

public class PopupOutOfHeartHome : Popup
{
    [SerializeField] TextMeshProUGUI txtHeart, txtCoin;
    [SerializeField] EasyButton btnBuy, btnClosePopup;
    [SerializeField] Transform nParentSub;

    private void Awake()
    {
        btnBuy.OnClick(BuyHeart);
        btnClosePopup.OnClick(Hide);
    }

    public static async void Show()
    {
        PopupOutOfHeartHome pop = await ManagerPopup.ShowPopup<PopupOutOfHeartHome>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        ManagerPopup.HidePopup<PopupRestart>();
        btnBuy.enabled = true;

        txtCoin.text = SaveGame.Coin.ToString();
    }

    void BuyHeart()
    {
        if (SaveGame.Coin >= 100)
        {
            btnBuy.enabled = false;
            PlayAnimSubGold();
            GameManager.SubGold(100);
            txtCoin.text = SaveGame.Coin.ToString();
            SaveGame.Heart += 1;
            StartCoroutine(LoadGame());
        }
        else
        {
            EasyUI.Toast.Toast.Show("Not enough money!", 1f);
            Debug.Log("Not enough coin");
        }
    }

    public void PlayAnimSubGold()
    {
        GameObject obj = PoolManager.Spawn(ScriptableObjectData.ObjectConfig.GetObject(EnumObject.SUBGOLD));
        obj.transform.SetParent(nParentSub);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        SubGold subBook = obj.GetComponent<SubGold>();
        subBook.Init(100);
        subBook.gameObject.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(0.5f);
        Hide();
    }
}