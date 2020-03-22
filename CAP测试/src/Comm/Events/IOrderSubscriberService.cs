using System.Threading.Tasks;

namespace Comm
{
    public interface IOrderSubscriberService
    {
        Task ConsumeOrderMessage(OrderMessage message);
        //void ConsumeOrderMessage1(OrderMessage message);
    }
}
