/*******************************************************************************
* Copyright (C) 2016 Intel Corp. All rights reserved.
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

using FwUpdateDriverApi;
using System;
using System.ComponentModel;
using System.Management;

namespace FwUpdateApiSample
{
    /// <summary>
    /// This class provides the application with ability 
    /// to subscribe for getting updates about the flash process progression.
    ///
    /// The class exposes an event to subscribe to and can be used also by WPF
    /// by binding to its Progress property.
    /// The Registered property can be used to check if the event registration
    /// succeeded or not. If Registered is false, no progress update will come.
    /// </summary>
    public class FlashProgress : INotifyPropertyChanged, IDisposable
    {
        private UInt32 _progress;

        private readonly bool _registered = true;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns the current progress value
        /// </summary>
        public UInt32 Progress
        {
            get { return _progress; }

            private set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        /// <summary>
        /// Returns if it's registered. If it's false, no progress
        /// update will come.
        /// </summary>
        public bool Registered
        {
            get { return _registered; }
        }

        public FlashProgress()
        {
            var watcher = FwUpdateProgressFactory.GetProgressWatcher();
            watcher.ProgressUpdated += OnProgressDetected;
        }

        private void OnProgressDetected(object sender, FlashProgressEventArgs e)
        {
            Progress = e.Progress;
        }

        private void OnPropertyChanged(string name)
        {
            var clients = PropertyChanged;
            if (clients != null)
            {
                clients(this, new PropertyChangedEventArgs(name));
            }
        }

        public void Dispose()
        {
            var watcher = FwUpdateProgressFactory.GetProgressWatcher();
            watcher.ProgressUpdated -= OnProgressDetected;
        }
    }
}
