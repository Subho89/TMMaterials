using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TMMaterials.Services;

namespace TMMaterials.ViewModels
{
    public class TMAdminVM : INotifyPropertyChanged
    {
        private readonly TMMaterialsServicesVM _dbService;

        // Properties for UI Binding
        private string _filePath = "Select file...";
        private int _progress;
        private string _logs = "Ready.";

        public string FilePath { get => _filePath; set { _filePath = value; OnPropChanged("FilePath"); } }
        public int ProgressValue { get => _progress; set { _progress = value; OnPropChanged("ProgressValue"); } }
        public string LogOutput { get => _logs; set { _logs = value; OnPropChanged("LogOutput"); } }

        public ICommand BrowseCommand { get; }
        public ICommand ProcessCommand { get; }
        public ICommand ClearCommand { get; }

        public TMAdminVM()
        {
            _dbService = new TMMaterialsServicesVM();
            BrowseCommand = new RelayCommand(OnBrowse);
            ProcessCommand = new RelayCommand(async () => await OnProcess());
            ClearCommand = new RelayCommand(OnClear);
        }

        private void OnBrowse()
        {
            var dialog = new OpenFileDialog { Filter = "XML files (*.xml)|*.xml" };
            if (dialog.ShowDialog() == true) FilePath = dialog.FileName;
        }

        private async Task OnProcess()
        {
            LogOutput = "Starting service operation...";
            ProgressValue = 0;

            var progressReporter = new Progress<int>(val => ProgressValue = val);

            bool success = await _dbService.ProcessXmlFileAsync(
                FilePath,
                progressReporter,
                msg => LogOutput += $"\n{msg}"
            );

            LogOutput += success ? "\nSuccess: XML mapped to EAV structure." : "\nError: Process aborted.";
            if (success)
            {
                MessageBox.Show("Success: XML transfered to database.");
            }
            else
            {
                MessageBox.Show("Error: Process aborted.");
            }
                
        }

        private void OnClear()
        {
            FilePath = string.Empty;
            ProgressValue = 0;
            LogOutput = "Form reset. Ready for new import.";
            // If you have any internal lists or temporary data, clear them here too.
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropChanged(string p) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}