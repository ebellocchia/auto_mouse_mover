/*
 * Copyright (c) 2020 Emanuele Bellocchia
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Reflection;

namespace AutoMouseMover.Utils
{
    //
    // Assembly helper
    //
    class AssemblyHelper
    {
        //
        // Members
        //
        #region Members

        // Current assembly
        private Assembly mAssembly;

        #endregion

        //
        // Public methods
        //
        #region Public methods

        // Constructor
        public AssemblyHelper()
        {
            mAssembly = Assembly.GetExecutingAssembly();
        }

        #endregion

        //
        // Properties
        //
        #region Properties

        // Title property
        public string Title
        {
            get { return mAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title; }
        }

        // Description property
        public string Description
        {
            get { return mAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description; }
        }

        // Configuration property
        public string Configuration
        {
            get { return mAssembly.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration; }
        }

        // Company property
        public string Company
        {
            get { return mAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company; }
        }

        // Product property
        public string Product
        {
            get { return mAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product; }
        }

        // Copyright property
        public string Copyright
        {
            get { return mAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright; }
        }

        // Trademark property
        public string Trademark
        {
            get { return mAssembly.GetCustomAttribute<AssemblyTrademarkAttribute>().Trademark; }
        }

        // Version property
        public string Version
        {
            get { return mAssembly.GetName().Version.ToString(); }
        }

        #endregion
    }
}
