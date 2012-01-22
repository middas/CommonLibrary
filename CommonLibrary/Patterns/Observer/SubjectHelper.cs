using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CommonLibrary.Patterns.Observer
{
    public class SubjectHelper : ISubject
    {
        private ArrayList _Observers = new ArrayList();

        #region ISubject Members

        public void Add(IObserver observer)
        {
            _Observers.Add(observer);
        }

        public void Remove(IObserver observer)
        {
            _Observers.Remove(observer);
        }

        public void Notify(object realSubject)
        {
            foreach (IObserver observer in _Observers)
            {
                observer.Update(realSubject);
            }
        }

        #endregion
    }
}
