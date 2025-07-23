namespace Puya.Logging
{
    public class CsvFileLoggerConfig : FileLoggerConfig
    {
        private char rowSeparator;
        public char RowSeparator
        {
            get { return rowSeparator; }
            set
            {
                rowSeparator = value;

                var f = Formatter as CsvStringLogFormatter;

                if (f != null)
                {
                    f.RowSeparator = value;
                }
            }
        }
        private char colSeparator;
        public char ColSeparator
        {
            get { return colSeparator; }
            set
            {
                colSeparator = value;

                var f = Formatter as CsvStringLogFormatter;

                if (f != null)
                {
                    f.ColSeparator = value;
                }
            }
        }
        public bool FirstRowIsHeading { get; set; }
        public override string FileExtension { get; set; }
        #region ctor
        public CsvFileLoggerConfig() : this(null)
        { }
        public CsvFileLoggerConfig(ILogFormatter formatter) : base(formatter)
        {
            FileExtension = ".csv";
            RowSeparator = '\n';
            ColSeparator = ';';
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            var result = new CsvStringLogFormatter();

            result.RowSeparator = RowSeparator;
            result.ColSeparator = ColSeparator;

            return result;
        }
        #endregion
    }
}
