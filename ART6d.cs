using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cShART
{
    public class ART6d : ARTObject
    {
        private ARTAngle angle;

        public static ART6d Empty()
        {
            return new ART6d(-1, ARTPoint.Empty(), ARTAngle.Empty(), ARTMatrix.Empty());
        }

        public ART6d(int id, ARTPoint position, ARTAngle angle, ARTMatrix matrix) : base(id, position, matrix)
        {
            this.angle = angle;
        }

        public ARTAngle getAngle()
        {
            return this.angle;
        }

        override protected String nameToString()
        {
            return "ART6d";
        }

        override protected String extensionsToString()
        {
            return getAngle() + Environment.NewLine;
        }
    }
}
