using Puya.Data;
using Puya.Extensions;
using Puya.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Core.Tests.Service
{
    public class ServiceResponse_Tests
    {
        [Fact]
        public void test_finalize1()
        {
            var args = new
            {
                Result = CommandHelper.Result()
            };

            var res = new ServiceResponse();

            res.Finalize(args);

            Assert.True(res.Success);
        }
        [Fact]
        public void test_finalize2()
        {
            var args = new
            {
                Result = CommandHelper.Result()
            };

            args.Result.Value = "Success";

            var res = new ServiceResponse();

            res.Finalize(args);

            Assert.True(res.Success);
        }
        [Fact]
        public void test_finalize3()
        {
            var args = new
            {
                Result = CommandHelper.Result()
            };

            args.Result.Value = "Failed";

            var res = new ServiceResponse();

            res.Finalize(args);

            Assert.False(res.Success);
            Assert.Equal(res.Status, "Failed");
        }
    }
}
