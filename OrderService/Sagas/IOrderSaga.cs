using Common.Messages;
using OrderService.Data;

namespace OrderService.Sagas
{
    public interface IOrderSaga
    {
        void ApproveOrder();
        void CancelOrder();
        void CreatePendingOrder(OrderCreated orderCreated);
        void DebitAccount();
    }
}