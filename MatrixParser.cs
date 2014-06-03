using UnityEngine;

namespace cShART
{
    /// <summary>
    /// Class providing various methods for converting ARTObjects to useful objects for Unity3d-Engine
    /// </summary>
    public class UnityParser
    {
        private static Matrix4x4 uM;

        /// <summary>
        /// Method used for parsing ARTMatrices to Unity3d matrices (Matrix4x4).
        /// </summary>
        ///<returns>
        /// Returns the converted Matrix4x4 object.
        /// </returns>
        /// <param name="artM">
        /// To be converted ARTMatrix
        /// </param>
        public static Matrix4x4 ParseMatrix(ARTMatrix artM, bool transposed)
        {
            uM = new Matrix4x4();
            Vector4 v4_0 = new Vector4(artM.getB0(), artM.getB1(), artM.getB2(), 0);
            Vector4 v4_1 = new Vector4(artM.getB3(), artM.getB4(), artM.getB5(), 0);
            Vector4 v4_2 = new Vector4(artM.getB6(), artM.getB7(), artM.getB8(), 0);
            Vector4 v4_end = new Vector4(0, 0, 0, 1);
            uM.SetColumn(0, v4_0);
            uM.SetColumn(1, v4_1);
            uM.SetColumn(2, v4_2);
            uM.SetColumn(3, v4_end);
            if (transposed) return uM.transpose;
            return uM;
        }

        /// <summary>
        /// This Method parses various object such as vectors or matrices to a quaternion with a directional and an up vector.
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
            Matrix4x4 mat = UnityParser.ParseMatrix(artM, true);
            Vector3 targetDir = mat.MultiplyVector(Vector3.forward);
            Vector3 targetUp = mat.MultiplyVector(Vector3.up);
            return UnityParser.ParseQuaternion(targetDir, targetUp);
        }

        public static Quaternion ParseQuaternion(Matrix4x4 mat)
        {
            Vector3 targetDir = mat.MultiplyVector(Vector3.forward);
            Vector3 targetUp = mat.MultiplyVector(Vector3.up);
            return UnityParser.ParseQuaternion(targetDir, targetUp);
        }

        /// <summary>
        /// This method parses various input objects and computes a quaternion to use with a camera object in Unity3d.
        /// E.g. mapping the rotation of a ART 6d object to the camera.
        /// 
        /// If you wish to inverse an axis just change the correspondent boolean to true.
        /// </summary>
        /// <param name="artM">Input ARTMatrix</param>
        /// <param name="invX">Boolean used to inverse X axis</param>
        /// <param name="invY">Boolean used to inverse Y axis</param>
        /// <param name="invZ">Boolean used to inverse Z axis</param>
        /// <returns>Returns a quaternion</returns>
        public static Quaternion ParseCameraQuaternion(ARTMatrix artM, bool invX, bool invY, bool invZ)
        {
            Quaternion target = UnityParser.ParseQuaternion(artM);
            float angle = 0;
            Vector3 axis = Vector3.zero;
            target.ToAngleAxis(out angle, out axis);
            if (invX){axis.x *= -1;}
            if (invY){axis.y *= -1;}
            if (invZ) {axis.z *= -1;}
            return target = Quaternion.AngleAxis(angle, axis);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mat">Input Matrix4x4 matrix</param>
        /// <returns></returns>
        public static Quaternion ParseCameraQuaternion(Matrix4x4 mat, bool invX, bool invY, bool invZ)
        {
            Quaternion target = UnityParser.ParseQuaternion(mat);
            float angle = 0;
            Vector3 axis = Vector3.zero;
            target.ToAngleAxis(out angle, out axis);
            if (invX) { axis.x *= -1; }
            if (invY) { axis.y *= -1; }
            if (invZ) { axis.z *= -1; }
            return target = Quaternion.AngleAxis(angle, axis);
        }

        public static Quaternion ParseCameraQuaternion(Quaternion q, bool invX, bool invY, bool invZ)
        {
            float angle = 0;
            Vector3 axis = Vector3.zero;
            q.ToAngleAxis(out angle, out axis);
            if (invX) { axis.x *= -1; }
            if (invY) { axis.y *= -1; }
            if (invZ) { axis.z *= -1; }
            return q = Quaternion.AngleAxis(angle, axis);
        }

        /// <summary>
        /// Parses an ARTPoint to a vector.
        /// </summary>
        /// <param name="ap">To be converted ARTPoint</param>
        /// <returns>Returns the converted vector.</returns>
        public static Vector3 ParseVector(ARTPoint ap)
        {
            Vector3 v = new Vector3(ap.getX(), ap.getY(), ap.getZ());
            return v;
        } 
    }
}
