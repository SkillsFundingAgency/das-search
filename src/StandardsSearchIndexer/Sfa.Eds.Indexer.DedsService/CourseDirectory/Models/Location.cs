﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

namespace Sfa.Infrastructure.Models
{
    using Newtonsoft.Json.Linq;

    public class Location
    {
        /// <summary>
        ///     Optional.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        ///     Optional.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     Deserialize the object
        /// </summary>
        public virtual void DeserializeJson(JToken inputObject)
        {
            if (inputObject != null && inputObject.Type != JTokenType.Null)
            {
                var addressValue = inputObject["address"];
                if (addressValue != null && addressValue.Type != JTokenType.Null)
                {
                    var address = new Address();
                    address.DeserializeJson(addressValue);
                    Address = address;
                }

                var idValue = inputObject["id"];
                if (idValue != null && idValue.Type != JTokenType.Null)
                {
                    ID = (int)idValue;
                }

                var nameValue = inputObject["name"];
                if (nameValue != null && nameValue.Type != JTokenType.Null)
                {
                    Name = (string)nameValue;
                }

                var emailValue = inputObject["email"];
                if (emailValue != null && emailValue.Type != JTokenType.Null)
                {
                    Email = (string)emailValue;
                }

                var phoneValue = inputObject["phone"];
                if (phoneValue != null && phoneValue.Type != JTokenType.Null)
                {
                    Phone = (string)phoneValue;
                }

                var websiteValue = inputObject["website"];
                if (websiteValue != null && websiteValue.Type != JTokenType.Null)
                {
                    Website = (string)websiteValue;
                }
            }
        }
    }
}