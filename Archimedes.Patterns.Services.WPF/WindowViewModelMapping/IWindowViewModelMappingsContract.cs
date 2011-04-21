using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Archimedes.Services.WPF.WindowViewModelMapping
{
    [ContractClassFor(typeof(IWindowViewModelMappings))]
    abstract class IWindowViewModelMappingsContract : IWindowViewModelMappings
    {
        /// <summary>
        /// Gets the window type based on registered ViewModel type.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel.</param>
        /// <returns>
        /// The window type based on registered ViewModel type.
        /// </returns>
        public Type GetWindowTypeFromViewModelType(Type viewModelType) {
            Contract.Ensures(Contract.Result<Type>().IsSubclassOf(typeof(Window)));

            return default(Type);
        }

        
        public void RegisterMapping(Type viewModel, Type view) {
 
        }

        public Type GetViewTypeFromViewModelType(Type viewModelType) {
            Contract.Ensures(Contract.Result<Type>().IsSubclassOf(typeof(FrameworkElement)));
            return default(Type);
        }
    }
}
