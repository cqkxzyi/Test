﻿using System.Threading.Tasks;

namespace Manulife.DNC.MSAD.WS.Events
{
    public interface IOrderSubscriberService
    {
        Task ConsumeOrderMessage(OrderMessage message);
        //void ConsumeOrderMessage1(OrderMessage message);
    }
}
