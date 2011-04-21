using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Services.WPF.WindowViewModelMapping
{
    /// <summary>
    /// Class describing the Window-ViewModel mappings used by the dialog service.
    /// </summary>
    public class WindowViewModelMappings : IWindowViewModelMappings
    {
        private IDictionary<Type, Type> mappings;


        /// <summary>
        /// Initializes a new instance of the <see cref="WindowViewModelMappings"/> class.
        /// </summary>
        public WindowViewModelMappings() {
            mappings = new Dictionary<Type, Type>();
        }

        public void RegisterMapping(Type viewModel, Type view){
            if (!mappings.ContainsKey(viewModel))
                mappings.Add(viewModel, view);
            else
                mappings[viewModel] = view;
        }


        public Type GetViewTypeFromViewModelType(Type viewModelType) {
            return mappings[viewModelType];
        }

        /// <summary>
        /// Gets the window type based on registered ViewModel type.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel.</param>
        /// <returns>The window type based on registered ViewModel type.</returns>
        public Type GetWindowTypeFromViewModelType(Type viewModelType) {
            return mappings[viewModelType];
        }



    }
}
