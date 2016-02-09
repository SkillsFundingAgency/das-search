using System;
using System.Linq;
using System.Runtime.Serialization;

namespace StandardModule.Web.Services.Standards.ViewModels
{
    /// <summary>
    /// A view model for the standard properties.
    /// This view model is used by the services.
    /// </summary>
    [DataContract]
    public class StandardPropertyViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        public string Name
        {
            get;
            set;
        }
    }
}
