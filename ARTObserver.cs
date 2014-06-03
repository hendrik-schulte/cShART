using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cShART
{
    public interface ARTObserver
    {
        void onFrameUpdate(ARTClient paramARTClient);

        void on6dUpdate(ARTClient paramARTClient);

        void onFlystickUpdate(ARTClient paramARTClient);
    }
}
