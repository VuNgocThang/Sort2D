using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public static ArrowController instance;
    public float delay = 1f;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator LightUpArrows(List<ColorPlate> listArrow)
    {
        int index = 0;
        int type = 0;

        List<ColorPlate> listColorPlate = SortListArrow(listArrow);

        while (true)
        {
            if (type == 0)
            {
                ColorPlate c = listColorPlate[index];

                foreach (ColorPlate cl in listColorPlate)
                {
                    cl.logicVisual.grow.SetActive(false);
                }

                //if (c.logicVisual.arrow.activeSelf)
                //{
                c.logicVisual.grow.SetActive(true);
                //}
            }
            else if (type == 1)
            {
                foreach (ColorPlate cl in listColorPlate)
                {
                    cl.logicVisual.grow.SetActive(false);
                }

                for (int i = 0; i < listColorPlate.Count; i++)
                {
                    if ((i + index) % 2 == 0)
                    {
                        //if (listColorPlate[i].logicVisual.arrow.activeSelf)
                        listColorPlate[i].logicVisual.grow.SetActive(true);
                    }
                }

            }

            yield return new WaitForSeconds(delay);
            index++;

            if (index == listColorPlate.Count)
            {
                index = 0;

                type++;
                if (type == 2)
                {
                    type = 0;

                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
    }

    private List<ColorPlate> SortListArrow(List<ColorPlate> listArrow)
    {
        List<ColorPlate> listSort = new List<ColorPlate>();

        List<ColorPlate> listRight = new List<ColorPlate>();
        List<ColorPlate> listUp = new List<ColorPlate>();
        List<ColorPlate> listLeft = new List<ColorPlate>();

        for (int i = 0; i < listArrow.Count; i++)
        {
            if (listArrow[i].status == Status.Right)
            {
                listRight.Add(listArrow[i]);
            }
            else if (listArrow[i].status == Status.Left)
            {
                listLeft.Add(listArrow[i]);
            }
            else if (listArrow[i].status == Status.Up)
            {
                listUp.Add(listArrow[i]);
            }
        }

        for (int i = listRight.Count - 1; i >= 0; i--)
        {
            listSort.Add(listRight[i]);
        }

        for (int i = 0; i < listUp.Count; i++)
        {
            listSort.Add(listUp[i]);
        }

        for (int i = 0; i < listLeft.Count; i++)
        {
            listSort.Add(listLeft[i]);
        }
        return listSort;
    }

    public void PlayAnim(List<ColorPlate> listArrow)
    {
        for (int i = 0; i < listArrow.Count; i++)
        {
            ColorPlate c = listArrow[i];

            if (c.CheckHolderStatus(LogicGame.Instance.listNextPlate[0]))
            {
                c.logicVisual.PlayAnimationArrowPending();
            }
            else
            {
                c.logicVisual.RefreshAnimation();
            };
        }

    }
}

