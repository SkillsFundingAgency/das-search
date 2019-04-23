namespace Sfa.Das.Sas.Shared.Components.Domain.Interfaces
{
    public interface IUtilitiesCssClasses
    {
        string ClassPrefix { get; set; }
        string FontWeightBold { get; }
        string Margin(string type, int size);

    }
}
