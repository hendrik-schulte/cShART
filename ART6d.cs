using System;

namespace cShART
{
    public class ART6d : ARTObject
    {
        private readonly ARTAngle angle;

        public ART6d(int id, ARTPoint position, ARTAngle angle, ARTMatrix matrix) : base(id, position, matrix)
        {
            this.angle = angle;
        }

        public static ART6d Empty()
        {
            return new ART6d(-1, ARTPoint.Empty(), ARTAngle.Empty(), ARTMatrix.Empty());
        }

        public ARTAngle GetAngle()
        {
            return angle;
        }

        protected override string NameToString()
        {
            return "ART6d";
        }

        protected override string ExtensionsToString()
        {
            return GetAngle() + Environment.NewLine;
        }
    }
}