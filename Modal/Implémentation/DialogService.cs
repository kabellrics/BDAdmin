using BDAdmin.Modal.Interface;
using BDAdmin.Modal.Modal;
using BDAdmin.Modal.ViewModel;
using BDAdmin.ViewModel;
using BDAdmin.ViewModel.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.Modal.Implémentation
{
    public class DialogService : IDialogService
    {

       
        public bool ShowMessageOk(string title, string message)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.DataContext = new DialogViewModel()
            {
                Title = title,
                Message = message,
                ContentButtonYes = "Ok",
                ContentButtonCancel = string.Empty
            };
            var dialogResult = dialogWindow.ShowDialog();
            return dialogResult.Value;
        }

        public bool ShowMessageOkCancel(string title, string message)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.DataContext = new DialogViewModel()
            {
                Title = title,
                Message = message,
                ContentButtonYes = "Ok",
                ContentButtonCancel = "Cancel"
            };
            var dialogResult = dialogWindow.ShowDialog();
            return dialogResult.Value;
        }

        public bool ShowMessageOkCancel(string title, string message, string contentButtonOk, string contentButtonCancel)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.DataContext = new DialogViewModel()
            {
                Title = title,
                Message = message,
                ContentButtonYes = contentButtonOk,
                ContentButtonCancel = contentButtonCancel
            };
            var dialogResult = dialogWindow.ShowDialog();
            return dialogResult.Value;
        }

        public bool showMessageYesNoCancel(string title, string message, string contentButtonYes, string contentButtonNo, string contentButtonCancel)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.DataContext = new DialogViewModel()
            {
                Title = title,
                Message = message,
                ContentButtonYes = contentButtonYes,
                ContentButtonCancel = contentButtonCancel,
                ContentButtonNo = contentButtonNo
            };
            var dialogResult = dialogWindow.ShowDialog();
            return dialogResult.Value;
        }
        public string OpenUniqueFileDialog(string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.Multiselect = false;
            var dialogresult = openFileDialog.ShowDialog();
            if (dialogresult.Value)
                return openFileDialog.FileName;
            else
                return null;
        }
        public IEnumerable<string> OpenMultiFileDialog(string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.Multiselect = true;
            var dialogresult = openFileDialog.ShowDialog();
            if (dialogresult.Value)
                return openFileDialog.FileNames;
            else
                return null;
        }

        public SerieViewModel ShowChooseParent(SerieViewModel serie)
        {
            DialogWindow dialogWindow = new DialogWindow();
            var vm = new DialogChooseParentViewModel();
            dialogWindow.DataContext = vm;
            vm.Serie = serie;
            if (dialogWindow.ShowDialog().Value)
            {
                return vm.SelectedParent;
            }
            return null;
        }
    }
}
