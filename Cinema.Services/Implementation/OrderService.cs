using System;
using System.Collections.Generic;
using System.Linq;
using Cinema.Models.Domain;
using Cinema.Repository.Interface;
using Cinema.Services.Interface;

namespace Cinema.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        
        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public List<Order> GetAllOrders()
        {
            return this._orderRepository.GetAll().ToList();
        }

        public List<Order> GetAllOrdersForUser(string name)
        {
            var orders = this._orderRepository.GetAll()
                .Where(z => z.CinemaUserId != null && z.CinemaUserId.Equals(name))
                .ToList();
    
            if (orders.Count == 0)
            {
                return new List<Order>();
            }
    
            return orders;
        }


        public List<Order> GetAllOrdersForUser(Guid? id)
        {
            return null;
            // return this._orderRepository.GetAll().Where(z => z.CinemaUserId.Equals(id))
            //     .ToList();
        }

        public Order GetDetailsForOrder(Guid? id)
        {
            return this._orderRepository.Get(id);
        }

        public void CreateNewOrder(Order order)
        {
            this._orderRepository.Insert(order);
        }

        public void UpdateExistingOrder(Order order)
        {
            this._orderRepository.Update(order);
        }

        public void DeleteOrder(Guid id)
        {
            Order order = this.GetDetailsForOrder(id);
            this._orderRepository.Delete(order);
        }
    }
}