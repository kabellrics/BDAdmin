using BDAdmin.Modal.Interface;
using BDAdmin.Modal.Modal;
using BDAdmin.Modal.ViewModel;
using BDAdmin.ViewModel;
using BDAdmin.ViewModel.Model;
using Common;
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
        public bool ShowCreateOrUpdateSerieModal(SerieViewModel serie = null)
        {
            DialogWindow dialog = new DialogWindow();
            var vm = new AddUpdateSerieViewModel();
            if(serie != null)
                vm.Serie = serie.Serie;
            dialog.DataContext = vm;
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.Value)
                return true;
            else
                return false;
        }
        public bool ShowValidateFile(string filepath)
        {
            DialogWindow dialogWindow = new DialogWindow();
            var vm = new AddSingleComicViewModel(filepath);
            dialogWindow.DataContext = vm;
            var dialogResult = dialogWindow.ShowDialog();
            if (dialogResult.Value)
                return true;
            else
                return false;
        }
        public bool ShowValidateMultiFile(IEnumerable<string> paths)
        {
            DialogWindow dialogWindow = new DialogWindow();
            var vm = new AddMultiComicViewModel(paths);
            dialogWindow.DataContext = vm;
            var dialogResult = dialogWindow.ShowDialog();
            if (dialogResult.Value)
                return true;
            else
                return false;
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
        public async Task<IEnumerable<Page>> ShowNotifExtract(string FilePath)
        {
            DialogWindow dialogWindow = new DialogWindow();
            var vm = new ExtractFileViewModel(FilePath);
            dialogWindow.DataContext = vm;
            if (dialogWindow.ShowDialog().Value)
            {
                return vm.Pages;
            }
            return null;
        }
        public async Task<bool> ShowImage(byte[] data)
        {
            DialogWindow dialogWindow = new DialogWindow();
            var vm = new ShowImageViewModel(data);
            dialogWindow.DataContext = vm;
            return dialogWindow.ShowDialog().Value;

        }
        public async Task<ComicVineResult> ShowResolveAttempt(String name)
        {
            DialogWindow dialogWindow = new DialogWindow();
            var vm = new ResolveIssueViewModel(name);
            dialogWindow.DataContext = vm;
            if (dialogWindow.ShowDialog().Value)
            {
                return vm.ChoosenIssue;
            }
            return null;
        }
    }
}
