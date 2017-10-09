namespace cShART
{
    public class ARTPoint : ARTConstruct
    {
        private readonly float x;
        private readonly float y;
        private readonly float z;

        public ARTPoint(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int GetSize()
        {
            return 3;
        }

        public float GetElement(int i)
        {
            switch (i)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
                    default:
                        return 0.0f;
            }
        }

        public static ARTPoint Empty()
        {
            return new ARTPoint(0.0F, 0.0F, 0.0F);
        }

        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }

        public float GetZ()
        {
            return z;
        }

        public override string ToString()
        {
            return "ARTPoint: x " + GetX() + ", y " + GetY() + ", z " + GetZ();
        }
    }
}