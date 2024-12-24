using BaseGame;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;

public class DefaultFinishPlate : IVisualPlate
{
    public void Execute(ColorPlate colorPlate, int count, ColorEnum colorEnum, bool plusPoint)
    {
        Sequence sq = DOTween.Sequence();
        float delay = 0f;

        List<LogicColor> listTest = new List<LogicColor>();

        ManagerAudio.PlaySound(ManagerAudio.Data.soundEat1);

        LogicColor colorFirst = colorPlate.ListColor[colorPlate.ListColor.Count - 1];


        for (int i = colorPlate.ListColor.Count - 1; i >= colorPlate.ListValue.Count; --i)
        {
            LogicColor color = colorPlate.ListColor[i];
            if (i != colorPlate.ListValue.Count) listTest.Add(color);
            colorPlate.ListColor.Remove(color);

            if (i == colorPlate.ListValue.Count)
            {
                sq.Insert(delay, color.transform.DOScale(0.5f, 0.3f)
                    .OnComplete(() =>
                    {
                        //color.trail.enabled = true;
                        color.trail.SetActive(true);
                        // Camera overlay
                        Vector3 viewportPos = new Vector3(colorPlate.targetUIPosition.position.x / Screen.width, colorPlate.targetUIPosition.position.y / Screen.height, Camera.main.nearClipPlane);
                        Vector3 targetPos = Camera.main.ViewportToWorldPoint(viewportPos);

                        // Camera Screen Space
                        //Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, colorPlate.targetUIPosition.position);
                        //Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane));


                        //ParticleSystem eatParticle = LogicGame.Instance.eatParticlePool.Spawn(colorPlate.transform.position, true);
                        ParticleSystem eatParticle = LogicGame.Instance.eatParticlePool.Spawn();
                        eatParticle.transform.SetParent(colorPlate.transform);
                        eatParticle.transform.localPosition = Vector3.zero;
                        eatParticle.transform.localScale = Vector3.one;
                        eatParticle.Play();

                        var main = eatParticle.main;
                        main.startColor = SwitchColor(colorEnum);
                        for (int j = 1; j < eatParticle.transform.childCount; j++)
                        {
                            ParticleSystem c = eatParticle.transform.GetChild(j).GetComponent<ParticleSystem>();
                            var mainC = c.main;
                            mainC.startColor = SwitchColor(colorEnum);
                        }

                        CreatePathAnimation(color, targetPos, plusPoint, count);

                        //color.transform.DOMove(targetPos, GameConfig.TIME_FLY)
                        //    .OnStart(() =>
                        //    {
                        //        ManagerAudio.PlaySound(ManagerAudio.Data.soundPlusScore);
                        //    })
                        //    .OnComplete(() =>
                        //    {
                        //        if (plusPoint)
                        //            ManagerEvent.RaiseEvent(EventCMD.EVENT_POINT, count);

                        //        Debug.Log(LogicGame.Instance.point + " ____POINT");
                        //        LogicGame.Instance.ExecuteLockCoin(LogicGame.Instance.point);
                        //        LogicGame.Instance.IncreaseCountDiff();
                        //        LogicGame.Instance.SpawnSpecialColor();

                        //        //color.trail.enabled = false;
                        //        color.trail.SetActive(false);
                        //        color.gameObject.SetActive(false);
                        //    });
                    }));
            }
            else
            {
                sq.Insert(delay, color.transform.DOScale(0, 0.3f));
                sq.Insert(delay, color.transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.LocalAxisAdd));
                delay += 0.05f;
                sq.OnComplete(() =>
                {
                    for (int i = 0; i < listTest.Count; ++i)
                    {
                        listTest[i].gameObject.SetActive(false);
                    }
                });
            }
        }
    }

    Color SwitchColor(ColorEnum colorEnum)
    {
        Color color;
        switch (colorEnum)
        {
            case ColorEnum.Blue:
                color = Color.blue;
                break;
            case ColorEnum.Green:
                color = Color.green;
                break;
            case ColorEnum.Red:
                color = Color.red;
                break;
            case ColorEnum.Orange:
                color = new Color(1, 0.4205002f, 0);
                break;
            case ColorEnum.Yellow:
                color = new Color(1, 0.8548728f, 0);
                break;
            case ColorEnum.Pink:
                color = new Color(1, 0, 0.8339343f);
                break;
            case ColorEnum.Purple:
                color = new Color(0.5351658f, 0f, 1f);
                break;
            default:
                color = Color.white;
                break;
        }

        return color;
    }

    void CreatePathAnimation(LogicColor color, Vector3 to, bool PlusPoint, int count)
    {
        Vector3 from = color.transform.position;

        float randomY = Random.Range(0f, 3.5f);

        float x = (from.x + to.x) / 2;
        float randomX;
        int rdom = Random.Range(0, 2);
        randomX = rdom % 2 == 0 ? x : -x;

        Vector3 midPoint = new Vector3(randomX, randomY, 0);

        color.transform.DOPath(new Vector3[] { from, midPoint, to }, GameConfig.TIME_FLY, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad)
            .OnStart(() =>
            {
                color.spriteRender.sortingOrder = 15;
                ManagerAudio.PlaySound(ManagerAudio.Data.soundPlusScore);
            })
            .OnComplete(() =>
            {
                if (PlusPoint)
                    ManagerEvent.RaiseEvent(EventCMD.EVENT_POINT, count);

                //Debug.Log(LogicGame.Instance.point + " ____POINT");
                LogicGame.Instance.ExecuteLockCoin(LogicGame.Instance.point);
                LogicGame.Instance.IncreaseCountDiff();
                LogicGame.Instance.SpawnSpecialColor();

                //color.trail.enabled = false;
                color.trail.SetActive(false);
                color.gameObject.SetActive(false);
            });
    }
}
