﻿using BDAdmin.ViewModel;
using BDAdmin.ViewModel.Model;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.Modal.Interface
{
    public interface IDialogService //: GalaSoft.MvvmLight.Views.IDialogService
    {
        bool ShowMessageOk(string title, string message);
        bool ShowMessageOkCancel(string title, string message);
        bool ShowMessageOkCancel(string title, string message, string contentButtonOk, string contentButtonCancel);
        bool showMessageYesNoCancel(string title, string message, string contentButtonYes, string contentButtonNo, string contentButtonCancel);
        SerieViewModel ShowChooseParent(SerieViewModel serie);
        bool ShowCreateOrUpdateSerieModal(SerieViewModel serie = null);
        string OpenUniqueFileDialog(string filter);
        IEnumerable<string> OpenMultiFileDialog(string filter);
        bool ShowValidateFile(string filepath);
        bool ShowValidateMultiFile(IEnumerable<string> paths);
        Task<IEnumerable<Page>> ShowNotifExtract(string FilePath);
        Task<ComicVineResult> ShowResolveAttempt(String name);
        Task<bool> ShowImage(byte[] data);
    }
}
