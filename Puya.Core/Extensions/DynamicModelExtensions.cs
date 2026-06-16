using Puya.Collections;

namespace Puya.Extensions
{
    public static class DynamicModelExtensions
    {
        public static bool HasProp(this DynamicModel model, string prop)
        {
            return model.ContainsKey(prop) && !string.IsNullOrEmpty(model[prop]?.ToString());
        }
    }
}
