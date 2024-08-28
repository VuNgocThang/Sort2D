using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGetListMove : ICheckListMove
{
    public List<ColorPlate> CheckListMove(ColorPlate colorPlate)
    {
        return GetListSlotVisual(colorPlate);
    }
    public List<ColorPlate> GetListSlotVisual(ColorPlate colorPlate)
    {
        switch (colorPlate.status)
        {
            //case Status.Right:
            //    return CheckPossibleMoveRight(colorPlate);
            //case Status.Left:
            //    return CheckPossibleMoveLeft(colorPlate);
            //case Status.Up:
            //    return CheckPossibleMoveUp(colorPlate);
            case Status.Down:
                return CheckPossibleMoveDown(colorPlate);
            default:
                return null;
        }
    }
    bool IsBreak(ColorPlate c)
    {
        if (c.ListValue.Count > 0 || c.status == Status.Frozen || c.status == Status.LockCoin || c.status == Status.CannotPlace || c.status == Status.Ads
            || c.status == Status.Left || c.status == Status.Right || c.status == Status.Up || c.status == Status.Down) return true;
        else return false;
    }
    //List<ColorPlate> CheckPossibleMoveRight(ColorPlate arrowRight)
    //{
    //    //int maxCol = -1;
    //    int maxCol = arrowRight.Col;
    //    List<ColorPlate> possiblePlate = new List<ColorPlate>();

    //    for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
    //    {
    //        if (LogicGame.Instance.ListColorPlate[i].Row == arrowRight.Row)
    //        {
    //            if (LogicGame.Instance.ListColorPlate[i].Col > maxCol /*&& LogicGame.Instance.ListColorPlate[i].status != Status.Left*/)
    //            {
    //                if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
    //                {
    //                    break;
    //                }
    //                else if (LogicGame.Instance.ListColorPlate[i].status == Status.SpeicalArrowRight)
    //                {
    //                    CheckPossibleSpeicalArrowRight(LogicGame.Instance.ListColorPlate[i]);
    //                }
    //                else
    //                {
    //                    maxCol = LogicGame.Instance.ListColorPlate[i].Col;
    //                    possiblePlate = LogicGame.Instance.ListColorPlate[i];
    //                }

    //            }
    //        }
    //    }

    //    if (possiblePlate != null)
    //    {
    //        return possiblePlate;
    //    }
    //    else
    //    {
    //        if (arrowRight.ListValue.Count == 0)
    //        {
    //            possiblePlate = arrowRight;
    //            return possiblePlate;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //}
    //List<ColorPlate> CheckPossibleMoveLeft(ColorPlate arrowLeft)
    //{
    //    //int minCol = LogicGame.Instance.cols;
    //    int minCol = arrowLeft.Col;
    //    ColorPlate possiblePlate = null;

    //    for (int i = LogicGame.Instance.ListColorPlate.Count - 1; i >= 0; i--)
    //    {
    //        if (LogicGame.Instance.ListColorPlate[i].Row == arrowLeft.Row)
    //        {
    //            if (LogicGame.Instance.ListColorPlate[i].Col < minCol /*&& LogicGame.Instance.ListColorPlate[i].status != Status.Right*/)
    //            {
    //                if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
    //                {
    //                    break;
    //                }

    //                minCol = LogicGame.Instance.ListColorPlate[i].Col;
    //                possiblePlate = LogicGame.Instance.ListColorPlate[i];
    //            }
    //        }
    //    }

    //    if (possiblePlate != null)
    //    {
    //        return possiblePlate;
    //    }
    //    else
    //    {
    //        if (arrowLeft.ListValue.Count == 0)
    //        {
    //            possiblePlate = arrowLeft;
    //            return possiblePlate;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //}
    //List<ColorPlate> CheckPossibleMoveUp(ColorPlate arrowUp)
    //{
    //    //int maxRow = -1;
    //    int maxRow = arrowUp.Row;
    //    ColorPlate possiblePlate = null;

    //    for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
    //    {
    //        if (LogicGame.Instance.ListColorPlate[i].Col == arrowUp.Col)
    //        {
    //            if (LogicGame.Instance.ListColorPlate[i].Row > maxRow /*&& LogicGame.Instance.ListColorPlate[i].status != Status.Down*/)
    //            {
    //                if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
    //                {
    //                    break;
    //                }

    //                maxRow = LogicGame.Instance.ListColorPlate[i].Row;
    //                possiblePlate = LogicGame.Instance.ListColorPlate[i];
    //            }
    //        }
    //    }

    //    if (possiblePlate != null)
    //    {
    //        return possiblePlate;
    //    }
    //    else
    //    {
    //        if (arrowUp.ListValue.Count == 0)
    //        {
    //            possiblePlate = arrowUp;
    //            return possiblePlate;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //}
    List<ColorPlate> CheckPossibleMoveDown(ColorPlate arrowDown)
    {
        //int minRow = LogicGame.Instance.rows;
        int minRow = arrowDown.Row;
        List<ColorPlate> listPossiblePlate = new List<ColorPlate>();

        for (int i = LogicGame.Instance.ListColorPlate.Count - 1; i >= 0; i--)
        {
            if (LogicGame.Instance.ListColorPlate[i].Col == arrowDown.Col)
            {
                if (LogicGame.Instance.ListColorPlate[i].Row < minRow)
                    if (LogicGame.Instance.ListColorPlate[i].Row < minRow)
                    {
                        if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
                        {
                            break;
                        }


                        if (LogicGame.Instance.ListColorPlate[i].status == Status.SpeicalArrowRight)
                        {
                            Debug.Log("check special arrow");
                            listPossiblePlate.Add(LogicGame.Instance.ListColorPlate[i]);

                            listPossiblePlate.AddRange(CheckPossibleSpeicalArrowRight(LogicGame.Instance.ListColorPlate[i]));
                            break;
                        }
                        else
                        {
                            Debug.Log("no check anything");
                            minRow = LogicGame.Instance.ListColorPlate[i].Row;
                            listPossiblePlate.Add(LogicGame.Instance.ListColorPlate[i]);
                        }
                    }
            }
        }

        for (int i = 0; i < listPossiblePlate.Count; i++)
            Debug.Log("possible: " + listPossiblePlate[i].name);

        if (listPossiblePlate.Count > 0)
        {
            return listPossiblePlate;
        }
        else
        {
            if (arrowDown.ListValue.Count == 0)
            {
                listPossiblePlate.Add(arrowDown);
                return listPossiblePlate;
            }
            else
            {
                return null;
            }
        }
    }

    List<ColorPlate> CheckPossibleSpeicalArrowRight(ColorPlate specialRight)
    {
        List<ColorPlate> listPossible = new List<ColorPlate>();
        int maxCol = specialRight.Col;

        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            if (LogicGame.Instance.ListColorPlate[i].Row == specialRight.Row)
            {
                if (LogicGame.Instance.ListColorPlate[i].Col > maxCol)
                {
                    if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
                    {
                        break;
                    }

                    maxCol = LogicGame.Instance.ListColorPlate[i].Col;
                    listPossible.Add(LogicGame.Instance.ListColorPlate[i]);
                }
            }
        }

        if (listPossible.Count > 0)
        {
            return listPossible;
        }
        else
        {
            if (specialRight.ListValue.Count == 0)
            {
                listPossible.Add(specialRight);
                return listPossible;
            }
            else
            {
                return null;
            }
        }
    }


}
