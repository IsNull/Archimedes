using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Archimedes.Services.WPF.WindowViewModelMapping
{
    /// <summary>
    /// Class describing the Window-ViewModel mappings used by the dialog service.
    /// </summary>
    public class WindowViewModelMappings : IWindowViewModelMappings
    {
        private readonly IDictionary<Type, Type> _mappings;


        /// <summary>
        /// Initializes a new instance of the <see cref="WindowViewModelMappings"/> class.
        /// </summary>
        public WindowViewModelMappings() {
            _mappings = new Dictionary<Type, Type>();
        }

        public void RegisterMapping(Type viewModel, Type view){
            if (!_mappings.ContainsKey(viewModel))
                _mappings.Add(viewModel, view);
            else
                _mappings[viewModel] = view;
        }

        [DebuggerStepThrough]
        public Type GetViewTypeFromViewModelType(Type viewModelType) {
            if (_mappings.ContainsKey(viewModelType))
                return _mappings[viewModelType];
            else {
                var possibleMatches = from kt in _mappings.Keys
                                      where kt.IsAssignableFrom(viewModelType)
                                      select kt;
                if (possibleMatches.Any())
                    return _mappings[possibleMatches.First()];
            }
            throw new KeyNotFoundException(viewModelType + " was not found!");
        }

        /// <summary>
        /// Gets the window type based on registered ViewModel type.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel.</param>
        /// <returns>The window type based on registered ViewModel type.</returns>
        public Type GetWindowTypeFromViewModelType(Type viewModelType) {
            return _mappings[viewModelType];
        }



    }
}
