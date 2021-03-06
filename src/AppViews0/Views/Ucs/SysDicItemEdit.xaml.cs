﻿using NTMiner.Vms;
using System.Windows.Controls;

namespace NTMiner.Views.Ucs {
    public partial class SysDicItemEdit : UserControl {
        public static void ShowWindow(FormType formType, SysDicItemViewModel source) {
            ContainerWindow.ShowWindow(new ContainerWindowViewModel {
                Title = "系统字典项",
                FormType = formType,
                Width= 520,
                IsMaskTheParent = true,
                CloseVisible = System.Windows.Visibility.Visible,
                IconName = "Icon_SysDic"
            }, ucFactory: (window) =>
            {
                SysDicItemViewModel vm = new SysDicItemViewModel(source);
                window.AddCloseWindowOnecePath(vm.Id);
                return new SysDicItemEdit(vm);
            }, fixedSize: true);
        }

        private SysDicItemViewModel Vm {
            get {
                return (SysDicItemViewModel)this.DataContext;
            }
        }

        public SysDicItemEdit(SysDicItemViewModel vm) {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
