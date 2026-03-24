using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneServiceLib.Interfaces
{
    public interface IProduct
    {
        string GetClientName();
        void SetClientName(string name);
        string GetModel();
        void SetModel(string model);
        string GetServiceProblem();
        void SetServiceProblem(string problem);
        double GetServiceCost();
        void SetServiceCost(double cost);
        int GetServiceTag();
        void SetServiceTag(int tag);

        string Display();
    }
}
