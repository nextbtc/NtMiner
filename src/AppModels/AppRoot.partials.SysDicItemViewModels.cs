﻿using NTMiner.Vms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTMiner {
    public static partial class AppRoot {
        public class SysDicItemViewModels : ViewModelBase {
            public static readonly SysDicItemViewModels Instance = new SysDicItemViewModels();
            private readonly Dictionary<Guid, SysDicItemViewModel> _dicById = new Dictionary<Guid, SysDicItemViewModel>();

            private SysDicItemViewModels() {
                if (WpfUtil.IsInDesignMode) {
                    return;
                }
#if DEBUG
                NTStopwatch.Start();
#endif
                VirtualRoot.AddEventPath<ServerContextReInitedEvent>("ServerContext刷新后刷新VM内存", LogEnum.DevConsole,
                    action: message => {
                        _dicById.Clear();
                        Init();
                    }, location: this.GetType());
                VirtualRoot.AddEventPath<ServerContextVmsReInitedEvent>("ServerContext的VM集刷新后刷新视图界面", LogEnum.DevConsole,
                    action: message => {
                        OnPropertyChangeds();
                    }, location: this.GetType());
                AddEventPath<SysDicItemAddedEvent>("添加了系统字典项后调整VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        if (!_dicById.ContainsKey(message.Source.GetId())) {
                            _dicById.Add(message.Source.GetId(), new SysDicItemViewModel(message.Source));
                            OnPropertyChangeds();
                            if (SysDicVms.TryGetSysDicVm(message.Source.DicId, out SysDicViewModel sysDicVm)) {
                                sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItems));
                                sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItemsSelect));
                            }
                        }
                    }, location: this.GetType());
                AddEventPath<SysDicItemUpdatedEvent>("更新了系统字典项后调整VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        if (_dicById.TryGetValue(message.Source.GetId(), out SysDicItemViewModel vm)) {
                            int sortNumber = vm.SortNumber;
                            vm.Update(message.Source);
                            if (sortNumber != vm.SortNumber) {
                                if (SysDicVms.TryGetSysDicVm(vm.DicId, out SysDicViewModel sysDicVm)) {
                                    sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItems));
                                    sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItemsSelect));
                                }
                            }
                        }
                    }, location: this.GetType());
                AddEventPath<SysDicItemRemovedEvent>("删除了系统字典项后调整VM内存", LogEnum.DevConsole,
                    action: (message) => {
                        _dicById.Remove(message.Source.GetId());
                        OnPropertyChangeds();
                        if (SysDicVms.TryGetSysDicVm(message.Source.DicId, out SysDicViewModel sysDicVm)) {
                            sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItems));
                            sysDicVm.OnPropertyChanged(nameof(sysDicVm.SysDicItemsSelect));
                        }
                    }, location: this.GetType());
                Init();
#if DEBUG
                var elapsedMilliseconds = NTStopwatch.Stop();
                if (elapsedMilliseconds.ElapsedMilliseconds > NTStopwatch.ElapsedMilliseconds) {
                    Write.DevTimeSpan($"耗时{elapsedMilliseconds} {this.GetType().Name}.ctor");
                }
#endif
            }

            private void Init() {
                foreach (var item in NTMinerContext.Instance.ServerContext.SysDicItemSet.AsEnumerable()) {
                    _dicById.Add(item.GetId(), new SysDicItemViewModel(item));
                }
            }

            private void OnPropertyChangeds() {
                OnPropertyChanged(nameof(List));
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged(nameof(KernelBrandItems));
                OnPropertyChanged(nameof(KernelBrandsSelect));
                OnPropertyChanged(nameof(PoolBrandItems));
                OnPropertyChanged(nameof(AlgoItems));
            }

            public List<SysDicItemViewModel> KernelBrandItems {
                get {
                    List<SysDicItemViewModel> list = new List<SysDicItemViewModel>();
                    if (SysDicVms.TryGetSysDicVm(NTKeyword.KernelBrandSysDicCode, out SysDicViewModel sysDic)) {
                        list.AddRange(List.Where(a => a.DicId == sysDic.Id).OrderBy(a => a.SortNumber));
                    }
                    return list;
                }
            }

            public List<SysDicItemViewModel> KernelBrandsSelect {
                get {
                    List<SysDicItemViewModel> list = new List<SysDicItemViewModel> {
                        SysDicItemViewModel.PleaseSelect
                    };
                    if (SysDicVms.TryGetSysDicVm(NTKeyword.KernelBrandSysDicCode, out SysDicViewModel sysDic)) {
                        list.AddRange(List.Where(a => a.DicId == sysDic.Id).OrderBy(a => a.SortNumber));
                    }
                    return list;
                }
            }

            public List<SysDicItemViewModel> PoolBrandItems {
                get {
                    List<SysDicItemViewModel> list = new List<SysDicItemViewModel>();
                    if (SysDicVms.TryGetSysDicVm(NTKeyword.PoolBrandSysDicCode, out SysDicViewModel sysDic)) {
                        list.AddRange(List.Where(a => a.DicId == sysDic.Id).OrderBy(a => a.SortNumber));
                    }
                    return list;
                }
            }

            public List<SysDicItemViewModel> AlgoItems {
                get {
                    List<SysDicItemViewModel> list = new List<SysDicItemViewModel>();
                    if (SysDicVms.TryGetSysDicVm(NTKeyword.AlgoSysDicCode, out SysDicViewModel sysDic)) {
                        list.AddRange(List.Where(a => a.DicId == sysDic.Id).OrderBy(a => a.SortNumber));
                    }
                    return list;
                }
            }

            public int Count {
                get {
                    return _dicById.Count;
                }
            }

            public bool TryGetValue(Guid id, out SysDicItemViewModel vm) {
                return _dicById.TryGetValue(id, out vm);
            }

            public List<SysDicItemViewModel> List {
                get {
                    return _dicById.Values.ToList();
                }
            }

            public IEnumerable<SysDicItemViewModel> GetSysDicItemVmsByDicId(Guid dicId) {
                return _dicById.Values.Where(a => a.DicId == dicId);
            }
        }
    }
}
