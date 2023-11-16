#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace General.Editor.Helper
{

    [RequireComponent(typeof(Camera))]
    public class CameraFrustumVisualizer : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private Camera camera;

        private void OnValidate()
        {
            camera ??= GetComponent<Camera>();
        }

        private void OnDrawGizmos()
        {
            DrawGizmoForCamera(camera, GizmoType.Active);
        }

        private void DrawGizmoForCamera(Camera camera, GizmoType gizmoType)
        {
            var nearCorners = new Vector3[4];
            var farCorners = new Vector3[4];
            var camPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            (camPlanes[1], camPlanes[2]) = (camPlanes[2], camPlanes[1]);

            for (int i = 0; i < 4; i++)
            {
                nearCorners[i] =
                    Plane3Intersect(camPlanes[4], camPlanes[i],
                        camPlanes[(i + 1) % 4]); 
                farCorners[i] =
                    Plane3Intersect(camPlanes[5], camPlanes[i],
                        camPlanes[(i + 1) % 4]); 
            }

            for (int i = 0; i < 4; i++)
            {
                Debug.DrawLine(nearCorners[i], nearCorners[(i + 1) % 4], Color.red, Time.deltaTime,
                    true);
                Debug.DrawLine(farCorners[i], farCorners[(i + 1) % 4], Color.blue, Time.deltaTime,
                    true);
                Debug.DrawLine(nearCorners[i], farCorners[i], Color.green, Time.deltaTime,
                    true);
            }
        }

        private Vector3 Plane3Intersect(Plane p1, Plane p2, Plane p3)
        {
            return ((-p1.distance * Vector3.Cross(p2.normal, p3.normal)) +
                    (-p2.distance * Vector3.Cross(p3.normal, p1.normal)) +
                    (-p3.distance * Vector3.Cross(p1.normal, p2.normal))) /
                   (Vector3.Dot(p1.normal, Vector3.Cross(p2.normal, p3.normal)));
        }
    }
}

#endif