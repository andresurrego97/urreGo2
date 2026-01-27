using UnityEditor;

namespace Unity.Splines.Examples
{
    [CustomEditor(typeof(AnimateCarAlongSpline))]
    public class AnimateCarAlongSplineEditor : UnityEditor.Editor
    {
        void OnEnable()
        {
            ((AnimateCarAlongSpline)target).Initialize();
        }
    }
}
