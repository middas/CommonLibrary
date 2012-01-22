using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.Patterns.Observer
{
    public interface IObserver
    {
        void Update(object subject);
    }
}
