using OpenTK;
using System;
namespace CStrawberry3D.Core
{
    public class Mathf
    {
        public static Vector3 QuaternionMultiplyVector3(Quaternion quat, Vector3 vec)
        {
            float num = quat.X * 2f;
            float num2 = quat.Y * 2f;
            float num3 = quat.Z * 2f;
            float num4 = quat.X * num;
            float num5 = quat.Y * num2;
            float num6 = quat.Z * num3;
            float num7 = quat.X * num2;
            float num8 = quat.X * num3;
            float num9 = quat.Y * num3;
            float num10 = quat.W * num;
            float num11 = quat.W * num2;
            float num12 = quat.W * num3;
            Vector3 result = new Vector3();
            result.X = (1f - (num5 + num6)) * vec.X + (num7 - num12) * vec.Y + (num8 + num11) * vec.Z;
            result.Y = (num7 + num12) * vec.X + (1f - (num4 + num6)) * vec.Y + (num9 - num10) * vec.Z;
            result.Z = (num8 - num11) * vec.X + (num9 + num10) * vec.Y + (1f - (num4 + num5)) * vec.Z;
            return result;
        }
        public static float PI = 3.14159f;
        public static Matrix4 TransformNormalMatrix(Matrix4 worldMatrix)
        {
            try
            {
                worldMatrix.Invert();
                worldMatrix.Transpose();
            }
            catch
            { }
                return worldMatrix;
        }
        public static float DegreeToRadian(float degree)
        {
            float radian = (PI / 180) * degree;
            return radian;
        }
        public static Vector3 DegreeToRadian(Vector3 degree)
        {
            degree.X = DegreeToRadian(degree.X);
            degree.Y = DegreeToRadian(degree.Y);
            degree.Z = DegreeToRadian(degree.Z);
            return degree;
        }
        public static float RadianToDegree(float radian)
        {
            float degree = (180 / PI) * radian;
            return degree;
        }
        public static Vector3 RadianToDegree(Vector3 radian)
        {
            radian.X = RadianToDegree(radian.X);
            radian.Y = RadianToDegree(radian.Y);
            radian.Z = RadianToDegree(radian.Z);
            return radian;
        }
        public static Quaternion EulerToQuaternion(Vector3 v)
        {
            return EulerToQuaternion(v.Y, v.X, v.Z);
        }

        public static Quaternion EulerToQuaternion(float yaw, float pitch, float roll)
        {
            yaw = Mathf.DegreeToRadian(yaw);
            pitch = Mathf.DegreeToRadian(pitch);
            roll = Mathf.DegreeToRadian(roll);
            float rollOver2 = roll * 0.5f;
            float sinRollOver2 = (float)Math.Sin((double)rollOver2);
            float cosRollOver2 = (float)Math.Cos((double)rollOver2);
            float pitchOver2 = pitch * 0.5f;
            float sinPitchOver2 = (float)Math.Sin((double)pitchOver2);
            float cosPitchOver2 = (float)Math.Cos((double)pitchOver2);
            float yawOver2 = yaw * 0.5f;
            float sinYawOver2 = (float)Math.Sin((double)yawOver2);
            float cosYawOver2 = (float)Math.Cos((double)yawOver2);
            Quaternion result = new Quaternion();
            result.W = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.X = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.Y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            result.Z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

            return result;
        }

        public static Vector3 QuaternionToEuler(Quaternion q1)
        {
            float sqw = q1.W * q1.W;
            float sqx = q1.X * q1.X;
            float sqy = q1.Y * q1.Y;
            float sqz = q1.Z * q1.Z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = q1.X * q1.W - q1.Y * q1.Z;
            Vector3 v;

            if (test > 0.4995f * unit)
            { // singularity at north pole
                v.Y = 2f * (float)Math.Atan2(q1.Y, q1.X);
                v.X = Mathf.PI / 2;
                v.Z = 0;
                var result = new Vector3(Mathf.RadianToDegree(v.X), Mathf.RadianToDegree(v.Y), Mathf.RadianToDegree(v.Z));
                return DegreeToRadian(NormalizeAngles(result));
            }
            if (test < -0.4995f * unit)
            { // singularity at south pole
                v.Y = -2f * (float)Math.Atan2(q1.Y, q1.X);
                v.X = -Mathf.PI / 2;
                v.Z = 0;
                var result = new Vector3(Mathf.RadianToDegree(v.X), Mathf.RadianToDegree(v.Y), Mathf.RadianToDegree(v.Z));
                return DegreeToRadian(NormalizeAngles(result));
            }
            Quaternion q = new Quaternion(q1.W, q1.Z, q1.X, q1.Y);
            v.Y = (float)Math.Atan2(2f * q.X * q.W + 2f * q.Y * q.Z, 1 - 2f * (q.Z * q.Z + q.W * q.W));     // Yaw
            v.X = (float)Math.Asin(2f * (q.X * q.Z - q.W * q.Y));                             // Pitch
            v.Z = (float)Math.Atan2(2f * q.X * q.Y + 2f * q.Z * q.W, 1 - 2f * (q.Y * q.Y + q.Z * q.Z));      // Roll
            var result2 = new Vector3(Mathf.RadianToDegree(v.X), Mathf.RadianToDegree(v.Y), Mathf.RadianToDegree(v.Z));
            return DegreeToRadian(NormalizeAngles(result2));
        }

        static Vector3 NormalizeAngles(Vector3 angles)
        {
            angles.X = NormalizeAngle(angles.X);
            angles.Y = NormalizeAngle(angles.Y);
            angles.Z = NormalizeAngle(angles.Z);
            return angles;
        }

        static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
        
    }
}
