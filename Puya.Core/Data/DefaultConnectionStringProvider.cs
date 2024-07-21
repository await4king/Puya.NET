namespace Puya.Data
{
    public class DefaultConnectionStringProvider : IConnectionStringProvider
    {
        private string constr;
        public string GetConnectionString()
        {
            return constr;
        }
        public void SetConnectionString(string constr)
        {
            this.constr = constr;
        }
    }
}
