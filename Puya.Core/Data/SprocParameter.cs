namespace Puya.Data
{
    public class SprocParameter
    {
        public string name { get; set; }
        public string type_name { get; set; }
        public bool is_output { get; set; }
        public int parameter_id { get; set; }
        public int max_length { get; set; }
        public int precision { get; set; }
        public int scale { get; set; }
        public int system_type_id { get; set; }
    }
}
