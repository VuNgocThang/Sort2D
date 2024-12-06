using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    public static float TIME_MOVE = 0.25f;

    public static int MAX_HEART = 5;

    public static float TIME_COUNT_DOWN = 300f;

    public static float TIME_FLY = 1f;
    //public static string COUNT_DOWN_TIMER = "CountdownTimer";

    public static string LAST_HEART_LOSS = "LastHeartLoss";

    public static float OFFSET_PLATE = 0.1f;

    public static float OFFSET_PLATE_Z = 0.1f;

    public static string TXT_REFRESH = "refresh tray to get new stack options";

    public static string TXT_SWAP = "drag any stack to move it";

    public static string TXT_HAMMER = "tap any stack to clear it";

    public static int LEVEL_REFRESH = 6;

    public static int LEVEL_HAMMER = 9;

    public static int LEVEL_SWAP = 14;

    public static int LEVEL_YELLOW = 2;

    public static int LEVEL_PURPLE = 5;

    public static int LEVEL_PINK = 8;

    public static int LEVEL_RANDOM = 12;

    public static int LEVEL_ORANGE = 16;

    public static int COIN_HAMMER = 250;

    public static int COIN_SWAP = 400;

    public static int COIN_REFRESH = 200;

    public static int PIGMENT_UNLOCK = 200;

    public static int ROW_COUNT = 5;

    public static int OFFSET_LAYER = 10;

    public static int MAX_LEVEL = 49;

    public static int MAX_LEVEL_BONUS = 10011;

    public static Color DEFAULT_COLOR = new Color(1, 1, 1, 1);

    public static Vector3 OFFSET_HAMMER = Vector3.zero;

    public static Vector3 OFFSET_NROOM = new Vector3(0.07f, -1.22f, 7.59f);

    public static float MID_POINT = 0.4f;

    public static bool EnoughCoinBuyHammer
    {
        get
        {
            return SaveGame.Coin >= COIN_HAMMER;
        }
    }

    public static bool EnoughCoinBuySwap
    {
        get
        {
            return SaveGame.Coin >= COIN_SWAP;
        }
    }

    public static bool EnoughCoinBuyRefresh
    {
        get
        {
            return SaveGame.Coin >= COIN_REFRESH;
        }
    }

    public static bool EnoughPigment
    {
        get
        {
            return SaveGame.Pigment >= PIGMENT_UNLOCK;
        }
    }

    public static string DATACOIN = "DATACOIN";

    public static string GAMESAVENORMAL = "GameSaveNormal";

    public static string GAMESAVECHALLENGES = "GameSaveChallenges";

    public static string GAMESAVEBONUS = "GameSaveBonus";

    public static string TASK_DATA = "TaskData";
}
