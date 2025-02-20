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
            // độ ưu tiên thứ nhất: check stack thứ 2 có merge được với stack nào bên cạnh không
            List<ColorPlate> listCanBeRoot = new List<ColorPlate>();
            listCanBeRoot.AddRange(listDataConnect);

            foreach (ColorPlate c in listDataConnect)
            {
                if (c.listTypes.Count < 2) continue;

                foreach (ColorPlate n in c.ListConnect)
                {
                    if (n.ListValue.Count == 0 || n.countFrozen != 0) continue;

                    if (listDataConnect.Contains(n)) continue;

                    if (c.listTypes[c.listTypes.Count - 2].type == n.TopValue)
                    {
                        listCanBeRoot.Remove(c);
                    }
                }
            }

            if (listCanBeRoot.Count == 1)
            {
                colorResult = listCanBeRoot[0];
                return colorResult;
            }

            // độ ưu tiên thứ 1.1: gần ô poison, gần nhiều poison
            Dictionary<ColorPlate, int> countPoisonDictionary = new Dictionary<ColorPlate, int>();
            foreach (ColorPlate c in listDataConnect)
            {
                foreach (ColorPlate cl in c.ListConnect)
                {
                    if (cl.status != Status.Poison) continue;

                    if (countPoisonDictionary.ContainsKey(c))
                    {
                        countPoisonDictionary[c]++;
                    }
                    else
                    {
                        countPoisonDictionary.Add(c, 1);
                    }
                }
            }

            if (countPoisonDictionary.Count > 0)
            {
                int maxCount = 0;
                bool isSame = true;
                if (countPoisonDictionary.Count == 1)
                {
                    foreach (var obj in countPoisonDictionary)
                    {
                        colorResult = obj.Key;
                        return colorResult;
                    }
                }
                else
                {
                    if (countPoisonDictionary.ElementAt(0).Value == countPoisonDictionary.ElementAt(1).Value)
                    {
                        isSame = true;
                    }
                    else isSame = false;
                }

                if (!isSame)
                {
                    foreach (var obj in countPoisonDictionary)
                    {
                        if (obj.Value > maxCount)
                        {
                            maxCount = obj.Value;
                            colorResult = obj.Key;
                        }
                    }

                    Debug.Log("colorResult if isSame: " + colorResult);
                    return colorResult;
                }
            }

            // độ ưu tiên thứ 2: gần băng, gần nhiều băng
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

            if (countFrozenDictionary.Count > 0)
            {
                //Debug.Log("countFrozenDictionary.Count : " + countFrozenDictionary.Count);
                //foreach (var obj in countFrozenDictionary)
                //{
                //    Debug.Log(obj.Key + ": " + obj.Value);
                //}

                int maxCount = 0;
                bool isSame = true;
                if (countFrozenDictionary.Count == 1)
                {
                    foreach (var obj in countFrozenDictionary)
                    {
                        colorResult = obj.Key;
                        return colorResult;
                    }
                }
                else
                {
                    if (countFrozenDictionary.ElementAt(0).Value == countFrozenDictionary.ElementAt(1).Value)
                    {
                        isSame = true;
                    }
                    else isSame = false;
                }

                if (!isSame)
                {
                    foreach (var obj in countFrozenDictionary)
                    {
                        if (obj.Value > maxCount)
                        {
                            maxCount = obj.Value;
                            colorResult = obj.Key;
                        }
                    }

                    Debug.Log("colorResult if isSame: " + colorResult);
                    return colorResult;
                }
            }


            // độ ưu tiên 3: 
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

            // độ ưu tiên thứ 4: ưu tiên merge sang có ít stacks hơn
            if (listDataConnect[0].listTypes.Count > listDataConnect[1].listTypes.Count)
            {
                colorResult = listDataConnect[1];
                return colorResult;
            }
            else if (listDataConnect[0].listTypes.Count < listDataConnect[1].listTypes.Count)
            {
                colorResult = listDataConnect[0];
                return colorResult;
            }


            // độ ưu tiên thứ 5: ưu tiên merge vào giữa
            colorResult = ComparePlates(listDataConnect[0], listDataConnect[1]);

            if (colorResult != null)
                return colorResult;

            //Debug.Log(listDataConnect[0].name + " default");

            // độ ưu tiên cuối cùng: bản cũ ();
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
                if (listDataConnect[0].listTypes.Count > listDataConnect[1].listTypes.Count)
                    colorResult = listDataConnect[1];
                else colorResult = listDataConnect[0];
            }
            else if (countArrow == 1)
            {
                if (IsArrow(listDataConnect[0])) colorResult = listDataConnect[1];
                else colorResult = listDataConnect[0];
            }
            else
            {
                if (listDataConnect[0].listTypes.Count > listDataConnect[1].listTypes.Count)
                    colorResult = listDataConnect[1];
                else colorResult = listDataConnect[0];
            }
        }
        else if (listDataConnect.Count >= 3)
        {
            List<ColorPlate> listCanBeRoot = new List<ColorPlate>();
            listCanBeRoot.AddRange(listDataConnect);

            // Check condition has frozen colorPlate 
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

            if (countFrozenDictionary.Count > 0)
            {
                listCanBeRoot.Clear();
                Debug.Log("countFrozenDictionary.Count : " + countFrozenDictionary.Count);
                foreach (var obj in countFrozenDictionary)
                {
                    listCanBeRoot.Add(obj.Key);
                    Debug.Log(obj.Key + ": " + obj.Value);
                }

                List<ColorPlate> listCanBeRootFake = new List<ColorPlate>();
                listCanBeRootFake.AddRange(listCanBeRoot);


                if (countFrozenDictionary.Count == 1)
                {
                    foreach (var obj in countFrozenDictionary)
                    {
                        colorResult = obj.Key;
                        return colorResult;
                    }
                }
                else
                {
                    bool isSame = true;

                    int firstValue = countFrozenDictionary.First().Value;

                    foreach (var obj in countFrozenDictionary)
                    {
                        if (obj.Value != firstValue)
                        {
                            isSame = false;
                            break;
                        }
                    }


                    if (isSame)
                    {
                        foreach (ColorPlate c in listCanBeRoot)
                        {
                            if (c.listTypes.Count < 2) continue;

                            foreach (ColorPlate n in c.CheckNearByCanConnect())
                            {
                                if (n.listTypes.Count < 2) continue;

                                if (n.listTypes[n.listTypes.Count - 2].type == c.listTypes[c.listTypes.Count - 2].type)
                                {
                                    listCanBeRootFake.Remove(c);
                                }
                            }
                        }

                        if (listCanBeRootFake.Count > 0 && listCanBeRootFake.Count < listCanBeRoot.Count)
                        {
                            for (int i = 0; i < listCanBeRootFake.Count; i++)
                            {
                                //Debug.Log(listCanBeRoot[i].name + " after solve");
                            }

                            int maxCount = -1;

                            for (int i = 0; i < listCanBeRootFake.Count; i++)
                            {
                                int count = listCanBeRootFake[i].CountHasSameTopValueInConnect();
                                if (count > maxCount)
                                {
                                    maxCount = count;
                                    colorResult = listCanBeRootFake[i];
                                }
                            }
                        }
                        else if (listCanBeRootFake.Count == 0)
                        {
                            int minCount = 5;

                            for (int i = 0; i < listCanBeRoot.Count; i++)
                            {
                                int count = listCanBeRoot[i].CountHasSameTopValueInConnect();
                                if (count < minCount)
                                {
                                    minCount = count;
                                    colorResult = listCanBeRoot[i];
                                }
                            }
                        }
                        else if (listCanBeRootFake.Count == listCanBeRoot.Count)
                        {
                            int minTypeDiff = 5;

                            for (int i = 0; i < listCanBeRoot.Count; i++)
                            {
                                int countDiff = listCanBeRoot[i].listTypes.Count;
                                if (countDiff < minTypeDiff)
                                {
                                    minTypeDiff = countDiff;
                                    colorResult = listCanBeRoot[i];
                                }
                            }
                        }
                    }
                    else
                    {
                        int maxValue = 0;
                        foreach (var obj in countFrozenDictionary)
                        {
                            if (obj.Value >= maxValue)
                            {
                                maxValue = obj.Value;
                                colorResult = obj.Key;
                            }
                        }

                        return colorResult;
                    }
                }
            }


            // if hasnot frozen colorplate
            foreach (ColorPlate c in listDataConnect)
            {
                if (c.listTypes.Count < 2) continue;

                foreach (ColorPlate n in c.CheckNearByCanConnect( /*c*/))
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
        if (c.status == Status.Left || c.status == Status.Right || c.status == Status.Up ||
            c.status == Status.Down) return true;
        else return false;
    }

    public ColorPlate ComparePlates(ColorPlate plateA, ColorPlate plateB)
    {
        if (LogicGame.Instance == null) return null;

        int totalRows = LogicGame.Instance.rows;
        int totalCols = LogicGame.Instance.cols;

        float distanceA = plateA.GetDistanceToCenter(totalRows, totalCols);
        float distanceB = plateB.GetDistanceToCenter(totalRows, totalCols);

        return distanceA <= distanceB ? plateA : plateB;
    }
}