using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGame
{
    #region Tutorial

    const string TUTORIALFIRST = "TUTORIALFIRST";
    static int tutorialFirst = -1;

    public static bool TutorialFirst
    {
        set
        {
            ES3.Save(TUTORIALFIRST, value ? 1 : 0);
            tutorialFirst = value ? 1 : 0;
        }
        get
        {
            if (tutorialFirst == -1) tutorialFirst = ES3.Load(TUTORIALFIRST, 1);
            return tutorialFirst == 1;
        }
    }

    const string ISDONETUTPOINT = "ISDONETUTPOINT";
    static int isDoneTutPoint = -1;

    public static bool IsDoneTutPoint
    {
        set
        {
            ES3.Save(ISDONETUTPOINT, value ? 1 : 0);
            isDoneTutPoint = value ? 1 : 0;
        }
        get
        {
            if (isDoneTutPoint == -1) isDoneTutPoint = ES3.Load(ISDONETUTPOINT, 0);
            return isDoneTutPoint == 1;
        }
    }

    const string ISDONETUTORIAL = "ISDONETUTORIAL";
    static int isDoneTutorial = -1;

    public static bool IsDoneTutorial
    {
        set
        {
            ES3.Save(ISDONETUTORIAL, value ? 1 : 0);
            isDoneTutorial = value ? 1 : 0;
        }
        get
        {
            if (isDoneTutorial == -1) isDoneTutorial = ES3.Load(ISDONETUTORIAL, 0);
            return isDoneTutorial == 1;
        }
    }

    const string ISDONETUTORIALDECOR = "ISDONETUTORIALDECOR";
    static int isDoneTutorialDecor = -1;

    public static bool IsDoneTutorialDecor
    {
        set
        {
            ES3.Save(ISDONETUTORIALDECOR, value ? 1 : 0);
            isDoneTutorialDecor = value ? 1 : 0;
        }
        get
        {
            // change to 0
            if (isDoneTutorialDecor == -1) isDoneTutorialDecor = ES3.Load(ISDONETUTORIALDECOR, 0);
            return isDoneTutorialDecor == 1;
        }
    }

    const string ISDONETUTGIFT = "ISDONETUTGIFT";
    static int isDoneTutGift = -1;

    public static bool IsDoneTutGift
    {
        set
        {
            ES3.Save(ISDONETUTGIFT, value ? 1 : 0);
            isDoneTutGift = value ? 1 : 0;
        }
        get
        {
            if (isDoneTutGift == -1) isDoneTutGift = ES3.Load(ISDONETUTGIFT, 0);
            return isDoneTutGift == 1;
        }
    }


    const string ISDONETUTLOCKCOIN = "ISDONETUTLOCKCOIN";
    static int isDoneTutLockCoin = -1;

    public static bool IsDoneTutLockCoin
    {
        set
        {
            ES3.Save(ISDONETUTLOCKCOIN, value ? 1 : 0);
            isDoneTutLockCoin = value ? 1 : 0;
        }
        get
        {
            if (isDoneTutLockCoin == -1) isDoneTutLockCoin = ES3.Load(ISDONETUTLOCKCOIN, 0);
            return isDoneTutLockCoin == 1;
        }
    }

    const string ISDONETUTFROZEN = "ISDONETUTFROZEN";
    static int isDoneTutFrozen = -1;

    public static bool IsDoneTutFrozen
    {
        set
        {
            ES3.Save(ISDONETUTFROZEN, value ? 1 : 0);
            isDoneTutFrozen = value ? 1 : 0;
        }
        get
        {
            if (isDoneTutFrozen == -1) isDoneTutFrozen = ES3.Load(ISDONETUTFROZEN, 0);
            return isDoneTutFrozen == 1;
        }
    }

    const string ISDONETUTHAMMER = "ISDONETUTHAMMER";
    static int isDoneTutHammer = -1;

    public static bool IsDoneTutHammer
    {
        set
        {
            ES3.Save(ISDONETUTHAMMER, value ? 1 : 0);
            isDoneTutHammer = value ? 1 : 0;
        }
        get
        {
            if (isDoneTutHammer == -1) isDoneTutHammer = ES3.Load(ISDONETUTHAMMER, 0);
            return isDoneTutHammer == 1;
        }
    }

    const string ISDONETUTSWAP = "ISDONETUTSWAP";
    static int isDoneTutSwap = -1;

    public static bool IsDoneTutSwap
    {
        set
        {
            ES3.Save(ISDONETUTSWAP, value ? 1 : 0);
            isDoneTutSwap = value ? 1 : 0;
        }
        get
        {
            if (isDoneTutSwap == -1) isDoneTutSwap = ES3.Load(ISDONETUTSWAP, 0);
            return isDoneTutSwap == 1;
        }
    }

    const string ISSHOWBOOK = "ISSHOWBOOK";
    static int isShowBook = -1;

    public static bool IsShowBook
    {
        set
        {
            ES3.Save(ISSHOWBOOK, value ? 1 : 0);
            isShowBook = value ? 1 : 0;
        }
        get
        {
            if (isShowBook == -1) isShowBook = ES3.Load(ISSHOWBOOK, 0);
            return isShowBook == 1;
        }
    }


    const string ISSHOWHAMMER = "ISSHOWHAMMER";
    static int isShowHammder = -1;

    public static bool IsShowHammer
    {
        set
        {
            ES3.Save(ISSHOWHAMMER, value ? 1 : 0);
            isShowHammder = value ? 1 : 0;
        }
        get
        {
            if (isShowHammder == -1) isShowHammder = ES3.Load(ISSHOWHAMMER, 0);
            return isShowHammder == 1;
        }
    }

    const string ISSHOWSWAP = "ISSHOWSWAP";
    static int isShowSwap = -1;

    public static bool IsShowSwap
    {
        set
        {
            ES3.Save(ISSHOWSWAP, value ? 1 : 0);
            isShowSwap = value ? 1 : 0;
        }
        get
        {
            if (isShowSwap == -1) isShowSwap = ES3.Load(ISSHOWSWAP, 0);
            return isShowSwap == 1;
        }
    }

    const string ISSHOWREFRESH = "ISSHOWREFRESH";
    static int isShowRefresh = -1;

    public static bool IsShowRefresh
    {
        set
        {
            ES3.Save(ISSHOWREFRESH, value ? 1 : 0);
            isShowRefresh = value ? 1 : 0;
        }
        get
        {
            if (isShowRefresh == -1) isShowRefresh = ES3.Load(ISSHOWREFRESH, 0);
            return isShowRefresh == 1;
        }
    }

    const string ISTUTFREECOIN = "ISTUTFREECOIN";
    static int isTutFreeCoin = -1;

    public static bool IsTutFreeCoin
    {
        set
        {
            ES3.Save(ISTUTFREECOIN, value ? 1 : 0);
            isTutFreeCoin = value ? 1 : 0;
        }
        get
        {
            if (isTutFreeCoin == -1) isTutFreeCoin = ES3.Load(ISTUTFREECOIN, 0);
            return isTutFreeCoin == 1;
        }
    }

    const string ISTUTCHALLENGES = "ISTUTCHALLENGES";
    static int isTutChallenges = -1;

    public static bool IsTutChallenges
    {
        set
        {
            ES3.Save(ISTUTCHALLENGES, value ? 1 : 0);
            isTutChallenges = value ? 1 : 0;
        }
        get
        {
            if (isTutChallenges == -1) isTutChallenges = ES3.Load(ISTUTCHALLENGES, 0);
            return isTutChallenges == 1;
        }
    }

    const string ISTUTDAILYTASK = "ISTUTDAILYTASK";
    static int isTutDailyTask = -1;

    public static bool IsTutDailyTask
    {
        set
        {
            ES3.Save(ISTUTDAILYTASK, value ? 1 : 0);
            isTutDailyTask = value ? 1 : 0;
        }
        get
        {
            if (isTutDailyTask == -1) isTutDailyTask = ES3.Load(ISTUTDAILYTASK, 0);
            return isTutDailyTask == 1;
        }
    }

    #endregion

    #region InGame

    const string SOUND = "SOUND";
    static int sound = -1;

    public static bool Sound
    {
        set
        {
            ES3.Save(SOUND, value ? 1 : 0);
            sound = value ? 1 : 0;
        }
        get
        {
            if (sound == -1) sound = ES3.Load(SOUND, 1);
            return sound == 1;
        }
    }

    const string MUSIC = "MUSIC";
    static int music = -1;

    public static bool Music
    {
        set
        {
            ES3.Save(MUSIC, value ? 1 : 0);
            music = value ? 1 : 0;
        }
        get
        {
            if (music == -1) music = ES3.Load(MUSIC, 1);
            return music == 1;
        }
    }

    const string VIBRATE = "VIBRATE";
    static int vibrate = -1;

    public static bool Vibrate
    {
        set
        {
            ES3.Save(VIBRATE, value ? 1 : 0);
            vibrate = value ? 1 : 0;
        }
        get
        {
            if (vibrate == -1) vibrate = ES3.Load(VIBRATE, 1);
            return vibrate == 1;
        }
    }

    const string LEVEL = "LEVEL";
    static int level = -1;

    public static int Level
    {
        set
        {
            ES3.Save(LEVEL, value);
            level = value;
        }
        get
        {
            if (level == -1) level = ES3.Load(LEVEL, 0);
            return level;
        }
    }

    const string LEVELGIFT = "LEVELGIFT";
    static int levelGift = -1;

    public static int LevelGift
    {
        set
        {
            ES3.Save(LEVELGIFT, value);
            levelGift = value;
        }
        get
        {
            if (levelGift == -1) levelGift = ES3.Load(LEVELGIFT, 0);
            return levelGift;
        }
    }


    const string HAMMER = "HAMMER";
    static int hammer = -1;

    public static int Hammer
    {
        set
        {
            ES3.Save(HAMMER, value);
            hammer = value;
        }
        get
        {
            if (hammer == -1) hammer = ES3.Load(HAMMER, 2);
            return hammer;
        }
    }


    const string SWAP = "SWAP";
    static int swap = -1;

    public static int Swap
    {
        set
        {
            ES3.Save(SWAP, value);
            swap = value;
        }
        get
        {
            if (swap == -1) swap = ES3.Load(SWAP, 2);
            return swap;
        }
    }

    const string REFRESH = "REFRESH";
    static int refresh = -1;

    public static int Refresh
    {
        set
        {
            ES3.Save(REFRESH, value);
            refresh = value;
        }
        get
        {
            if (refresh == -1) refresh = ES3.Load(REFRESH, 2);
            return refresh;
        }
    }


    const string COIN = "COIN";
    static int coin = -1;

    public static int Coin
    {
        set
        {
            ES3.Save(COIN, value);
            coin = value;
        }
        get
        {
            if (coin == -1) coin = ES3.Load(COIN, 0);
            return coin;
        }
    }


    const string PIGMENT = "PIGMENT";
    static int pigment = -1;

    public static int Pigment
    {
        set
        {
            ES3.Save(PIGMENT, value);
            pigment = value;
        }
        get
        {
            if (pigment == -1) pigment = ES3.Load(PIGMENT, 0);
            return pigment;
        }
    }

    const string CURRENTSCORE = "CURRENTSCORE";
    static int currentScore = -1;

    public static int CurrentScore
    {
        set
        {
            ES3.Save(CURRENTSCORE, value);
            currentScore = value;
        }
        get
        {
            if (currentScore == -1) currentScore = ES3.Load(CURRENTSCORE, 0);
            return currentScore;
        }
    }

    const string BESTSCORE = "BESTSCORE";
    static int bestScore = -1;

    public static int BestScore
    {
        set
        {
            ES3.Save(BESTSCORE, value);
            bestScore = value;
        }
        get
        {
            if (bestScore == -1) bestScore = ES3.Load(BESTSCORE, 0);
            return bestScore;
        }
    }

    const string PLAYBONUS = "PLAYBONUS";
    static int playBonus = -1;

    public static bool PlayBonus
    {
        set
        {
            ES3.Save(PLAYBONUS, value ? 1 : 0);
            playBonus = value ? 1 : 0;
        }
        get
        {
            if (playBonus == -1) playBonus = ES3.Load(PLAYBONUS, 0);
            return playBonus == 1;
        }
    }

    const string CHALLENGES = "CHALLENGES";
    static int challenges = -1;

    public static bool Challenges
    {
        set
        {
            ES3.Save(CHALLENGES, value ? 1 : 0);
            challenges = value ? 1 : 0;
        }
        get
        {
            if (challenges == -1) challenges = ES3.Load(CHALLENGES, 0);
            return challenges == 1;
        }
    }

    const string HEART = "HEART";
    static int heart = -1;

    public static int Heart
    {
        set
        {
            ES3.Save(HEART, value);
            heart = value;
        }
        get
        {
            if (heart == -1) heart = ES3.Load(HEART, 5);
            return heart;
        }
    }

    const string LEVELBONUS = "LEVELBONUS";
    static int levelBonus = -1;

    public static int LevelBonus
    {
        set
        {
            ES3.Save(LEVELBONUS, value);
            levelBonus = value;
        }
        get
        {
            if (levelBonus == -1) levelBonus = ES3.Load(LEVELBONUS, 10000);
            return levelBonus;
        }
    }

    const string LEVELCHALLENGES = "LEVELCHALLENGES";
    static int levelChallenges = -1;

    public static int LevelChallenges
    {
        set
        {
            ES3.Save(LEVELCHALLENGES, value);
            levelChallenges = value;
        }
        get
        {
            if (levelChallenges == -1) levelChallenges = ES3.Load(LEVELCHALLENGES, 1000);
            return levelChallenges;
        }
    }

    const string TIMEPLAYED = "TIMEPLAYED";
    static int timePlayed = -1;

    public static int TimePlayed
    {
        set
        {
            ES3.Save(TIMEPLAYED, value);
            timePlayed = value;
        }
        get
        {
            if (timePlayed == -1)
                timePlayed = ES3.Load(TIMEPLAYED, 0);
            return timePlayed;
        }
    }

    #endregion

    #region Decor

    const string LISTROOMDATA = "LISTROOMDATA";
    static ListRoomPainted listRoomPainted;

    public static ListRoomPainted ListRoomPainted
    {
        set
        {
            listRoomPainted = value;
            ES3.Save(LISTROOMDATA, listRoomPainted);
        }
        get
        {
            if (listRoomPainted == null)
            {
                listRoomPainted = ES3.Load(LISTROOMDATA, new ListRoomPainted
                {
                    listRoomPainted = new List<ListObjetRoomPainted>()
                    {
                    }
                });
            }

            return listRoomPainted;
        }
    }

    const string LISTBOOKDATA = "LISTBOOKDATA";
    static ListBookDecorated listBookDecorated;

    public static ListBookDecorated ListBookDecorated
    {
        set
        {
            listBookDecorated = value;
            ES3.Save(LISTBOOKDATA, listBookDecorated);
        }
        get
        {
            if (listBookDecorated == null)
            {
                listBookDecorated = ES3.Load(LISTBOOKDATA, new ListBookDecorated
                {
                    listBookDecorated = new List<BookDecorated>()
                    {
                        new BookDecorated()
                        {
                            idBookDecorated = 0,
                            progress = 0,
                            isPainted = false,
                            isCollectedReward = false,
                            colorPainted = GameConfig.DEFAULT_COLOR,
                            isSetupFull = false,
                            listItemDecorated = new List<ItemDecorated>()
                            {
                            }
                        }
                    }
                });
            }

            return listBookDecorated;
        }
    }

    const string CURRENTBOOK = "CURRENTBOOK";
    static int currentBook = -1;

    public static int CurrentBook
    {
        set
        {
            ES3.Save(CURRENTBOOK, value);
            currentBook = value;
        }
        get
        {
            if (currentBook == -1) currentBook = ES3.Load(CURRENTBOOK, 0);
            return currentBook;
        }
    }

    const string MAXCURRENTBOOK = "MAXCURRENTBOOK";
    static int maxCurrentBook = -1;

    public static int MaxCurrentBook
    {
        set
        {
            ES3.Save(MAXCURRENTBOOK, value);
            maxCurrentBook = value;
        }
        get
        {
            if (maxCurrentBook == -1) maxCurrentBook = ES3.Load(MAXCURRENTBOOK, 0);
            return maxCurrentBook;
        }
    }


    const string CURRENTROOM = "CURRENTROOM";
    static int currentRoom = -1;

    public static int CurrentRoom
    {
        set
        {
            ES3.Save(CURRENTROOM, value);
            currentRoom = value;
        }
        get
        {
            if (currentRoom == -1) currentRoom = ES3.Load(CURRENTROOM, 0);
            return currentRoom;
        }
    }


    const string CURRENTOBJECT = "CURRENTOBJECT";
    static int currentObject = -1;

    public static int CurrentObject
    {
        set
        {
            ES3.Save(CURRENTOBJECT, value);
            currentObject = value;
        }

        get
        {
            if (currentObject == -1) currentObject = ES3.Load(CURRENTOBJECT, 0);
            return currentObject;
        }
    }

    const string FIRSTDECOR = "FIRSTDECOR";
    static int firstDecor = -1;

    public static bool FirstDecor
    {
        set
        {
            ES3.Save(FIRSTDECOR, value ? 1 : 0);
            firstDecor = value ? 1 : 0;
        }
        get
        {
            // change to 1
            if (firstDecor == -1) firstDecor = ES3.Load(FIRSTDECOR, 1);
            return firstDecor == 1;
        }
    }

    const string REDECORATED = "REDECORATED";
    static int redecorated = -1;

    public static bool Redecorated
    {
        set
        {
            ES3.Save(REDECORATED, value ? 1 : 0);
            redecorated = value ? 1 : 0;
        }
        get
        {
            if (redecorated == -1) redecorated = ES3.Load(REDECORATED, 0);
            return redecorated == 1;
        }
    }

    #endregion

    #region HomeUI

    const string COUNTDOWNTIMERBOOK = "COUNTDOWNTIMERBOOK";
    static float countDownTimerBook = -1;

    public static float CountDownTimerBook
    {
        set
        {
            ES3.Save(COUNTDOWNTIMERBOOK, value);
            countDownTimerBook = value;
        }
        get
        {
            if (countDownTimerBook == -1)
                countDownTimerBook = ES3.Load(COUNTDOWNTIMERBOOK, GameConfig.TIME_COUNT_DOWN_BOOK);
            return countDownTimerBook;
        }
    }

    const string COUNTDOWNTIMER = "COUNTDOWNTIMER";
    static float countDownTimer = -1;

    public static float CountDownTimer
    {
        set
        {
            ES3.Save(COUNTDOWNTIMER, value);
            countDownTimer = value;
        }
        get
        {
            if (countDownTimer == -1) countDownTimer = ES3.Load(COUNTDOWNTIMER, GameConfig.TIME_COUNT_DOWN);
            return countDownTimer;
        }
    }

    const string LASTHEARTLOSS = "LASTHEARTLOSS";
    static string lastHeartLoss = null;

    public static string LastHeartLoss
    {
        set
        {
            ES3.Save(LASTHEARTLOSS, value);
            lastHeartLoss = value;
        }
        get
        {
            if (lastHeartLoss == null) lastHeartLoss = ES3.Load<string>(LASTHEARTLOSS, DateTime.Now.ToString());
            return lastHeartLoss;
        }
    }

    const string ISFIRSTCHALLENGES = "ISFIRSTCHALLENGES";
    static int isFirstChallenges = -1;

    public static bool IsFirstChallenges
    {
        set
        {
            ES3.Save(ISFIRSTCHALLENGES, value ? 1 : 0);
            isFirstChallenges = value ? 1 : 0;
        }
        get
        {
            if (isFirstChallenges == -1) isFirstChallenges = ES3.Load(ISFIRSTCHALLENGES, 0);
            return isFirstChallenges == 1;
        }
    }


    const string CANSHOW = "CANSHOW";
    static int canShow = -1;

    public static bool CanShow
    {
        set
        {
            ES3.Save(CANSHOW, value ? 1 : 0);
            canShow = value ? 1 : 0;
        }
        get
        {
            if (canShow == -1) canShow = ES3.Load(CANSHOW, 0);
            return canShow == 1;
        }
    }

    const string CANSHOWGIFTBOOK = "CANSHOWGIFTBOOK";
    static int canShowGiftBook = -1;

    public static bool CanShowGiftBook
    {
        set
        {
            ES3.Save(CANSHOWGIFTBOOK, value ? 1 : 0);
            canShowGiftBook = value ? 1 : 0;
        }
        get
        {
            if (canShowGiftBook == -1) canShowGiftBook = ES3.Load(CANSHOWGIFTBOOK, 1);
            return canShowGiftBook == 1;
        }
    }

    const string NEWDAY = "NEWDAY";
    static int newDay = -1;

    public static int NewDay
    {
        set
        {
            ES3.Save(NEWDAY, value);
            newDay = value;
        }
        get
        {
            if (newDay == -1) newDay = ES3.Load(NEWDAY, 0);
            return newDay;
        }
    }

    const string NEWDAYFREECOIN = "NEWDAYFREECOIN";
    static int newDayFreeCoin = -1;

    public static int NewDayFreeCoin
    {
        set
        {
            ES3.Save(NEWDAYFREECOIN, value);
            newDayFreeCoin = value;
        }
        get
        {
            if (newDayFreeCoin == -1) newDayFreeCoin = ES3.Load(NEWDAYFREECOIN, 0);
            return newDayFreeCoin;
        }
    }

    const string DATAFREECOIN = "DATAFREECOIN";
    static DataClaimedFreecoin dataFreeCoin;

    public static DataClaimedFreecoin DataFreeCoin
    {
        set
        {
            dataFreeCoin = value;
            ES3.Save(DATAFREECOIN, dataFreeCoin);
        }
        get
        {
            if (dataFreeCoin == null)
            {
                dataFreeCoin = ES3.Load(DATAFREECOIN, new DataClaimedFreecoin
                {
                    currentIndex = 0,
                    listDataFreeCoin = new List<DataFreeCoin>()
                    {
                    }
                });
            }

            return dataFreeCoin;
        }
    }

    const string CLAIMREWARD1 = "CLAIMREWARD1";
    static int claimReward1 = -1;

    public static bool ClaimReward1
    {
        set
        {
            ES3.Save(CLAIMREWARD1, value ? 1 : 0);
            claimReward1 = value ? 1 : 0;
        }
        get
        {
            if (claimReward1 == -1) claimReward1 = ES3.Load(CLAIMREWARD1, 0);
            return claimReward1 == 1;
        }
    }

    const string CLAIMREWARD2 = "CLAIMREWARD2";
    static int claimReward2 = -1;

    public static bool ClaimReward2
    {
        set
        {
            ES3.Save(CLAIMREWARD2, value ? 1 : 0);
            claimReward2 = value ? 1 : 0;
        }
        get
        {
            if (claimReward2 == -1) claimReward2 = ES3.Load(CLAIMREWARD2, 0);
            return claimReward2 == 1;
        }
    }

    const string CLAIMREWARD3 = "CLAIMREWARD3";
    static int claimReward3 = -1;

    public static bool ClaimReward3
    {
        set
        {
            ES3.Save(CLAIMREWARD3, value ? 1 : 0);
            claimReward3 = value ? 1 : 0;
        }
        get
        {
            if (claimReward3 == -1) claimReward3 = ES3.Load(CLAIMREWARD3, 0);
            return claimReward3 == 1;
        }
    }

    const string SHOWFREECOIN = "SHOWFREECOIN";
    static int showFreeCoin = -1;

    public static bool ShowFreeCoin
    {
        set
        {
            ES3.Save(SHOWFREECOIN, value ? 1 : 0);
            showFreeCoin = value ? 1 : 0;
        }
        get
        {
            if (showFreeCoin == -1) showFreeCoin = ES3.Load(SHOWFREECOIN, 0);
            return showFreeCoin == 1;
        }
    }

    const string HIDEUI = "HIDEUI";
    static int hideUI = -1;

    public static bool HideUI
    {
        set
        {
            ES3.Save(HIDEUI, value ? 1 : 0);
            hideUI = value ? 1 : 0;
        }
        get
        {
            if (hideUI == -1) hideUI = ES3.Load(HIDEUI, 1);
            return hideUI == 1;
        }
    }

    #endregion

    #region Ads

    const string CANSHOWINTER = "CANSHOWINTER";
    static int canShowInter = -1;

    public static bool CanShowInter
    {
        set
        {
            ES3.Save(CANSHOWINTER, value ? 1 : 0);
            canShowInter = value ? 1 : 0;
        }
        get
        {
            if (canShowInter == -1) canShowInter = ES3.Load(CANSHOWINTER, 0);
            return canShowInter == 1;
        }
    }

    const string CANRATEUS = "CANRATEUS";
    static int canRateUs = -1;

    public static bool CanRateUs
    {
        set
        {
            ES3.Save(CANRATEUS, value ? 1 : 0);
            canRateUs = value ? 1 : 0;
        }
        get
        {
            if (canRateUs == -1) canRateUs = ES3.Load(CANRATEUS, 1);
            return canRateUs == 1;
        }
    }

    const string SUBMITTED = "SUBMITTED";
    static int submitted = -1;

    public static bool Submitted
    {
        set
        {
            ES3.Save(SUBMITTED, value ? 1 : 0);
            submitted = value ? 1 : 0;
        }
        get
        {
            if (submitted == -1) submitted = ES3.Load(SUBMITTED, 0);
            return submitted == 1;
        }
    }


    const string ISBOUGHTNOADS = "ISBOUGHTNOADS";
    static int isBoughtNoAds = -1;

    public static bool IsBoughtNoAds
    {
        set
        {
            ES3.Save(ISBOUGHTNOADS, value ? 1 : 0);
            isBoughtNoAds = value ? 1 : 0;
        }
        get
        {
            if (isBoughtNoAds == -1) isBoughtNoAds = ES3.Load(ISBOUGHTNOADS, 0);
            return isBoughtNoAds == 1;
        }
    }

    const string ISBOUGHTWELCOMEPACK = "ISBOUGHTWELCOMEPACK";
    static int isBoughtWelcomePack = -1;

    public static bool IsBoughtWelcomePack
    {
        set
        {
            ES3.Save(ISBOUGHTWELCOMEPACK, value ? 1 : 0);
            isBoughtWelcomePack = value ? 1 : 0;
        }
        get
        {
            if (isBoughtWelcomePack == -1) isBoughtWelcomePack = ES3.Load(ISBOUGHTWELCOMEPACK, 0);
            return isBoughtWelcomePack == 1;
        }
    }

    const string COUNTWATCHINTER = "COUNTWATCHINTER";
    static int countWatchInter = -1;

    public static int CountWatchInter
    {
        set
        {
            ES3.Save(COUNTWATCHINTER, value);
            countWatchInter = value;
        }
        get
        {
            if (countWatchInter == -1) countWatchInter = ES3.Load(COUNTWATCHINTER, 0);
            return countWatchInter;
        }
    }


    const string CANSHOWADSBUNDLE = "CANSHOWADSBUNDLE";
    static int canShowAdsBundle = -1;

    public static bool CanShowAdsBundle
    {
        set
        {
            ES3.Save(CANSHOWADSBUNDLE, value ? 1 : 0);
            canShowAdsBundle = value ? 1 : 0;
        }
        get
        {
            if (canShowAdsBundle == -1) canShowAdsBundle = ES3.Load(CANSHOWADSBUNDLE, 1);
            return canShowAdsBundle == 1;
        }
    }

    #endregion
}