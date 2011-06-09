using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Archimedes.Patterns.WPF.ViewModels;
using Archimedes.Patterns.WPF.Commands;


namespace Archimedes.Services.WPF.WorkBenchServices.MessageBox
{
    public class MessageBoxViewModel : WorkspaceViewModel
    {
        #region Fields
        string _detailHeader = "Details";
        string _message, _detailmessage;
        string _imageSource;
        MessageBoxType _messageBoxImage = MessageBoxType.None;
        readonly MessageBoxWPFButton _buttons = MessageBoxWPFButton.OK;
        DialogWPFResult _dialogeResult = DialogWPFResult.Abort;

        string _button1;
        string _button2;
        string _button3;
        #endregion

        public MessageBoxViewModel(string message, MessageBoxWPFButton buttons = MessageBoxWPFButton.OK) {
            Message = message;
            _buttons = buttons;
            Button1 = "Ok";
        }

        #region Properties

        /// <summary>
        /// The Message to display
        /// </summary>
        public string Message {
            get { return _message; }
            set {
                _message = value;
                OnPropertyChanged(() => Message);
            }
        }

        /// <summary>
        /// Specail Detail Message Data which is displayed in an Scrollable Messagefield
        /// </summary>
        public string DetailMessage {
            get { return _detailmessage; }
            set { 
                _detailmessage = value;
                OnPropertyChanged(
                    () => DetailMessage,
                    () => DetailMessageVisible);
            }
        }
        
        public string DetailHeader {
            get { return _detailHeader; }
            set { _detailHeader = value; }
        }

        public Visibility DetailMessageVisible {
            get { return (string.IsNullOrWhiteSpace(DetailMessage)) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public int? MaxTextAeraWidth {
            get { return (DetailMessageVisible == Visibility.Collapsed) ? (int?)500 : null; }
        }

        /// <summary>
        /// URI to the Image to display
        /// </summary>
        public string ImageSource {
            get { return _imageSource; }
            set { 
                _imageSource = value; 
                OnPropertyChanged(() => ImageSource); 
            }
        }

        public DialogWPFResult DialogeResult {
            get { return _dialogeResult; }
            private set { _dialogeResult = value; }
        }

        #region Buttons

        /// <summary>
        /// Button text 1
        /// </summary>
        public string Button1 {
            get { return _button1; }
            set {
                _button1 = value;
                OnPropertyChanged(() => Button1);
            }
        }

        /// <summary>
        /// Button text 2
        /// </summary>
        public string Button2 {
            get { return _button2; }
            set {
                _button2 = value;
                OnPropertyChanged(() => Button2);
            }
        }

        /// <summary>
        /// Button text 3
        /// </summary>
        public string Button3 {
            get { return _button3; }
            set {
                _button3 = value;
                OnPropertyChanged(() => Button3);
            }
        }

        #endregion

        /// <summary>
        /// Predefined Images
        /// </summary>
        public MessageBoxType MessageBoxImage {
            set {
                _messageBoxImage = value;

                switch (_messageBoxImage) {

                    case MessageBoxType.None:
                    ImageSource = "";
                    break;

                    case MessageBoxType.Ok:
                    ImageSource = "../../Images/dialog-clean.ico";
                    break;

                    // Error, Stop, Hand
                    case MessageBoxType.Error:
                    ImageSource = "../../Images/dialog-stop.ico";
                    break;

                    case MessageBoxType.Question:
                    ImageSource = "../../Images/dialog-question.ico";
                    break;

                    // Warning , Exclamation
                    case MessageBoxType.Warning:
                    ImageSource = "../../Images/dialog-stop.ico";
                    break;

                    // Information / Asterisk
                    case MessageBoxType.Information:
                    ImageSource = "../../Images/dialog-information.ico";
                    break;
                }
            }
        }

        #endregion

        #region Commands

        public ICommand Button1Command {
            get { return new RelayCommand(x => ButtonAction(1)); }
        }
        public ICommand Button2Command {
            get { return new RelayCommand(x => ButtonAction(2)); }
        }
        public ICommand Button3Command {
            get { return new RelayCommand(x => ButtonAction(3)); }
        }

        void ButtonAction(int n){
            switch(_buttons){
                case MessageBoxWPFButton.OK:
                _dialogeResult = DialogWPFResult.OK;
                break;

                case MessageBoxWPFButton.OKCancel:
                _dialogeResult = (n == 1) ? DialogWPFResult.OK : DialogWPFResult.Cancel;
                break;

                case MessageBoxWPFButton.YesNo:
                _dialogeResult = (n == 1) ? DialogWPFResult.Yes : DialogWPFResult.No;
                break;

                case MessageBoxWPFButton.YesNoCancel:
                switch (n) {
                    case 1:
                    _dialogeResult = DialogWPFResult.Yes;
                    break;
                    case 2:
                    _dialogeResult = DialogWPFResult.No;
                    break;
                    case 3:
                    _dialogeResult = DialogWPFResult.Cancel;
                    break;
                }
                break;
            }
            this.CloseCommand.Execute(null);
        }

        #endregion

        #region Public Methods

        public UserControl BuildView() {
            //todo: localisation
            UserControl view = null;

            if (_buttons == MessageBoxWPFButton.OK) {
                Button1 = "Ok";
                view = new MessageBoxView();
            } else if (_buttons == MessageBoxWPFButton.OKCancel) {
                Button1 = "Ok";
                Button2 = "Abbrechen";
                view = new MessageBox2View();
            } else if (_buttons == MessageBoxWPFButton.YesNo) {
                Button1 = "Ja";
                Button2 = "Nein";
                view = new MessageBox2View();
            } else if (_buttons == MessageBoxWPFButton.YesNoCancel) {
                Button1 = "Ja";
                Button2 = "Nein";
                Button3 = "Abbrechen";
                view = new MessageBox3View();
            }

            view.DataContext = this;
            return view;
        }

        #endregion
    }


}
