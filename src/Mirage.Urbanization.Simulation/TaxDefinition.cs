﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mirage.Urbanization.Simulation.Persistence;

namespace Mirage.Urbanization.Simulation
{
    public class TaxDefinition : BudgetComponentDefinition
    {
        private readonly Func<IEnumerable<PersistedCityStatisticsWithFinancialData>, int> _getProjectedIncome;

        private TaxDefinition(
            string name,
            Expression<Func<ICityBudgetConfiguration, decimal>> currentRate,
            Func<IEnumerable<PersistedCityStatisticsWithFinancialData>, int> getProjectedIncome)
            :base(name,currentRate)
        {
            _getProjectedIncome = getProjectedIncome;
        }

        public int GetProjectedIncome(IEnumerable<PersistedCityStatisticsWithFinancialData> cityStatistics) => _getProjectedIncome(cityStatistics);

        private static readonly TaxDefinition ResidentialTaxDefinition = new TaxDefinition("Residential", x => x.ResidentialTaxRate, x => x.Sum(y => y.ResidentialTaxIncome));
        private static readonly TaxDefinition CommercialTaxDefinition = new TaxDefinition("Commercial", x => x.CommercialTaxRate, x => x.Sum(y => y.CommercialTaxIncome));
        private static readonly TaxDefinition IndustrialTaxDefinition = new TaxDefinition("Industrial", x => x.IndustrialTaxRate, x => x.Sum(y => y.IndustrialTaxIncome));

        public static IEnumerable<TaxDefinition> TaxDefinitions
        {
            get
            {
                yield return ResidentialTaxDefinition;
                yield return CommercialTaxDefinition;
                yield return IndustrialTaxDefinition;
            }
        }

        public const decimal DefaultTaxRate = 0.07M;

        public override IEnumerable<decimal> GetSelectableRatePercentages() => GetSelectableTaxRatePercentages();

        private static readonly ISet<decimal> SelectableTaxRatePercentages 
            = new HashSet<decimal>(Enumerable.Range(0, 20).Select(x => (decimal) x/100)); 

        public static IEnumerable<decimal> GetSelectableTaxRatePercentages()
        {
            return SelectableTaxRatePercentages;
        } 
    }
}