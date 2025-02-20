using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBook
{
    public ColorPlate FindBookIsPoison(List<ColorPlate> listPoisonPlate, List<ColorPlate> listDataConnect)
    {
        HashSet<ColorPlate> excludedPlates = new HashSet<ColorPlate>();
        ColorPlate colorIsPoison = null;
        foreach (var poisonPlate in listPoisonPlate)
        {
            while (true)
            {
                colorIsPoison = FindNextPoisonBook(poisonPlate.ListConnect, excludedPlates);

                if (colorIsPoison == null)
                    break;

                if (listDataConnect.Contains(colorIsPoison))
                {
                    Debug.Log("Cant is poison: " + colorIsPoison.name);
                    excludedPlates.Add(colorIsPoison);
                    continue;
                }

                break;
            }

            if (colorIsPoison != null)
                break;
        }

        return colorIsPoison;
    }

    private ColorPlate FindNextPoisonBook(List<ColorPlate> ListConnect, HashSet<ColorPlate> excludedPlates)
    {
        ColorPlate cHasValue = null;
        ColorPlate cEmpty = null;

        for (var i = 0; i < ListConnect.Count; i++)
        {
            var plate = ListConnect[i];
            if (!CanSpread(plate) || excludedPlates.Contains(plate)) continue;
            // Debug.Log(plate.name + " ___ " + plate.ListValue.Count);
            if (plate.ListValue.Count > 0 && cHasValue == null)
            {
                cHasValue = plate;
            }
            else if (plate.ListValue.Count == 0 && cEmpty == null)
            {
                cEmpty = plate;
            }

            if (cHasValue != null) break;
        }

        return cHasValue ?? cEmpty;
    }


    private static bool CanSpread(ColorPlate c)
    {
        var canSpread = c.status is not (Status.LockCoin or Status.Ads or Status.Frozen or Status.CannotPlace
            or Status.Wood or Status.Poison or Status.Bag);

        return canSpread;
    }
}