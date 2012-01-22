using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CommonLibrary.Patterns.Observer
{
    public interface ISubject
    {
        void Add(IObserver observer);
        void Remove(IObserver observer);
        void Notify(object realSubject);
    }
}
