

namespace Archimedes.Patterns.WPF.ViewModels
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a dialog box.
    /// </summary>
    public enum DialogResultType
    {
        /// <summary>
        /// Nothing is returned from the dialog box.
        /// </summary>
        None = 0,

        /// <summary>
        /// The dialog box has been accepted/confirmed/liked/yes
        /// </summary>
        Affirmative = 1,

        /// <summary>
        /// The dialog box has been declined/aborted/disliked/no
        /// </summary>
        Negative = 2,

        /// <summary>
        /// The dialog box was canceled
        /// </summary>
        Cancel = 3,

        /// <summary>
        /// The dialog box return value is Retry (usually sent from a button labeled Retry).
        /// </summary>
        Retry = 4,

        /// <summary>
        /// The dialog box return value is Ignore (usually sent from a button labeled Ignore).
        /// </summary>
        Ignore = 5,
    }
}
