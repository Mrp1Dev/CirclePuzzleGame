using System;
using System.Collections;
using UnityEngine;

namespace MUtility
{
    public static class MUtils
    {
        //-----------EXTENSION METHODS--------------

        public static Vector3 X0Z(this Vector3 vec) => new Vector3(vec.x, 0, vec.z);
        public static Vector3 X0Y(this Vector2 vec) => new Vector3(vec.x, 0, vec.y);
        public static Vector3 WithY(this Vector3 vec, float y) => new Vector3(vec.x, y, vec.z);
        public static Vector3 WithX(this Vector3 vec, float x) => new Vector3(x, vec.y, vec.z);
        public static Vector3 WithZ(this Vector3 vec, float z) => new Vector3(vec.x, vec.y, z);
        public static Vector3 WithZ(this Vector2 vec, float z) => new Vector3(vec.x, vec.y, z);
        public static Vector2 WithY(this Vector2 vec, float y) => new Vector2(vec.x, y);
        public static Vector2 WithX(this Vector2 vec, float x) => new Vector2(x, vec.y);
        public static Vector2 XZ(this Vector3 vec) => new Vector2(vec.x, vec.z);
        public static Vector2 XY(this Vector3 vec) => new Vector2(vec.x, vec.y);
        public static Quaternion RotationTo(this Quaternion from, Quaternion to) => Quaternion.Inverse(from) * to;

        public static float DistTo(this Component owner, Component other) =>
            (owner.transform.position - other.transform.position).magnitude;

        public static float DistTo(this GameObject owner, GameObject other) =>
            (owner.transform.position - other.transform.position).magnitude;

        public static Vector3 DirTo(this Vector3 vec, Vector3 other) => (other - vec).normalized;
        public static Vector2 DirTo(this Vector2 vec, Vector2 other) => (other - vec).normalized;
        public static bool LongerThan(this Vector3 lhs, Vector3 rhs) => lhs.sqrMagnitude > rhs.sqrMagnitude;
        public static bool ShorterThan(this Vector3 lhs, Vector3 rhs) => lhs.sqrMagnitude < rhs.sqrMagnitude;
        public static bool LongerThan(this Vector2 lhs, Vector2 rhs) => lhs.sqrMagnitude > rhs.sqrMagnitude;
        public static bool ShorterThan(this Vector2 lhs, Vector2 rhs) => lhs.sqrMagnitude < rhs.sqrMagnitude;
        public static float Sqr(this float f) => Mathf.Pow(f, 2);
        public static float Sqrt(this float f) => Mathf.Sqrt(f);

        public static float ReMap(this float f, float min, float max, float newMin, float newMax) =>
            Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(min, max, f));

        public static void Delay(this MonoBehaviour owner, Action action, float delay) =>
            owner.StartCoroutine(DelayInternal(action, delay, false));

        public static void DelayUnscaled(this MonoBehaviour owner, Action action, float delay) =>
            owner.StartCoroutine(DelayInternal(action, delay, true));

        public static void LoopWithDelay(this MonoBehaviour owner,
            Action action,
            float delay,
            bool startWithDelay = false
        ) => LoopWithDelayInternal(action, delay, startWithDelay, false);

        public static void LoopWithDelayUnscaled(this MonoBehaviour owner,
            Action action,
            float delay,
            bool startWithDelay = false
        ) => LoopWithDelayInternal(action, delay, startWithDelay, true);

        public static bool HasComponent<T>(this Component component) where T : MonoBehaviour =>
            component.TryGetComponent<T>(out _);

        public static bool IsInLayerMask(this GameObject go, LayerMask layerMask) => ((1 << go.layer) & layerMask) != 0;

        public static void SetLayerIncludingChildren(this GameObject go, int layerIndex)
        {
            go.layer = layerIndex;
            for (var i = 0; i < go.transform.childCount; i++)
                go.transform.GetChild(i).gameObject.SetLayerIncludingChildren(layerIndex);
        }

        public static void SetActiveIncludingChildren(this GameObject go, bool active)
        {
            go.SetActive(active);
            for (var i = 0; i < go.transform.childCount; i++) go.transform.GetChild(i).gameObject.SetActive(active);
        }

        public static bool ApproxEquals(this Color lhs, Color rhs) =>
            Mathf.Approximately(lhs.r, rhs.r) && Mathf.Approximately(lhs.g, rhs.g) &&
            Mathf.Approximately(lhs.b, rhs.b) && Mathf.Approximately(lhs.a, rhs.a);

        //-------------NORMAL FUNCTIONS---------------

        public static bool Approx(float a, float b, float tolerance = 0.01f) => Mathf.Abs(a - b) <= tolerance;

        public static int MaskToIndex(LayerMask mask)
        {
            var res = Mathf.Log(mask.value, 2f);
            if (res % 1.0f > Mathf.Epsilon)
            {
                Debug.LogError(
                    "MUtils.MaskToIndex expects a layermask which only has one layer ticked. Layermask provided has more than one layer.");
            }

            return Mathf.RoundToInt(res);
        }

        private static IEnumerator DelayInternal(Action action, float delay, bool realtime)
        {
            if (realtime)
                yield return new WaitForSecondsRealtime(delay);
            else yield return new WaitForSeconds(delay);
            action();
        }

        private static IEnumerator LoopWithDelayInternal(Action action, float time, bool startwithDelay, bool realtime)
        {
            while (true)
            {
                if (startwithDelay)
                    action();
                if (realtime)
                    yield return new WaitForSecondsRealtime(time);
                else yield return new WaitForSeconds(time);
                if (!startwithDelay) action();
            }
        }
    }
}
