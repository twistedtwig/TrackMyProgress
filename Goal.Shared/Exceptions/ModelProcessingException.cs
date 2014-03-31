using System;
using System.Collections.Generic;
using System.Linq;

namespace Goals.Shared.Exceptions
{
    public class ModelProcessingException : Exception
    {
        public ModelProcessingException()
        {
            PropertyErrors = new List<PropertyError>();
            GeneralErrors = new List<string>();
        }

        public IList<PropertyError> PropertyErrors { get; set; }
        public IList<string> GeneralErrors { get; set; }

        public void AddPropertyError(string name, string error)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(error))
            {
                if (!PropertyErrors.Any(p => p.PropertyName.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var pe = new PropertyError();
                    pe.PropertyName = name;
                    pe.Errors.Add(error);
                    PropertyErrors.Add(pe);
                }
                else
                {
                    var pe = PropertyErrors.FirstOrDefault(p => p.PropertyName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    if (pe != null)
                    {
                        pe.Errors.Add(error);
                    }
                }
            }

        }
    }

    public class PropertyError
    {
        public PropertyError()
        {
            Errors = new List<string>();
        }

        public string PropertyName { get; set; }
        public IList<string> Errors { get; set; }
    }
}