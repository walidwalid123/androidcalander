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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Management;
using FwUpdateTool.WizardSteps;
using FwUpdateApiSample;

namespace FwUpdateTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly List<IWizardScreen> _pages = new List<IWizardScreen>();
        public string ImageFileName { private get; set; }
        public SdkTbtBase CurrentController { private get; set; }
        private string FlashingResult { get; set; }
        private Task _flashTask;
        private int _currentScreen = 0;
        private bool _isDuringFwUpdate = false;
        private bool _needsG3 = false;

        private class InitException : Exception
        {
        }

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                var controllers = LoadControllers().Values.ToList();
                var devices = LoadDevices().Values.ToList();
                BuildWizard(controllers, devices);
                ConfigFwUpdateTask();
            }
            catch (InitException)
            {
                Close();
            }
        }

        private static Dictionary<string, SdkTbtDevice> LoadDevices()
        {
            // If LoadControllers() worked, this must work too
            return SdkTbtDevice.GetDevices();
        }

        /// <summary>
        /// load controllers list from API
        /// </summary>
        /// <exception cref="InitException">If initialization fails, throws to stop the c-tor</exception>
        private static Dictionary<string, SdkTbtController> LoadControllers()
        {
            try
            {
                while (true)
                {
                    var controllers = SdkTbtController.GetControllers();
                    if (controllers.Count != 0) // Success
                    {
                        return controllers;
                    }
                    var res = MessageBox.Show(FwUpdateApiSample.Resources.NoControllerStringPart0 + "\n"
                                              + FwUpdateApiSample.Resources.NoDeviceStringPart1 + "\n"
                                              + FwUpdateApiSample.Resources.NoDeviceStringPart2GUI,
                        FwUpdateApiSample.Resources.MessageBoxCaption,
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (res != MessageBoxResult.OK)
                    {
                        break; // Stop trying
                    }
                }
            }
            catch (ManagementException)
            {
                MessageBox.Show(FwUpdateApiSample.Resources.SWNotInstalled, FwUpdateApiSample.Resources.MessageBoxCaption,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, FwUpdateApiSample.Resources.MessageBoxCaption, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            // We come here only in case of failure
            throw new InitException();
        }

        /// <summary>
        /// Preparing async task for running the FW update in other thread
        /// </summary>
        private void ConfigFwUpdateTask()
        {
            _flashTask = new Task(() =>
            {
                _isDuringFwUpdate = true;
                try
                {
                    CurrentController.UpdateFirmwareFromFile(ImageFileName);
                    FlashingResult = FwUpdateApiSample.Resources.FWUpdateSuccessMessage;
                }
                catch (TbtException e)
                {
                    FlashingResult = e.TbtMessage;
                }
                catch (Exception e)
                {
                    var msg = new TbtException(TbtStatus.SDK_GENERAL_ERROR_CODE, e.Message).Message;
                    if (!e.Message.Any())
                    {
                        msg += "\n" + e.HResult;
                    }

                    FlashingResult = msg;
                }
            });

            //When FW update task done update result message and go to finish screen
            _flashTask.ContinueWith(task => Dispatcher.Invoke(
                () =>
                {
                    NextButton_Click(this, new RoutedEventArgs());
                    var resultScreen = PageContent.Content as UpdateCompletedScreen;
                    if (resultScreen != null)
                    {
                        resultScreen.ResultMessage.Text = FlashingResult;
                    }
                    _isDuringFwUpdate = false;
                    if (FlashingResult == FwUpdateApiSample.Resources.FWUpdateSuccessMessage && _needsG3)
                    {
                        MessageBox.Show(CurrentController.GetNeedPowerDownMessage,
                            FwUpdateApiSample.Resources.MessageBoxCaption, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }));
        }

        /// <summary>
        /// For preventing window to be closed during FW update process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_isDuringFwUpdate)
            {
                MessageBox.Show(FwUpdateApiSample.Resources.CloseDuringUpdate, FwUpdateApiSample.Resources.MessageBoxCaption,
                    MessageBoxButton.OK, MessageBoxImage.Information);

                e.Cancel = true;
            }
            else
            {
                var screen = _pages.Find(x => x.GetType() == typeof (FwUpdateProcessScreen)) as FwUpdateProcessScreen;
                if (screen != null)
                {
                    screen.Dispose();
                }
            }
        }

        #region Building wizard function

        /// <summary>
        /// Build and initialize wizard screens and steps panel
        /// </summary>
        /// <param name="controllers"></param>
        /// <param name="devices"></param>
        private void BuildWizard(List<SdkTbtController> controllers, List<SdkTbtDevice> devices)
        {
            LoadScreens(controllers, devices);
            PageContent.Content = _pages[_currentScreen];
            DataContext = PageContent.Content;
            ((Button) Steps.Children[_currentScreen]).Style = FindResource("CurrentWizardStepStyle") as Style;
        }

        /// <summary>
        /// Add all wizard screens in a sequential order 
        /// </summary>
        /// <param name="controllers"></param>
        /// <param name="devices"></param>
        private void LoadScreens(List<SdkTbtController> controllers, List<SdkTbtDevice> devices)
        {
            AddScreen(new WelcomeScreen());
            AddScreen(new SelectController(controllers, devices));
            AddScreen(new SelectImageScreen());
            AddScreen(new ConfirmationScreen());
            AddScreen(new FwUpdateProcessScreen());
            AddScreen(new UpdateCompletedScreen());
        }

        /// <summary>
        /// Adding a screen to the wizard screen lists
        /// </summary>
        /// <param name="screen">screen to add</param>
        private void AddScreen(IWizardScreen screen)
        {
            _pages.Add(screen);
            var stackPanel = new StackPanel();
            foreach (var str in screen.Title)
            {
                stackPanel.Children.Add(new TextBlock {Text = str, HorizontalAlignment = HorizontalAlignment.Center});
            }
            var button = new Button
            {
                Content = stackPanel,
                Style = FindResource("WizardStepStyle") as Style
            };
            Steps.Children.Add(button);
        }

        #endregion

        #region Wizard buttons commands

        /// <summary>
        /// Wizard next button command, loading next page in the wizard page list
        /// </summary>
        public void NextButton_Click(object sender, RoutedEventArgs re)
        {
            if (PageContent.Content is ConfirmationScreen)
            {
                _flashTask.Start();
            }
            else if (PageContent.Content is SelectImageScreen)
            {
                try
                {
                    CurrentController.ValidateImage(ImageFileName);
                    SafeModeWarning();
                    var nextWindow = ((ConfirmationScreen) _pages[_currentScreen + 1]);

                    // extracting current FW version
                    nextWindow.NvmVersion = Utilities.SafeGetVersion(() => CurrentController.GetCurrentFullNvmVersion());
                    nextWindow.NvmFileVersion = Utilities.GetImageFullNvmVersion(ImageFileName);

                    //extracting current PD version 
                    nextWindow.PdVersion = Utilities.SafeGetVersion(() => Utilities.GetTIPdInfo(CurrentController));
                    nextWindow.PdFileVersion = Utilities.GetImageTIPdVersion(ImageFileName);

                    //this indicates if power down is necessary after the upgrade.
                    _needsG3 = (nextWindow.PdFileVersion != nextWindow.PdVersion);

                    var controller = CurrentController as SdkTbtController;
                    nextWindow.IsHost = controller != null;
                    if (controller != null)
                    {
                        nextWindow.OsNativePciEnumeration = controller.IsInSafeMode
                            ? null
                            : (bool?) controller.OsNativePciEnumeration;
                        nextWindow.OsNativePciEnumerationFile =
                            Utilities.GetImageOsNativePciEnumerationStatus(ImageFileName);
                    }
                }
                catch (Exception e)
                {
                    var msg = e.Message;
                    MessageBox.Show(msg, FwUpdateApiSample.Resources.MessageBoxCaption, MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }
            //change previous step style to inactive
            ((Button) Steps.Children[_currentScreen]).Style = FindResource("WizardStepStyle") as Style;
            PageContent.Content = _pages[++_currentScreen];
            DataContext = PageContent.Content;

            //change previous step style to active
            ((Button) Steps.Children[_currentScreen]).Style = FindResource("CurrentWizardStepStyle") as Style;
        }

        /// <summary>
        /// Wizard next button command, loading previous page in the wizard page list
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button) Steps.Children[_currentScreen]).Style = FindResource("WizardStepStyle") as Style;
            PageContent.Content = _pages[--_currentScreen];
            DataContext = PageContent.Content;
            ((Button) Steps.Children[_currentScreen]).Style = FindResource("CurrentWizardStepStyle") as Style;
        }

        /// <summary>
        /// Wizard cancel button command, closing the application
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event 
        public void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void SafeModeWarning()
        {
            var controller = CurrentController as SdkTbtController;
            if (controller != null && controller.IsInSafeMode)
            {
                MessageBox.Show(FwUpdateApiSample.Resources.MinimalValidationInSafeMode,
                    FwUpdateApiSample.Resources.MessageBoxCaption,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        #endregion
    }

    #region Interfaces

    public interface IWizardScreen
    {
        bool CancelButtonActive { get; }
        bool NextButtonActive { get; }
        bool BackButtonActive { get; }
        string[] Title { get; }
    }

    #endregion
}
