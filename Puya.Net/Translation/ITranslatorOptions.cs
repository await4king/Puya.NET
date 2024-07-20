namespace Puya.Translation
{
    public interface IKeyOptions
    {
        string PartSeparator { get; set; }
        bool RequiresFirstSeparator { get; set; }
    }
    public class KeyOptions : IKeyOptions
    {
        public string PartSeparator { get; set; }
        public bool RequiresFirstSeparator { get; set; }
        public KeyOptions()
        {
            PartSeparator = "/";
            RequiresFirstSeparator = true;
        }
    }
    public interface ITranslatorOptions
    {
        string CultureDependentTextExtension { get; set; }
        string CultureIndependentTextExtension { get; set; }
        IKeyOptions KeyOptions { get; set; }
        IKeyOptions MapKeyOptions { get; set; }
        char KeyValueSeparator { get; set; }
        char[] CommentCharacters { get; set; }
    }
}
