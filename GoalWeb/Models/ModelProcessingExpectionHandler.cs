using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Goals.Shared.Exceptions;

namespace GoalWeb.Models
{
    public class ModelProcessingExpectionHandler
    {

        public static void Consume(ModelStateDictionary modelState, ModelProcessingException exception)
        {
            foreach (var ex in exception.GeneralErrors)
            {
                if (!string.IsNullOrWhiteSpace(ex))
                {
                    modelState.AddModelError(string.Empty, ex);
                }
            }

            foreach (var propError in exception.PropertyErrors)
            {
                if (!propError.Errors.Any()) continue;

                var ms = new ModelState();
                foreach (string error in propError.Errors)
                {
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        ms.Errors.Add(new ModelError(error));
                    }
                }

                if (modelState.ContainsKey(propError.PropertyName))
                {
                    foreach (var error in ms.Errors)
                    {
                        modelState[propError.PropertyName].Errors.Add(error);
                    }
                }
                else
                {
                    modelState.Add(new KeyValuePair<string, ModelState>(propError.PropertyName, ms));
                }
            }
        }
    }
}