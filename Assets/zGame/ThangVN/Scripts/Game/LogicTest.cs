using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogicTest : MonoBehaviour
{
    public List<int> lst;

    public List<int> sortedList;

    public int diff;

    private void Start()
    {
        diff = 3;
        InitList();
        Debug.Log("lst: " + string.Join(", ", lst));
        sortedList = SortList(lst);
        Debug.Log("sorted List:" + string.Join(", ", sortedList));
    }
    void InitList()
    {
        int total = Random.Range(10, 20);
        for (int i = 0; i < total; i++)
        {
            int a = Random.Range(0, 5);
            lst.Add(a);
        }

    }

    static List<int> SortList(List<int> lst)
    {
        Dictionary<int, (int count, int index)> countDict = new Dictionary<int, (int count, int index)>();

        for (int i = 0; i < lst.Count; i++)
        {
            int value = lst[i];
            if (countDict.ContainsKey(value))
            {
                countDict[value] = (countDict[value].count + 1, countDict[value].index);
            }
            else
            {
                countDict[value] = (1, i);
            }
        }

        List<int> sortedList = lst.OrderByDescending(x => (countDict[x].count, -countDict[x].index)).ToList();

        return sortedList;
    }
}
