using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class AutoMovingMapPath : AutoMoving
    {
        public List<MapPath> listMapPath = new List<MapPath>();

        public async void UpdatePath(MapPath from, MapPath to)
        {
            listMapPath.Clear();
            List<MapPath> path = await Task.Run(() => MapPath.FindPathNew(from, to));
            listMapPath.AddRange(path);
            if (listMapPath.Count == 0)
            {
                Debug.LogError("Stuck " + gameObject.name + " From: " + from.gameObject.name + " To: " + to.gameObject.name);
                Moving = false;
                return;
            }
            InitMove();
        }

        protected override void InitMove()
        {
            if (listMapPath.Count > 0)
            {
                From = transform.position;
                To = listMapPath[0].transform.position;

                currentTime = 0;
                totalTime = Ez.GetDistance3D(From, To) / (Speed * (1 + (listPath.Count * 0.1f)));
                Moving = true;
            }
            else
            {
                Moving = false;
            }
        }

        // protected override void Update()
        // {
        //     if (!Moving) return;
        //     currentTime += Ez.TimeMod;
        //     if (currentTime >= totalTime)
        //     {
        //         transform.position = To;
        //         listMapPath.RemoveAt(0);
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