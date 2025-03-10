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

        while (true)
        {
            ColorPlate c = listArrow[index];

            foreach (ColorPlate cl in listArrow)
            {
                cl.logicVisual.grow.SetActive(false);
            }
            c.logicVisual.grow.SetActive(true);


            yield return new WaitForSeconds(delay);

            index++;
            if (index == listArrow.Count) index = 0;
        }
    }
}

