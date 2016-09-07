namespace Sfa.Das.Sas.Core.Extensions
{
    using System.Web.Script.Serialization;

    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
    }
}
