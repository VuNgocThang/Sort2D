using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindColorEnum
{
    public ColorEnum FindTargetColorEnum(List<ColorPlate> listConnect)
    {
        ColorEnum resultColorEnum = new();

        bool allNull = true;
        for (int i = 0; i < listConnect.Count; i++)
        {
            if (listConnect[i].ListValue.Count > 0)
            {
                allNull = false;
                break;
            }
        }

        if (allNull)
        {
            resultColorEnum = (ColorEnum)UnityEngine.Random.Range(0, 4);
        }
        else
        {
            Dictionary<ColorEnum, int> colorEnumDictionary = new Dictionary<ColorEnum, int>();

            List<ColorEnum> listColorEnum = new List<ColorEnum>();
            foreach (ColorPlate c in listConnect)
            {
                if (c.ListValue.Count == 0) continue;
                if (!listColorEnum.Contains(c.TopValue))
                {
                    listColorEnum.Add(c.TopValue);
                }
            }

            foreach (ColorPlate c in listConnect)
            {
                if (c.ListValue.Count == 0) continue;

                if (colorEnumDictionary.ContainsKey(c.TopValue))
                {
                    colorEnumDictionary[c.TopValue]++;
                }
                else
                {
                    colorEnumDictionary[c.TopValue] = 1;
                }
            }

            int maxCount = 0;
            bool allSame = true;
            foreach (var obj in colorEnumDictionary)
            {
                if (obj.Value >= 2)
                {
                    resultColorEnum = obj.Key;
                    maxCount = obj.Value;
                    allSame = false;
                }
            }

            if (allSame)
            {
                int countMax = 0;

                for (int i = 0; i < listConnect.Count; i++)
                {
                    if (listConnect[i].ListValue.Count == 0) continue;

                    if (listConnect[i].listTypes[listConnect[i].listTypes.Count - 1].listPlates.Count >= countMax)
                    {
                        countMax = listConnect[i].listTypes[listConnect[i].listTypes.Count - 1].listPlates.Count;
                        resultColorEnum = listConnect[i].TopValue;
                    }
                }

                //Debug.Log("countMax: " + countMax);
                //Debug.Log("resultColorEnumCountMax: " + resultColorEnum);
            }

            //Debug.Log("maxCount: " + maxCount);
            //Debug.Log("resultColorEnum: " + resultColorEnum);
        }

        return resultColorEnum;
    }
}
