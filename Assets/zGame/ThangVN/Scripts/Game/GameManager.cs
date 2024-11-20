using BaseGame;

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
}
