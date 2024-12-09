using BaseGame;
using System.Collections.Generic;

public static class GameManager
{
    public static void SubPigment(int _subPigment)
    {
        if (SaveGame.Pigment < _subPigment) return;

        ManagerAudio.PlaySound(ManagerAudio.Data.soundDropPigment);
        SaveGame.Pigment -= _subPigment;
    }

    public static void SubGold(int _subGold)
    {
        if (SaveGame.Coin < _subGold) return;

        ManagerAudio.PlaySound(ManagerAudio.Data.soundDropGold);
        SaveGame.Coin -= _subGold;
    }

    public static void AddPigment(int _addPigment)
    {
        ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimPigment);
        SaveGame.Pigment += _addPigment;
    }

    public static void AddGold(int _addGold)
    {
        ManagerAudio.PlaySound(ManagerAudio.Data.soundClaimGold);
        SaveGame.Coin += _addGold;
    }

    public static bool IsNormalGame()
    {
        return (!SaveGame.Challenges && !SaveGame.PlayBonus);
    }

    public static bool IsChallengesGame()
    {
        return SaveGame.Challenges;
    }

    public static bool IsBonusGame()
    {
        return SaveGame.PlayBonus;
    }

    public static bool ShowPopupBonus()
    {
        return false;
        //(SaveGame.Level >= 5 && SaveGame.Level % 5 == 0);
    }

    public static int GetRandomWithRatio(this float[] list)
    {
        List<float> listRatio = new List<float>();
        listRatio.AddRange(list);
        return listRatio.GetRandomWithRatio();
    }

    public static int GetRandomWithRatio(this List<float> list)
    {
        float sum = 0;
        foreach (float i in list) sum += i;
        float random = UnityEngine.Random.Range(0, sum);
        int t = 0;
        sum = list[t];
        while (random >= sum)
        {
            ++t;
            sum += list[t];
        }
        return t;
    }

    public static List<float> ChangeToList(this float[] list)
    {
        List<float> listRatio = new List<float>();
        listRatio.AddRange(list);
        return listRatio;
    }
}
