using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps.GeoCoding
{
    /// <summary>
    /// Represents the status code of a GeoCode operation
    /// </summary>
    public enum GeoCodeStatus
    {
        /// <summary>
        /// No or unknown status
        /// </summary>
        None,

        /// <summary>
        /// Success
        /// </summary>
        Success,


        /// <summary>
        /// The query was valid but no results found
        /// </summary>
        NotFound,

        /// <summary>
        /// Indicates that the query was wrong/malformed and could not be understand
        /// </summary>
        QueryError,

        /// <summary>
        /// Indicates that the request was denied by the server
        /// </summary>
        RequestDenied,

        /// <summary>
        /// Indicates that too many querys or a too big query was requested and the server has denied it
        /// </summary>
        ExeededQueryLimit,

        /// <summary>
        /// Server error (such as Network Communication, Server-Error etc)
        /// </summary>
        ServerError,

        /// <summary>
        /// General Error 
        /// </summary>
        Error

    }
}
