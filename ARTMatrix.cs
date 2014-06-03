using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cShART
{
    public class ARTMatrix :  ARTConstruct
    {
        private ARTPoint row1;
        private ARTPoint row2;
        private ARTPoint row3;

        public static ARTMatrix Empty()
        {
            return new ARTMatrix(0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
        }

        public ARTMatrix(float b0, float b1, float b2, float b3, float b4, float b5, float b6, float b7, float b8) : this(new float[] { b1, b2, b3, b4, b5, b6, b7, b8 }) {}

        public ARTMatrix(float[] b)
        {
            float[] val = new float[9];
            for (int i = 0; (i < b.Length) && (i < 9); i++)
            {
                val[i] = b[i];
            }
            this.row1 = new ARTPoint(val[0], val[1], val[2]);
            this.row2 = new ARTPoint(val[3], val[4], val[5]);
            this.row3 = new ARTPoint(val[6], val[7], val[8]);
        }

        public float getB0()
        {
            return this.row1.getX();
        }

        public float getB1()
        {
            return this.row1.getY();
        }

        public float getB2()
        {
            return this.row1.getZ();
        }

        public float getB3()
        {
            return this.row2.getX();
        }

        public float getB4()
        {
            return this.row2.getY();
        }

        public float getB5()
        {
            return this.row2.getZ();
        }

        public float getB6()
        {
            return this.row3.getX();
        }

        public float getB7()
        {
            return this.row3.getY();
        }

        public float getB8()
        {
            return this.row3.getZ();
        }

        public int getSize()
        {
            return 9;
        }

        public float getElement(int i)
        {
            if (i == 0)
            {
                return getB0();
            }
            if (i == 1)
            {
                return getB1();
            }
            if (i == 2)
            {
                return getB2();
            }
            if (i == 3)
            {
                return getB3();
            }
            if (i == 4)
            {
                return getB4();
            }
            if (i == 5)
            {
                return getB5();
            }
            if (i == 6)
            {
                return getB6();
            }
            if (i == 7)
            {
                return getB7();
            }
            if (i == 8)
            {
                return getB8();
            }
            return 0.0F;
        }

        public String toString()
        {
            return "ARTMatrix:  " + Environment.NewLine + "b0: " + getB0() + ", b1: " + getB1() + ", b2: " + getB2() + Environment.NewLine + "b3: " + getB3() + ", b4: " + getB4() + ", b5: " + getB5() + Environment.NewLine + "b6: " + getB6() + ", b7: " + getB7() + ", b8: " + getB8();
        }

        /// <summary>
        /// With this method you can check if a matrix is not completely set to 0.0 floats.
        /// </summary>
        /// <returns>Returns false when every element of the matrix is 0. Returns true otherwise.</returns>
        public bool IsSet()
        {
            bool set = false;
            for (int i = 0; i < getSize(); i++)
            {
                if (Math.Abs(getElement(i)) != 0)
                {
                    set = true;
                }
            }
            return set;
        }
    }
}
