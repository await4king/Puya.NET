namespace Puya.Collections
{
    public class DynamicModel : CaseInsensitiveDictionary<object>
    {
        public DynamicModel() : base(true)
        { }
    }
    public class DynamicModel<T> : CaseInsensitiveDictionary<T>
    {
        public DynamicModel() : base(true)
        { }
    }
}
