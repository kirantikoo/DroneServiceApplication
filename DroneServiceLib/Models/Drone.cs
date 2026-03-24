using DroneServiceLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneServiceLib.Models
{
    public class Drone : IProduct
    {
        private string _clientName;
        private string _model;
        private string _serviceProblem;
        private double _serviceCost;
        private int _serviceTag;

        public Drone()
        {
            _clientName = string.Empty;
            _model = string.Empty;
            _serviceProblem = string.Empty;
            _serviceCost = 0.0;
            _serviceTag = 100;
        }

        public Drone(string clientName, string model, string problem, double cost, int tag) : this()
        {
            SetClientName(clientName);
            SetModel(model);
            SetServiceProblem(problem);
            SetServiceCost(cost);
            SetServiceTag(tag);
        }

        public string GetClientName()
        {
            return _clientName;
        }

        public void SetClientName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _clientName = string.Empty;
                return;
            }

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            _clientName = textInfo.ToTitleCase(name.Trim().ToLower());
        }

        public string GetModel()
        {
            return _model;
        }

        public void SetModel(string model)
        {
            _model = model?.Trim() ?? string.Empty;
        }

        public string GetServiceProblem()
        {
            return _serviceProblem;
        }

        public void SetServiceProblem(string problem)
        {
            if (string.IsNullOrWhiteSpace(problem))
            {
                _serviceProblem = string.Empty;
                return;
            }

            string text = problem.Trim().ToLower();
            _serviceProblem = char.ToUpper(text[0]) + text.Substring(1);
        }

        public double GetServiceCost()
        {
            return _serviceCost;
        }

        public void SetServiceCost(double cost)
        {
            _serviceCost = cost;
        }

        public int GetServiceTag()
        {
            return _serviceTag;
        }

        public void SetServiceTag(int tag)
        {
            _serviceTag = tag;
        }

        public string Display()
        {
            return $"{_clientName} - ${_serviceCost:F2}";
        }
    }
}
