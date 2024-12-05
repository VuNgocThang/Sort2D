using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LogicTest : MonoBehaviour
{
    public List<int> lst;

    public List<int> sortedList;

    public int diff;

    private void Start()
    {
        diff = 3;
        InitList();
        Debug.Log("lst: " + string.Join(", ", lst));
        sortedList = SortList(lst);
        Debug.Log("sorted List:" + string.Join(", ", sortedList));
    }
    void InitList()
    {
        int total = Random.Range(10, 20);
        for (int i = 0; i < total; i++)
        {
            int a = Random.Range(0, 5);
            lst.Add(a);
        }

    }

    static List<int> SortList(List<int> lst)
    {
        Dictionary<int, (int count, int index)> countDict = new Dictionary<int, (int count, int index)>();

        for (int i = 0; i < lst.Count; i++)
        {
            int value = lst[i];
            if (countDict.ContainsKey(value))
            {
                countDict[value] = (countDict[value].count + 1, countDict[value].index);
            }
            else
            {
                countDict[value] = (1, i);
            }
        }

        List<int> sortedList = lst.OrderByDescending(x => (countDict[x].count, -countDict[x].index)).ToList();

        return sortedList;
    }


    //public IEnumerator Transfer(ConnectPlate connectControl, List<Minmon> listPieceTransfer, Leaf sender,
    //Leaf receiver, UnityAction action)
    //{
    //    if ((slot1.holdingLeaf, slot2.holdingLeaf) != (sender, receiver) &&
    //        (slot1.holdingLeaf, slot2.holdingLeaf) != (receiver, sender))
    //    {
    //        connectControl.FinishConnect();
    //        yield break;
    //    }

    //    yield return new WaitWhile((() =>
    //        sender.leafState != Leaf.LeafState.OnTable || receiver.leafState != Leaf.LeafState.OnTable));

    //    int receiverSlotRemain = 6 - receiver.listHavingMinmon.Count;
    //    if (listPieceTransfer.Count > receiverSlotRemain)
    //    {
    //        listPieceTransfer.RemoveRange(receiverSlotRemain, listPieceTransfer.Count - receiverSlotRemain);
    //    }

    //    if (listPieceTransfer.Count == 0)
    //    {
    //        if (action != null)
    //        {
    //            action();
    //        }

    //        yield break;
    //    }

    //    float timeConnect = (listPieceTransfer.Count - 1) * 0.2f + 0.6f;
    //    sender.SendPiece(listPieceTransfer);
    //    receiver.ReceivePiece(listPieceTransfer);

    //    receiver.SortPiece(listPieceTransfer);
    //    Sequence transferSequence = DOTween.Sequence();
    //    for (var index = 0; index < listPieceTransfer.Count; index++)
    //    {
    //        var piece = listPieceTransfer[index];
    //        int a = index;
    //        Quaternion before = piece.animMinmon.transform.rotation;
    //        Vector3 distance = piece.transform.position -
    //                           receiver.listPieceHolder[piece.slotInPlate].transform.position;
    //        float angle = Mathf.Atan2(distance.z, distance.x) * Mathf.Rad2Deg + 90;
    //        transferSequence.Insert(a * 0.2f, piece.animMinmon.transform.DOLocalRotate(new Vector3(
    //            0, -angle, 0), 0.1f));
    //        transferSequence.InsertCallback(a * 0.2f, () => { piece.animMinmon.Play("Ready"); });
    //        transferSequence.Insert(a * 0.2f + 0.1f, piece.animMinmon.transform.DOLocalMoveY(0.3f, 0.2f)) /*.SetEase()*/;
    //        transferSequence.Insert(a * 0.2f + 0.3f, piece.animMinmon.transform.DOLocalMoveY(0, 0.2f));

    //        Tweener moveTween = piece.transform
    //            .DOMove(receiver.listPieceHolder[piece.slotInPlate].transform.position, 0.4f).OnStart((() =>
    //            {
    //                sender.SortPiece(new List<Minmon>(/*listPieceTransfer*/)
    //                {
    //                piece
    //                });
    //                SoundManager.Instance.sfxAudioSource.PlayOneShot(SoundManager.Instance.soundConnect);
    //                piece.animMinmon.Play("Jump");
    //            })).OnComplete(() =>
    //            {
    //                GameManager.Instance.jumpParticlePool.Spawn(receiver.transform.position, true);
    //            });
    //        piece.moveTween = moveTween;
    //        transferSequence.Insert(a * 0.2f + 0.1f, moveTween);
    //        transferSequence.InsertCallback(a * 0.2f + 0.5f,
    //            () =>
    //            {
    //                receiver.SortPiece(new List<Minmon>());
    //                sender.SortPiece(new List<Minmon>());
    //                piece.animMinmon.transform.DORotateQuaternion(before, 0.1f)
    //                    .OnComplete(() => { piece.moveTween = null; });
    //            });
    //    }



    //    transferSequence.InsertCallback(timeConnect, (() =>
    //    {
    //        sender.SortPiece(new List<Minmon>(listPieceTransfer));
    //        sender.CheckEmpty();
    //        receiver.CheckFullCake(listPieceTransfer[0].idType);

    //        if (action != null)
    //        {
    //            action();
    //        }
    //    }));
    //}
}
