using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cShART
{
    public abstract class ARTObject
    {
        private int id;
        private ARTPoint position;
        private ARTMatrix matrix;

        public ARTObject(int id, ARTPoint position, ARTMatrix matrix)
        {
            this.id = id;
            this.position = position;
            this.matrix = matrix;
        }
        public int getId()
        {
            return this.id;
        }

        public ARTPoint getPosition()
        {
            return this.position;
        }

        public ARTMatrix getMatrix()
        {
            return this.matrix;
        }

        protected abstract String nameToString();

        protected abstract String extensionsToString();

        public String toString()
        {
            return "-- " + nameToString() + " ---------------------------------------------" + Environment.NewLine + "id: " + getId() + Environment.NewLine + getPosition() + Environment.NewLine + getMatrix() + Environment.NewLine + extensionsToString() + "-- " + nameToString() + " ------------------------------------- Ende --" + Environment.NewLine;
        }
    }
}
