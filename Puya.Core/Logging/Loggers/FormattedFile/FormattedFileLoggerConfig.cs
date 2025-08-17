using System;

namespace Puya.Logging
{
    public class FormattedFileLoggerConfig : FileLoggerConfig
    {
        private string rowSeparator;
        public string RowSeparator
        {
            get { return rowSeparator; }
            set
            {
                rowSeparator = value;

                var f = Formatter as FormattedFileStringLogFormatter;

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

                var f = Formatter as FormattedFileStringLogFormatter;

                if (f != null)
                {
                    f.ColSeparator = value;
                }
            }
        }
        #region ctor
        public FormattedFileLoggerConfig() : this(null)
        { }
        public FormattedFileLoggerConfig(ILoggingPolicy policy) : base(policy)
        {
            RowSeparator = Environment.NewLine;
            ColSeparator = ',';
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            var result = new FormattedFileStringLogFormatter();

            result.RowSeparator = RowSeparator;
            result.ColSeparator = ColSeparator;

            return result;
        }
        #endregion
    }
}
