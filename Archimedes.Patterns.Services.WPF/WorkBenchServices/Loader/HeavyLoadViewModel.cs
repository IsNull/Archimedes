using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.WPF.ViewModels;

namespace Archimedes.Services.WPF.WorkBenchServices.Loader
{
    public class HeavyLoadViewModel : WorkspaceViewModel
    {
        bool _isIndeterminate;
        int _progressPercent;
        string _currentProcessedInfo;
        string _innerTitle;
        string _info;

        public HeavyLoadViewModel() { }


        #region VM Properties

        public bool IsIndeterminate {
            get { return _isIndeterminate; }
            set { 
                _isIndeterminate = value;
                OnPropertyChanged(() => IsIndeterminate);
            }
        }

        public int ProgressPercent {
            get { return _progressPercent; }
            set {
                _progressPercent = value;
                OnPropertyChanged(() => ProgressPercent);
            }
        }
        
        public string CurrentProcessedInfo {
            get { return _currentProcessedInfo; }
            set {
                _currentProcessedInfo = value;
                OnPropertyChanged(() => CurrentProcessedInfo);
            }
        }

        public string InnerTitle {
            get { return _innerTitle; }
            set {
                _innerTitle = value;
                OnPropertyChanged(() => InnerTitle);
            }
        }

        public string Info {
            get { return _info; }
            set {
                _info = value;
                OnPropertyChanged(() => Info);
            }
        }

        #endregion
    }
}
