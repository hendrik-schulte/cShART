namespace cShART
{
    public class ARTAngle : ARTConstruct
    {
        private readonly ARTPoint point;

        public ARTAngle(float eta, float theta, float phi)
        {
            point = new ARTPoint(eta, theta, phi);
        }

        public int GetSize()
        {
            return point.GetSize();
        }

        public float GetElement(int i)
        {
            return point.GetElement(i);
        }

        public static ARTAngle Empty()
        {
            return new ARTAngle(0.0F, 0.0F, 0.0F);
        }

        public float GetEta()
        {
            return point.GetX();
        }

        public float GetTheta()
        {
            return point.GetY();
        }

        public float GetPhi()
        {
            return point.GetZ();
        }

        public override string ToString()
        {
            return
                "ARTAngle: eta " + GetEta() + ", theta " + GetTheta() + ", phi " + GetPhi();
        }
    }
}