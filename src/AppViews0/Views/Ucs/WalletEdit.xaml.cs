﻿using NTMiner.Vms;
using System.Windows.Controls;

namespace NTMiner.Views.Ucs {
    public partial class WalletEdit : UserControl {
        public static void ShowWindow(FormType formType, WalletViewModel source) {
            ContainerWindow.ShowWindow(new ContainerWindowViewModel {
                Title = "钱包",
                FormType = formType,
                Width = 520,
                IconName = "Icon_Wallet",
                IsMaskTheParent = true,
                CloseVisible = System.Windows.Visibility.Visible
            }, ucFactory: (window) =>
            {
                WalletViewModel vm = new WalletViewModel(source) {
                    AfterClose = source.AfterClose
                };
                window.AddCloseWindowOnecePath(vm.Id);
                return new WalletEdit(vm);
            }, fixedSize: true);
        }

        private WalletViewModel Vm {
            get {
                return (WalletViewModel)this.DataContext;
            }
        }

        public WalletEdit(WalletViewModel vm) {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
