namespace Puya.Data
{
    public class SequenceCreateOptions
    {
        public string Schema { get; set; }
        public int Start { get; set; }
        public int Increment { get; set; }
        public SequenceCreateOptions()
        {
            Schema = "dbo";
        }
    }
}
