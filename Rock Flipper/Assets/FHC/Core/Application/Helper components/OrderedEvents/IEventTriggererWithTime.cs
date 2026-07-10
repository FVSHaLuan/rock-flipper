using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.HelperComponent
{
    public interface IEventTriggererWithTime : IEventTriggerer
    {
        void Trigger(IEventTriggererWithTimeCallback finishCallback);
    }

    public delegate void IEventTriggererWithTimeCallback(IEventTriggererWithTime eventTriggererWithTime);
}