using UnityEngine;

//DOTween.To(() => balance, x => balance = x, to, 2).OnUpdate(UpdateUI).OnComplete(UpdateUI);
namespace HardBit.Universal.Math {
    public class MathHelp : MonoBehaviour {

        public static bool IsInFront(Vector3 p1, Vector3 p2, Vector3 forward)
        {
            Vector3 toTarget = (p1 - p2).normalized;
            return Vector3.Dot(toTarget, forward) > 0;
        }

        public static Vector3 Vec3InRange(Vector3 min, Vector3 max)
        {
            Vector3 v = Vector3.zero;
            v.x = Random.Range(min.x, max.x);
            v.y = Random.Range(min.y, max.y);
            v.z = Random.Range(min.z, max.z);
            return v;
        }

    }
}
