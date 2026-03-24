using DroneServiceLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneServiceLib.Services
{
    public class ServiceManager
    {
        private Queue<IProduct> regularService;
        private Queue<IProduct> expressService;
        private List<IProduct> finishedList;

        public ServiceManager()
        {
            regularService = new Queue<IProduct>();
            expressService = new Queue<IProduct>();
            finishedList = new List<IProduct>();
        }

        public Queue<IProduct> GetRegularService()
        {
            return regularService;
        }

        public Queue<IProduct> GetExpressService()
        {
            return expressService;
        }

        public List<IProduct> GetFinishedList()
        {
            return finishedList;
        }

        public string GetServicePriority(bool isExpress)
        {
            return isExpress ? "Express" : "Regular";
        }

        public int GetNextServiceTag(int currentTag)

        {
            return currentTag < 900 ? currentTag + 10 : 100;
        }

        public void AddNewItem(IProduct product, string priority)
        {
            
            if (priority == "Express")
            {
                product.SetServiceCost(product.GetServiceCost() * 1.15);
                expressService.Enqueue(product);
            }
            else
            {
                regularService.Enqueue(product);
            }
        }

        public IProduct? PeekRegular()
        {
            return regularService.Count > 0 ? regularService.Peek() : null;
        }

        public IProduct? PeekExpress()
        {
            return expressService.Count > 0 ? expressService.Peek() : null;
        }

        public IProduct? ProcessRegular()
        {
            if (regularService.Count == 0)
            {
                return null;
            }

            IProduct finishedItem = regularService.Dequeue();
            finishedList.Add(finishedItem);
            return finishedItem;
        }

        public IProduct? ProcessExpress()
        {
            if (expressService.Count == 0)
            {
                return null;
            }
            IProduct finishedItem = expressService.Dequeue();
            finishedList.Add(finishedItem);
            return finishedItem;
        }

        public bool RemoveFinishedItemAt(int index)
        {
            if (index >= 0 && index < finishedList.Count)
            {
                finishedList.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}