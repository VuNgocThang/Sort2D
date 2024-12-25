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

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_CHECK_MISSION_COMPLETED, CheckCustomerCompleted);
    }

    private void Start()
    {
        currentTimer = 0f;
        timer = 300f;
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
    }

    void CheckCustomerCompleted(object e)
    {
        int index = 0;
        for (int i = 0; i < listCustomers.Count; i++)
        {
            if (listCustomers[i].IsCompleted())
            {
                listCustomers[i].ChangeSpriteIfDone();

                index = i;
                Move(index);
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

        if (currentTimer < timer)
            currentTimer += Time.deltaTime;

        imgFillTimer.fillAmount = currentTimer / timer;
    }

}
