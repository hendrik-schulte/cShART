using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cShART
{
    public class ARTPoint : ARTConstruct
    {
        private float x;
        private float y;
        private float z;

        public static ARTPoint Empty()
        {
            return new ARTPoint(0.0F, 0.0F, 0.0F);
        }

        public ARTPoint(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float getX()
        {
            return this.x;
        }

        public float getY()
        {
            return this.y;
        }

        public float getZ()
        {
            return this.z;
        }

        public int getSize()
        {
            return 3;
        }

        public float getElement(int i)
        {
            if (i == 0)
            {
                return this.x;
            }
            if (i == 1)
            {
                return this.y;
            }
            if (i == 2)
            {
                return this.z;
            }
            return 0.0F;
        }

        public String toString()
        {
            return "ARTPoint: x " + getX() + ", y " + getY() + ", z " + getZ();
        }
    }
}
