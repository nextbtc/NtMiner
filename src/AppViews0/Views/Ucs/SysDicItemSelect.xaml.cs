﻿using NTMiner.Vms;
using System.Windows.Controls;

namespace NTMiner.Views.Ucs {
    public partial class SysDicItemSelect : UserControl {
        public SysDicItemSelectViewModel Vm {
            get {
                return (SysDicItemSelectViewModel)this.DataContext;
            }
        }

        public SysDicItemSelect(SysDicItemSelectViewModel vm) {
            this.DataContext = vm;
            InitializeComponent();
        }

        private void DataGrid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Vm.OnOk?.Invoke(Vm.SelectedResult);
        }

        private void DataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                Vm.OnOk?.Invoke(Vm.SelectedResult);
                e.Handled = true;
            }
        }
    }
}
