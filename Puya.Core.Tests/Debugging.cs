using Puya.Core.Debugging;
using Puya.Debugging;

namespace Puya.Core.Tests
{
    public class Debugging
    {
        #region debug generally
        [Fact]
        public void TestManualDebugger_Init_DebuggingDisabled_NoGlobal()
        {
            var options = new DebuggerOptions();
            var debugger = new ManualDebugger(options);

            Assert.False(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_Init_DebuggingEnabled_NoGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true };
            var debugger = new ManualDebugger(options);

            Assert.False(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_Init_DebuggingEnabled_UseGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true, GlobalDebugging = true };
            var debugger = new ManualDebugger(options);

            Assert.True(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_Init_DebuggingDisabled_UseGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = false, GlobalDebugging = true };
            var debugger = new ManualDebugger(options);

            Assert.False(debugger.IsDebugging);
        }
        #endregion
        #region debug by username
        [Fact]
        public void TestManualDebugger_DebuggingEnabled_User_Is_Debugger_NoGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true, DebuggerUsers = "reza, ali, hasan" };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("ali");
            
            Assert.True(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_DebuggingDisabled_User_Is_Debugger_NoGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = false, DebuggerUsers = "reza, ali, hasan" };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("ali");

            Assert.False(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_DebuggingEnabled_User_IsNot_Debugger_NoGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true, DebuggerUsers = "reza, ali, hasan" };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("saeed");

            Assert.False(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_DebuggingEnabled_User_IsNot_Debugger_UseGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true, DebuggerUsers = "reza, ali, hasan", GlobalDebugging =  true };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("saeed");

            Assert.True(debugger.IsDebugging);
        }
        #endregion
        #region debug by role
        [Fact]
        public void TestManualDebugger_DebuggingEnabled_User_Has_DebuggerRole_NoGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true, DebuggerRoleName = "debugger" };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("ali");
            debugger.SetRoles("member,debugger,operator");

            Assert.True(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_DebuggingDisabled_User_Has_DebuggerRole_NoGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = false, DebuggerRoleName = "debugger" };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("ali");
            debugger.SetRoles("member,debugger,operator");

            Assert.False(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_DebuggingEnabled_User_HasNot_DebuggerRole_NoGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true, DebuggerRoleName = "debugger" };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("ali");
            debugger.SetRoles("member,operator");

            Assert.False(debugger.IsDebugging);
        }
        [Fact]
        public void TestManualDebugger_DebuggingEnabled_User_HasNot_DebuggerRole_UseGlobal()
        {
            var options = new DebuggerOptions { DebuggingEnabled = true, DebuggerRoleName = "debugger", GlobalDebugging = true };
            var debugger = new ManualDebugger(options);

            debugger.SetUserName("ali");
            debugger.SetRoles("member,operator");

            Assert.True(debugger.IsDebugging);
        }
        #endregion
    }
}
