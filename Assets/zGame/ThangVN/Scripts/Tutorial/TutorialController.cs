using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;

    private void Awake()
    {
        Instance = this;
    }



    public void InitTutorial()
    {
        ChangeLayerObject(LogicGame.Instance.ListArrowPlate[1].logicVisual.arrow);
    }

    public void ChangeLayerObject(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().sortingOrder = GameConfig.LAYER_TUTORIAL;
    }
}
