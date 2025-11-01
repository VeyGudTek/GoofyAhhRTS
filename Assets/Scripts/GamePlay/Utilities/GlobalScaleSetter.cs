using UnityEngine;

namespace Source.GamePlay.Utilities
{
    public static class GlobalScaleSetter
    {
        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;

            if (transform.lossyScale.x != 0 && transform.lossyScale.y != 0  && transform.lossyScale.z != 0)
            {
                transform.localScale = new Vector3(
                    globalScale.x / transform.lossyScale.x,
                    globalScale.y / transform.lossyScale.y,
                    globalScale.z / transform.lossyScale.z
                );
            }
        }
    }
}
