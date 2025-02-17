using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingPlayer : MonoBehaviour
{
    public TextMeshProUGUI txtStt, txtName, txtScore;

    public void Init(int stt, string name, float score)
    {
        txtStt.text = $"{stt}.";
        txtName.text = name;
        txtScore.text = score.ToString();
    }
}