using Puya.Data;

namespace Puya.Net.Tests
{
    public class ConnectionStringProvider
    {
        [Fact]
        public void TestDefaultEntry()
        {
            var constrProvider = new DefaultConnectionStringProvider();

            Assert.Equal(0, constrProvider.Count);

            var constr = "server=.;database=mydb;user id=sa;password=1234";

            constrProvider.SetConnectionString(constr);

            var value = constrProvider.GetConnectionString();

            Assert.Equal(constr, value);
        }
        [Fact]
        public void TestNewEntry()
        {
            var constrProvider = new DefaultConnectionStringProvider();

            var constr = "server=.;database=mydb;user id=sa;password=1234";
            var name = "c1";

            constrProvider.SetConnectionString(name, constr);

            Assert.Equal(1, constrProvider.Count);

            var value = constrProvider.GetConnectionString(name);

            Assert.Equal(constr, value);
        }
        [Fact]
        public void TestMixed()
        {
            var constrProvider = new DefaultConnectionStringProvider();

            var constr1 = "server=.;database=mydb;user id=sa;password=1234";
            var constr2 = "server=.;database=hisdb;user id=sa;password=1234";
            var name = "c1";

            constrProvider.SetConnectionString(constr1);
            constrProvider.SetConnectionString(name, constr2);

            Assert.Equal(2, constrProvider.Count);

            var value = constrProvider.GetConnectionString(name);

            Assert.Equal(constr2, value);

            value = constrProvider.GetConnectionString();

            Assert.Equal(constr1, value);
        }
        [Fact]
        public void TestCurrent()
        {
            var constrProvider = new DefaultConnectionStringProvider();

            var constr1 = "server=.;database=mydb;user id=sa;password=1234";
            var constr1new = "server=.;database=mydb;user id=user;password=1234";
            var constr2 = "server=.;database=hisdb;user id=sa;password=1234";
            var name1 = "c1";
            var name2 = "c2";

            constrProvider.SetConnectionString(name1, constr1);
            constrProvider.SetConnectionString(name2, constr2);

            Assert.Equal(2, constrProvider.Count);

            constrProvider.SetCurrent(name2);

            var value = constrProvider.GetConnectionString();

            Assert.Equal(constr2, value);

            constrProvider.SetCurrent(name1);

            value = constrProvider.GetConnectionString();

            Assert.Equal(constr1, value);

            constrProvider.SetConnectionString(constr1new);

            value = constrProvider.GetConnectionString(name1);

            Assert.Equal(constr1new, value);

            value = constrProvider.GetConnectionString();

            Assert.Equal(constr1new, value);
        }
    }
}
