using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindTarget
{
    public ColorPlate FindTargetRoot(List<ColorPlate> listDataConnect)
    {
        ColorPlate colorResult = null;

        if (listDataConnect.Count < 3)
        {
            Dictionary<ColorPlate, int> countFrozenDictionary = new Dictionary<ColorPlate, int>();
            foreach (ColorPlate c in listDataConnect)
            {
                //Debug.Log(c.name);
                foreach (ColorPlate cl in c.ListConnect)
                {
                    //Debug.Log(cl.countFrozen + " cl ");

                    if (cl.ListValue.Count == 0) continue;
                    if (cl.countFrozen == 0) continue;

                    if (countFrozenDictionary.ContainsKey(c))
                    {
                        countFrozenDictionary[c]++;
                    }
                    else
                    {
                        countFrozenDictionary.Add(c, 1);
                    }
                }

            }

            //int maxCount = 0;
            //bool isSame = true;
            //if (countFrozenDictionary.Count == 1)
            //{
            //    foreach (var obj in countFrozenDictionary)
            //    {
            //        colorResult = obj.Key;
            //    }
            //}
            //else
            //{
            //    if (countFrozenDictionary.ElementAt(0).Value == countFrozenDictionary.ElementAt(1).Value)
            //    {
            //        isSame = true;
            //    }
            //    else isSame = false;
            //}
            //if (!isSame)
            //{
            //    foreach (var obj in countFrozenDictionary)
            //    {
            //        if (obj.Value > maxCount)
            //        {
            //            maxCount = obj.Value;
            //            colorResult = obj.Key;
            //        }
            //    }
            //    return colorResult;
            //}

            foreach (ColorPlate c in listDataConnect)
            {
                if (c.listTypes.Count < 2) continue;

                foreach (ColorPlate n in c.ListConnect)
                {
                    //Debug.Log(c.name + " c test in foreach");
                    if (n.ListValue.Count == 0) continue;

                    if (c.listTypes[c.listTypes.Count - 2].type == n.TopValue)
                    {
                        ColorPlate remainingElement = listDataConnect.FirstOrDefault(cp => cp != c);
                        if (remainingElement != null)
                        {
                            colorResult = remainingElement;
                        }
                        return colorResult;
                    }
                }

            }

            //Debug.Log(listDataConnect[0].name + " default");
            int countArrow = 0;
            foreach (ColorPlate c in listDataConnect)
            {
                if (IsArrow(c))
                {
                    countArrow++;
                }
            }

            if (countArrow == 2)
            {
                if (listDataConnect[0].listTypes.Count > listDataConnect[1].listTypes.Count) colorResult = listDataConnect[1];
                else colorResult = listDataConnect[0];
            }
            else if (countArrow == 1)
            {
                if (IsArrow(listDataConnect[0])) colorResult = listDataConnect[1];
                else colorResult = listDataConnect[0];
            }
            else
            {
                if (listDataConnect[0].listTypes.Count > listDataConnect[1].listTypes.Count) colorResult = listDataConnect[1];
                else colorResult = listDataConnect[0];
            }
        }
        else if (listDataConnect.Count >= 3)
        {
            List<ColorPlate> listCanBeRoot = new List<ColorPlate>();
            listCanBeRoot.AddRange(listDataConnect);

            foreach (ColorPlate c in listDataConnect)
            {
                if (c.listTypes.Count < 2) continue;

                foreach (ColorPlate n in c.CheckNearByCanConnect(/*c*/))
                {
                    if (n.listTypes.Count < 2) continue;

                    if (n.listTypes[n.listTypes.Count - 2].type == c.listTypes[c.listTypes.Count - 2].type)
                    {
                        listCanBeRoot.Remove(c);
                    }
                }
            }
            //Debug.Log(listCanBeRoot.Count + " count after solve");

            if (listCanBeRoot.Count > 0 && listCanBeRoot.Count < listDataConnect.Count)
            {
                for (int i = 0; i < listCanBeRoot.Count; i++)
                {
                    //Debug.Log(listCanBeRoot[i].name + " after solve");
                }

                int maxCount = -1;

                for (int i = 0; i < listCanBeRoot.Count; i++)
                {

                    //Debug.Log(listCanBeRoot[i].name + " _-_ " + listCanBeRoot[i].CountHasSameTopValueInConnect());
                    int count = listCanBeRoot[i].CountHasSameTopValueInConnect();
                    if (count > maxCount)
                    {
                        maxCount = count;
                        colorResult = listCanBeRoot[i];
                    }
                }
            }
            else if (listCanBeRoot.Count == 0)
            {
                int minCount = 5;

                for (int i = 0; i < listDataConnect.Count; i++)
                {
                    //Debug.Log(listDataConnect[i].name + " _-_ " + listDataConnect[i].CountHasSameTopValueInConnect());
                    int count = listDataConnect[i].CountHasSameTopValueInConnect();
                    if (count < minCount)
                    {
                        minCount = count;
                        colorResult = listDataConnect[i];
                    }
                }
            }
            else if (listCanBeRoot.Count == listDataConnect.Count)
            {
                //Debug.Log("case equal");
                int minTypeDiff = 5;

                for (int i = 0; i < listDataConnect.Count; i++)
                {
                    int countDiff = listDataConnect[i].listTypes.Count;
                    //Debug.Log("countDiff: " + countDiff + " at " + i);
                    if (countDiff < minTypeDiff)
                    {
                        minTypeDiff = countDiff;
                        colorResult = listDataConnect[i];
                    }
                }
            }
        }

        return colorResult;
    }

    bool IsArrow(ColorPlate c)
    {
        if (c.status == Status.Left || c.status == Status.Right || c.status == Status.Up || c.status == Status.Down) return true;
        else return false;
    }
}
