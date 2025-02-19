using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBook
{
    public ColorPlate FindBookIsPoison(List<ColorPlate> listPoisonPlate)
    {
        ColorPlate colorIsPoison = null;
        for (int i = 0; i < listPoisonPlate.Count; i++)
        {
            colorIsPoison = FindNextPoisonBook(listPoisonPlate[i].ListConnect);
            if (colorIsPoison == null) continue;
            else
            {
                break;
            }
        }

        return colorIsPoison;
    }

    public ColorPlate FindNextPoisonBook(List<ColorPlate> listDataConnect)
    {
        ColorPlate cHasValue = null;
        ColorPlate cEmpty = null;

        for (var i = 0; i < listDataConnect.Count; i++)
        {
            var plate = listDataConnect[i];
            if (!CanSpread(plate)) continue;
            Debug.Log(plate.name + " ___ " + plate.ListValue.Count);
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