using EcommerceWebMVC.Data;
using System;

namespace EcommerceWebMVC.IOrderState
{
    public interface IOrderState
    {
        void ProcessOrder(OrderContext context);
        string GetState();
    }
    public class PendingState : IOrderState
    {
        public void ProcessOrder(OrderContext context)
        {
            Console.WriteLine("Đơn hàng đã xác nhận.");
            context.SetState(new ConfirmedState());
        }

        public string GetState() => "Chờ xác nhận";
    }
    public class ConfirmedState : IOrderState
    {
        public void ProcessOrder(OrderContext context)
        {
            context.SetState(new ShippingState());
        }
        public string GetState() => "Đã xác nhận";
    }
    public class ShippingState : IOrderState
    {
        public void ProcessOrder(OrderContext context)
        {
            context.SetState(new DeliveredState());
        }
        public string GetState() => "Đang giao";
    }
    public class DeliveredState : IOrderState
    {
        public void ProcessOrder(OrderContext context)
        {
            Console.WriteLine("Đơn hàng đã hoàn tất.");
        }
        public string GetState() => "Hoàn thành";
    }
    public class CancelledState : IOrderState
    {
        public void ProcessOrder(OrderContext context)
        {
            Console.WriteLine("Đơn hàng đã bị hủy.");
        }
        public string GetState() => "Đã hủy";
    }
    public class PaidState : IOrderState
    {
        public void ProcessOrder(OrderContext context)
        {
            Console.WriteLine("Đơn hàng đã thanh toán.");   
        }
        public string GetState() => "Đã thanh toán";
    }
    public class OrderContext
    {
        private IOrderState _currentState;
        private HoaDon _order;
        private readonly EcommerceWebContext _dbContext;

        public OrderContext(HoaDon order, EcommerceWebContext dbContext)
        {
            _dbContext = dbContext;
            _order = order;

            _currentState = order.MaTrangThai switch
            {
                1 => new PendingState(),
                2 => new ConfirmedState(),
                3 => new ShippingState(),
                4 => new DeliveredState(),
                5 => new CancelledState(),
                6 => new PaidState(),
                _ => new PendingState()
            };
        }

        public void SetState(IOrderState state)
        {
            _currentState = state;

            _order.MaTrangThai = state switch
            {
                PendingState => 1,
                ConfirmedState => 2,
                ShippingState => 3,
                DeliveredState => 4,
                CancelledState => 5,
                PaidState => 6,
                _ => 1
            };

            _dbContext.SaveChanges();
        }

        public void ProcessOrder()
        {
            _currentState.ProcessOrder(this);
        }

        public string GetCurrentState()
        {
            return _currentState.GetState();
        }
    }

}
