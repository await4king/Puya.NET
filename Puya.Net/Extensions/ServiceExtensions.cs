using Puya.Data;
using Puya.Reflection;
using Puya.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Puya.Extensions
{
    public static class ServiceExtensions
    {
        #region Validation
        static bool CheckCatalogLayerRule(IDb db, PropertyInfo prop, object req, ServiceResponse res)
        {
            var isValid = true;
            var layerAttr = prop.GetCustomAttribute<CatalogLayerAttribute>();
            var parentAttr = prop.GetCustomAttribute<ParentAttribute>();
            var isRequired = prop.GetCustomAttribute<RequiredAttribute>() != null;

            if (layerAttr != null && parentAttr != null)
            {
                var value = prop.GetValue(req);
                var name = prop.Name;
                var table = parentAttr.Name;
                var shouldBeLeaf = layerAttr.Layer;

                if (table.EndzWith("Cg", "Cb", "Bd"))
                {
                    var args = new
                    {
                        Result = CommandHelper.Result(),
                        table,
                        name,
                        value,
                        isRequired,
                        shouldBeLeaf
                    };

                    db.ExecuteNonQueryCommand("usp1_Catalog_isValid", args);

                    var sr = new ServiceResponse();

                    sr.Finalize(args);
                    sr.Info = prop.Name;
                    isValid = sr.IsSucceeded();

                    if (!isValid)
                    {
                        res.InnerResponses.Add(sr);
                    }
                }
            }

            return isValid;
        }
        public static bool Validate<TRequest, TResponse>(this IServiceAction action, IDb db, TRequest req, TResponse res)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse, new()
        {
            var result = true;

            if (req == null)
            {
                res.SetStatus("NoRequest");
            }
            else
            {
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(
                    req,
                    new ValidationContext(req),
                    validationResults,
                    validateAllProperties: true
                );
                result = isValid;

                if (!isValid)
                {
                    foreach (var error in validationResults)
                    {
                        res.InnerResponses.Add(ServiceResponse.FromStatus("InvalidProp").SetMessage(error.ErrorMessage));
                    }
                }

                var props = ReflectionHelper.GetPublicInstanceReadableProperties(req.GetType());

                foreach (var prop in props)
                {
                    CheckMinValueRule(prop, req, res);
                    CheckMaxValueRule(prop, req, res);
                    CheckCatalogLayerRule(db, prop, req, res);
                }

                if (res.InnerResponses.Count > 0)
                {
                    res.SetStatus("InvalidRequest");
                }
            }

            return result;
        }
        #endregion
    }
}
