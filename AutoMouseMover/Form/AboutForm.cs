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

using System;
using System.Windows.Forms;
using AutoMouseMover.Utils;

namespace AutoMouseMover
{
    //
    // About form
    //
    public partial class AboutForm : Form
    {
        //
        // Constructor
        //
        #region Constructor

        // Constructor
        public AboutForm()
        {
            InitializeComponent();
            // Get assembly information
            var ass_helper = new AssemblyHelper();
            // Set labels
            ProductLabel.Text = String.Format(ProductLabel.Text, ass_helper.Product);
            DescriptionLabel.Text = String.Format(DescriptionLabel.Text, ass_helper.Description);
            VersionLabel.Text = String.Format(VersionLabel.Text, ass_helper.Version);
            CopyrightLabel.Text = String.Format(CopyrightLabel.Text, ass_helper.Copyright);
        }

        #endregion

        //
        // GUI events
        //
        #region GUI events

        /* Close button */
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
