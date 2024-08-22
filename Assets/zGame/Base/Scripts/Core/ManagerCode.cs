using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// by nt.Dev93
namespace ntDev
{
    public static class Ez
    {
        public static void Log(object o)
        {
            if (CoreGame.DEV) Debug.Log(o);
        }
        public static void LogError(object o)
        {
            if (CoreGame.DEV) Debug.LogError(o);
        }
        public static float TimeMod => Time.deltaTime * ManagerGame.TIME_SCALE;
        public static float FixedTimeMod => Time.fixedDeltaTime * ManagerGame.TIME_SCALE;

        // Transform
        public static void PosX(this Component obj, float v) => obj.transform.position = new Vector3(v, obj.transform.position.y, obj.transform.position.z);
        public static void PosX(this GameObject obj, float v) => obj.transform.position = new Vector3(v, obj.transform.position.y, obj.transform.position.z);
        public static void PosY(this Component obj, float v) => obj.transform.position = new Vector3(obj.transform.position.x, v, obj.transform.position.z);
        public static void PosY(this GameObject obj, float v) => obj.transform.position = new Vector3(obj.transform.position.x, v, obj.transform.position.z);
        public static void PosZ(this Component obj, float v) => obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, v);
        public static void PosZ(this GameObject obj, float v) => obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, v);
        public static Vector3 Pos(this Component obj) => obj == null || obj.transform == null ? Vector3.zero : obj.transform.position;
        public static Vector3 Pos(this GameObject obj) => obj == null || obj.transform == null ? Vector3.zero : obj.transform.position;
        public static void Pos(this Component obj, Vector3 pos) => obj.transform.position = new Vector3(pos.x, pos.y, pos.y);
        public static void Pos(this GameObject obj, Vector3 pos) => obj.transform.position = new Vector3(pos.x, pos.y, pos.y);
        public static void Pos2D(this Component obj, Vector3 pos) => obj.transform.position = new Vector3(pos.x, pos.y, pos.y);
        public static void Pos2D(this GameObject obj, Vector3 pos) => obj.transform.position = new Vector3(pos.x, pos.y, pos.y);
        public static void LposX(this Component obj, float v) => obj.transform.localPosition = new Vector3(v, obj.transform.localPosition.y, obj.transform.localPosition.z);
        public static void LposX(this GameObject obj, float v) => obj.transform.localPosition = new Vector3(v, obj.transform.localPosition.y, obj.transform.localPosition.z);
        public static void LposY(this Component obj, float v) => obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, v, obj.transform.localPosition.z);
        public static void LposY(this GameObject obj, float v) => obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, v, obj.transform.localPosition.z);
        public static Vector2 Lpos(this Component obj) => obj == null || obj.transform == null ? Vector3.zero : obj.transform.localPosition;
        public static Vector2 Lpos(this GameObject obj) => obj == null || obj.transform == null ? Vector3.zero : obj.transform.localPosition;
        public static void Lpos(this Component obj, Vector2 pos) => obj.transform.localPosition = new Vector3(pos.x, pos.y, obj.transform.localPosition.z);
        public static void Lpos(this GameObject obj, Vector2 pos) => obj.transform.localPosition = new Vector3(pos.x, pos.y, obj.transform.localPosition.z);
        public static void Lpos2D(this Component obj, Vector2 pos) => obj.transform.localPosition = new Vector3(pos.x, pos.y, pos.y);
        public static void Lpos2D(this GameObject obj, Vector2 pos) => obj.transform.localPosition = new Vector3(pos.x, pos.y, pos.y);

        // Distance
        public static float GetDistance(Vector2 pos1, Vector2 pos2)
        {
            return (pos1 - pos2).magnitude;
        }
        public static float GetDistance3D(Vector3 pos1, Vector3 pos2)
        {
            return (pos1 - pos2).magnitude;
        }

        public static void RotateToY(this Transform me, Vector3 target, float d = 0, float turnRate = 1000f)
        {
            Vector2 v = new Vector2(target.x - me.position.x, target.z - me.position.z);
            me.rotation = Quaternion.RotateTowards(me.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg + d, 0.0f), turnRate);
        }

        public static void RotateToY(this Transform me, Vector3 from, Vector3 to, float d = 0, float turnRate = 1000f)
        {
            Vector2 v = new Vector2(from.x - to.x, from.z - to.z);
            me.rotation = Quaternion.RotateTowards(me.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg + d, 0.0f), turnRate);
        }

        public static void RotateToZ(this Transform me, Transform target, float d = 0, float turnRate = 1000f)
        {
            Vector2 v = new Vector2(target.position.x - me.position.x, target.position.y - me.position.y);
            if (v.magnitude < 0.05f) return;
            me.rotation = Quaternion.RotateTowards(me.rotation, Quaternion.Euler(0.0f, Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg + d, 0.0f), turnRate);
        }

        // Random
        public static int Random(int from = 0, int to = 100)
        {
            return UnityEngine.Random.Range(from, to);
        }
        public static float Random(float from, float to)
        {
            return UnityEngine.Random.Range(from, to);
        }
        public static int Randomize(this int value, float from = 0.8f, float to = 1.0f)
        {
            return (int)(value * UnityEngine.Random.Range(from, to));
        }
        public static float Randomize(this float value, float from = 0.8f, float to = 1.0f)
        {
            return value * UnityEngine.Random.Range(from, to);
        }

        // String
        public static string ToString(this int value, int n = 3)
        {
            string result = "" + (Math.Pow(10, n) + value);
            result = result.Substring(1, result.Length - 1);
            return result;
        }
        public static bool IsUnicode(string str)
        {
            return System.Text.Encoding.UTF8.GetByteCount(str) == str.Length;
        }
        public static string ConvertMoney(this double value, bool KM = false, bool change = false)
        {
            return ConvertMoney((int)value, KM, change);
        }
        public static string ConvertMoney(this float value, bool KM = false, bool change = false)
        {
            return ConvertMoney((int)value, KM, change);
        }
        public static string ConvertMoney(this int value, bool KM = false, bool change = false)
        {
            string first = change ? (value >= 0 ? "+" : "-") : "";
            if (value == 0) return first + "0";
            string last = "";
            value = Math.Abs(value);

            if (KM && value > 10000)
            {
                string str = $"{value}";
                if (value > 999999999)
                {
                    value = int.Parse(str.Substring(0, str.Length - 9));
                    last = $".{str.Substring(str.Length - 9, 1)}B";
                }
                else if (value > 999999)
                {
                    value = int.Parse(str.Substring(0, str.Length - 6));
                    last = $".{str.Substring(str.Length - 6, 1)}M";
                }
                else if (value > 999)
                {
                    value = int.Parse(str.Substring(0, str.Length - 3));
                    last = $".{str.Substring(str.Length - 3, 1)}K";
                }
            }

            string begin = $"{value}";
            string result = "";
            while (begin.Length > 3)
            {
                result = $",{begin.Substring(begin.Length - 3, 3)}{result}";
                begin = begin.Substring(0, begin.Length - 3);
            }
            return first + begin + result + last;
        }
        public static int GetMoneyValue(this string str)
        {
            str = str.Replace(".", "");
            str = str.Replace(",", "");
            str = str.Replace(" ", "");
            str = str.Replace("+", "");
            str = str.Replace("-", "");
            return int.Parse(str);
        }
        public static string StringShorten(this string value, int len)
        {
            string str = $"{value}";
            return str.Length > len ? $"{str.Substring(0, len - 1)}..." : str;
        }
        public static string ConvertTime(this int value, bool h = false, bool d = false)
        {
            return ConvertTime((long)value, h, d);
        }
        public static string ConvertTime(this double value, bool h = false, bool d = false)
        {
            return ConvertTime((long)value, h, d);
        }
        public static string ConvertTime(this float value, bool h = false, bool d = false)
        {
            return ConvertTime((long)value, h, d);
        }
        public static string ConvertTime(this long value, bool h = false, bool d = false)
        {
            if (value < 0) value = 0;
            long day = value / (60 * 60 * 24);
            long hour = value / (60 * 60);
            long min = value / 60;
            long second = value - min * 60;

            string str = "";

            if (d)
            {
                min = min - hour * 60;
                hour = hour - day * 24;
                str = "" + day + "D ";
                str = hour > 9 ? $"{str}{hour}" : $"{str}0{hour}";
                str = min > 9 ? $"{str}:{min}" : $"{str}:0{min}";
                str = second > 9 ? $"{str}:{second}" : $"{str}:0{second}";
                return str;
            }
            else if (h)
            {
                min = min - hour * 60;
                str = hour > 9 ? $"{hour}" : $"0{hour}";
                str = min > 9 ? $"{str}:{min}" : $"{str}:0{min}";
                str = second > 9 ? $"{str}:{second}" : $"{str}:0{second}";
                return str;
            }
            else
            {
                str = min > 9 ? $"{min}" : $"0{min}";
                str = second > 9 ? $"{str}:{second}" : $"{str}:0{second}";
                return str;
            }
        }

        // List
        public static void Refresh<T>(this List<T> list) where T : Component
        {
            for (int i = 0; i < list.Count; ++i)
            {
                // list[i].transform.DOKill(false);
                list[i].gameObject.SetActive(false);
            }
        }
        public static T GetClone<T>(this List<T> list, int t = 0, Transform papa = null) where T : Component
        {
            if (list == null || list.Count < 1) return null;
            Transform parent = papa == null ? list[0].transform.parent : papa;
            for (int i = 0; i < list.Count; ++i)
            {
                if (!list[i].gameObject.activeSelf)
                {
                    list[i].transform.SetParent(parent);
                    list[i].gameObject.SetActive(true);
                    return list[i];
                }
            }

            T obj = UnityEngine.Object.Instantiate(list[t], parent);
            list.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }
        public static T GetRandom<T>(this T[] arr)
        {
            if (arr == null || arr.Length < 1) return default(T);
            return arr[Ez.Random(0, arr.Length)];
        }
        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count < 1) return default(T);
            return list[Ez.Random(0, list.Count)];
        }
        public static T GetCloneByID<T>(this List<T> list, int id, Transform papa = null) where T : IdComponent
        {
            if (list == null || list.Count < 1) return null;
            Transform parent = papa == null ? list[0].transform.parent : papa;
            T prefab = null;
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].ID != id) continue;
                if (list[i].gameObject.scene.name != null)
                {
                    if (!list[i].gameObject.activeSelf)
                    {
                        list[i].transform.SetParent(parent);
                        list[i].gameObject.SetActive(true);
                        return list[i];
                    }
                }
                if (prefab == null) prefab = list[i];
            }

            if (prefab == null) prefab = list[0];

            T obj = UnityEngine.Object.Instantiate(prefab, parent);
            list.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public static List<int> GetRandomListFair(List<float> list, int count)
        {
            List<int> listT = new List<int>();
            for (int i = 0; i < list.Count; ++i)
            {
                while (list[i] >= 1)
                {
                    listT.Add(i);
                    --list[i];
                }
            }

            while (listT.Count < count)
            {
                int t = GetRandomWithRatio(list);
                list[t] -= 1;
                if (list[t] < 0) list[t] = 0;
                listT.Add(t);
            }

            List<int> result = new List<int>();
            while (listT.Count > 0)
            {
                int r = UnityEngine.Random.Range(0, listT.Count);
                result.Add(listT[r]);
                listT.RemoveAt(r);
            }
            return result;
        }

        public static int GetRandomWithRatio(float[] list)
        {
            List<float> listRatio = new List<float>();
            listRatio.AddRange(list);
            return GetRandomWithRatio(listRatio);
        }

        public static int GetRandomWithRatio(List<float> list)
        {
            float s = 0;
            foreach (float i in list) s += i;

            float r = UnityEngine.Random.Range(0, s);
            int t = 0;
            s = list[t];
            while (r >= s)
            {
                ++t;
                s += list[t];
            }
            return t;
        }

        public static void SetNativeSize(this Image img, float mX, float mY)
        {
            img.SetNativeSize();
            float x = img.rectTransform.sizeDelta.x < mX ? img.rectTransform.sizeDelta.x : mX;
            float y = img.rectTransform.sizeDelta.y < mY ? img.rectTransform.sizeDelta.y : mY;
            img.rectTransform.sizeDelta = new Vector2(x, y);
        }

        public static int EzBinarySearch(this List<int> list, int id)
        {
            if (list.Count == 0) return -1;
            int min = 0;
            int max = list.Count;
            int t = 0;
            do
            {
                t = (min + max) / 2;
                if (id == list[t]) return t;
                if (id > t) min = t;
                else max = t;
            }
            while (t > 0 && t < list.Count - 1);
            return -1;
        }

        public static void UpdateRootBone(this SkinnedMeshRenderer skinnedMeshRenderer, Transform root)
        {
            skinnedMeshRenderer.rootBone = root;

            Transform[] arr = root.GetComponentsInChildren<Transform>();
            Transform[] arrBones = new Transform[skinnedMeshRenderer.bones.Length];
            for (int i = 0; i < skinnedMeshRenderer.bones.Length; ++i)
            {
                foreach (Transform t in arr)
                {
                    if (skinnedMeshRenderer.bones[i].gameObject.name == t.gameObject.name)
                    {
                        arrBones[i] = t;
                        break;
                    }
                }
            }

            skinnedMeshRenderer.bones = arrBones;
        }

        public static Color GetColor(this Vector4 v)
        {
            return new Color(v.x / 255f, v.y / 255f, v.z / 255f, v.w);
        }
    }
}
