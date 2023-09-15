using System;
using System.Collections.Generic;
using Cinema.Models.Domain;

namespace Cinema.Services.Interface
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        
        List<Order> GetAllOrdersForUser(string name);
        List<Order> GetAllOrdersForUser(Guid? id);
        Order GetDetailsForOrder(Guid? id);
        void CreateNewOrder(Order order);
        void UpdateExistingOrder(Order order);
        void DeleteOrder(Guid id);
        
        
    }
}