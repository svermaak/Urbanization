using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mirage.Urbanization.Simulation
{
    public abstract class BudgetComponentDefinition
    {
        private readonly Func<ICityBudgetConfiguration, decimal> _getCurrentRate;
        private readonly Action<ICityBudgetConfiguration, decimal> _setCurrentRate;

        protected BudgetComponentDefinition(
            string name,
            Expression<Func<ICityBudgetConfiguration, decimal>> currentRate)
        {
            Name = name;
            _getCurrentRate = currentRate.Compile();
            _setCurrentRate = (budget, rate) => ((PropertyInfo)((MemberExpression)currentRate.Body).Member).SetValue(budget, rate);
        }

        public abstract IEnumerable<decimal> GetSelectableRatePercentages();
        public string Name { get; }
        public decimal CurrentRate(ICityBudgetConfiguration cityBudgetConfiguration) { return _getCurrentRate(cityBudgetConfiguration); }
        public void SetCurrentRate(ICityBudgetConfiguration cityBudgetConfiguration, decimal rate) { _setCurrentRate(cityBudgetConfiguration, rate); }
    }
}