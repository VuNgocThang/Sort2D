using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class AutoMoving : IdComponent
    {
        protected List<Vector3> listPath = new List<Vector3>();

        public virtual void UpdatePath(Vector3 v)
        {
            listPath.Add(v);
            while (listPath.Count > 10)
            {
                listPath.RemoveAt(0);
                transform.position = listPath[0];
            }
            InitMove();
        }

        protected virtual void InitMove()
        {
            if (listPath.Count > 0)
            {
                From = transform.position;
                To = listPath[0];

                currentTime = 0;
                totalTime = Ez.GetDistance3D(From, To) / (Speed * (1 + (listPath.Count * 0.1f)));
                Moving = true;
            }
            else
            {
                Moving = false;
            }
        }

        protected bool Moving = false;
        protected Vector3 From;
        protected Vector3 To;
        protected float currentTime;
        protected float totalTime;

        public float Speed = 6f;
        public float TurnRate = 3f;

        // protected virtual void Update()
        // {
        //     if (!Moving) return;
        //     currentTime += Ez.TimeMod;
        //     if (currentTime >= totalTime)
        //     {
        //         transform.position = To;
        //         listPath.RemoveAt(0);
        //         InitMove();
        //     }
        //     else
        //     {
        //         transform.position = Vector3.Lerp(From, To, currentTime / totalTime);
        //         transform.RotateToY(To, 0, TurnRate);
        //     }
        // }
    }
}
