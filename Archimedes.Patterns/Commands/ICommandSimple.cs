using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Commands
{
    public interface ICommandSimple
    {
       void Execute(object args);
       bool CanExecute(object parameter);
       event EventHandler CanExecuteChanged;
    }
}
