using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PopupRanking : Popup
{
    [Serializable]
    public class LeaderBoardEntry
    {
        public string name;
        public float score;
        public int rank;
    }

    [SerializeField] private TextMeshProUGUI txtCoin,
        txtPigment,
        txtSttFirst,
        txtNameFirst,
        txtScoreFirst,
        txtYourStt,
        txtYourName,
        txtYourScore;

    [SerializeField] private RankingPlayer rankingPrefab;
    [SerializeField] private Transform nContent;
    [SerializeField] private List<LeaderBoardEntry> leaderBoard = new List<LeaderBoardEntry>();

    private readonly string[] randomNames =
    {
        "Alex", "Jordan", "Sam", "Taylor", "Chris", "Morgan", "Pat", "Jamie", "Riley", "Quinn", "Riven", "Yasuo",
        "Thresh", "Cait"
    };

    private void Awake()
    {
    }

    public static async void Show()
    {
        PopupRanking pop = await ManagerPopup.ShowPopup<PopupRanking>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        transform.localScale = Vector3.one;
        txtCoin.text = SaveGame.Coin.ToString();
        txtPigment.text = SaveGame.Pigment.ToString();
        GenerateLeaderBoard("PlayerName", SaveGame.CurrentScore);
    }

    private void GenerateLeaderBoard(string playerName, int playerScore)
    {
        leaderBoard.Clear();
        int rankingPlayer = 1;
        // int rankingPlayer = Random.Range(1, randomNames.Length);

        for (int i = 0; i < randomNames.Length; i++)
        {
            if (i == rankingPlayer)
            {
                leaderBoard.Add(new LeaderBoardEntry()
                {
                    name = playerName,
                    score = playerScore,
                    rank = i
                });
            }
            else
            {
                int fakeScore = Mathf.Max(0, playerScore + Random.Range(-30, 30));

                leaderBoard.Add(new LeaderBoardEntry()
                {
                    name = randomNames[i],
                    score = fakeScore,
                    rank = i
                });
            }
        }

        leaderBoard.Sort((a, b) => b.score.CompareTo(a.score));

        ShowLeaderBoard(rankingPlayer, playerName, playerScore);
    }

    private void ShowLeaderBoard(int ranking, string playerName, int playerScore)
    {
        for (var index = 0; index < leaderBoard.Count; index++)
        {
            var entry = leaderBoard[index];

            if (entry.rank == ranking)
            {
                txtYourStt.text = $"{index + 1}.";
                txtYourName.text = playerName;
                txtYourScore.text = playerScore.ToString();
            }

            if (index == 0) continue;
            RankingPlayer rankingPlayer = Instantiate(rankingPrefab, nContent);
            rankingPlayer.Init(index + 1, entry.name, entry.score);
        }

        txtSttFirst.text = "1.";
        txtNameFirst.text = leaderBoard[0].name;
        txtScoreFirst.text = leaderBoard[0].score.ToString();
    }

    public override void Hide()
    {
        // base.Hide();
        ManagerEvent.ClearEvent();
        SceneManager.LoadScene("SceneHome");
    }
}