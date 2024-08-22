using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGetHolderStatus : ICheckStatus
{
    public ColorPlate CheckHolder(ColorPlate colorPlate)
    {
        return GetSlotHolderStatus(colorPlate);
    }

    public ColorPlate GetSlotHolderStatus(ColorPlate arrow)
    {
        switch (arrow.status)
        {
            case Status.Right:
                return CheckPossibleMoveRight(arrow);
            case Status.Left:
                return CheckPossibleMoveLeft(arrow);
            case Status.Up:
                return CheckPossibleMoveUp(arrow);
            case Status.Down:
                return CheckPossibleMoveDown(arrow);
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
    ColorPlate CheckPossibleMoveRight(ColorPlate arrowRight)
    {
        //int maxCol = -1;
        int maxCol = arrowRight.Col;
        ColorPlate possiblePlate = null;

        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            if (LogicGame.Instance.ListColorPlate[i].Row == arrowRight.Row)
            {
                if (LogicGame.Instance.ListColorPlate[i].Col > maxCol /*&& LogicGame.Instance.ListColorPlate[i].status != Status.Left*/)
                {
                    if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
                    {
                        break;
                    }

                    maxCol = LogicGame.Instance.ListColorPlate[i].Col;
                    possiblePlate = LogicGame.Instance.ListColorPlate[i];
                }
            }
        }

        if (possiblePlate != null)
        {
            return possiblePlate;
        }
        else
        {
            if (arrowRight.ListValue.Count == 0)
            {
                possiblePlate = arrowRight;
                return possiblePlate;
            }
            else
            {
                return null;
            }
        }

    }
    ColorPlate CheckPossibleMoveLeft(ColorPlate arrowLeft)
    {
        //int minCol = LogicGame.Instance.cols;
        int minCol = arrowLeft.Col;
        ColorPlate possiblePlate = null;

        for (int i = LogicGame.Instance.ListColorPlate.Count - 1; i >= 0; i--)
        {
            if (LogicGame.Instance.ListColorPlate[i].Row == arrowLeft.Row)
            {
                if (LogicGame.Instance.ListColorPlate[i].Col < minCol /*&& LogicGame.Instance.ListColorPlate[i].status != Status.Right*/)
                {
                    if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
                    {
                        break;
                    }

                    minCol = LogicGame.Instance.ListColorPlate[i].Col;
                    possiblePlate = LogicGame.Instance.ListColorPlate[i];
                }
            }
        }

        if (possiblePlate != null)
        {
            return possiblePlate;
        }
        else
        {
            if (arrowLeft.ListValue.Count == 0)
            {
                possiblePlate = arrowLeft;
                return possiblePlate;
            }
            else
            {
                return null;
            }
        }
    }
    ColorPlate CheckPossibleMoveUp(ColorPlate arrowUp)
    {
        //int maxRow = -1;
        int maxRow = arrowUp.Row;
        ColorPlate possiblePlate = null;

        for (int i = 0; i < LogicGame.Instance.ListColorPlate.Count; i++)
        {
            if (LogicGame.Instance.ListColorPlate[i].Col == arrowUp.Col)
            {
                if (LogicGame.Instance.ListColorPlate[i].Row > maxRow /*&& LogicGame.Instance.ListColorPlate[i].status != Status.Down*/)
                {
                    if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
                    {
                        break;
                    }

                    maxRow = LogicGame.Instance.ListColorPlate[i].Row;
                    possiblePlate = LogicGame.Instance.ListColorPlate[i];
                }
            }
        }

        if (possiblePlate != null)
        {
            return possiblePlate;
        }
        else
        {
            if (arrowUp.ListValue.Count == 0)
            {
                possiblePlate = arrowUp;
                return possiblePlate;
            }
            else
            {
                return null;
            }
        }
    }
    ColorPlate CheckPossibleMoveDown(ColorPlate arrowDown)
    {
        //int minRow = LogicGame.Instance.rows;
        int minRow = arrowDown.Row;
        ColorPlate possiblePlate = null;

        for (int i = LogicGame.Instance.ListColorPlate.Count - 1; i >= 0; i--)
        {
            if (LogicGame.Instance.ListColorPlate[i].Col == arrowDown.Col)
            {
                if (LogicGame.Instance.ListColorPlate[i].Row < minRow /*&& LogicGame.Instance.ListColorPlate[i].status != Status.Up*/)
                {
                    if (IsBreak(LogicGame.Instance.ListColorPlate[i]))
                    {
                        break;
                    }

                    minRow = LogicGame.Instance.ListColorPlate[i].Row;
                    possiblePlate = LogicGame.Instance.ListColorPlate[i];
                }
            }
        }

        if (possiblePlate != null)
        {
            return possiblePlate;
        }
        else
        {
            if (arrowDown.ListValue.Count == 0)
            {
                possiblePlate = arrowDown;
                return possiblePlate;
            }
            else
            {
                return null;
            }
        }
    }

}
