using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public Image imgIcon;
    public TextMeshProUGUI txtCount;
    public TypeReward typeReward;
    public int count;
    public List<Sprite> listSprite;

    public void Init(TypeReward typeReward, int count)
    {
        this.typeReward = typeReward;
        this.count = count;

        txtCount.text = count.ToString();
        imgIcon.sprite = ShowReward(typeReward, count);
        imgIcon.SetNativeSize();
    }

    public Sprite ShowReward(TypeReward typeReward, int count)
    {
        Sprite sprite = listSprite[0];

        switch (typeReward)
        {
            case TypeReward.GOLD:
                sprite = listSprite[0];
                SaveGame.Coin += count;
                break;

            case TypeReward.REFRESH:
                sprite = listSprite[1];
                SaveGame.Refresh += count;
                break;

            case TypeReward.SWAP:
                sprite = listSprite[2];
                SaveGame.Swap += count;
                break;

            case TypeReward.WAND:
                sprite = listSprite[3];
                SaveGame.Hammer += count;
                break;

            default:
                break;
        }

        return sprite;
    }
}
