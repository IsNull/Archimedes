using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Archimedes.Services.WPF.WindowViewModelMapping
{
    /// <summary>
    /// Interface describing the Window-ViewModel mappings used by the dialog service.
    /// </summary>
    [ContractClass(typeof(IWindowViewModelMappingsContract))]
    public interface IWindowViewModelMappings
    {
        /// <summary>
        /// Gets the window type based on registered ViewModel type.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel.</param>
        /// <returns>The window type based on registered ViewModel type.</returns>
        Type GetWindowTypeFromViewModelType(Type viewModelType);

        /// <summary>
        /// Gets the view type based on registered ViewModel type.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel.</param>
        /// <returns>The view type based on registered ViewModel type.</returns>
        Type GetViewTypeFromViewModelType(Type viewModelType);

        /// <summary>
        /// Registers a new viewmodel-view mapping
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="view"></param>
        void RegisterMapping(Type viewModel, Type view);
    }
}
