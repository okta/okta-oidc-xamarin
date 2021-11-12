// <copyright file="Iri.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Widget
{
    /// <summary>
    /// Represents an Iri, see https://tools.ietf.org/html/rfc3987.  This implementation should be considered a place holder for the extension defined in rfc3987;
    /// for now Iri extends Uri, in the future this will likely not be the case.
    /// </summary>
    public class Iri : Uri
    {
        public static implicit operator string(Iri iri)
        {
            return iri.ToString();
        }

        public static implicit operator Iri(string value)
        {
            return new Iri(value);
        }

        public Iri(string uriString) : base(uriString)
        {
        }

        /// <summary>
        /// Returns a value indicating if the specified string is a valid `Iri`.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <param name="iri">The parsed `Iri`.</param>
        /// <param name="exceptionHandler">Action that receives the exception when a failure occurs.</param>
        /// <returns>`bool`.</returns>
        public static bool IsIri(string url, out Iri iri, Action<Exception> exceptionHandler = null)
        {
            iri = null;
            try
            {
                iri = new Iri(url);
                return true;
            }
            catch (Exception ex)
            {
                if(exceptionHandler == null)
                {
                    exceptionHandler = (exception) => Console.WriteLine($"{exception.Message}:\r\n\t{exception.StackTrace}");
                }
                exceptionHandler(ex);
                return false;
            }
        }
    }
}
