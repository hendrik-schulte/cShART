using System;

namespace cShART
{
    public abstract class ARTObject
    {
        private readonly int id;
        private readonly ARTMatrix matrix;
        private readonly ARTPoint position;

        public ARTObject(int id, ARTPoint position, ARTMatrix matrix)
        {
            this.id = id;
            this.position = position;
            this.matrix = matrix;
        }

        public int GetId()
        {
            return id;
        }

        public ARTPoint GetPosition()
        {
            return position;
        }

        public ARTMatrix GetMatrix()
        {
            return matrix;
        }

        protected abstract string NameToString();

        protected abstract string ExtensionsToString();

        public override string ToString()
        {
            return "-- " + NameToString() + " ---------------------------------------------" + Environment.NewLine +
                   "id: " + GetId() + Environment.NewLine + GetPosition() + Environment.NewLine + GetMatrix() +
                   Environment.NewLine + ExtensionsToString() + "-- " + NameToString() +
                   " ------------------------------------- Ende --" + Environment.NewLine;
        }
    }
}