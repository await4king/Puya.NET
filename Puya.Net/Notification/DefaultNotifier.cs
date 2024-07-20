using Hangfire;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Puya.Notification
{
    public class DefaultNotifier : INotifier
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        public DefaultNotifier(IBackgroundJobClient backgroundJobClient)
        {
            this.backgroundJobClient = backgroundJobClient;
        }
        public void Notify(Expression<Action> action)
        {
            backgroundJobClient.Enqueue(action);
        }
        public void Notify<T>(Expression<Action<T>> action)
        {
            backgroundJobClient.Enqueue<T>(action);
        }
        public void Notify(Expression<Func<Task>> fn)
        {
            backgroundJobClient.Enqueue(fn);
        }
        public void Notify<T>(Expression<Func<T, Task>> fn)
        {
            backgroundJobClient.Enqueue<T>(fn);
        }
    }
}
