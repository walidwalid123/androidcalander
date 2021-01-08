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

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using FwUpdateApiSample;

namespace FwUpdateTool.WizardSteps
{
    internal interface ISdk
    {
        SdkTbtBase SdkHandle { get; }
    }

    internal class Device : ISdk
    {
        private readonly SdkTbtDevice _sdkHandle;

        public SdkTbtBase SdkHandle
        {
            get { return _sdkHandle; }
        }

        public Device(SdkTbtDevice sdkHandle)
        {
            _sdkHandle = sdkHandle;
            DisplayName = string.Format("{0}, {1}", _sdkHandle.VendorName, _sdkHandle.ModelName);
            if (_sdkHandle.NumOfControllers > 1)
            {
                DisplayName += string.Format(": {0}/{1}", _sdkHandle.ControllerNum, _sdkHandle.NumOfControllers);
            }
            IsSelectable = _sdkHandle.Updatable && Utilities.IsSupported(_sdkHandle.ControllerId);
        }

        public string DisplayName { get; private set; }

        public bool IsSelectable { get; private set; }
    }

    internal class Port
    {
        public string DisplayName { get; private set; }

        public Port(uint index, IOrderedEnumerable<SdkTbtDevice> currentDevices)
        {
            DisplayName = "Port " + (index + 1) + ":";
            Devices = new List<Device>();
            foreach (var device in currentDevices)
            {
                Devices.Add(new Device(device));
            }
        }

        public List<Device> Devices { get; private set; }

        public bool IsSelectable
        {
            get { return false; }
        }
    }

    internal class Controller : ISdk
    {
        private readonly SdkTbtController _sdkHandle;

        public SdkTbtBase SdkHandle
        {
            get { return _sdkHandle; }
        }

        public Controller(SdkTbtController sdkHandle, IEnumerable<SdkTbtDevice> devices)
        {
            _sdkHandle = sdkHandle;

            DisplayName = _sdkHandle.ControllerId;

            var sdkTbtDevices = devices as IList<SdkTbtDevice> ?? devices.ToList();
            var ports = from device in sdkTbtDevices
                group device by device.PortNum
                into port
                select port;
            Ports = new List<Port>();
            foreach (var port in ports)
            {
                var localPort = port;
                var currentDevices = from device in sdkTbtDevices
                    where device.PortNum == localPort.Key
                    orderby device.PositionInChain
                    select device;

                Ports.Add(new Port(port.Key, currentDevices));
            }

            IsSelectable = Utilities.HostUpdateSupported(DisplayName);
        }

        public string DisplayName { get; private set; }
        public List<Port> Ports { get; private set; }
        public bool IsSelectable { get; private set; }
    }

    /// <summary>
    /// Interaction logic for SelectController.xaml
    /// </summary>
    public partial class SelectController : IWizardScreen, INotifyPropertyChanged
    {
        public SelectController(List<SdkTbtController> controllers, List<SdkTbtDevice> devices)
        {
            InitializeComponent();
            var treeItems = new List<Controller>();
            foreach (var controller in controllers)
            {
                var localController = controller;
                var deviceOfController = from device in devices
                    where device.ControllerId == localController.ControllerId
                    select device;

                treeItems.Add(new Controller(controller, deviceOfController));
            }
            DevicesTree.ItemsSource = treeItems;
        }

        #region IWizardScreen

        public bool CancelButtonActive
        {
            get { return true; }
        }

        public bool NextButtonActive
        {
            get { return DevicesTree.SelectedItem != null; }
        }

        public bool BackButtonActive
        {
            get { return true; }
        }

        public string[] Title
        {
            get { return new[] {"Select", "Controller/Device"}; }
        }

        #endregion

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var mainWin = (Application.Current.MainWindow as MainWindow);
            if (mainWin != null)
            {
                mainWin.CurrentController = ((ISdk) DevicesTree.SelectedItem).SdkHandle;
                OnPropertyChanged("NextButtonActive");
            }
        }

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

        #endregion
    }
}
