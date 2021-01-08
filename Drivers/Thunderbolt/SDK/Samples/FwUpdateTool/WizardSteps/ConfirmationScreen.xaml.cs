/*******************************************************************************
* Copyright (C) 2014-2017 Intel Corp. All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*  - Redistributions of source code must retain the above copyright notice,
*    this list of conditions and the following disclaimer.
*
*  - Redistributions in binary form must reproduce the above copyright notice,
*    this list of conditions and the following disclaimer in the documentation
*    and/or other materials provided with the distribution.
*
*  - Neither the name of Intel Corp. nor the names of its
*    contributors may be used to endorse or promote products derived from this
*    software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ``AS IS''
* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
* ARE DISCLAIMED. IN NO EVENT SHALL Intel Corp. OR THE CONTRIBUTORS
* BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
* CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
* SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
* CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
* ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
* POSSIBILITY OF SUCH DAMAGE.
*******************************************************************************/

using System.ComponentModel;
using System.Windows;
using FwUpdateApiSample;

namespace FwUpdateTool.WizardSteps
{
    /// <summary>
    /// Interaction logic for ConfirmationScreen.xaml
    /// </summary>
    public partial class ConfirmationScreen : IWizardScreen, INotifyPropertyChanged
    {
        #region members

        private bool _isHost;
        private string _nvmVersion;
        private string _nvmFileVersion;
        private string _pdVersion;
        private string _pdFileVersion;
        private bool? _osNativePciEnumeration;
        private bool _osNativePciEnumerationFile;

        /// <summary>
        /// Tells if the current controller is host controller or device
        /// Controls the visibility of host-specific UI elements
        /// </summary>
        public bool IsHost
        {
            get { return _isHost; }
            set
            {
                _isHost = value;
                OnPropertyChanged("IsHost");
            }
        }

        /// <summary>
        /// Current NVM version that will appear in confirmation screen
        /// </summary>
        public string NvmVersion
        {
            get { return _nvmVersion; }
            set
            {
                _nvmVersion = value;
                OnPropertyChanged("NvmVersion");
            }
        }

        public string NvmFileVersion
        {
            get { return _nvmFileVersion; }
            set
            {
                _nvmFileVersion = value;
                OnPropertyChanged("NvmFileVersion");
            }
        }

        /// <summary>
        /// Current PD version that will appear in confirmation screen
        /// </summary>
        public string PdVersion
        {
            get { return _pdVersion; }
            set
            {
                _pdVersion = value;
                OnPropertyChanged("PdVersion");
            }
        }

        /// <summary>
        /// PD version in FW image file that will appear in confirmation screen
        /// </summary>
        public string PdFileVersion
        {
            get { return _pdFileVersion; }
            set
            {
                _pdFileVersion = value;
                OnPropertyChanged("PdFileVersion");
            }
        }

        /// <summary>
        /// Controller "native express" status that will appear in confirmation screen
        /// null - for safe-mode
        /// Host-only property; doesn't matter when updating a device
        /// </summary>
        public bool? OsNativePciEnumeration
        {
            get { return _osNativePciEnumeration; }
            set
            {
                _osNativePciEnumeration = value;
                OnPropertyChanged("OsNativePciEnumeration");
            }
        }

        /// <summary>
        /// FW image file "native express" status that will appear in confirmation screen
        /// Host-only property; doesn't matter when updating a device
        /// </summary>
        public bool OsNativePciEnumerationFile
        {
            get { return _osNativePciEnumerationFile; }
            set
            {
                _osNativePciEnumerationFile = value;
                OnPropertyChanged("OsNativePciEnumerationFile");
            }
        }

        #endregion

        public ConfirmationScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start FW update process button, switching to next screen ( FwUpdateProcessScreen )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWin = (Application.Current.MainWindow as MainWindow);
            if (mainWin != null)
            {
                mainWin.NextButton_Click(sender, e);
            }
        }

        #region IWizardScreen

        public bool CancelButtonActive
        {
            get { return true; }
        }

        public bool NextButtonActive
        {
            get { return false; }
        }

        public bool BackButtonActive
        {
            get { return true; }
        }

        public string[] Title
        {
            get { return new[] {"Confirmation"}; }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event 
        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void OnPageChange()
        {
            OnPropertyChanged("CancelButtonActive");
            OnPropertyChanged("NextButtonActive");
            OnPropertyChanged("BackButtonActive");
        }

        #endregion
    }
}
