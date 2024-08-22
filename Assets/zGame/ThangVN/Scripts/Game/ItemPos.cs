using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ItemPos 
{
    public int Row;
    public int Col;


    //IEnumerator ExecuteSequence()
    //{
    //    yield return null;
    //    //yield return new WaitForSeconds(0.1f);
    //    if (listSteps.Count >= 1)
    //    {
    //        isMergeing = true;
    //        Sequence sequence = DOTween.Sequence();
    //        //float time = 0f;

    //        for (int i = listSteps.Count - 1; i >= 0; i--)
    //        {
    //            int index = i;
    //            //Debug.Log("11111");
    //            sequence.AppendCallback(() =>
    //            {
    //                //Debug.Log("index:" + index);
    //                //Debug.Log("22222");
    //                Merge(listSteps[index].nearByColorPlate, listSteps[index].rootColorPlate);
    //                //Debug.Log("33333");
    //            }
    //            );
    //            //Debug.Log("44444");
    //            sequence.AppendInterval(0.55f);
    //            Debug.Log("Count: " + listSteps[i].nearByColorPlate.listTypes[listSteps[i].nearByColorPlate.listTypes.Count - 1].listPlates.Count);
    //            //time = listSteps[i].nearByColorPlate.listTypes[listSteps[i].nearByColorPlate.listTypes.Count - 1].listPlates.Count * 0.15f;
    //            //sequence.AppendInterval(time);
    //            //Debug.Log("55555");
    //        }

    //        sequence.AppendCallback(() =>
    //        {
    //            //Debug.Log("66666");
    //            listSteps.Clear();
    //            //listDataConnect.Clear();
    //            isMergeing = false;
    //            colorRoot = null;
    //            CheckClear();
    //            ProcessRemainingPlates();
    //        }
    //        );
    //        //Debug.Log("77777");
    //        sequence.Play();
    //    }
    //}
}
