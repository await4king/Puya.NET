using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Data
{
    public class FakeDbParameterCollection : DbParameterCollection
    {
        public override int Count => 0;

        public override object SyncRoot => new object();

        public override int Add(object value)
        {
            return 0;
        }
        public override void AddRange(Array values)
        { }
        public override void Clear()
        { }
        public override bool Contains(object value)
        {
            return false;
        }
        public override bool Contains(string value)
        {
            return false;
        }
        public override void CopyTo(Array array, int index)
        { }
        public override IEnumerator GetEnumerator()
        {
            return null;
        }

        public override int IndexOf(object value)
        {
            return -1;
        }

        public override int IndexOf(string parameterName)
        {
            return -1;
        }

        public override void Insert(int index, object value)
        { }

        public override void Remove(object value)
        { }

        public override void RemoveAt(int index)
        { }

        public override void RemoveAt(string parameterName)
        { }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return null;
        }

        protected override void SetParameter(int index, DbParameter value)
        { }

        protected override void SetParameter(string parameterName, DbParameter value)
        { }
    }
}
