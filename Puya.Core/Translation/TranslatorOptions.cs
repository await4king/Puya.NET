namespace Puya.Translation
{
    public class TranslatorOptions: ITranslatorOptions
    {
        public string CultureDependentTextExtension { get; set; }
        public string CultureIndependentTextExtension { get; set; }
        public char KeyValueSeparator { get; set; }
        public char[] CommentCharacters { get; set; }
        private IKeyOptions keyOptions;
        public IKeyOptions KeyOptions
        {
            get
            {
                if (keyOptions == null)
                {
                    keyOptions = new KeyOptions();
                }

                return keyOptions;
            }
            set { keyOptions = value; }
        }
        public IKeyOptions MapKeyOptions { get; set; }
        public TranslatorOptions()
        {
            CultureDependentTextExtension = ".cdt";
            CultureIndependentTextExtension = ".cit";
            KeyValueSeparator = ':';
            CommentCharacters = new char[] { '#' };
        }
    }
}
