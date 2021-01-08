/*******************************************************************************
* Copyright (C) 2014-2016 Intel Corp. All rights reserved.
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

using System;
using System.ComponentModel;
using System.Windows.Media.Animation;
using FwUpdateApiSample;

namespace FwUpdateTool.WizardSteps
{
    /// <summary>
    /// Interaction logic for FwUpdateProcessScreen.xaml
    /// In this form a progress bar is implemented. 
    /// </summary>
    public partial class FwUpdateProcessScreen : IWizardScreen, INotifyPropertyChanged, IDisposable
    {
        private readonly DoubleAnimation _ani = new DoubleAnimation(1, 0.3, TimeSpan.FromMilliseconds(1000));
        private readonly FlashProgress _flashProgress = new FlashProgress();

        public FwUpdateProcessScreen()
        {
            InitializeComponent();
            InitializeFlashAnimation();
            DataContext = _flashProgress;
        }

        /// <summary>
        /// Initialize the animation that appears during the FW update process
        /// </summary>
        private void InitializeFlashAnimation()
        {
            _ani.AutoReverse = true;
            _ani.Completed += ani_Completed;
            Logo.BeginAnimation(OpacityProperty, _ani);
        }

        /// <summary>
        /// restart the animation once completed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ani_Completed(object sender, EventArgs e)
        {
            Logo.BeginAnimation(OpacityProperty, _ani);
        }

        #region IWizardScreen

        public bool CancelButtonActive
        {
            get { return false; }
        }

        public bool NextButtonActive
        {
            get { return false; }
        }

        public bool BackButtonActive
        {
            get { return false; }
        }

        public string[] Title
        {
            get { return new[] {"Flashing..."}; }
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

        public void Dispose()
        {
            _flashProgress.Dispose();
        }
    }
}
