using System;

namespace cShART
{
    public class ARTMatrix : ARTConstruct
    {
        private readonly ARTPoint row1;
        private readonly ARTPoint row2;
        private readonly ARTPoint row3;

        public ARTMatrix(float b0, float b1, float b2, float b3, float b4, float b5, float b6, float b7, float b8) :
            this(new[] {b1, b2, b3, b4, b5, b6, b7, b8})
        {
        }

        public ARTMatrix(float[] b)
        {
            var val = new float[9];
            for (var i = 0; i < b.Length && i < 9; i++)
                val[i] = b[i];
            row1 = new ARTPoint(val[0], val[1], val[2]);
            row2 = new ARTPoint(val[3], val[4], val[5]);
            row3 = new ARTPoint(val[6], val[7], val[8]);
        }

        public int GetSize()
        {
            return 9;
        }

        public float GetElement(int i)
        {
            if (i == 0)
                return GetB0();
            if (i == 1)
                return GetB1();
            if (i == 2)
                return GetB2();
            if (i == 3)
                return GetB3();
            if (i == 4)
                return GetB4();
            if (i == 5)
                return GetB5();
            if (i == 6)
                return GetB6();
            if (i == 7)
                return GetB7();
            if (i == 8)
                return GetB8();
            return 0.0F;
        }

        public static ARTMatrix Empty()
        {
            return new ARTMatrix(0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
        }

        public float GetB0()
        {
            return row1.GetX();
        }

        public float GetB1()
        {
            return row1.GetY();
        }

        public float GetB2()
        {
            return row1.GetZ();
        }

        public float GetB3()
        {
            return row2.GetX();
        }

        public float GetB4()
        {
            return row2.GetY();
        }

        public float GetB5()
        {
            return row2.GetZ();
        }

        public float GetB6()
        {
            return row3.GetX();
        }

        public float GetB7()
        {
            return row3.GetY();
        }

        public float GetB8()
        {
            return row3.GetZ();
        }

        public override string ToString()
        {
            return "ARTMatrix:  " + Environment.NewLine + "b0: " + GetB0() + ", b1: " + GetB1() + ", b2: " + GetB2() +
                   Environment.NewLine + "b3: " + GetB3() + ", b4: " + GetB4() + ", b5: " + GetB5() +
                   Environment.NewLine + "b6: " + GetB6() + ", b7: " + GetB7() + ", b8: " + GetB8();
        }

        /// <summary>
        ///     With this method you can check if a matrix is not completely set to 0.0 floats.
        /// </summary>
        /// <returns>Returns false when every element of the matrix is 0. Returns true otherwise.</returns>
        public bool IsSet()
        {
            var set = false;
            for (var i = 0; i < GetSize(); i++)
                if (Math.Abs(GetElement(i)) != 0)
                    set = true;
            return set;
        }
    }
}