using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Puya.Notification
{
    public interface INotifier
    {
        void Notify(Expression<Action> action);
        void Notify<T>(Expression<Action<T>> action);
        void Notify(Expression<Func<Task>> fn);
        void Notify<T>(Expression<Func<T, Task>> fn);
    }
}
