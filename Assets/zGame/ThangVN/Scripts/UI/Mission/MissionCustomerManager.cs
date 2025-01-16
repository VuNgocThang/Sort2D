using DG.Tweening;
using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionCustomerManager : MonoBehaviour
{
    public Customer customerPrefab;

    public CustomerMissionData data;
    public List<Customer> listCustomers;
    public Transform nContent;
    public TextMeshProUGUI txtQuantityCustomer;
    public Transform nParent;

    public Image imgFillTimer;
    public float timer;
    public float currentTimer;
    public Image bg;
    public Sprite nightSprite;
    bool IsChangeBG;
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    public RectTransform movingClock;

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_CHECK_MISSION_COMPLETED, CheckCustomerCompleted);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < listCustomers.Count; i++)
        {
            listCustomers[i].gameObject.SetActive(false);
        }

        listCustomers.Clear();

        //10000 Config Start Level Bonus
        int indexLevelBonus = SaveGame.LevelBonus - 10000;

        txtQuantityCustomer.text = data.listLevelBonus[indexLevelBonus].listCustomers.Count.ToString();

        for (int i = 0; i < data.listLevelBonus[indexLevelBonus].listCustomers.Count; i++)
        {
            DataCustomer dataCustomer = data.listLevelBonus[indexLevelBonus].listCustomers[i];

            Customer customer = Instantiate(customerPrefab, nContent);
            customer.Init(dataCustomer);
            listCustomers.Add(customer);
        }

        currentTimer = 0f;
        timer = data.listLevelBonus[indexLevelBonus].timer;
    }

    void CheckCustomerCompleted(object e)
    {
        int index = 0;
        for (int i = 0; i < listCustomers.Count; i++)
        {
            if (listCustomers[i].IsCompleted() && listCustomers[i].gameObject.activeSelf)
            {
                index = i;
                listCustomers[index].transform.SetAsFirstSibling();
                listCustomers[index].Invoke("ChangeSpriteIfDone", 1f);
                //listCustomers[i].ChangeSpriteIfDone();

                //index = i;
                //Move(index);
            }
        }
    }

    void Move(int index)
    {
        listCustomers[index].transform.SetParent(nParent);
        listCustomers[index].transform.DOLocalMoveX(0f, 1f).SetEase(Ease.InExpo);
    }

    private void Update()
    {
        if (LogicGame.Instance.isLose || LogicGame.Instance.isWin) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            LogicGame.Instance.isWin = true;
            if (SaveGame.LevelBonus < GameConfig.MAX_LEVEL_BONUS)
                SaveGame.LevelBonus++;
            Debug.Log("completed");
            RaiseEventWin();
        }

        if (currentTimer < timer)
            currentTimer += Time.deltaTime;

        imgFillTimer.fillAmount = currentTimer / timer;

        Vector2 newPosition = Vector2.Lerp(startPos, endPos, imgFillTimer.fillAmount);
        movingClock.anchoredPosition = newPosition;

        if (currentTimer > timer / 2 && !IsChangeBG)
        {
            Debug.Log("sprite night");
            bg.sprite = nightSprite;
            IsChangeBG = true;
        }

        int count = CountCustomer();

        txtQuantityCustomer.text = count.ToString();

        if (IsOverTime() || LogicGame.Instance.isLose)
        {
            LogicGame.Instance.isLose = true;
            RaiseEventLose();
            Debug.Log("isoverTime");
        }

        if (IsAllCompleted())
        {
            LogicGame.Instance.isWin = true;
            Debug.Log("completed");
            if (SaveGame.LevelBonus < GameConfig.MAX_LEVEL_BONUS)
                SaveGame.LevelBonus++;
            RaiseEventWin();
        }
    }

    bool IsOverTime()
    {
        return (currentTimer >= timer && !LogicGame.Instance.isWin);
    }

    int CountCustomer()
    {
        int count = 0;
        for (int i = 0; i < listCustomers.Count; i++)
        {
            if (listCustomers[i].IsCompleted()) continue;

            count++;
        }

        return count;
    }

    bool IsAllCompleted()
    {
        bool isAllCompleted = true;

        for (int i = 0; i < listCustomers.Count; i++)
        {
            if (!listCustomers[i].IsCompleted())
            {
                isAllCompleted = false;
            }
        }

        return isAllCompleted;
    }

    void RaiseEventLose()
    {
        PopupLoseMiniGame.Show();
    }

    void RaiseEventWin()
    {
        SaveGame.PlayBonus = false;
        //PopupWinMiniGame.Show();
        StartCoroutine(ShowPopupWin());
    }

    IEnumerator ShowPopupWin()
    {
        yield return new WaitForSeconds(1.5f);
        PopupWinMiniGame.Show();
    }
}
