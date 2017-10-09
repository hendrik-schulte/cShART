using UnityEngine;

namespace cShART
{
    /// <summary>
    /// Class providing various methods for converting ARTObjects to useful objects for Unity3d-Engine
    /// </summary>
    public static class UnityParser
    {
        /// <summary>
        ///     Method used for parsing ARTMatrices to Unity3d matrices (Matrix4x4).
        /// </summary>
        /// <returns>
        ///     Returns the converted Matrix4x4 object.
        /// </returns>
        /// <param name="artM">
        ///     To be converted ARTMatrix
        /// </param>
        /// <param name="transposed"></param>
        public static Matrix4x4 UnityMatrix(this ARTMatrix artM, bool transposed)
        {
            var uM = new Matrix4x4();
            var v4_0 = new Vector4(artM.GetB0(), artM.GetB1(), artM.GetB2(), 0);
            var v4_1 = new Vector4(artM.GetB3(), artM.GetB4(), artM.GetB5(), 0);
            var v4_2 = new Vector4(artM.GetB6(), artM.GetB7(), artM.GetB8(), 0);
            var v4_end = new Vector4(0, 0, 0, 1);
            uM.SetColumn(0, v4_0);
            uM.SetColumn(1, v4_1);
            uM.SetColumn(2, v4_2);
            uM.SetColumn(3, v4_end);
            return transposed ? uM.transpose : uM;
        }

        /// <summary>
        /// Method used for parsing ARTMatrices to Unity3d matrices (Matrix4x4).
        /// </summary>
        /// <param name="artM"></param>
        /// <param name="transposed"></param>
        /// <returns></returns>
        public static Matrix4x4 ParseMatrix(ARTMatrix artM, bool transposed)
        {
            return artM.UnityMatrix(transposed);
        }

        /// <summary>
        ///     This Method parses various object such as vectors or matrices to a quaternion with a directional and an up vector.
        /// </summary>
        /// <param name="directionV">Input directional vector</param>
        /// <param name="upV">Input up vector</param>
        /// <returns>Returns the quaternion</returns>
        public static Quaternion ParseQuaternion(Vector3 directionV, Vector3 upV)
        {
            return Quaternion.LookRotation(directionV, upV);
        }

        public static Quaternion ParseQuaternion(ARTMatrix artM)
        {
            var mat = ParseMatrix(artM, true);
            var targetDir = mat.MultiplyVector(Vector3.forward);
            var targetUp = mat.MultiplyVector(Vector3.up);
            return ParseQuaternion(targetDir, targetUp);
        }

        public static Quaternion ToQuaternion(this Matrix4x4 mat)
        {
            var targetDir = mat.MultiplyVector(Vector3.forward);
            var targetUp = mat.MultiplyVector(Vector3.up);
            return ParseQuaternion(targetDir, targetUp);
        }

        /// <summary>
        /// This method parses various input objects and computes a quaternion to use with a camera object in Unity3d.
        /// E.g. mapping the rotation of a ART 6d object to the camera.
        /// If you wish to inverse an axis just change the correspondent boolean to true.
        /// </summary>
        /// <param name="artM">Input ARTMatrix</param>
        /// <param name="invX">Boolean used to inverse X axis</param>
        /// <param name="invY">Boolean used to inverse Y axis</param>
        /// <param name="invZ">Boolean used to inverse Z axis</param>
        /// <returns>Returns a quaternion</returns>
        public static Quaternion ParseCameraQuaternion(this ARTMatrix artM, bool invX, bool invY, bool invZ)
        {
            var target = ParseQuaternion(artM);
            float angle = 0;
            var axis = Vector3.zero;
            target.ToAngleAxis(out angle, out axis);
            if (invX) axis.x *= -1;
            if (invY) axis.y *= -1;
            if (invZ) axis.z *= -1;
            return target = Quaternion.AngleAxis(angle, axis);
        }

        /// <summary>
        /// </summary>
        /// <param name="mat">Input Matrix4x4 matrix</param>
        /// <returns></returns>
        public static Quaternion ParseCameraQuaternion(Matrix4x4 mat, bool invX, bool invY, bool invZ)
        {
            var target = mat.ToQuaternion();
            float angle = 0;
            var axis = Vector3.zero;
            target.ToAngleAxis(out angle, out axis);
            if (invX) axis.x *= -1;
            if (invY) axis.y *= -1;
            if (invZ) axis.z *= -1;
            return target = Quaternion.AngleAxis(angle, axis);
        }

        public static Quaternion ParseCameraQuaternion(Quaternion q, bool invX, bool invY, bool invZ)
        {
            float angle = 0;
            var axis = Vector3.zero;
            q.ToAngleAxis(out angle, out axis);
            if (invX) axis.x *= -1;
            if (invY) axis.y *= -1;
            if (invZ) axis.z *= -1;
            return q = Quaternion.AngleAxis(angle, axis);
        }

        /// <summary>
        ///     Parses an ARTPoint to a vector.
        /// </summary>
        /// <param name="ap">To be converted ARTPoint</param>
        /// <returns>Returns the converted vector.</returns>
        public static Vector3 ParseVector(ARTPoint ap)
        {
            var v = new Vector3(ap.GetX(), ap.GetY(), ap.GetZ());
            return v;
        }
    }
}