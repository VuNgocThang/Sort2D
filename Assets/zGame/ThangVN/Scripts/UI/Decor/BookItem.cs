using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTitleBook, txtProgress;
    [SerializeField] Image imgIconBook;
    [SerializeField] int indexBook;
    [SerializeField] EasyButton btnSelect;

    private void Awake()
    {
        btnSelect.OnClick(() => ShowBook(indexBook));
    }

    void ShowBook(int index)
    {
        Debug.Log("indexBook: " + index);
        PopupBookItem.Show(index);
    }

    public void Init(int _index)
    {
        indexBook = _index;
    }
}
