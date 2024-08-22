using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class FindData
    {
        public int from;
        public int to;
        public List<MapPath> foundPath;
    }

    public enum MapPathEvent
    {
        NONE,
        CHECK_BACK,
        WAIT,
        END
    }

    public class MapPath : IdComponent
    {
        public MapPathEvent mapPathEvent;
        public List<MapPath> ConnectTo = new List<MapPath>();

#if UNITY_EDITOR
        void OnValidate()
        {
            if (ConnectTo != null)
                foreach (MapPath m in ConnectTo)
                    if (m == this)
                    {
                        ConnectTo.Remove(m);
                        return;
                    }
        }
#endif

        void OnDrawGizmos()
        {
            if (ConnectTo != null && ConnectTo.Count > 0)
            {
                foreach (MapPath m in ConnectTo)
                {
                    Gizmos.color = m.ConnectTo.Contains(this) ? Color.green : Color.blue;
                    Gizmos.DrawLine(transform.position, m.transform.position);
                }
            }
        }

        static List<FindData> listCache = new List<FindData>();
        public static List<MapPath> FindPathNew(MapPath From, MapPath To)
        {
            List<MapPath> result = new List<MapPath>();
            if (From.ID == To.ID) return result;

            if (listCache.Count > 0)
                for (int i = 0; i < listCache.Count; ++i)
                {
                    if (listCache[i] == null) continue;
                    if (listCache[i].from == From.ID && listCache[i].to == To.ID)
                        return listCache[i].foundPath;
                }

            List<int> listPass = new List<int>();
            List<List<MapPath>> allWay = new List<List<MapPath>>();
            allWay.Add(new List<MapPath>());
            allWay[0].Add(From);

            while (allWay.Count > 0)
            {
                List<MapPath> aWay = allWay[0];
                MapPath last = aWay[aWay.Count - 1];
                allWay.RemoveAt(0);

                for (int i = 0; i < last.ConnectTo.Count; ++i)
                {
                    var mapPath = last.ConnectTo[i];
                    if (listPass.Contains(mapPath.ID)) continue;
                    else listPass.Add(mapPath.ID);
                    listPass.Sort(delegate (int a, int b) { return a > b ? 1 : 0; });

                    List<MapPath> newWay = new List<MapPath>();
                    newWay.AddRange(aWay);
                    newWay.Add(mapPath);

                    if (mapPath.ID == To.ID)
                    {
                        listCache.Add(new FindData
                        {
                            from = From.ID,
                            to = To.ID,
                            foundPath = newWay
                        });
                        return newWay;
                    }
                    allWay.Add(newWay);
                }
            }
            return new List<MapPath>();
        }
    }
}