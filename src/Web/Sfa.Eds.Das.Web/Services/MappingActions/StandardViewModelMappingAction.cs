namespace Sfa.Eds.Das.Web.Services.MappingActions
{
    using AutoMapper;

    using Sfa.Das.ApplicationServices.Models;
    using ViewModels;

    internal class StandardViewModelMappingAction : IMappingAction<StandardSearchResultsItem, StandardResultItemViewModel>
    {
        public void Process(StandardSearchResultsItem source, StandardResultItemViewModel destination)
        {
            if (source.TypicalLength == null || source.TypicalLength.From > source.TypicalLength.To)
            {
                destination.TypicalLengthMessage = string.Empty;
            }
            else if (this.GetSingelValue(source.TypicalLength.From, source.TypicalLength.To) != 0)
            {
                var value = this.GetSingelValue(source.TypicalLength.From, source.TypicalLength.To);
                destination.TypicalLengthMessage = $"{value} {this.GetUnit(source.TypicalLength.Unit)}";
            }
            else
            {
                destination.TypicalLengthMessage = $"{source.TypicalLength.From} to {source.TypicalLength.To} {this.GetUnit(source.TypicalLength.Unit)}";
            }
        }

        private int GetSingelValue(int from, int to)
        {
            if (from == to)
            {
                return from;
            }

            if (from > 0 && to == 0)
            {
                return from;
            }

            if (from == 0 && to > 0)
            {
                return to;
            }

            return 0;
        }

        private string GetUnit(string unit)
        {
            if (unit == "m")
            {
                return "months";
            }

            return string.Empty;
        }
    }
}