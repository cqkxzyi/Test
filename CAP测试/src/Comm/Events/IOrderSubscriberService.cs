using System.Threading.Tasks;

namespace Comm
{
    public interface IOrderSubscriberService
    {
        Task Receive(string message);
        //void ConsumeOrderMessage1(OrderMessage message);
    }
    public interface IDeliverySubscriberService
    {
        Task Receive(OrderMessage message);
        //void ConsumeOrderMessage1(OrderMessage message);
    }
}
