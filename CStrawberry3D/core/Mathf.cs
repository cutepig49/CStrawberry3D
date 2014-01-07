
namespace CStrawberry3D.core
{
    public class Mathf
    {
        public static float PI = 3.14159f;
        public static float degreeToRadian(float degree)
        {
            float radian = (PI / 180) * degree;
            return radian;
        }
        public static float radianToDegree(float radian)
        {
            float degree = (180 / PI) * radian;
            return degree;
        }
        
    }
}
