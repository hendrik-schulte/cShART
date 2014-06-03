using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cShART
{
    public class ARTAngle : ARTConstruct
    {
        private ARTPoint point;

        public static ARTAngle Empty()
        {
            return new ARTAngle(0.0F, 0.0F, 0.0F);
        }

        public ARTAngle(float eta, float theta, float phi)
        {
            this.point = new ARTPoint(eta, theta, phi);
        }

        public float getEta()
        {
            return this.point.getX();
        }

        public float getTheta()
        {
            return this.point.getY();
        }

        public float getPhi()
        {
            return this.point.getZ();
        }

        public int getSize()
        {
            return this.point.getSize();
        }

        public float getElement(int i)
        {
            return this.point.getElement(i);
        }

        public String toString()
        {
            return
              "ARTAngle: eta " + getEta() + ", theta " + getTheta() + ", phi " + getPhi();
        }
    }
}
